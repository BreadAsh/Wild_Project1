using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using System.IO;
using LitJson;

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
	public bool Wild_OnEnemyList()
	{
		bool res = false;
		if(m_l_enemy != null)
			res = true;
			
		return res;
	}

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
	public void Wild_Init(Wild_Dungeon_Manager _c_manager, int _dugeonX, JsonData _dungeonData)
	{
		m_number = int.Parse(_dungeonData["roomNumber"].ToString());

		m_basic = Object.Instantiate(Resources.Load<GameObject>("Basic"));
		m_basic.name = "room_" + m_number;
		m_basic.transform.parent = _c_manager.Wild_Tile_GetBasic().transform;
		m_basic.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		m_basic.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		m_basic.SetActive(false);

		//
		m_x = m_number % _dugeonX;
		m_y = m_number / _dugeonX;

		m_isFind = false;

		switch(_dungeonData["roomType"].ToString())
		{
			case "MOVE":	m_type = Wild_Dungeon_UI.Move;		break;
			case "BATTLE":	m_type = Wild_Dungeon_UI.Battle;	break;
		}
		Wild_Init_Move(_dungeonData);
		Wild_Init_Monster(_c_manager, _dungeonData["monsterList"]);
		/*
		Wild_Init_Item(strs[5]);
		Wild_Init_Map(strs[7]);
		*/
	}

	void Wild_Init_Move(JsonData _dungeonData)
	{
		m_a_isMove = new bool[4];
		for(int i = 0; i < m_a_isMove.Length; i++)
		{
			if(_dungeonData["move" + i].ToString().Equals("1"))
				m_a_isMove[i] = true;
			else if(_dungeonData["move" + i].ToString().Equals("0"))
				m_a_isMove[i] = false;
		}
	}

	void Wild_Init_Monster(Wild_Dungeon_Manager _c_manager, JsonData _dungeonData)
	{
		if(_dungeonData.Count > 0)
		{
			m_l_enemy = new List<Wild_Character>();
			for(int i = 0; i < _dungeonData.Count; i++)
			{
				string str
					= _dungeonData[i]["Name"].ToString()
					+ "/" + _dungeonData[i]["Model"].ToString()
					+ "/" + _dungeonData[i]["Level"].ToString()
					+ "/" + _dungeonData[i]["Exp"].ToString()
					+ "/" + _dungeonData[i]["RightHand"].ToString()
					+ "/" + _dungeonData[i]["LeftHand"].ToString()
					+ "/" + _dungeonData[i]["Head"].ToString()
					+ "/" + _dungeonData[i]["Armor"].ToString()
					+ "/" + _dungeonData[i]["Gloves"].ToString()
					+ "/" + _dungeonData[i]["Boots"].ToString()
					+ "/" + _dungeonData[i]["Position"].ToString();
				Wild_Character enemy = new Wild_Character();
				enemy.Wild_InitAnother(_c_manager);
				enemy.Wild_Init(m_basic, str, Wild_Object_TYPE.ENEMY );
				m_l_enemy.Add(enemy);
			}
		}
		else
		{
			m_l_enemy = null;
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
