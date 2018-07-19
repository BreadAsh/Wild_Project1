using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using System.IO;

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
}
