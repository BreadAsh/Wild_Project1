using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:화면에 무언가를 배치할 때에는 이것을 사용하도록!
public class Wild_Singleton_Screen
{
	// Singleton
	static Wild_Singleton_Screen m_instance = null;

	static public Wild_Singleton_Screen Wild_GetInstance()
	{
		if(m_instance == null)
		{
			m_instance = new Wild_Singleton_Screen();
			m_instance.SetScreenSize();
		}

		return m_instance;
	}

	//////////////////////////////////////////////////
	// class
	Vector2 m_screenSize;

	/********** Getter & Setter	**********/
	public Vector2 Wild_GetScreenSize()
	{
		return m_screenSize;
	}

	/**********	Method	**********/

	// 화면의 종횡비를 계산하여 게임에 최적화된 종횡비를 얻어낸다.
	void SetScreenSize()
	{
		float width = Screen.width;
		float height = Screen.height;
		float screenAspect = width / height;

		float fixedAspect = 19.0f / 10.0f;

		// 화면의 가로비율이 더 길 때
		if(screenAspect > fixedAspect)
		{
			m_screenSize.x = height / 10.0f * 19.0f;
			m_screenSize.y = height;
		}

		// 화면의 세로비율이 더 길 때
		else if(screenAspect < fixedAspect)
		{
			m_screenSize.x = width;
			m_screenSize.y = width / 19.0f * 10.0f;
		}
	}
}
