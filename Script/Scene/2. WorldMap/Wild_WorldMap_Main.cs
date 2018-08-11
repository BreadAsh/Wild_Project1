using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Wild_WorldMap_Main : Wild_UI_Manager
{
	/********** Default Method	**********/
	public override void Wild_Init(Camera _camera, GameObject _canvas)
	{
		base.Wild_Init(_camera, _canvas);

		m_enum = (int)Wild_WorldMap_UI.Wild_WorldMap_UI_Main;


		GameObject m_background = Object.Instantiate(Resources.Load<GameObject>("UI/Background"));
		m_background.transform.parent = _canvas.transform;
		m_background.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
		m_background.transform.Rotate(90.0f, 180.0f, 0.0f);
		Vector2 scale = Wild_Singleton_Screen.Wild_GetInstance().Wild_GetScreenSize();
		m_background.transform.localScale = new Vector3(scale.x / 10.0f, 1.0f, scale.y / 10.0f);
		m_background.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("UI/2. WorldMap/Background_tex");

		// btn
		m_basic.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

		int btn_num = 0;
		Wild_WorldMap_Btn_Battle dungeon_btn = new Wild_WorldMap_Btn_Battle();
		dungeon_btn.Wild_Init(btn_num, "2. WorldMap/Dungeon_Idle_Tex", "1. MainMenu/Battle_Press_Tex", m_basic.transform);
		Wild_Button_Add(dungeon_btn);
	}
}

class Wild_WorldMap_Btn_Battle : Wild_UI_Btn
{
	/********** Method	**********/
	public override void Wild_Click()
	{
		SceneManager.LoadScene("3. Dungeon");
		Debug.Log("Wild_UI_Btn_Battle");
	}
}