using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using System.IO;

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

	/********** Getter & Setter	**********/
	public int Wild_GetDungeonX() { return m_dungeonX; }

	public int Wild_GetDungeonY() { return m_dungeonY; }

	/********** Method	**********/

	/********** Default Method	**********/
	protected override void Wild_Init()
	{
		base.Wild_Init();

		//
		Wild_Tile_Init();
		Wild_Rooms_Init();
		Wild_Player_Init();

		//
		m_c_move = new Wild_Dungeon_Move();
		m_c_move.Wild_Init(m_canvas);
		m_c_move.Wild_Setting(this);
		m_l_UIManager.Add(m_c_move);

		Wild_Dungeon_Battle c_battle = new Wild_Dungeon_Battle();
		c_battle.Wild_Init(m_canvas);
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
	}

	//////////	//////////
	#region player
	List<Wild_Character> m_l_player;

	/********** Getter & Setter	**********/
	// m_l_player
	public Wild_Character Wild_Player_GetCharacter(int _num) { return m_l_player[_num]; }

	public int Wild_Player_GetCharacterCount() { return m_l_player.Count; }

	/********** Method	**********/

	/********** Default Method	**********/
	void Wild_Player_Init()
	{
		m_l_player = new List<Wild_Character>();
	}
	#endregion

	//////////	//////////
	#region rooms
	List<Wild_Dungeon_Room> m_l_room;
	int m_dungeonX;
	int m_dungeonY;
	int m_roomCount;

	/********** Getter & Setter	**********/

	/********** Method	**********/
	public void Wild_Rooms_Move(int _moving)
	{
		Wild_Dungeon_Room room = m_l_room[m_roomCount];
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
			if(room.Wild_GetX().Equals(m_dungeonX - 1))
			{
				Debug.Log("Wild_Moving error");
				return;
			}
		}
		else if( _moving.Equals(-m_dungeonX) )
		{
			if(room.Wild_GetY().Equals(0))
			{
				Debug.Log("Wild_Moving error");
				return;
			}
		}
		else if( _moving.Equals(m_dungeonX) )
		{
			if(room.Wild_GetY().Equals(m_dungeonY - 1))
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

		if(m_l_room[_roomNumber].Wild_GetNumber().Equals(_roomNumber))
		{
			m_roomCount = _roomNumber;
			res = true;
		}
		else
		{
			for(int i = 0; i < m_l_room.Count; i++)
			{
				if(m_l_room[i].Wild_GetNumber().Equals(_roomNumber))
				{
					m_roomCount = i;
					res = true;
					break;
				}
			}
		}

		// SetUI
		if(res)
		{
			Wild_Dungeon_Room room = m_l_room[m_roomCount];
			Wild_Dungeon_UI temp = room.Wild_GetDungeonType();

			Wild_SetActive((int)temp);

			switch(temp)
			{
				case Wild_Dungeon_UI.Move:
					{
						m_c_move.Wild_SettingBtnActive(room.Wild_GetMoving());
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
		m_l_room = new List<Wild_Dungeon_Room>();

        try
        {
            using(StreamReader reader = new StreamReader("Assets/Wild_Project1/Data/Dungeon/" + "0" + ".txt"))
            {
				//
				{
					string temp = reader.ReadLine();
					string[] strs1 = temp.Split('!');
					string[] dungeonSize = strs1[0].Split(',');

					m_dungeonX = int.Parse(dungeonSize[0]);
					m_dungeonY = int.Parse(dungeonSize[1]);
					m_roomCount = int.Parse(strs1[1]);
					Debug.Log("Wild_Init_Dungeon " + m_dungeonX + " " + m_dungeonY);
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
						room.Wild_Init(m_map.transform, m_dungeonX, i, temp);
						m_l_room.Add(room);
						
						// 셋팅
						i++;
					}
				}
			}
		}
		catch(System.Exception e)
		{
			return;
		}
	}
	#endregion

	//////////	//////////
	#region tile
	//
	GameObject m_map;
	Wild_Tile[] m_a_tile;

	/********** Getter & Setter	**********/
	public Wild_Tile Wild_GetTile(int _num) { return m_a_tile[_num]; }

	/********** Method	**********/
	public int Wild_FindMyTileNumber(Wild_Object _obj)
	{
		int res = -1;
		for(int i = 0; i < m_a_tile.Length; i++)
		{
			Wild_Object obj = m_a_tile[i].Wild_GetObject();

			if(	m_a_tile[i].Wild_GetObject() != null &&
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
		m_map = Object.Instantiate(Resources.Load<GameObject>("Basic"));
		m_map.transform.parent = m_canvas.transform;
		float screenY = Wild_Singleton_Screen.Wild_GetInstance().Wild_GetScreenSize().y;
		m_map.transform.localPosition = new Vector3(0.0f, -screenY * 0.2f, 0.0f);
		float scale = screenY / 10.0f * 0.5f;
		m_map.transform.localScale = new Vector3(scale, scale, scale);

		//
		m_a_tile = new Wild_Tile[(int)Wild_Map_SIZE.X * (int)Wild_Map_SIZE.Y];
        for(int y = 0; y < (int)Wild_Map_SIZE.Y; y++)
        {
            for(int x = 0; x < (int)Wild_Map_SIZE.X; x++)
            {
				// 헥스의 속성을 셋팅
				m_a_tile[0] = new Wild_Tile();
                m_a_tile[0].Wild_Init(x + (y * (int)Wild_Map_SIZE.Y), x, y, (int)Wild_Tile_TYPE.Wild_Tile_TYPE_GRASS, m_map);

				// 맵이동 범위 지정
				//if(m_right < obj.transform.localPosition.x) m_right = hexPos.x;
            }
        }
		m_map.transform.Rotate(-45.0f, 0.0f, 0.0f);
	}
	#endregion
}
