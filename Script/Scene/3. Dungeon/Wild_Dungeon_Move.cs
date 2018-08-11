using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild_Dungeon_Move : Wild_UI_Manager
{
	Wild_Dungeon_Manager m_c_manager;
	/********** Method	**********/
	public void Wild_InitAnother(Wild_Dungeon_Manager _c_manager)
	{
		m_c_manager = _c_manager;

		//
		int btn_num = 0;
		Wild_Dungeon_Move_Btn_LeftMove leftMove_btn = new Wild_Dungeon_Move_Btn_LeftMove();
		leftMove_btn.Wild_Init(btn_num, "2. WorldMap/Dungeon_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		leftMove_btn.Wild_SetClassManager(_c_manager);
		Wild_Button_Add(leftMove_btn);

		btn_num++;
		Wild_Dungeon_Move_Btn_TopMove topMove_btn = new Wild_Dungeon_Move_Btn_TopMove();
		topMove_btn.Wild_Init(btn_num, "2. WorldMap/Dungeon_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		topMove_btn.Wild_SetClassManager(_c_manager);
		Wild_Button_Add(topMove_btn);

		btn_num++;
		Wild_Dungeon_Move_Btn_RightMove rightMove_btn = new Wild_Dungeon_Move_Btn_RightMove();
		rightMove_btn.Wild_Init(btn_num, "2. WorldMap/Dungeon_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		rightMove_btn.Wild_SetClassManager(_c_manager);
		Wild_Button_Add(rightMove_btn);

		btn_num++;
		Wild_Dungeon_Move_Btn_BottomMove bottomMove_btn = new Wild_Dungeon_Move_Btn_BottomMove();
		bottomMove_btn.Wild_Init(btn_num, "2. WorldMap/Dungeon_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		bottomMove_btn.Wild_SetClassManager(_c_manager);
		Wild_Button_Add(bottomMove_btn);
	}

	//
	public void Wild_SettingUI(bool[] _a_isMove)
	{
		// button
		for(int i = 0; i < _a_isMove.Length; i++)
		{
			if(_a_isMove[i])
				Wild_Button_Find(i).Wild_On();
			else
				Wild_Button_Find(i).Wild_Off();
		}

		Wild_Minimap_Setting();
	}

	/********** Default Method	**********/
	public override void Wild_Init(Camera _camera, GameObject _canvas)
	{
		base.Wild_Init(_camera, _canvas);

		Wild_Minimap_Init();

		m_enum = (int)Wild_Dungeon_UI.Move;
	}

	#region mini map
	GameObject[] m_minimap_a_tile;
	/********** Getter & Setter	**********/

	/********** Method	**********/
	void Wild_Minimap_Setting()
	{
		//
		for(int i = 0; i < m_minimap_a_tile.Length; i++)
		{
			m_minimap_a_tile[i].GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("UI/1. MainMenu/Battle_Idle_Tex");
		}

		//
		{
			int minimap_number = 0;
			int room_number = 0;

			for(int y = 0; y < 5; y++)
			{
				for(int x = 0; x < 5; x++)
				{
					room_number = m_c_manager.Wild_Rooms_GetNowRoom().Wild_GetNumber() - 2 - (m_c_manager.Wild_Rooms_GetDungeonX() * 2);
					room_number = room_number + x + (m_c_manager.Wild_Rooms_GetDungeonX() * y);

					minimap_number = x + (y * 5);

					if((0 <= room_number) && (room_number < m_minimap_a_tile.Length))
					{
						Wild_Dungeon_Room tempRoom = m_c_manager.Wild_Dungeon_GetRoomNumber(room_number);
						if(tempRoom != null)
						{
							Texture tempTexture = null;
							switch(tempRoom.Wild_GetDungeonType())
							{
								case Wild_Dungeon_UI.Move:		tempTexture = Resources.Load<Texture>("UI/2. WorldMap/Background_tex");		break;
								case Wild_Dungeon_UI.Battle:	tempTexture = Resources.Load<Texture>("UI/1. MainMenu/Battle_Press_Tex");	break;
							}

							m_minimap_a_tile[minimap_number].GetComponent<Renderer>().material.mainTexture = tempTexture;
						}
					}

					minimap_number += 1;

					// room_number
					room_number = room_number + 1;
				}
			}
		}
	}

	/********** Default Method	**********/
	void Wild_Minimap_Init()
	{
		Vector2 screenSize = Wild_Singleton_Screen.Wild_GetInstance().Wild_GetScreenSize();

		//
		GameObject minimap_base = Object.Instantiate(Resources.Load<GameObject>("UI/background"));
		minimap_base.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("UI/2. WorldMap/Background_tex");
		minimap_base.transform.parent = m_basic.transform;
		minimap_base.transform.localScale = new Vector3(screenSize.y / 10.0f * 0.7f, 1.0f, screenSize.y / 10.0f * 0.7f);
		minimap_base.transform.localPosition = new Vector3(
			(screenSize.x * 0.5f) - (screenSize.x * 0.2f),
			-1.0f,
			(screenSize.y * 0.5f) - (screenSize.x * 0.2f));
		minimap_base.transform.Rotate(-90.0f, 0.0f, 0.0f);
		minimap_base.transform.Rotate(0.0f, 180.0f, 0.0f);
		//
		Transform minimap_start = minimap_base.transform.Find("0");
		Transform minimap_end = minimap_base.transform.Find("1");
		float lengthX = minimap_end.position.x - minimap_start.position.x;
		float lengthY = minimap_end.position.y - minimap_start.position.y;

		//
		m_minimap_a_tile = new GameObject[25];
		for(int i = m_minimap_a_tile.Length - 1; i >= 0; i--)
		{
			m_minimap_a_tile[i] = Object.Instantiate(Resources.Load<GameObject>("UI/3. Dungeon/MiniMap_Tile"));
			m_minimap_a_tile[i].transform.name = "" + i;
			m_minimap_a_tile[i].transform.parent = m_basic.transform;
			m_minimap_a_tile[i].transform.localScale = new Vector3(minimap_base.transform.localScale.x / 5.0f, 1.0f, minimap_base.transform.localScale.z / 5.0f);
			m_minimap_a_tile[i].transform.position = new Vector3(
				minimap_start.transform.position.x + (lengthX / 6.0f * (1 + (i % 5))),
				minimap_start.transform.position.y + lengthY - (lengthY / 6.0f * (1 + (i / 5))),
				m_basic.transform.position.z);
			m_minimap_a_tile[i].transform.Rotate(-90.0f, 0.0f, 0.0f);
			m_minimap_a_tile[i].transform.Rotate(0.0f, 180.0f, 0.0f);
		}
	}
	#endregion
}

#region btn class
class Wild_Dungeon_Move_Btn_LeftMove : Wild_UI_Btn
{
	Wild_Dungeon_Manager m_c_manager;

	/********** Getter & Setter	**********/
	public void Wild_SetClassManager(Wild_Dungeon_Manager _c_manager) { m_c_manager = _c_manager; }

	/********** Method	**********/
	public override void Wild_Click()
	{
		m_c_manager.Wild_Rooms_Move(-1);
	}

	/********** Default Method	**********/
	public override void Wild_Init(int _number, string _idle, string _onTouch, Transform _parent)
	{
		base.Wild_Init(_number, _idle, _onTouch, _parent);
		Wild_SetPosition(-100.0f, 0.0f);
	}
}

class Wild_Dungeon_Move_Btn_TopMove : Wild_UI_Btn
{
	Wild_Dungeon_Manager m_c_manager;

	/********** Getter & Setter	**********/
	public void Wild_SetClassManager(Wild_Dungeon_Manager _c_manager) { m_c_manager = _c_manager; }

	/********** Method	**********/
	public override void Wild_Click()
	{
		m_c_manager.Wild_Rooms_Move(m_c_manager.Wild_Rooms_GetDungeonX());
	}

	/********** Default Method	**********/
	public override void Wild_Init(int _number, string _idle, string _onTouch, Transform _parent)
	{
		base.Wild_Init(_number, _idle, _onTouch, _parent);
		Wild_SetPosition(0.0f, 100.0f);
	}
}

class Wild_Dungeon_Move_Btn_RightMove : Wild_UI_Btn
{
	Wild_Dungeon_Manager m_c_manager;

	/********** Getter & Setter	**********/
	public void Wild_SetClassManager(Wild_Dungeon_Manager _c_manager) { m_c_manager = _c_manager; }

	/********** Method	**********/
	public override void Wild_Click()
	{
		m_c_manager.Wild_Rooms_Move(1);
	}

	/********** Default Method	**********/
	public override void Wild_Init(int _number, string _idle, string _onTouch, Transform _parent)
	{
		base.Wild_Init(_number, _idle, _onTouch, _parent);
		Wild_SetPosition(100.0f, 0.0f);
	}
}

class Wild_Dungeon_Move_Btn_BottomMove : Wild_UI_Btn
{
	Wild_Dungeon_Manager m_c_manager;

	/********** Getter & Setter	**********/
	public void Wild_SetClassManager(Wild_Dungeon_Manager _c_manager) { m_c_manager = _c_manager; }

	/********** Method	**********/
	public override void Wild_Click()
	{
		m_c_manager.Wild_Rooms_Move(-m_c_manager.Wild_Rooms_GetDungeonX());
	}

	/********** Default Method	**********/
	public override void Wild_Init(int _number, string _idle, string _onTouch, Transform _parent)
	{
		base.Wild_Init(_number, _idle, _onTouch, _parent);
		Wild_SetPosition(0.0f, -100.0f);
	}
}

#endregion