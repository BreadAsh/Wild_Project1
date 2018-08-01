using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Touch_TYPE
{
	NOT,

	// UI
	BUTTON,

	// Object
	TILE,
}

public class Wild_Touch_Area
{
	protected Touch_TYPE m_type;
	protected int m_number;

	protected Transform[] m_a_vertex;


	/********** Getter & Setter	**********/
	public Touch_TYPE Wild_GetType() { return m_type; }
	public int Wild_GetNumber() { return m_number; }
	
	/********** Method	**********/
	public bool Wild_CheckTouch(Vector3 rayOrigin, Vector3 rayDir)
	{
		for(int i = 2; i < m_a_vertex.Length; i++)
		{
			Vector3 edge1 = m_a_vertex[i - 1].position - m_a_vertex[0].position;
			Vector3 edge2 = m_a_vertex[i].position - m_a_vertex[0].position;

			//
			Vector3 pvec = Vector3.Cross(rayDir, edge2);

			//
			float det = Vector3.Dot(edge1, pvec);

			Vector3 tvec;
			if(det > 0)
			{
				tvec = rayOrigin - m_a_vertex[0].position;
			}
			else
			{
				tvec = m_a_vertex[0].position - rayOrigin;
				det = -det;
			}

			if( det < 0.0001f)
				continue;

			//
			float u = Vector3.Dot(tvec, pvec);
			if(u < 0.0f || u > det)
				continue;

			//
			Vector3 qvec;
			qvec = Vector3.Cross(tvec, edge1);

			//
			float v = Vector3.Dot(rayDir, qvec);
			if(v < 0.0f || u + v > det)
				continue;

			return true;
		}

		return false;
	}
}
