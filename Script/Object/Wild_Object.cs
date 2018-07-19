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

	public GameObject Wild_GetModel() { return m_model; }

	/********** Method	**********/

	/********** Default Method	**********/
	public virtual void Wild_Init(GameObject _parent, string _str, Wild_Object_TYPE _type)
	{
		if(_parent != null)
		{
			m_model.transform.parent = _parent.transform;
			m_model.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			m_model.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		}
		if(_str != null)
		{
			GameObject model = Object.Instantiate(Resources.Load<GameObject>(_str));
		}
		m_type = _type;

		m_AStar_a_data = null;
	}

	public virtual void Wild_Release()
	{
		
	}

	public virtual void Wild_Update()
	{

	}

	#region AStar
    public enum Wild_AStar_CONDITION
    {
        NOSEARCH,
        MOVE,
        OBJECT,
        OWN,
        PLAYER,
        ALLAIS,
        ENEMY,
        NOTMOVE
    }

    public struct Wild_AStar_Data
    {
        public int m_distance;

        public Wild_AStar_CONDITION m_condition;
    }
    protected Wild_AStar_Data[] m_AStar_a_data;

	/********** Getter & Setter	**********/
	public int Wild_AStar_Distance(int _num) { return m_AStar_a_data[_num].m_distance; }

	/********** Method	**********/

	/********** Default Method	**********/
	#endregion
}
