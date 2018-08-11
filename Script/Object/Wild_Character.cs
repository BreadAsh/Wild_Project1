using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using System.IO;

#region ActiveSource
    
// 캐릭터 모션에 대한 정보 클래스
public enum Wild_Character_Active_TYPE
{
    IDLE,
    MOVE,
	DAMAGE,
	DEATH,
    ATTACK_PUNCH,
    ATTACK_SWORD,
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

	// 행동이 종료할 때 발생
	public virtual void Wild_ActiveEnd() {}

	/********** Default Method	**********/
	public virtual string[] Wild_Init(string _str)
	{
		string[] res = _str.Split('!');
		//
		m_l_motion_right = new List<Wild_Motion>();
		m_l_motion_left = new List<Wild_Motion>();

        m_isLoop = false;
        if(res[0].Equals("1")) m_isLoop = true;
		Wild_Init_SetFrameList(true, res[1]);
		Wild_Init_SetFrameList(false, res[2]);

		return res;
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

	/********** Default Method	**********/
	public override string[] Wild_Init(string _str)
    {
        base.Wild_Init(_str);

        m_type = Wild_Character_Active_TYPE.IDLE;

		return null;
    }
}

public class Wild_Active_Move : Wild_Active
{
	Wild_Character m_c_character;
	Wild_Dungeon_Manager m_c_manager;

	Transform m_transform;

	Wild_Tile m_destination;

	/********** Getter & Setter	**********/

	/********** Method	**********/

	// 업데이트가 끝날 때마다 이벤트가 발생
	public override void Wild_ActiveAlways()
    {
		m_transform.position = Vector3.Lerp(m_transform.position, m_destination.Wild_GetPosition(), Time.deltaTime * 2.0f);

		if(Vector3.Distance(m_transform.position, m_destination.Wild_GetPosition()) < 0.1f)
		{
			m_transform.position = m_destination.Wild_GetPosition();
			m_c_character.Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.IDLE);
		}
    }

	//
	public void Wild_InitAnother(Wild_Character _c_character, Wild_Dungeon_Manager _c_manager, Transform _transform)
	{
		m_c_character = _c_character;
		m_c_manager = _c_manager;
		m_transform = _transform;
	}

	public void Wild_SettingDestination(int _des)
	{
		int now = m_c_character.Wild_Active_GetNowTile();
		m_c_manager.Wild_Tile_GetTile(now).Wild_SetObject(null);

		m_c_character.Wild_Active_SetNowTile(_des);
		m_c_manager.Wild_Tile_GetTile(_des).Wild_SetObject(m_c_character);
		m_destination = m_c_manager.Wild_Tile_GetTile(_des);
	}

	/********** Default Method	**********/
	public override string[] Wild_Init(string _str)
    {
        base.Wild_Init(_str);

        m_type = Wild_Character_Active_TYPE.MOVE;

		return null;
    }
}

public class Wild_Active_Damage : Wild_Active
{
	Wild_Character m_c_character;
	/********** Method	**********/

	// 행동이 종료할 때 발생
	public override void Wild_ActiveEnd()
	{
		Debug.Log("damage");
		if(m_c_character.Wild_GetHp() <= 0)
		{
			m_c_character.Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.DEATH);
		}
		else
		{
			m_c_character.Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.IDLE);
		}
	}

	//
	public void Wild_InitAnother(Wild_Character _c_character)
	{
		m_c_character = _c_character;
	}

	/********** Default Method	**********/
	public override string[] Wild_Init(string _str)
    {
        base.Wild_Init(_str);

        m_type = Wild_Character_Active_TYPE.DAMAGE;

		return null;
    }
}

public class Wild_Active_Death : Wild_Active
{
	Wild_Character m_c_character;
	Wild_Dungeon_Manager m_c_manager;
	
	/********** Method	**********/
	public override void Wild_ActiveEnd()
	{
		m_c_manager.Wild_Tile_GetTile(m_c_character.Wild_Active_GetNowTile()).Wild_SetObject(null);
		m_c_character.Wild_Active_SetNowTile(-1);
		m_c_character.Wild_GetModel().SetActive(false);
		m_c_character.Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.IDLE);
	}

	//
	public void Wild_InitAnother(Wild_Character _c_character, Wild_Dungeon_Manager _c_manager)
	{
		m_c_character = _c_character;
		m_c_manager = _c_manager;
	}

	/********** Default Method	**********/
	public override string[] Wild_Init(string _str)
    {
        base.Wild_Init(_str);

        m_type = Wild_Character_Active_TYPE.DEATH;

		return null;
    }
}

public class Wild_Active_Attack : Wild_Active
{
	Wild_Character m_c_character;

	Wild_Object m_target;

	/********** Method	**********/

	// 업데이트가 끝날 때마다 이벤트가 발생
	public override void Wild_ActiveFrameEnd()
    {
		if(m_c_character.Wild_Active_GetFrameCount() < m_l_motion_right.Count)
		{
			m_target.Wild_Damage(0);
		}
    }

	// 행동이 종료할 때 발생
	public override void Wild_ActiveEnd()
	{
		if(	m_target.Wild_Active_GetActive().Wild_GetType().Equals(Wild_Character_Active_TYPE.IDLE) ||
			(m_target.Wild_GetHp() <= 0) ||
			// 오브젝트일 때
			m_target.Wild_Active_GetActive().Equals(null))
		{
			Debug.Log("attack end");
			m_target = null;
			m_c_character.Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.IDLE);
		}
	}

	//
	public void Wild_InitAnother(Wild_Character _c_character)
	{
		m_c_character = _c_character;
	}

	//
	public void Wild_ActiveSetting(Wild_Object _target)
	{
		m_target = _target;
	}

	/********** Default Method	**********/
	public override string[] Wild_Init(string _str)
    {
        string[] strs = base.Wild_Init(_str);

		switch(strs[3])
		{
		case "0":	m_type = Wild_Character_Active_TYPE.ATTACK_PUNCH;	break;
		}

		m_target = null;

		return null;
    }
}
#endregion

//////////	//////////
#region Character

public enum Wild_Character_COMMAND
{
    IDLE,
    //
    MOVE,
    //
    ATTACK,
    ATTACK_AUTO
}

public class Wild_Character : Wild_Object
{
	int m_level;
	int m_exp;

	Wild_Weapon m_rHand;
	Wild_Equipment m_helm;
	Wild_Equipment m_armor;
	Wild_Equipment m_gloves;
	Wild_Equipment m_boots;

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
	public override void Wild_Init(GameObject _parent, string _str, Wild_Object_TYPE _type)
	{
		Debug.Log(_str);
		string[] strs = _str.Split('/');

		string path = "";
		switch(_type)
		{
			case Wild_Object_TYPE.CHARACTER:
			case Wild_Object_TYPE.ALLIY:
				path = "Assets/Wild_Project1/Data/Character/";	break;
			//
			case Wild_Object_TYPE.ENEMY:
				path = "Assets/Wild_Project1/Data/Enemy/";	break;
		}

		StreamReader reader = Wild_Static_File.Wild_FileReader(path + strs[1]);
		if(reader != null)
		{
			//
			if( Wild_Init_Character(reader, _type) )
			{
				//
				m_number = int.Parse(strs[0]);
				//
				m_active_defaultTileNumber = m_active_nowTileNumber = int.Parse(strs[10]);
				m_c_manager.Wild_Tile_GetTile(m_active_defaultTileNumber).Wild_SetObject(this);
				//
				m_level = int.Parse(strs[2]);
				m_exp = int.Parse(strs[3]);

				reader.Close();
			}
			else
			{
				if(reader != null)
				{
					reader.Close();
					reader = null;
				}
			}
		}

		base.Wild_Init(_parent, null, _type);

        Wild_AStar_Init();
	}

	bool Wild_Init_Character(StreamReader _reader, Wild_Object_TYPE _type)
	{
		m_rHand = null;
		m_helm = null;
		m_armor = null;
		m_gloves = null;
		m_boots = null;

		bool res = false;

		if(_reader != null)
		{
			string path = "";
			switch(_type)
			{
				case Wild_Object_TYPE.CHARACTER:
				case Wild_Object_TYPE.ALLIY:
					path = "Character/";	break;
				//
				case Wild_Object_TYPE.ENEMY:
					path = "Enemy/";	break;
			}
			m_model = Object.Instantiate(Resources.Load<GameObject>(path + _reader.ReadLine()));
			m_c_defaultStatus = new BA_Unit_Status();
			string[] strs = _reader.ReadLine().Split(',');
			m_c_defaultStatus.m_str		= int.Parse(strs[0]);
			m_c_defaultStatus.m_dex		= int.Parse(strs[1]);
			m_c_defaultStatus.m_int		= int.Parse(strs[2]);
			m_c_defaultStatus.m_vital	= int.Parse(strs[3]);
			m_c_defaultStatus.m_speed	= int.Parse(strs[4]);

			_reader.ReadLine();
			//
			Wild_Active_Init(_reader);

			res = true;
		}

		return res;
	}

	public override void Wild_Release()
	{
		
	}

	public override void Wild_Update()
	{
		Wild_Active_Update();
	}

    //////////  //////////
	#region Active
	Wild_Active_Move m_active_c_move;
	Wild_Active_Attack m_active_c_attack_punch;

	int m_active_defaultTileNumber;
	int m_active_nowTileNumber;
	int m_active_activePoint;

    List<Wild_Active> m_active_l_active;
    
    int m_active_selActive;

	// animation
    Animation m_active_ani;
	float m_active_animationTimer;
    int m_active_frameCount;

    bool m_active_isRight;

	/********** Getter & Setter	**********/
	public override Wild_Active Wild_Active_GetActive() { return m_active_l_active[m_active_selActive]; }

	public int Wild_Active_GetActivePoint() { return m_active_activePoint; }

	// animation
    public void Wild_Active_SetMotionTime(float _timer, float _end)
    {
        float temp = _timer;
        if(temp > _end) temp = _end;
        m_active_animationTimer = temp;
    }

	public int Wild_Active_GetFrameCount() { return m_active_frameCount; }

    public void Wild_Active_SetIsRight(bool _isRight) { m_active_isRight = _isRight; }

	public Wild_Character_Active_TYPE Wild_Active_type() { return m_active_l_active[m_active_selActive].Wild_GetType(); }

	public int Wild_Active_GetNowTile() { return m_active_nowTileNumber; }
	public void Wild_Active_SetNowTile(int _nowTileNumber) { m_active_nowTileNumber = _nowTileNumber; }
	/********** Method	**********/

	public override void Wild_Damage(int _damage)
	{
		base.Wild_Damage(_damage);
		Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.DAMAGE);
	}

	public void Wild_Active_BattleStart()
	{
		m_active_activePoint = 0;
		Wild_AStar_Setting();
		m_active_nowTileNumber = m_active_defaultTileNumber;
		m_model.transform.position = m_c_manager.Wild_Tile_GetTile(m_active_nowTileNumber).Wild_GetPosition();
	}

	public void Wild_Active_TurnSetting()
	{
		m_active_activePoint += 100;//m_c_finalStatus.m_speed;
	}

	// Phase
	public void Wild_Active_PhaseSetting()
	{
		Wild_AStar_Setting();

		int dis = 9999;
		int des = -1;

		// 가장 가까운 적을 찾아낸다.
		for(int i = 0; i < m_c_manager.Wild_Tile_GetTileCount(); i++)
		{
			Wild_Object obj = m_c_manager.Wild_Tile_GetTile(i).Wild_GetObject();
			if(obj != null)
			{
				switch(m_type)
				{
					case Wild_Object_TYPE.CHARACTER:
						{
							if( obj.Wild_GetType() == Wild_Object_TYPE.ENEMY )
							{
								if( m_AStar_a_data[i].m_distance < dis)
								{
									dis = m_AStar_a_data[i].m_distance;
									des = i;
								}
							}
						}
						break;
					case Wild_Object_TYPE.ENEMY:
						{
							if( obj.Wild_GetType() == Wild_Object_TYPE.CHARACTER )
							{
								if( m_AStar_a_data[i].m_distance < dis)
								{
									dis = m_AStar_a_data[i].m_distance;
									des = i;
								}
							}
						}
						break;
				}
			}
		}

		// 사거리
		int range = 1;
		if(m_rHand != null) range = m_rHand.Wild_GetRange();

		// 이동
		if(dis > range)
		{
			int dis2 = 9999;
			int des2 = -1;
			Wild_Object obj = m_c_manager.Wild_Tile_GetTile(des).Wild_GetObject();
			for(int i = 0; i < m_AStar_a_data.Length; i++)
			{
				if((m_AStar_a_data[i].m_distance == 1) && (m_AStar_a_data[i].m_condition == Wild_AStar_CONDITION.MOVE))
				{
					if( obj.Wild_AStar_Distance(i) < dis2)
					{
						dis2 = obj.Wild_AStar_Distance(i);
						des2 = i;
					}
				}
			}
			m_active_c_move.Wild_SettingDestination(des2);
			Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.MOVE);
		}
		// 공격
		else
		{
			if(m_rHand == null)
			{
				m_active_c_attack_punch.Wild_ActiveSetting( m_c_manager.Wild_Tile_GetTile(des).Wild_GetObject() );
				Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.ATTACK_PUNCH);
			}
			else
			{
				switch(m_rHand.Wild_GetType())
				{
					
				}
			}
		}
	}

	// animation
    public bool Wild_Active_FrameEnd(float _time)
    {
        bool res = false;

        if(m_active_animationTimer >= _time)
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
		Wild_Active_Animation_Reset();
    }

	void Wild_Active_Animation_Reset()
	{
        m_active_frameCount = 0;
		m_active_animationTimer = m_active_l_active[m_active_selActive].Wild_GetMotion(true, m_active_frameCount).Wild_GetStart();
	}

	/********** Default Method	**********/
	void Wild_Active_Init(StreamReader _reader)
	{
//		Wild_Active_BattleStart();

        m_active_ani = m_model.GetComponent<Animation>();
		m_active_ani.playAutomatically = false;
		m_active_ani.Stop();
        m_active_frameCount = 0;

        m_active_isRight = true;

		m_active_l_active = new List<Wild_Active>();
		// init
		Wild_Active_Idle idle = new Wild_Active_Idle();
		idle.Wild_Init(_reader.ReadLine());
		m_active_l_active.Add(idle);
		// move
		m_active_c_move = new Wild_Active_Move();
		m_active_c_move.Wild_Init(_reader.ReadLine());
		m_active_c_move.Wild_InitAnother(this, m_c_manager, m_model.transform);
		m_active_l_active.Add(m_active_c_move);
		// damage
		Wild_Active_Damage damage = new Wild_Active_Damage();
		damage.Wild_Init(_reader.ReadLine());
		damage.Wild_InitAnother(this);
		m_active_l_active.Add(damage);
		// death
		Wild_Active_Death death = new Wild_Active_Death();
		death.Wild_Init(_reader.ReadLine());
		death.Wild_InitAnother(this, m_c_manager);
		m_active_l_active.Add(death);
		// punch
		m_active_c_attack_punch = new Wild_Active_Attack();
		m_active_c_attack_punch.Wild_Init(_reader.ReadLine());
		m_active_c_attack_punch.Wild_InitAnother(this);
		m_active_l_active.Add(m_active_c_attack_punch);

		Wild_Active_SettingSelActive(Wild_Character_Active_TYPE.IDLE);
	}

	void Wild_Active_Update()
	{
		m_active_ani.Stop();
        Wild_Active active = m_active_l_active[m_active_selActive];

		active.Wild_ActiveAlways();

		//
		int frameCount = active.Wild_GetMotionCount(m_active_isRight) - 1;
		if(m_active_frameCount < active.Wild_GetMotionCount(m_active_isRight))
		{
			frameCount = m_active_frameCount;
		}

		Wild_Motion motion = active.Wild_GetMotion(m_active_isRight, frameCount);

		float aniTime = m_active_animationTimer;

		if(Wild_Active_FrameEnd(motion.Wild_GetEnd()))
		{
			active.Wild_ActiveFrameEnd();

			// animation
			m_active_frameCount++;
            
            if(m_active_frameCount >= active.Wild_GetMotionCount(m_active_isRight))
            {
                if(active.Wild_GetIsLoop())
				{
                    m_active_frameCount = 0;
					aniTime = motion.Wild_GetStart();
				}
                else
				{
					active.Wild_ActiveEnd();
				}
            }
			else
			{
				motion = active.Wild_GetMotion(m_active_isRight, frameCount);
				aniTime = motion.Wild_GetStart();
			}
		}
		else
		{
			aniTime += Time.deltaTime;
            float frameEnd = motion.Wild_GetEnd();

			if (aniTime >= frameEnd)
			{
                aniTime = frameEnd;
			}
		}

		m_active_ani.Play();
		m_active_animationTimer = aniTime;
        m_active_ani["Take 001"].time = m_active_animationTimer;
	}
	#endregion

    //////////  //////////
    #region A-Star

    List<int> m_AStar_l_settingTemp;

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
    public void Wild_AStar_Setting()
    {
        Wild_AStar_Reset();

		// 죽어서 현재 타일에 없는가?
		if(m_active_nowTileNumber.Equals(-1))
		{
			return;
		}
		
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
            Wild_Tile tile = m_c_manager.Wild_Tile_GetTile(num);
			int x = tile.Wild_GetX();
			int y = tile.Wild_GetY();
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
		Wild_Tile hex = m_c_manager.Wild_Tile_GetTile(_num);
		// 헥스가 지나갈 수 있는 곳인가?
		if(hex.Wild_IsMove())
		{
			if(m_AStar_a_data[_num].m_distance > _dis)
				m_AStar_a_data[_num].m_distance = _dis;

			bool isDoing = (m_AStar_a_data[_num].m_condition == Wild_AStar_CONDITION.NOSEARCH);
			if(isDoing)
			{
				Wild_Object obj = hex.Wild_GetObject();
				Wild_AStar_CONDITION con = Wild_AStar_CONDITION.MOVE;
				if(obj != null)
				{
					switch(obj.Wild_GetType())
					{
						case Wild_Object_TYPE.OBJECT:		con = Wild_AStar_CONDITION.OBJECT;	break;
						case Wild_Object_TYPE.CHARACTER:	con = Wild_AStar_CONDITION.PLAYER;	break;
						case Wild_Object_TYPE.ENEMY:		con = Wild_AStar_CONDITION.ENEMY;	break;
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