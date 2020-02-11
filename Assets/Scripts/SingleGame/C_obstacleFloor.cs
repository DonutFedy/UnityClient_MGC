using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_obstacleFloor : floor
{
    // obj rule
    public static float fLimitLeftX = -4;
    public static float fLimitRightX = 4;
    public static float fLimitUpY = 9;
    public static float fLimitDownY = -3;
    public List<obstacleOBJ> m_list;
    public float        m_fOffsetSpeed;

    public void setFloor(int nFloor, int nDirection, float fOffsetSpeed, obstacleOBJ.onHitPlayer func)
    {
        m_list = new List<obstacleOBJ>();
        m_curFloor = nFloor;
        m_nDirection = nDirection;
        m_fOffsetSpeed = fOffsetSpeed;
        m_func = func;
    }

    public void setObstacleDirection()
    {
        Vector3 pos = new Vector3(0, m_curFloor - singleGameManager.m_nCurMoveLength, 0);
        float fLimitX = 0;
        int nXoffset = 0;
        if (m_nDirection > 0)
        {
            fLimitX = fLimitRightX;
            pos.x = fLimitLeftX;
        }
        else
        {
            fLimitX = fLimitLeftX;
            pos.x = fLimitRightX;
        }
        for (int i = 0; i < m_list.Count; ++i)
        {
            nXoffset += UnityEngine.Random.Range(0, 2) + i;
            m_list[i].setOBJ(m_nDirection, pos + Vector3.right * m_nDirection * -1 * (nXoffset), fLimitX, m_fOffsetSpeed, m_func);
        }
    }


    public override void moveDown()
    {
        if ((m_curFloor - singleGameManager.m_nCurMoveLength) <= fLimitDownY)
        {
            singleGameManager.m_Instance.getNextFloor(ref m_fOffsetSpeed);
            m_curFloor = singleGameManager.maxFloor;
            m_nDirection = UnityEngine.Random.Range(1, 101) > 50 ? -1 : 1;
            setObstacleDirection();
            return;
        }
        for (int i = 0; i < m_list.Count; ++i)
        {
            m_list[i].moveDown();
        }
    }
    public void moveHor()
    {
        for (int i = 0; i < m_list.Count; ++i)
        {
            m_list[i].updateOBJ();
        }
    }

    public void removeAll()
    {
        for (int i = 0; i < m_list.Count; ++i)
        {
            DestroyImmediate(m_list[i].gameObject);
        }
        m_list.Clear();
    }
}
