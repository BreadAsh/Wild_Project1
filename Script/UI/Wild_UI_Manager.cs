using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: UI를 관리하는 씬이 상속받는 클래스. 해당 씬의 UI는 전부 이곳에서 관리하세요.
public class Wild_UI_Manager
{
	protected int m_enum;

	protected GameObject m_basic;

	protected Camera m_camera;

	protected List<Wild_Touch_Area> m_l_touchArea;

	/********** Getter & Setter	**********/
	public int Wild_GetEnum() { return m_enum; }
	
	/********** Method	**********/
	// basic
	public void Wild_On() { m_basic.SetActive(true); }
	public void Wild_Off() { m_basic.SetActive(false); }

	public Wild_Touch_Area Wild_CheckTouch(Vector3 rayOrigin, Vector3 rayDir)
	{
		Wild_Touch_Area res = null;
		for(int i = 0; i < m_l_touchArea.Count; i++)
		{
			if(m_l_touchArea[i].Wild_CheckTouch(rayOrigin, rayDir))
			{
				res = m_l_touchArea[i];
				break;
			}
		}

		return res;
	}

	public Wild_Touch_Area Wild_CheckTouch()
	{
		Ray ray;
		ray = m_camera.ScreenPointToRay(Input.mousePosition);

		Wild_Touch_Area res = null;
		for(int i = 0; i < m_l_touchArea.Count; i++)
		{
			if(m_l_touchArea[i].Wild_CheckTouch(ray.origin, ray.direction))
			{
				res = m_l_touchArea[i];
				break;
			}
		}

		return res;
	}

	/********** Default Method	**********/
	public virtual void Wild_Init(Camera _camera, GameObject _canvas)
	{
		m_camera = _camera;

		m_basic = Object.Instantiate(Resources.Load<GameObject>("Basic"));
		m_basic.transform.parent = _canvas.transform;
		m_basic.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		m_basic.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		m_basic.transform.Rotate(-90.0f, 0.0f, 0.0f);
		m_basic.SetActive(false);

		Wild_Button_Init();
		m_l_touchArea = new List<Wild_Touch_Area>();
	}

	public virtual void Wild_Release()
	{
		while(m_l_btn.Count > 0)
		{
			m_l_btn[0].Wild_Release();
			m_l_btn[0] = null;

			m_l_btn.RemoveAt(0);
		}

		m_l_btn = null;
	}

	public virtual void Wild_Update()
	{
	}

	//////////////////////////////////////////////////
	#region Button

	protected List<Wild_UI_Btn> m_l_btn;
	
	/********** Method	**********/
	public void Wild_Button_Add(Wild_UI_Btn _btn)
	{
		m_l_btn.Add(_btn);
		m_l_touchArea.Add(_btn);
	}

	public Wild_UI_Btn Wild_Button_Find(int _num)
	{
		Wild_UI_Btn res = null;

		if(m_l_btn[_num].Wild_GetNumber().Equals(_num))
		{
			res = m_l_btn[_num];
		}
		else
		{
			for(int i = 0; i < m_l_btn.Count; i++)
			{
				if(m_l_btn[i].Wild_GetNumber().Equals(_num))
				{
					res = m_l_btn[i];
					break;
				}
			}
		}

		return res;
	}

	/********** Default Method	**********/
	void Wild_Button_Init()
	{
		m_l_btn = new List<Wild_UI_Btn>();
	}
	#endregion
}
