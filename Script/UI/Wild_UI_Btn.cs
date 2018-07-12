using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 버튼이 상속받는 클래스. 추가로 정의되어야하는 변수는 해당 클래스에서 정의하세요
public class Wild_UI_Btn
{
	int m_number;

	// 그래픽
	GameObject m_model;
	Renderer m_renderer;

    Texture m_imgIdle;
    Texture m_imgOnTouch;

	/********** Getter & Setter	**********/
	public int Wild_GetNumber() { return m_number; }
	
	/********** Method	**********/
	public virtual void Wild_Click() { }

	public void Wild_On() { m_model.SetActive(true); }
	public void Wild_Off() { m_model.SetActive(false); }

	// GUI
	protected void Wild_SetPosition(float _x, float _y)
	{
		m_model.transform.localPosition = new Vector3(_x, _y, 0.0f);
	}
	
	public void Wild_Tex_Idle()
	{
		m_renderer.material.mainTexture = m_imgIdle;
	}

	public void Wild_Tex_OnTouch()
	{
		m_renderer.material.mainTexture = m_imgOnTouch;
	}

	/********** Default Method	**********/
	public virtual void Wild_Init(int _number, string _idle, string _onTouch, Transform _parent)
	{
		m_number = _number;

		// obj
		m_model = Object.Instantiate(Resources.Load<GameObject>("UI/Button"));
		m_model.name = "" + _number;
		m_model.transform.parent = _parent;
		m_model.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		m_model.transform.localScale = new Vector3(10.0f, 1.0f, 5.0f);
		m_model.transform.Rotate(0.0f, 180.0f, 0.0f);

		// tex
		m_renderer = m_model.GetComponent<Renderer>();

		m_imgIdle = Resources.Load<Texture>("UI/" + _idle);
		m_imgOnTouch = Resources.Load<Texture>("UI/" + _onTouch);

		m_renderer.material.mainTexture = m_imgIdle;
	}

	public void Wild_Release()
	{
		// 그래픽
		if(m_model != null)	MonoBehaviour.Destroy(m_model);

		if(m_imgIdle != null)	 MonoBehaviour.Destroy(m_imgIdle);
		if(m_imgOnTouch != null) MonoBehaviour.Destroy(m_imgOnTouch);
	}
}
