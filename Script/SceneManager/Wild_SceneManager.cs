using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 씬매니저들이 공동으로 상속받는 클래스. 상속받은 클래스는 카메라에 넣어서 쓰세요~
public class Wild_SceneManager : MonoBehaviour
{
	public Camera m_UI_camera;
	public GameObject m_UICanvas;
	protected List<Wild_UI_Manager> m_l_UIManager;
	protected int m_selUICount;

	private static Touch_TYPE g_inputType;
	private static int g_inputCount;
	private static bool g_isInputOn;
	private static Vector3 g_beganPosition;

	/********** Getter & Setter	**********/
	public static Touch_TYPE Wild_S_GetInputType(){ return g_inputType; }

	/********** Method	**********/
	public bool Wild_ScreenInput(TouchPhase _phase)
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
		g_inputCount = -1;
		g_inputType = Touch_TYPE.NOT;
		g_isInputOn = false;
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

	public Vector3 Wild_TouchPosition()
	{
		Vector3 res = new Vector3(0, 0, 0);
		#if UNITY_EDITOR
		res = Input.mousePosition;
		#elif UNITY_IOS || UNITY_ANDROID
		res.x = Input.GetTouch(0).position.x;
		res.y = Input.GetTouch(0).position.y;
		res.z = 0;
		#endif

		return res;
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
			Wild_Touch_Area obj = Wild_Update_Raycast();
			if(obj != null)
			{
				Wild_Update_Began(obj);
			}
		}
		else if(Wild_ScreenInput(TouchPhase.Moved))
		{
			g_isInputOn = false;
			
			Wild_Touch_Area obj = Wild_Update_Raycast();
			if(obj != null)
			{
				Wild_Update_Moved(obj);
			}

			// 선택한 객체의 범위에서 벗어나면 해당 객체를 초기화
			if(!g_isInputOn)
			{
				switch(g_inputType)
				{
					case Touch_TYPE.BUTTON:
						m_l_UIManager[m_selUICount].Wild_Button_Find(g_inputCount).Wild_Tex_Idle();
						break;
				}
			}
		}
		else if(Wild_ScreenInput(TouchPhase.Ended))
		{
			if(g_isInputOn)
			{
				Wild_Update_Ended();
			}
			Wild_InputReset();
		}

		m_l_UIManager[m_selUICount].Wild_Update();
	}

	//
	protected virtual void Wild_Update_Began(Wild_Touch_Area _obj)
	{
		g_inputType = _obj.Wild_GetType();
		g_inputCount = _obj.Wild_GetNumber();
		if(g_inputType == Touch_TYPE.TILE)
		{
			Debug.Log("aa " + g_inputCount);
		}

		g_beganPosition = Wild_TouchPosition();
	}

	//
	protected virtual void Wild_Update_Moved(Wild_Touch_Area _obj)
	{
		switch(_obj.Wild_GetType())
		{
			case Touch_TYPE.BUTTON:
				if((g_inputType == Touch_TYPE.BUTTON) && (g_inputCount == _obj.Wild_GetNumber()))
				{
					g_isInputOn = true;
					m_l_UIManager[m_selUICount].Wild_Button_Find(g_inputCount).Wild_Tex_OnTouch();
				}
				break;
			case Touch_TYPE.TILE:
				{
					if((g_inputType == Touch_TYPE.TILE))
					{
						Wild_Update_Moved_RangeCheck();
					}
				}
				break;
		}
	}

	void Wild_Update_Moved_RangeCheck()
	{
		if(Vector3.Distance(g_beganPosition, Wild_TouchPosition()) > 10.0f)
		{
			Wild_InputReset();
		}
	}

	//
	protected virtual void Wild_Update_Ended()
	{
		switch(g_inputType)
		{
			case Touch_TYPE.BUTTON:
				m_l_UIManager[m_selUICount].Wild_Button_Find(g_inputCount).Wild_Tex_Idle();
				m_l_UIManager[m_selUICount].Wild_Button_Find(g_inputCount).Wild_Click();
				break;
		}
	}
	
	Wild_Touch_Area Wild_Update_Raycast()
	{
		Wild_Touch_Area res = null;
/*
		Ray ray;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		res = m_l_UIManager[m_selUICount].Wild_CheckTouch(ray.origin, ray.direction);
*/
		res = m_l_UIManager[m_selUICount].Wild_CheckTouch();
		return res;
	}

	#region Touch

	/********** Getter & Setter	**********/

	/********** Method	**********/

	/********** Default Method	**********/
	#endregion

	/********** Unity Method	**********/
	// Use this for initialization
	void Awake ()
	{
		Wild_Init();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Wild_Update();
	}
}
