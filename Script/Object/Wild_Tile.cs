using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wild_Tile_ATTRIBUTE
{
	NOT,
	DEFAULT,
	GRASS,

}

public class Wild_Tile : Wild_Touch_Area
{
	Wild_Object m_object;

	//
	GameObject m_model;
	Transform m_texture;

	int m_x;							// 타일의 X위치
	int m_y;							// 타일의 Y위치
	Wild_Tile_ATTRIBUTE m_attribute;	// 타일의 속성

	/********** Getter & Setter	**********/
	public Wild_Object Wild_GetObject() { return m_object; }
	public void Wild_SetObject(Wild_Object _object) { m_object = _object; }

	public Vector3 Wild_GetPosition() { return m_model.transform.position; }

	//
	public Wild_Tile_ATTRIBUTE Wild_GetAttribute() { return m_attribute; }

	public int Wild_GetX() { return m_x; }

	public int Wild_GetY() { return m_y; }

	/********** Method	**********/
	public void Wild_ChangeType(Wild_Tile_ATTRIBUTE _attribute)
	{
		m_attribute = _attribute;

		string path = "";
		switch(m_attribute)
		{
			case Wild_Tile_ATTRIBUTE.DEFAULT:	path = "Tile/hex_not_tex";		break;
			case Wild_Tile_ATTRIBUTE.GRASS:		path = "Tile/hex_grass_tex";	break;
		}
		m_texture.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(path);
	}

	public bool Wild_IsMove()
	{
		bool res = true;
		if(m_attribute == Wild_Tile_ATTRIBUTE.NOT)
			res = false;

		return res;
	}

	/********** Default Method	**********/
	public void Wild_Init(int _number, int _x, int _y, Wild_Tile_ATTRIBUTE _attribute, GameObject _parent)
	{
		m_type = Touch_TYPE.TILE;
		m_number = _number;
		m_x = _x;
		m_y = _y;

		// 타일속성 셋팅
		m_model = Object.Instantiate(Resources.Load<GameObject>("Tile/tile2"));
		m_a_vertex = new Transform[6];
		m_a_vertex[0] = m_model.transform.Find("0");
		m_a_vertex[1] = m_model.transform.Find("1");
		m_a_vertex[2] = m_model.transform.Find("2");
		m_a_vertex[3] = m_model.transform.Find("3");
		m_a_vertex[4] = m_model.transform.Find("4");
		m_a_vertex[5] = m_model.transform.Find("5");
		m_model.name = "hex_" + _number;
		m_model.transform.parent = _parent.transform;
		m_model.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		m_model.transform.Rotate(0.0f, 0.0f, 0.0f);
		m_texture = m_model.transform.Find("texture").transform;
		// 위치
		Vector3 pos = new Vector3(m_x * 7.0f, 0.0f, m_y * 6.1f);
        if(m_y % 2 == 1) { pos.x += 3.5f; }
		m_model.transform.localPosition = pos;

		Wild_ChangeType(_attribute);
	}
}
