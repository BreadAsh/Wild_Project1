using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Wild_Dungeon_Battle_TURN
{
	TURN_START,
	PHASE_SETTING,
	PHASE_ACTIVE,
	BATTLE_END
}

public class Wild_Dungeon_Battle : Wild_UI_Manager
{
	Wild_Dungeon_Manager m_c_manager;

	Wild_Dungeon_Room m_room;

	int m_turn;
	int m_phase;
	Wild_Dungeon_Battle_TURN m_turnPhase;
	List<Wild_Character> m_l_phase_character;

	// 템프 셋팅용
	List<Wild_Character> m_l_tempList;

	/********** Getter & Setter	**********/

	/********** Method	**********/
	public void Wild_InitSetting( Wild_Dungeon_Manager _c_manager )
	{
		m_c_manager = _c_manager;
	}

	public void Wild_GameStart( Wild_Dungeon_Room _room )
	{
		m_room = _room;

		m_turn = 0;

		m_phase = 0;

		Wild_Update_SettingTempList();
		for(int i = 0; i < m_l_tempList.Count; i++)
		{
			m_l_tempList[i].Wild_Active_BattleStart();
		}

		m_turnPhase = Wild_Dungeon_Battle_TURN.TURN_START;
	}

	/********** Default Method	**********/
	public override void Wild_Init(GameObject _canvas)
	{
		base.Wild_Init(_canvas);

		m_enum = (int)Wild_Dungeon_UI.Battle;
		m_l_phase_character = new List<Wild_Character>();

		m_l_tempList = new List<Wild_Character>();
    }

	//////////
	public override void Wild_Update()
	{
		switch(m_turnPhase)
		{
			case Wild_Dungeon_Battle_TURN.TURN_START:		Wild_Update_TurnStart();	break;
			case Wild_Dungeon_Battle_TURN.PHASE_SETTING:	Wild_Update_PhaseSetting();	break;
			case Wild_Dungeon_Battle_TURN.PHASE_ACTIVE:		Wild_Update_PhaseActive();	break;
			case Wild_Dungeon_Battle_TURN.BATTLE_END:		Wild_Update_BattleEnd();	break;
		}
	}

	// 임시 리스트 셋팅
	void Wild_Update_SettingTempList()
	{
		m_l_tempList.Clear();

		// 플레이어 캐릭터 셋팅
		for(int i = 0; i < m_c_manager.Wild_Player_GetCharacterCount(); i++)
		{
			m_l_tempList.Add(m_c_manager.Wild_Player_GetCharacter(i));
		}

		// 적 캐릭터 셋팅
		//for(int i = 0; i < m_room.Wild_GetEnemyCount(); i++)
		{
			//m_l_tempList.Add(m_room.Wild_GetEnemy(i));
		}
	}

	// 턴시작
	void Wild_Update_TurnStart()
	{
		m_turn++;
		Wild_Update_SettingTempList();
		for(int i = 0; i < m_l_tempList.Count; i++)
		{
			m_l_tempList[i].Wild_Active_TurnSetting();
		}

		m_turnPhase = Wild_Dungeon_Battle_TURN.PHASE_SETTING;
	}

	// 페이즈 셋팅
	void Wild_Update_PhaseSetting()
	{
		m_phase++;

		//
		Wild_Update_SettingTempList();

		// 점수가 모자란 애들 빼기
		{
			int i = 0;
			while(i < m_l_tempList.Count)
			{
				if( m_l_tempList[i].Wild_Active_GetActivePoint() < 10 )
				{
					m_l_tempList.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		if(m_l_tempList.Count == 0)
		{
			m_turnPhase = Wild_Dungeon_Battle_TURN.TURN_START;
		}
		else
		{
			m_l_phase_character.Clear();

			// 페이즈에 셋팅
			while(m_l_tempList.Count > 0)
			{
				int tempCount = 0;
				int tempActiveNumber = 0;

				for( int i = 0; i < m_l_tempList.Count; i++)
				{
					if( m_l_tempList[i].Wild_Active_GetActivePoint() > tempActiveNumber )
					{
						tempCount = i;
						tempActiveNumber = m_l_tempList[i].Wild_Active_GetActivePoint();
					}
				}

				m_l_phase_character.Add(m_l_tempList[tempCount]);
				m_l_tempList.RemoveAt(tempCount);
			}

			m_turnPhase = Wild_Dungeon_Battle_TURN.PHASE_ACTIVE;
		}
	}

	// 캐릭터 행동개시
	void Wild_Update_PhaseActive()
	{
		if(m_l_phase_character[0].Wild_Active_GetIsEnd())
		{
			m_l_phase_character.RemoveAt(0);

			
			if(m_l_phase_character.Count == 0)
			{
				m_turnPhase = Wild_Dungeon_Battle_TURN.TURN_START;
			}
			else if(m_c_manager.Wild_Player_GetCharacterCount() == 0 || m_room.Wild_GetEnemyCount() == 0)
			{
				m_turnPhase = Wild_Dungeon_Battle_TURN.BATTLE_END;
			}
		}
	}

	// 턴종료
	void Wild_Update_BattleEnd()
	{
		m_c_manager.Wild_SetActive((int)Wild_Dungeon_UI.Move);
	}
}
