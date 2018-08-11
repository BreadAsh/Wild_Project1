using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Wild_WorldMap_UI
{
	Wild_WorldMap_UI_Main = 0
}

public class Wild_WorldMap_Manager : Wild_SceneManager
{
	/********** Default Method	**********/
	protected override void Wild_Init()
	{
		base.Wild_Init();

		Wild_WorldMap_Main c_main = new Wild_WorldMap_Main();
		c_main.Wild_Init(m_UI_camera, m_UICanvas);
		m_l_UIManager.Add(c_main);

		Wild_SetActive((int)Wild_WorldMap_UI.Wild_WorldMap_UI_Main);
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
