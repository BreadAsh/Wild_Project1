using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Wild_MainMenu_UI
{
	Wild_MainMenu_UI_Main = 0
}

public class Wild_MainMenu_Manager : Wild_SceneManager
{
	/********** Default Method	**********/
	protected override void Wild_Init()
	{
		base.Wild_Init();

		Wild_MainMenu_Main c_main = new Wild_MainMenu_Main();
		c_main.Wild_Init(m_canvas);
		m_l_UIManager.Add(c_main);

		Wild_SetActive((int)Wild_MainMenu_UI.Wild_MainMenu_UI_Main);
	}

	protected override void Wild_Release()
	{
		base.Wild_Release();
	}

	protected override void Wild_Update()
	{
		base.Wild_Update();
	}
}
