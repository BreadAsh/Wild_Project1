using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Wild_Dungeon_Room_MOVE
{
	LEFT = 0,
	TOP = 1,
	RIGHT = 2,
	BOTTOM = 3
}

public class Wild_Dungeon_Room
{
	int m_number;
	int m_x;
	int m_y;
	Wild_Dungeon_UI m_type;

	GameObject m_basic;

	//
	bool m_isFind;
	bool[] m_a_isMove;
	List<Wild_Character> m_l_enemy;

	/********** Getter & Setter	**********/
	public int Wild_GetNumber() { return m_number; }

	public int Wild_GetX() { return m_x; }

	public int Wild_GetY() { return m_y; }

	public Wild_Dungeon_UI Wild_GetDungeonType() { return m_type; }

	public bool[] Wild_GetMoving() { return m_a_isMove; }

	// m_l_enemy
	public Wild_Character Wild_GetEnemy(int _num) { return m_l_enemy[_num]; }
	
	public int Wild_GetEnemyCount() { return m_l_enemy.Count; }

	/********** Method	**********/
	public void Wild_On() { m_basic.SetActive(true); }
	public void Wild_Off() { m_basic.SetActive(false); }

	public void Wild_EnemyRemoveAt(int _num)
	{
		m_l_enemy.RemoveAt(_num);
	}

	/********** Default Method	**********/
	// Wild_Init
	public void Wild_Init(Wild_Dungeon_Manager _c_manager, int _dugeonX, int _number, string _str)
	{
		m_basic = Object.Instantiate(Resources.Load<GameObject>("Basic"));
		m_basic.name = "room_" + _number;
		m_basic.transform.parent = _c_manager.Wild_Tile_GetBasic().transform;
		m_basic.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		m_basic.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		m_basic.SetActive(false);

		//
		m_number = _number;
		m_x = _number % _dugeonX;
		m_y = _number / _dugeonX;

		m_isFind = false;

		string[] strs = _str.Split('!');
		//
		switch(strs[0])
		{
			case "MOVE":	m_type = Wild_Dungeon_UI.Move;		break;
			case "BATTLE":	m_type = Wild_Dungeon_UI.Battle;	break;
		}
		Wild_Init_Move(strs[2]);
		Wild_Init_Monster(_c_manager, strs[3]);
		/*
		Wild_Init_Item(strs[5]);
		Wild_Init_Map(strs[7]);
		*/
	}

	void Wild_Init_Move(string _str)
	{
		string[] strs_move = _str.Split(',');
		m_a_isMove = new bool[4];
		for(int i = 0; i < m_a_isMove.Length; i++)
		{
			if(strs_move[i].Equals("1"))
				m_a_isMove[i] = true;
			else if(strs_move[i].Equals("0"))
				m_a_isMove[i] = false;
		}
	}

	void Wild_Init_Monster(Wild_Dungeon_Manager _c_manager, string _str)
	{
		m_l_enemy = new List<Wild_Character>();

		if(!_str.Equals("NONE"))
		{
			string[] strs_monster = _str.Split('/');
			
			for(int i = 0; i < strs_monster.Length; i++)
			{
				Wild_Character enemy = new Wild_Character();
				enemy.Wild_InitAnother(_c_manager);
				enemy.Wild_Init(m_basic, strs_monster[i], Wild_Object_TYPE.ENEMY );
				m_l_enemy.Add(enemy);
			}
		}
	}

	void Wild_Init_Item(string _str)
	{
	}

	void Wild_Init_Map(string _str)
	{
	}

	// Wild_Release
	public void Wild_Release()
	{
		
	}

	public void Wild_Update()
	{
		
	}
}
