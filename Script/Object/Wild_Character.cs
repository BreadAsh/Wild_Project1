using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ActiveSource
    
// 캐릭터 모션에 대한 정보 클래스
public enum Wild_Character_Active_TYPE
{
    Wild_Character_Active_TYPE_IDLE,
    Wild_Character_Active_TYPE_MOVE,
    Wild_Character_Active_TYPE_ATTACK
}

public class Wild_Motion
{
    float m_start;
    float m_end;

	/********** Getter & Setter	**********/
    public float Wild_GetStart() { return m_start; }

    public float Wild_GetEnd() { return m_end; }

	/********** Default Method	**********/
    public void Wild_Init(int _start, int _end)
    {
        m_start = (1.0f / 30.0f) * (float)_start;
        m_end = (1.0f / 30.0f) * (float)_end;
    }
}

// 행동관련 상속 클라스
public class Wild_Active
{
    protected Wild_Character_Active_TYPE m_type;

    // motion
	protected List<Wild_Motion> m_l_motion_right;
	protected List<Wild_Motion> m_l_motion_left;

    bool m_isLoop;

	/********** Getter & Setter	**********/
    public Wild_Character_Active_TYPE Wild_GetType() { return m_type; }

    public Wild_Motion Wild_GetMotion(bool _isRight, int _frameNumber)
    {
        Wild_Motion res = null;

        if(_isRight) res = m_l_motion_right[_frameNumber];
        else         res = m_l_motion_left[_frameNumber];

        return res;
    }

    public int Wild_GetMotionCount(bool _isRight)
    {
        int res = 0;

        if(_isRight) res = m_l_motion_right.Count;
        else         res = m_l_motion_left.Count;

        return res;
    }

    //
    public bool Wild_GetIsLoop() { return m_isLoop; }

	/********** Method	**********/

	// 사이클이 끝날 때마다 이벤트가 발생
	public virtual void Wild_ActiveAlways() {}
	// 모션이 끝날 때마다 이벤트가 발생
	public virtual void Wild_ActiveFrameEnd() {}

	/********** Default Method	**********/
	public virtual void Wild_Init(string _str)
	{
		//
		m_l_motion_right = new List<Wild_Motion>();
		m_l_motion_left = new List<Wild_Motion>();

		string[] strs1 = _str.Split('!');
		Wild_Init_SetFrameList(true, strs1[0]);
		Wild_Init_SetFrameList(false, strs1[1]);

        m_isLoop = false;
        if(strs1[2].Equals("1")) m_isLoop = true;
	}

	void Wild_Init_SetFrameList(bool _isRight, string _strs1)
	{
		string[] strs2 = _strs1.Split('/');
		for(int i = 0; i < strs2.Length; i++)
		{
			string[] strs3 = strs2[i].Split(',');
			int start = int.Parse(strs3[0]);
			int end = int.Parse(strs3[1]);

			Wild_Motion frames = new Wild_Motion();
			frames.Wild_Init(start, end);
			if(_isRight) m_l_motion_right.Add(frames);
			else		 m_l_motion_left.Add(frames);
		}
	}
}

public class Wild_Active_Idle : Wild_Active
{
	/********** Method	**********/

	// 업데이트가 끝날 때마다 이벤트가 발생
	public override void Wild_ActiveAlways()
    {

    }

	/********** Default Method	**********/
	public override void Wild_Init(string _str)
    {
        base.Wild_Init(_str);

        m_type = Wild_Character_Active_TYPE.Wild_Character_Active_TYPE_IDLE;
    }
}

public class Wild_Active_Move : Wild_Active
{
	Wild_Character m_c_character;

	Transform m_transform;

	Vector3 m_destination;

	/********** Getter & Setter	**********/

	public void Wild_SetDestination(Vector3 _destination)
	{
		m_destination = _destination;
	}

	/********** Method	**********/

	// 업데이트가 끝날 때마다 이벤트가 발생
	public override void Wild_ActiveAlways()
    {
		Vector3.Lerp(m_transform.position, m_destination, Time.deltaTime);

		if(Vector3.Distance(m_transform.position, m_destination) < 0.1f)
		{
			m_transform.position = m_destination;
			m_c_character.Wild_Active_SetIsEnd(true);
		}
    }

	//
	public void Wild_InitSetting(Wild_Character _c_character, Transform _transform)
	{
		m_c_character = _c_character;
		m_transform = _transform;
	}

	/********** Default Method	**********/
	public override void Wild_Init(string _str)
    {
        base.Wild_Init(_str);

        m_type = Wild_Character_Active_TYPE.Wild_Character_Active_TYPE_MOVE;
    }
}

#endregion

//////////	//////////
#region Character

public enum Wild_Character_COMMAND
{
    Wild_Character_COMMAND_IDLE,
    //
    Wild_Character_COMMAND_MOVE,
    //
    Wild_Character_COMMAND_ATTACK,
    Wild_Character_COMMAND_ATTACK_AUTO
}

public class Wild_Character : Wild_Object
{
    struct BA_Unit_Status
    {
        public int m_str;
        public int m_dex;
        public int m_int;
        public int m_vital;
        public int m_speed;
    }
	BA_Unit_Status m_c_defaultStatus;
	// 아이템 및 스킬의 보정에 따른 최종스탯
	BA_Unit_Status m_c_finalStatus;

    //
    Wild_Dungeon_Manager m_c_manager;

	/********** Getter & Setter	**********/

	/********** Method	**********/
    public void Wild_InitAnother( Wild_Dungeon_Manager _c_manager )
    {
        m_c_manager = _c_manager;
    }

	/********** Default Method	**********/
	public override void Wild_Init(string _str)
	{
        Wild_AStar_Init();
		Wild_Active_Init();
	}

	public override void Wild_Release()
	{
		
	}

	public override void Wild_Update()
	{

	}

    //////////  //////////
	#region Active
	Wild_Active_Move m_c_move;

	bool m_active_isEnd;

	int m_active_formationTileNumber;
	int m_active_activePoint;

    List<Wild_Active> m_active_l_active;
    
    int m_active_selActive;

	// animation
    Animation m_active_ani;
    int m_active_frameCount;

    bool m_active_isRight;

	/********** Getter & Setter	**********/
	public bool Wild_Active_GetIsEnd() { return m_active_isEnd; }
	public void Wild_Active_SetIsEnd(bool _isEnd) { m_active_isEnd = _isEnd; }

	public int Wild_Active_GetActivePoint() { return m_active_activePoint; }

	// animation
    public float Wild_Active_GetMotionTime()
    {
        return m_active_ani["Take 001"].time;
    }
    public void Wild_Active_SetMotionTime(float _timer, float _end)
    {
        float temp = _timer;
        if(temp > _end) temp = _end;
        m_active_ani["Take 001"].time = temp;
    }

    public void Wild_Actlive_SetIsRight(bool _isRight) { m_active_isRight = _isRight; }

	/********** Method	**********/
	public void Wild_Active_BattleStart()
	{
		m_active_isEnd = false;
		m_active_activePoint = 0;
		Wild_AStar_Setting();
	}

	public void Wild_Active_TurnSetting()
	{
		m_active_activePoint += m_c_finalStatus.m_speed;
	}

	public void Wild_Active_PhaseStart()
	{

	}

	// animation
    public bool Wild_Active_FrameEnd(float _time)
    {
        bool res = false;

        if(m_active_ani["Take 001"].time >= _time)
            res = true;

        return res;
    }

    public void Wild_Active_SettingSelActive(Wild_Character_Active_TYPE _type)
    {
        for(int i = 0; i < m_active_l_active.Count; i++)
        {
            if(m_active_l_active[i].Wild_GetType().Equals(_type))
            {
                m_active_selActive = i;
                break;
            }
        }

        //
    }

	/********** Default Method	**********/
	void Wild_Active_Init()
	{
		Wild_Active_BattleStart();

		m_active_selActive = 0;

        m_active_ani = m_model.GetComponent<Animation>();
        m_active_frameCount = 0;

        m_active_isRight = true;
	}

	void Wild_Active_Update()
	{
        Wild_Active active = m_active_l_active[m_active_selActive];

		active.Wild_ActiveAlways();

		//
		Wild_Motion motion = active.Wild_GetMotion(m_active_isRight, m_active_frameCount);

		float aniTime = Wild_Active_GetMotionTime();

		if(Wild_Active_FrameEnd(motion.Wild_GetEnd()))
		{
			active.Wild_ActiveFrameEnd();

			// animation
			m_active_frameCount++;
            
            if(m_active_frameCount >= active.Wild_GetMotionCount(m_active_isRight))
            {
                if(active.Wild_GetIsLoop())
                    m_active_frameCount = 0;
                else
                    Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.Wild_Character_Active_TYPE_IDLE);
            }
		}
		else
		{
			aniTime += Time.deltaTime / 10.0f;
            float frameEnd = motion.Wild_GetEnd();

			if (aniTime >= frameEnd)
			{
                aniTime = frameEnd;
			}
		}

        m_active_ani["Take 001"].time = aniTime;
	}
	#endregion

    //////////  //////////
    #region A-Star

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
    Wild_AStar_Data[] m_AStar_a_data;

    List<int> m_AStar_l_settingTemp;

	/********** Getter & Setter	**********/

	/********** Method	**********/

    void Wild_AStar_Reset()
    {
        for(int i = 0; i < m_AStar_a_data.Length; i++)
        {
            m_AStar_a_data[i].m_distance = 9999;
            m_AStar_a_data[i].m_condition = Wild_AStar_CONDITION.NOSEARCH;
        }
    }

    //
    void Wild_AStar_Setting()
    {
        Wild_AStar_Reset();

        // 자기 위치 셋팅
        {
            int temp = m_c_manager.Wild_FindMyTileNumber(this);
            m_AStar_a_data[temp].m_condition = Wild_AStar_CONDITION.OWN;
            m_AStar_a_data[temp].m_distance = 0;
            m_AStar_l_settingTemp.Add(temp);
        }

        // 산출
		while(m_AStar_l_settingTemp.Count > 0)
		{
			int num = m_AStar_l_settingTemp[0];
            Wild_Tile tile = m_c_manager.Wild_GetTile(num);
			int x = tile.Wild_GetX();
			int y = tile.Wild_GetX();
            int distance = m_AStar_a_data[num].m_distance;
			switch(y % 2)
			{
				case 0:	Wild_AStar_Setting1(x, y, -1,	distance);	break;
				case 1:	Wild_AStar_Setting1(x, y,  0,	distance);	break;
			}

			m_AStar_l_settingTemp.RemoveAt(0);
		}
    }

	// 주변 6칸 색출
	void Wild_AStar_Setting1(int _x, int _y, int _valX, int _dis)
	{
		int dis = _dis + 1;
		int x = 0;
		int y = 0;

		y = _y - 1;
		int yCount = 0;
		// 하단
		if(y >= 0)
		{
			x = _x + _valX;
			yCount = y * (int)Wild_Map_SIZE.X;
			if(x >= 0)
			{
				Wild_AStar_Setting2(x + yCount, dis);
			}

			x += 1;
			if(x < (int)Wild_Map_SIZE.X)
			{
				Wild_AStar_Setting2(x + yCount, dis);
			}
		}

        // 중단
		y = _y;
		x = _x - 1;
		yCount = y * (int)Wild_Map_SIZE.X;
		if(x >= 0)
		{
			Wild_AStar_Setting2(x + yCount, dis);
		}

		x = _x + 1;
		if(x < (int)Wild_Map_SIZE.X)
		{
			Wild_AStar_Setting2(x + yCount, dis);
		}

        // 상단
		y = _y + 1;
		if(y < (int)Wild_Map_SIZE.Y)
		{
			x = _x + _valX;
			yCount = y * (int)Wild_Map_SIZE.X;
			if(x >= 0)
			{
				Wild_AStar_Setting2(x + yCount, dis);
			}

			x += 1;
			if(x < (int)Wild_Map_SIZE.X)
			{
				Wild_AStar_Setting2(x + yCount, dis);
			}
		}
	}

	// 칸마다 체크
	void Wild_AStar_Setting2(int _num, int _dis)
	{
		// 체크에 관련된 헥스의 정보 가져오기
		Wild_Tile hex = m_c_manager.Wild_GetTile(_num);
		// 헥스가 지나갈 수 없는 곳인가?
		if(hex.Wild_IsMove())
		{
            Wild_AStar_Data data = m_AStar_a_data[_num];
			if(data.m_distance > _dis)
				data.m_distance = _dis;

			bool isDoing = (data.m_condition == Wild_AStar_CONDITION.NOSEARCH);
			if(isDoing)
			{
				Wild_Object obj = hex.Wild_GetObject();
				Wild_AStar_CONDITION con = Wild_AStar_CONDITION.MOVE;
				if(obj != null)
				{
					switch(obj.Wild_GetType())
					{
						case Wild_Object_TYPE.OBJECT:	con = Wild_AStar_CONDITION.OBJECT;	break;
					}
				}

				m_AStar_a_data[_num].m_condition = con;

				if(con == Wild_AStar_CONDITION.MOVE)
					m_AStar_l_settingTemp.Add(_num);
			}
		}
		else
		{
			m_AStar_a_data[_num].m_condition = Wild_AStar_CONDITION.NOTMOVE;
		}
	}

	/********** Default Method	**********/
    void Wild_AStar_Init()
    {
        m_AStar_a_data = new Wild_AStar_Data[50];
        for(int i = 0; i < (int)Wild_Map_SIZE.X * (int)Wild_Map_SIZE.Y; i++)
        {
            m_AStar_a_data[i] = new Wild_AStar_Data();

        }
        Wild_AStar_Reset();

        m_AStar_l_settingTemp = new List<int>();
    }
    #endregion
}

#endregion