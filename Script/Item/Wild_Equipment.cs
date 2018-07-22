using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Option
public enum Wild_Equipment_Option_TYPE
{
}

public struct Wild_Equipment_Option
{
	Wild_Equipment_Option_TYPE m_type;
	int m_status;
}

#endregion

public class Wild_Equipment : Wild_Item
{
	protected List<Wild_Equipment_Option> m_l_options;

	/********** Getter & Setter	**********/
	public Wild_Equipment_Option Wild_GetOption(int _num) { return m_l_options[_num]; }

	public int Wild_GetOptionListCount() { return m_l_options.Count; }

	/********** Method	**********/

	/********** Default Method	**********/
	public override string[] Wild_Init(string _str)
	{
		string[] res = base.Wild_Init(_str);

		m_l_options = new List<Wild_Equipment_Option>();

		return res;
	}
}

public class Wild_Weapon : Wild_Equipment
{
	int m_range;

	/********** Getter & Setter	**********/
	public int Wild_GetRange() { return m_range; }

	/********** Method	**********/

	/********** Default Method	**********/
	public override string[] Wild_Init(string _str)
	{
		string[] res = base.Wild_Init(_str);

		return null;
	}
}