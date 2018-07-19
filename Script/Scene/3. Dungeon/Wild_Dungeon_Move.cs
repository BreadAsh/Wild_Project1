using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild_Dungeon_Move : Wild_UI_Manager
{
	/********** Method	**********/
	public void Wild_Setting(Wild_Dungeon_Manager _c_manager)
	{
		int btn_num = 0;
		Wild_Dungeon_Move_Btn_LeftMove leftMove_btn = new Wild_Dungeon_Move_Btn_LeftMove();
		leftMove_btn.Wild_Init(btn_num, "2. WorldMap/Dungeon_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		leftMove_btn.Wild_SetClassManager(_c_manager);
		m_l_btn.Add(leftMove_btn);

		btn_num++;
		Wild_Dungeon_Move_Btn_TopMove topMove_btn = new Wild_Dungeon_Move_Btn_TopMove();
		topMove_btn.Wild_Init(btn_num, "2. WorldMap/Dungeon_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		topMove_btn.Wild_SetClassManager(_c_manager);
		m_l_btn.Add(topMove_btn);

		btn_num++;
		Wild_Dungeon_Move_Btn_RightMove rightMove_btn = new Wild_Dungeon_Move_Btn_RightMove();
		rightMove_btn.Wild_Init(btn_num, "2. WorldMap/Dungeon_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		rightMove_btn.Wild_SetClassManager(_c_manager);
		m_l_btn.Add(rightMove_btn);

		btn_num++;
		Wild_Dungeon_Move_Btn_BottomMove bottomMove_btn = new Wild_Dungeon_Move_Btn_BottomMove();
		bottomMove_btn.Wild_Init(btn_num, "2. WorldMap/Dungeon_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		bottomMove_btn.Wild_SetClassManager(_c_manager);
		m_l_btn.Add(bottomMove_btn);
	}

	public void Wild_SettingBtnActive(bool[] _a_isMove)
	{
		for(int i = 0; i < _a_isMove.Length; i++)
		{
			if(_a_isMove[i])
				Wild_FindBtn(i).Wild_On();
			else
				Wild_FindBtn(i).Wild_Off();
		}
	}

	/********** Default Method	**********/
	public override void Wild_Init(GameObject _canvas)
	{
		base.Wild_Init(_canvas);

		Wild_Minimap_Init();

		m_enum = (int)Wild_Dungeon_UI.Move;
	}

	#region mini map
	GameObject[] m_minimap_a_tile;
	/********** Getter & Setter	**********/

	/********** Method	**********/

	/********** Default Method	**********/
	void Wild_Minimap_Init()
	{
		m_minimap_a_tile = new GameObject[25];
		for(int i = m_minimap_a_tile.Length - 1; i >= 0; i--)
		{
			m_minimap_a_tile[i] = Object.Instantiate(Resources.Load<GameObject>("UI/3. Dungeon/MiniMap_Tile"));
			m_minimap_a_tile[i].transform.parent = m_basic.transform;
			m_minimap_a_tile[i].transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
			m_minimap_a_tile[i].transform.Rotate(-90.0f, 0.0f, 0.0f);
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