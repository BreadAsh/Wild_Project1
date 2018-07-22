using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wild_Item_TYPE
{
	Sword,
	Helm,
	Armor,
	Gloves,
	Boots,
	Potion
}

public class Wild_Item
{
	protected Wild_Item_TYPE m_type;

	protected GameObject m_model;

	protected int m_mainStatus;

	/********** Getter & Setter	**********/
	public Wild_Item_TYPE Wild_GetType() { return m_type; }

	public GameObject Wild_GetModel() { return m_model; }

	/********** Method	**********/

	/********** Default Method	**********/
	public virtual string[] Wild_Init(string _str)
	{
		string[] res = null;

		return res;
	}
}
