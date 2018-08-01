using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using System.IO;
using LitJson;

public class Wild_Static_File
{
	public static StreamReader Wild_FileReader(string _path)
	{
		StreamReader res = null;

		for(int i = 0; i < 3; i++)
		{
			try
			{
				res = new StreamReader(_path);
			}
			catch(System.Exception e)
			{
				Debug.Log(e.ToString());
				if(res != null)
				{
					res.Close();
					res = null;
				}
			}
		}

		if(res == null)
			Debug.Log(_path + " open fail!");

		return res;
	}


	// Json
	static JsonData saveJson = null;

	public static void Wild_JsonAddSaveData(object _data)
	{
		if(saveJson == null) saveJson = JsonMapper.ToJson(_data);
		else saveJson.Add(_data);
	}

	public static void Wild_JsonSave(string _path)
	{
		File.WriteAllText(Application.dataPath + _path + ".json", saveJson.ToString());
		saveJson = null;
	}

	public static void Wild_JsonLoad(string _path)
	{
		string path = Application.dataPath + _path + ".json";
		if(File.Exists(path))
		{
			string str = File.ReadAllText(path);

			JsonData data = JsonMapper.ToObject(str);
		}
		else
		{
			Debug.Log("open fail!");
		}
	}
}
