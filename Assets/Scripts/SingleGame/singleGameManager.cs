#define DEBUGMODE

using PACKET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
[SerializeField]

public class singleGameManager : MonoBehaviour
{
    public static singleGameManager m_Instance;

    [Flags]
    public enum FUNC_TYPE : int
    {
        START_GAME,
        ON_HIT,
        UP_FLOOR,
        TYPE_SIZE,
    }
    
    [SerializeField]
    Camera                  m_carmera;

    [SerializeField]
    GameObject              m_gameStageOBJ;


    float                   m_fCurrentStayTime;
    [SerializeField]
    float                   m_fLimitStayTime = 5;


    #region GAME OBJ
    [SerializeField]
    GameObject              m_playerCharacterOBJ;
    int                     m_nPlayerHP;
    [SerializeField]
    int                     m_nStartPlayerHP = 3;
    bool                    m_bEndGame;

    [SerializeField]
    MeshRenderer            m_backgroundScrollMAT;

    
    float                   m_curYoffset = 0;
    public float            m_offsetY = 0.15f;
    public float            m_fMoveCoolTime = 0.5f;
    float                   m_fCurMoveCoolTime = 0;
    int                     m_nCurCharacterPosX = 0;
    float                   m_fOnHitCoolTime = 1f;
    float                   m_fCurrentOnHitCoolTime;

    [SerializeField]
    public static int       maxFloor;
    [SerializeField]
    public static int       m_nCurMoveLength;



    // obj list
    [SerializeField]
    GameObject              m_restZonePrefabs;
    [SerializeField]
    List<RestZone>          m_restZoneList = new List<RestZone>();
    [SerializeField]
    List<C_obstacleFloor>   m_objList = new List<C_obstacleFloor>();
    [SerializeField]
    GameObject       m_parentOfObj;

    [SerializeField]
    List<GameObject>        m_objPrefabs;
    [SerializeField]
    GameObject              m_floorPrefabs;
    #endregion

    #region GAME UI

    public delegate void eventFUNC(object data);
    public eventFUNC[] m_funcList = new eventFUNC[3];



    #endregion
    
    private void OnDisable()
    {
        closeGame();
    }


    #region OBJ

    // Update is called once per frame
    void Update()
    {
        // hp check 
        if(m_bEndGame)
        {
            return;
        }

        m_fCurrentStayTime += Time.deltaTime;

        if (m_fCurrentOnHitCoolTime > 0)
            m_fCurrentOnHitCoolTime -= Time.deltaTime;

        if (m_fCurMoveCoolTime <= 0)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                m_curYoffset += m_offsetY;
                Vector2 offset = new Vector2(0, m_curYoffset);

                m_backgroundScrollMAT.material.mainTextureOffset = offset;
                m_fCurMoveCoolTime = m_fMoveCoolTime;
                // move down another obj list
                moveVerticalOBJ();
                m_fCurrentStayTime = 0;
            }
            else
            {
                int direction = 0;
                if (Input.GetKey(KeyCode.LeftArrow) && m_nCurCharacterPosX > -2)
                {
                    --direction;
                }
                if (Input.GetKey(KeyCode.RightArrow) && m_nCurCharacterPosX < 2)
                {
                    ++direction;
                }
                if (direction != 0)
                {
                    m_fCurrentStayTime  = 0;
                    m_nCurCharacterPosX += direction;
                    m_fCurMoveCoolTime  = m_fMoveCoolTime;
                    m_playerCharacterOBJ.transform.localPosition = new Vector3(m_nCurCharacterPosX, 0);
                }
            }
        }
        else
            m_fCurMoveCoolTime -= Time.deltaTime;

        if (m_fCurrentStayTime >= m_fLimitStayTime)
        {
            m_fCurrentStayTime = 0;
            makeRestZoneObstacleOBJ();
        }
        else if(  m_fCurrentStayTime > 0)
        {
            RestZone restZone = getCurrentRestZone();
            if (restZone != null)
                restZone.setColorG(1-getStayTimeRate());
        }

        // move another obj horizontal
        moveHorizontalOBJ();
    }

    /// <summary>
    /// 일정 시간 안움직이면 발생
    /// </summary>
    void makeRestZoneObstacleOBJ()
    {
        int nCurRealFloor = m_nCurMoveLength;
        RestZone restZone = getCurrentRestZone();
        if(restZone!=null)    
           restZone.makeObstacleZone();
    }

    RestZone getCurrentRestZone()
    {
        return m_restZoneList.Find(x => x.m_curFloor == m_nCurMoveLength);
    }

    void moveHorizontalOBJ()
    {
        for (int i = 0; i < m_objList.Count; ++i)
        {
            if ((m_objList[i].m_curFloor - m_nCurMoveLength) >= C_obstacleFloor.fLimitUpY)
                continue;
            m_objList[i].moveHor();

        }
    }

    void moveVerticalOBJ()
    {
        RestZone restZone = getCurrentRestZone();
        if (restZone != null)
            restZone.disableFlag();
        ++m_nCurMoveLength;
        restZone = getCurrentRestZone();
        if (restZone != null)
            restZone.onFlag();
        m_funcList[(int)FUNC_TYPE.UP_FLOOR](m_nCurMoveLength);
        for (int i = 0; i < m_objList.Count; ++i)
            m_objList[i].moveDown();
        for(int i = 0; i < m_restZoneList.Count;++i)
        {
            m_restZoneList[i].moveDown();
            if (m_restZoneList[i].gameObject.activeSelf == false)
            {
                DestroyImmediate(m_restZoneList[i].gameObject);
                m_restZoneList.RemoveAt(i);
                --i;
            }
        }
    }

    float getStayTimeRate()
    {
        return m_fCurrentStayTime/m_fLimitStayTime;
    }

    public void getNextFloor(ref float fSpeed)
    {
        // 10층 전까지 무조건 1층의 빈공간 부여
        // 10~20 :: 20퍼 확률로 빈공간 제거, 이속 max 0.5증가
        // 20~40 :: 40퍼 확률로 빈공간 제거, 이속 max 1증가
        // 40~60 :: 60퍼 확률로 빈공간 제거, 이속 max 2증가
        int nOffset = maxFloor / 10 * 5;
        if (nOffset >= 90)
            nOffset = 90;
        fSpeed = maxFloor / 5 * 0.5f;
        bool bJumpFloor = UnityEngine.Random.Range(0, 100) >= nOffset ? true : false;
        if (bJumpFloor)
        {
            // 쉬는 공간 오브젝트 만들기
            RestZone obj = Instantiate(m_restZonePrefabs, m_parentOfObj.transform).GetComponent<RestZone>();
            m_restZoneList.Add(obj);
            obj.setFloor(maxFloor+1, onHitPlayer);
            maxFloor += 2;
        }
        else
            ++maxFloor;
    }

    void makeObstacleOBJ(int nCount)
    {
        Vector3 curPos = new Vector3(0, 0, 0);
        int curObstacleIndex = 0;
        int nDirection = 0;


        for (int i = 0; i < nCount; ++i)
        {
            curPos.y = i;
            // curfloor direction set
            nDirection = UnityEngine.Random.Range(0, 100) > 50 ? -1 : 1;
            float fOffsetSpeed = 0;
            getNextFloor(ref fOffsetSpeed);
            C_obstacleFloor newFloor = Instantiate(m_floorPrefabs, m_parentOfObj.transform).GetComponent<C_obstacleFloor>();
            newFloor.setFloor(maxFloor, nDirection, fOffsetSpeed, onHitPlayer);
            for (int j = 0; j < 4; ++j)
            {
                curObstacleIndex = UnityEngine.Random.Range(0, 6);
                obstacleOBJ obj = Instantiate(m_objPrefabs[curObstacleIndex], m_parentOfObj.transform).GetComponent<obstacleOBJ>();
                newFloor.m_list.Add(obj);
            }
            newFloor.setObstacleDirection();
            m_objList.Add(newFloor);
        }

    }

    void onHitPlayer()
    {
        if (m_fCurrentOnHitCoolTime > 0)
            return;
        if (m_bEndGame)
            return;
        m_fCurrentOnHitCoolTime = m_fOnHitCoolTime;
        --m_nPlayerHP;
        if (m_nPlayerHP <= 0)
        {
            m_nPlayerHP = 0;
            m_bEndGame = true;
        }
        m_funcList[(int)FUNC_TYPE.ON_HIT](m_nPlayerHP);
    }

    #endregion

    public void startGame()
    {
        if (m_bEndGame == false)
            return;
        m_bEndGame = false;
        moveVerticalOBJ();
    }

    public void setGame()
    {
        if (m_Instance == null)
            m_Instance = this;
        // 세팅
        m_fCurrentStayTime          = 0;
        m_carmera.pixelRect         = new Rect(0,0,544,768);
        m_fCurrentOnHitCoolTime     = 0;
        m_nCurMoveLength            = 0;
        maxFloor                    = 0;
        m_nCurCharacterPosX         = 0;
        m_playerCharacterOBJ.transform.localPosition = new Vector3(m_nCurCharacterPosX, 0);
        m_nPlayerHP                 = m_nStartPlayerHP;
        m_bEndGame = true;
        makeObstacleOBJ(12);

        m_gameStageOBJ.SetActive(true);

        m_funcList[(int)FUNC_TYPE.START_GAME](m_nPlayerHP);
    }

    public void closeGame()
    {
        // 초기화 
        for(int i = 0; i < (int)FUNC_TYPE.TYPE_SIZE; ++i)
        {
            m_funcList[i] = null;
        }

        clearList();
        m_gameStageOBJ.SetActive(false);
    }


    void clearList()
    {
        foreach (C_obstacleFloor obj in m_objList)
        {
            obj.removeAll();
            DestroyImmediate(obj.gameObject);
        }
        m_objList.Clear();

        foreach (RestZone obj in m_restZoneList)
            DestroyImmediate(obj.gameObject);
        m_restZoneList.Clear();
    }
}
