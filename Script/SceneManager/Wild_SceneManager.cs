using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Touch_TYPE
{
	Touch_TYPE_Not,

	// UI
	Touch_TYPE_Button,

	// Object
	Touch_TYPE_TILE,
}

// TODO: 씬매니저들이 공동으로 상속받는 클래스. 상속받은 클래스는 카메라에 넣어서 쓰세요~
public class Wild_SceneManager : MonoBehaviour
{
	public GameObject m_canvas;
	protected List<Wild_UI_Manager> m_l_UIManager;
	protected int m_selUICount;

	static Touch_TYPE m_inputType;
	static int m_inputCount;
	static bool m_isInputOn;

	/********** Method	**********/
	bool Wild_ScreenInput(TouchPhase _phase)
	{
		bool res = false;

		#if UNITY_EDITOR
		switch(_phase)
		{
			case TouchPhase.Began:	res = Input.GetMouseButtonDown(0);	break;
			case TouchPhase.Moved:	res = Input.GetMouseButton(0);		break;
			case TouchPhase.Ended:	res = Input.GetMouseButtonUp(0);	break;
		}
		#elif UNITY_IOS || UNITY_ANDROID
		res = (Input.GetTouch(0) == _phase);
		#endif

		return res;
	}

	void Wild_InputReset()
	{
		m_inputCount = -1;
		m_isInputOn = false;
	}

	//
	public void Wild_SetActive(int _selUINumber)
	{
		// 기존 UI비활성화
		if(m_selUICount != -1)
		{
			m_l_UIManager[m_selUICount].Wild_Off();
		}

		// 새로운 UI찾아서 활성화
		if(	((0 <= _selUINumber) && (_selUINumber < m_l_UIManager.Count)) &&
			m_l_UIManager[_selUINumber].Wild_GetEnum().Equals(_selUINumber))
		{
			m_selUICount = _selUINumber;
		}
		else
		{
			for(int i = 0; i < m_l_UIManager.Count; i++)
			{
				if(m_l_UIManager[i].Wild_GetEnum().Equals(_selUINumber))
				{
					m_selUICount = i;
					break;
				}
			}
		}

		m_l_UIManager[m_selUICount].Wild_On();
	}

	/********** Default Method	**********/
	protected virtual void Wild_Init()
	{
		m_l_UIManager = new List<Wild_UI_Manager>();
		m_selUICount = -1;

		Wild_InputReset();
	}

	protected virtual void Wild_Release(){}

	// Wild_Update
	protected virtual void Wild_Update()
	{
		if(Wild_ScreenInput(TouchPhase.Began))
		{
			GameObject obj = Wild_Update_Raycast();
			if(obj != null)
			{
				Wild_Update_Began(obj);
			}
		}
		else if(Wild_ScreenInput(TouchPhase.Moved))
		{
			m_isInputOn = false;
			
			GameObject obj = Wild_Update_Raycast();
			if((obj != null) && obj.tag.Equals("UIButton"))
			{
				Wild_Update_Moved(obj);
			}

			if(!m_isInputOn)
			{
				switch(m_inputType)
				{
					case Touch_TYPE.Touch_TYPE_Button:
						m_l_UIManager[m_selUICount].Wild_FindBtn(m_inputCount).Wild_Tex_Idle();
						break;
				}
			}
		}
		else if(Wild_ScreenInput(TouchPhase.Ended))
		{
			if(m_isInputOn)
			{
				Wild_Update_Ended();
			}
			Wild_InputReset();
		}

		m_l_UIManager[m_selUICount].Wild_Update();
	}
	protected virtual void Wild_Update_Began(GameObject _obj)
	{
		if(_obj.tag.Equals("UIButton"))
		{
			m_inputType = Touch_TYPE.Touch_TYPE_Button;

			m_inputCount = int.Parse(_obj.name);
		}
	}

	protected virtual void Wild_Update_Moved(GameObject _obj)
	{
		if(_obj.tag.Equals("UIButton"))
		{
			if((m_inputType == Touch_TYPE.Touch_TYPE_Button) && (m_inputCount == int.Parse(_obj.name)))
			{
				m_isInputOn = true;
				m_l_UIManager[m_selUICount].Wild_FindBtn(m_inputCount).Wild_Tex_OnTouch();
			}
		}
	}

	protected virtual void Wild_Update_Ended()
	{
		switch(m_inputType)
		{
			case Touch_TYPE.Touch_TYPE_Button:
				m_l_UIManager[m_selUICount].Wild_FindBtn(m_inputCount).Wild_Tex_Idle();
				m_l_UIManager[m_selUICount].Wild_FindBtn(m_inputCount).Wild_Click();
				break;
		}
	}

	GameObject Wild_Update_Raycast()
	{
		GameObject res = null;

		Ray ray;
		RaycastHit hitInfo;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo))
		{
			res = hitInfo.transform.gameObject;
		}

		return res;
	}

	/********** Unity Method	**********/
	// Use this for initialization
	void Start ()
	{
		Wild_Init();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Wild_Update();
	}
}
