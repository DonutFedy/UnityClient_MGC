using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestZone : floor
{
    [SerializeField]
    List<SpriteRenderer>        m_arrowList = new List<SpriteRenderer>();
    Color                       m_curColor;

    private void OnEnable()
    {
        m_curColor = Color.white;
        m_nDirection = -1;
    }

    // Update is called once per frame
    void Update()
    {
        m_curColor.b += m_nDirection * Time.deltaTime;

        if((m_nDirection <0 && m_curColor.b <= 0.5f)||(m_nDirection > 0 && m_curColor.b >= 1.0f))
            m_nDirection *= -1;
        for (int i = 0; i < m_arrowList.Count; ++i)
            m_arrowList[i].color = m_curColor;
    }
    public void setFloor(int nFloor)
    {
        m_curFloor = nFloor;
        gameObject.transform.localPosition = Vector3.up* realFloor();
    }

    int realFloor()
    {
        return m_curFloor - singleGameManager.m_nCurMoveLength;
    }

    public override void moveDown()
    {
        gameObject.transform.localPosition = Vector3.up * realFloor();
        if (realFloor() <= C_obstacleFloor.fLimitDownY)
        {
            gameObject.SetActive(false);
        }
    }
}
