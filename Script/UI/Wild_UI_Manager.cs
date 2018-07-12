using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: UI를 관리하는 씬이 상속받는 클래스. 해당 씬의 UI는 전부 이곳에서 관리하세요.
public class Wild_UI_Manager
{
	protected int m_enum;

	protected GameObject m_basic;

	protected List<Wild_UI_Btn> m_l_btn;

	/********** Getter & Setter	**********/
	public int Wild_GetEnum() { return m_enum; }
	
	/********** Method	**********/
	// basic
	public void Wild_On() { m_basic.SetActive(true); }
	public void Wild_Off() { m_basic.SetActive(false); }

	// Btn
	public Wild_UI_Btn Wild_FindBtn(int _num)
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
	public virtual void Wild_Init(GameObject _canvas)
	{
		m_basic = Object.Instantiate(Resources.Load<GameObject>("Basic"));
		m_basic.transform.parent = _canvas.transform;
		m_basic.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		m_basic.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		m_basic.SetActive(false);

		m_l_btn = new List<Wild_UI_Btn>();
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
}
