﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Wild_Tile_TYPE
{
	Wild_Tile_TYPE_NOT,
	Wild_Tile_TYPE_DEFAULT,
	Wild_Tile_TYPE_GRASS,

}

public class Wild_Tile
{
	Wild_Object m_object;

	//
	Transform m_texture;

	int m_number;	// 타일의 번호
	int m_x;		// 타일의 X위치
	int m_y;		// 타일의 Y위치
	int m_type;		// 타일의 속성

	/********** Getter & Setter	**********/
	public Wild_Object Wild_GetObject() { return m_object; }
	public void Wild_SetObject(Wild_Object _object) { m_object = _object; }

	//
	public int Wild_GetNumber() { return m_number; }

	public int Wild_GetX() { return m_x; }

	public int Wild_GetY() { return m_y; }

	public int Wild_GetType() { return m_type; }

	/********** Method	**********/
	public void Wild_ChangeType(int _type)
	{
		m_type = _type;

		string path = "";
		switch(m_type)
		{
			case (int)Wild_Tile_TYPE.Wild_Tile_TYPE_DEFAULT:	path = "Tile/hex_not_tex";		break;
			case (int)Wild_Tile_TYPE.Wild_Tile_TYPE_GRASS:		path = "Tile/hex_grass_tex";	break;
		}
		m_texture.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(path);
	}

	public bool Wild_IsMove()
	{
		bool res = true;
		if(m_type == (int)Wild_Tile_TYPE.Wild_Tile_TYPE_NOT)
			res = false;

		return res;
	}

	/********** Default Method	**********/
	public void Wild_Init(int _number, int _x, int _y, int _type, GameObject _parent)
	{
		m_number = _number;
		m_x = _x;
		m_y = _y;

		// 타일속성 셋팅
		GameObject model = Object.Instantiate(Resources.Load<GameObject>("Tile/tile"));
		model.name = "hex_" + _number;
		model.transform.parent = _parent.transform;
		model.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		model.transform.Rotate(0.0f, 0.0f, 0.0f);
		m_texture = model.transform.Find("texture").transform;
		for(int i = 0; i < 3; i++)
		{
			Transform tempObj = model.transform.Find("" + i);
			tempObj.name = "" + _number;
		}
		// 위치
		Vector3 pos = new Vector3(m_x * 7.0f, 0.0f, m_y * 6.1f);
        if(m_y % 2 == 1) { pos.x += 3.5f; }
		model.transform.localPosition = pos;

		Wild_ChangeType(_type);
	}
}
