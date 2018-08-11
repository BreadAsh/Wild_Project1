using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using System.IO;
using LitJson;

public enum Wild_Dungeon_UI
{
	Move = 0,
	Moving = 1,
	Battle = 2
}

enum Wild_Map_SIZE
{
	X = 10,
	Y = 5
}

public class Wild_Dungeon_Manager : Wild_SceneManager
{
	//
	Wild_Dungeon_Move m_c_move;
	public GameObject m_ObjectCanvas;

	/********** Getter & Setter	**********/

	/********** Method	**********/

	/********** Default Method	**********/
	protected override void Wild_Init()
	{
		base.Wild_Init();

		//
		Wild_Tile_Init();
		Wild_Player_Init();
		Wild_Rooms_Init();

		//
		m_c_move = new Wild_Dungeon_Move();
		m_c_move.Wild_Init(m_UI_camera, m_UICanvas);
		m_c_move.Wild_InitAnother(this);
		m_l_UIManager.Add(m_c_move);

		Wild_Dungeon_Battle c_battle = new Wild_Dungeon_Battle();
		c_battle.Wild_Init(this.GetComponent<Camera>(), m_ObjectCanvas);
		c_battle.Wild_InitSetting(this);
		m_l_UIManager.Add(c_battle);

		Wild_Rooms_Find_SetUI(0);
	}

	protected override void Wild_Release()
	{
		base.Wild_Release();
	}

	protected override void Wild_Update()
	{
		base.Wild_Update();
		Wild_Player_Update();
	}

	//////////	//////////
	#region rooms
	List<Wild_Dungeon_Room> m_rooms_l_room;
	int m_rooms_dungeonX;
	int m_rooms_dungeonY;
	int m_rooms_roomCount;

	/********** Getter & Setter	**********/
	public Wild_Dungeon_Room Wild_Rooms_GetNowRoom() { return m_rooms_l_room[m_rooms_roomCount]; }

	public Wild_Dungeon_Room Wild_Dungeon_GetRoomNumber(int _num)
	{
		Wild_Dungeon_Room res = null;

		for(int i = 0; i < m_rooms_l_room.Count; i++)
		{
			if(m_rooms_l_room[i].Wild_GetNumber().Equals(_num))
			{
				res = m_rooms_l_room[i];
			}
		}

		return res;
	}

	public int Wild_Rooms_GetDungeonX() { return m_rooms_dungeonX; }

	public int Wild_Rooms_GetDungeonY() { return m_rooms_dungeonY; }

	/********** Method	**********/
	public void Wild_Rooms_Move(int _moving)
	{
		Wild_Dungeon_Room room = m_rooms_l_room[m_rooms_roomCount];
		if( _moving.Equals(-1) )
		{
			if(room.Wild_GetX().Equals(0))
			{
				Debug.Log("Wild_Moving error");
				return;
			}
		}
		else if( _moving.Equals(1) )
		{
			if(room.Wild_GetX().Equals(m_rooms_dungeonX - 1))
			{
				Debug.Log("Wild_Moving error");
				return;
			}
		}
		else if( _moving.Equals(-m_rooms_dungeonX) )
		{
			if(room.Wild_GetY().Equals(0))
			{
				Debug.Log("Wild_Moving error");
				return;
			}
		}
		else if( _moving.Equals(m_rooms_dungeonX) )
		{
			if(room.Wild_GetY().Equals(m_rooms_dungeonY - 1))
			{
				Debug.Log("Wild_Moving error");
				return;
			}
		}

		int temp = room.Wild_GetNumber();
		
		temp += _moving;
		if(!Wild_Rooms_Find_SetUI(temp))
		{
			Debug.Log("Wild_Moving nothing");
		}
	}

	//
	public bool Wild_Rooms_Find_SetUI(int _roomNumber)
	{
		// FindRoom
		bool res = false;

		if(m_rooms_l_room[_roomNumber].Wild_GetNumber().Equals(_roomNumber))
		{
			m_rooms_l_room[m_rooms_roomCount].Wild_Off();
			m_rooms_roomCount = _roomNumber;
			m_rooms_l_room[m_rooms_roomCount].Wild_On();
			res = true;
		}
		else
		{
			for(int i = 0; i < m_rooms_l_room.Count; i++)
			{
				if(m_rooms_l_room[i].Wild_GetNumber().Equals(_roomNumber))
				{
					m_rooms_roomCount = i;
					res = true;
					break;
				}
			}
		}

		// SetUI
		if(res)
		{
			Wild_Dungeon_Room room = m_rooms_l_room[m_rooms_roomCount];
			Wild_Dungeon_UI temp = room.Wild_GetDungeonType();

			Wild_SetActive((int)temp);

			switch(temp)
			{
				case Wild_Dungeon_UI.Move:
					{
						m_c_move.Wild_SettingUI(room.Wild_GetMoving());
					}
					break;
				case Wild_Dungeon_UI.Battle:
					{
					}
					break;
			}
		}

		return res;
	}

	/********** Default Method	**********/
	void Wild_Rooms_Init()
	{
		m_rooms_l_room = new List<Wild_Dungeon_Room>();

		StreamReader reader = Wild_Static_File.Wild_FileReader("Assets/Wild_Project1/Data/Dungeon/" + "0" + ".txt");
		if(reader != null)
		{
			//
			{
				string temp = reader.ReadLine();
				string[] strs1 = temp.Split('!');
				string[] dungeonSize = strs1[0].Split(',');

				m_rooms_dungeonX = int.Parse(dungeonSize[0]);
				m_rooms_dungeonY = int.Parse(dungeonSize[1]);
				m_rooms_roomCount = int.Parse(strs1[1]);
				//Debug.Log("Wild_Init_Dungeon " + m_rooms_dungeonX + " " + m_rooms_dungeonY);
			}

			//
			{
				string temp = "";
				int i = 0;

				while(true)
				{
					temp = reader.ReadLine();

					if(temp.Equals("next"))
					{
						continue;
					}
					else if(temp.Equals("NONE"))
					{
						i++;
						continue;
					}
					else if(temp.Equals("end"))
					{
						break;
					}
					
					//Debug.Log("Wild_Init_Dungeon " + i);
					Wild_Dungeon_Room room = new Wild_Dungeon_Room();
					room.Wild_Init(this, m_rooms_dungeonX, i, temp);
					m_rooms_l_room.Add(room);
					
					// 셋팅
					i++;
				}
			}
			
			reader.Close();
		}
	}
	#endregion

	//////////	//////////
	#region tile
	//
	GameObject m_tile_basic;
	Wild_Tile[] m_tile_a_tile;

	/********** Getter & Setter	**********/
	public GameObject Wild_Tile_GetBasic() { return m_tile_basic; }

	// tile
	public Wild_Tile Wild_Tile_GetTile(int _num) { return m_tile_a_tile[_num]; }

	public int Wild_Tile_GetTileCount() { return m_tile_a_tile.Length; }

	/********** Method	**********/
	public int Wild_FindMyTileNumber(Wild_Object _obj)
	{
		int res = -1;
		for(int i = 0; i < m_tile_a_tile.Length; i++)
		{
			Wild_Object obj = m_tile_a_tile[i].Wild_GetObject();

			if(	m_tile_a_tile[i].Wild_GetObject() != null &&
				obj.Wild_GetType().Equals(_obj.Wild_GetType()) &&
				obj.Wild_GetNumber().Equals(_obj.Wild_GetNumber()))
			{
				res = i;
				break;
			}
		}

		return res;
	}

	/********** Default Method	**********/
	void Wild_Tile_Init()
	{
		// 베이직 설정
		m_tile_basic = Object.Instantiate(Resources.Load<GameObject>("Basic"));
		m_tile_basic.transform.parent = m_ObjectCanvas.transform;
		float screenY = Wild_Singleton_Screen.Wild_GetInstance().Wild_GetScreenSize().y;
		m_tile_basic.transform.localPosition = new Vector3(0.0f, -screenY * 0.2f, 0.0f);
		float scale = screenY / 10.0f * 0.6f;
		m_tile_basic.transform.localScale = new Vector3(scale, scale, scale);

		//
		m_tile_a_tile = new Wild_Tile[(int)Wild_Map_SIZE.X * (int)Wild_Map_SIZE.Y];
        for(int y = 0; y < (int)Wild_Map_SIZE.Y; y++)
        {
            for(int x = 0; x < (int)Wild_Map_SIZE.X; x++)
            {
				int tempNum = x + (y * (int)Wild_Map_SIZE.X);
				// 헥스의 속성을 셋팅
				m_tile_a_tile[tempNum] = new Wild_Tile();
                m_tile_a_tile[tempNum].Wild_Init(tempNum, x, y, Wild_Tile_ATTRIBUTE.GRASS, m_tile_basic);

				// 맵이동 범위 지정
				//if(m_right < obj.transform.localPosition.x) m_right = hexPos.x;
            }
        }
		m_tile_basic.transform.Rotate(-45.0f, 0.0f, 0.0f);
	}
	#endregion

	//////////	//////////
	#region player
	List<Wild_Character> m_player_l_characters;
	/********** Getter & Setter	**********/
	public Wild_Character Wild_Player_GetCharacter(int _num) { return m_player_l_characters[_num]; }
	public int Wild_Player_GetCharacterListCount() { return m_player_l_characters.Count; }

	/********** Method	**********/
	public void Wild_Player_CharacterRemoveAt(int _num)
	{
		m_player_l_characters.RemoveAt(_num);
	}

	/********** Default Method	**********/
	void Wild_Player_Init()
	{
		m_player_l_characters = new List<Wild_Character>();
		StreamReader reader = Wild_Static_File.Wild_FileReader("Assets/Wild_Project1/SaveData/Character/List");
        if(reader != null)
		{
			string str = reader.ReadLine();
			
			while(!str.Equals("END"))
			{
				if(!str.Split(',')[1].Equals("-1"))
				{
					Wild_Character character = new Wild_Character();
					character.Wild_InitAnother(this);
					character.Wild_Init(m_tile_basic, str, Wild_Object_TYPE.CHARACTER);
					m_player_l_characters.Add(character);
				}

				//
				str = reader.ReadLine();
			}

			reader.Close();
		}
	}

	void Wild_Player_Update()
	{
		for(int i = 0; i < m_player_l_characters.Count; i++)
		{
			m_player_l_characters[i].Wild_Update();
		}

		for(int i = 0; i < m_rooms_l_room[m_rooms_roomCount].Wild_GetEnemyCount(); i++)
		{
			m_rooms_l_room[m_rooms_roomCount].Wild_GetEnemy(i).Wild_Update();
		}
	}
	#endregion
}
