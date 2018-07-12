using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Wild_MainMenu_Main : Wild_UI_Manager
{
	/********** Default Method	**********/
	public override void Wild_Init(GameObject _canvas)
	{
		base.Wild_Init(_canvas);

		m_enum = (int)Wild_MainMenu_UI.Wild_MainMenu_UI_Main;

		int btn_num = 0;
		Wild_MainMenu_Main_Btn_Battle battle_btn = new Wild_MainMenu_Main_Btn_Battle();
		battle_btn.Wild_Init(btn_num, "1. MainMenu/Battle_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		m_l_btn.Add(battle_btn);
	}
}

class Wild_MainMenu_Main_Btn_Battle : Wild_UI_Btn
{
	/********** Method	**********/
	public override void Wild_Click()
	{
		SceneManager.LoadScene("2. WorldMap");
		Debug.Log("Wild_UI_Btn_Battle");
	}
}