using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wild_Object_TYPE
{
	OBJECT,
	CHARACTER,
	ALLIY,
	ENEMY
}

public class Wild_Object
{
	protected Wild_Object_TYPE m_type;
	protected int m_number;

	protected GameObject m_model;

	protected int m_hp;

	/********** Getter & Setter	**********/
	public Wild_Object_TYPE Wild_GetType() { return m_type; }

	public int Wild_GetNumber() { return m_number; }

	/********** Method	**********/

	/********** Default Method	**********/
	public virtual void Wild_Init(string _str)
	{

	}

	public virtual void Wild_Release()
	{
		
	}

	public virtual void Wild_Update()
	{

	}
}
