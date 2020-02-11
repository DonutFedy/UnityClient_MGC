﻿//#define NOTLOGINSERVER

using PACKET;
using PROTOCOL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controllerUI : UI
{
    [SerializeField]
    GameObject m_waitReconnectUI;
    public enum INDEX_OF_CONTROLLER_UI
    {
        LOGIN_UI = 0,
        MAINMENU_UI= 1,
        GAMEROOM_UI = 2,
        SHOP_UI= 3,
    }


    public override void initUI(UI manager, int uiType)
    {
        base.initUI(manager, uiType);
        // 기본 세팅

    }
    protected void onAnomalyEvnet(C_BasePacket curevent)
    {
        C_Anomaly data = (C_Anomaly)curevent;

        switch (data.m_type)
        {
            case AnomalyType.loginServer_Reconnect:
                m_waitReconnectUI.SetActive(false);
                break;
            case AnomalyType.loginServer_Disconnect:
#if NOTLOGINSERVER
                return;
#endif
                m_waitReconnectUI.SetActive(true);
                m_uiList[m_uiIndexStack.Peek()].stopWaitingUI();
                m_uiList[m_uiIndexStack.Peek()].closeUI(1);
                break;
            case AnomalyType.mainServer_Reconnect:
                break;
            case AnomalyType.mainServer_Disconnect:
                while (m_uiIndexStack.Count > 1)
                {
                    closeUI(1);
                }
                m_uiList[m_uiIndexStack.Peek()].releaseUI();

                GameManager.m_Instance.connect_loginServer();
                break;
        }
    }
    public override void update(C_BasePacket eventData)
    {
        if (eventData.m_basicType == BasePacketType.basePacketTypeSize)
        {
            onAnomalyEvnet(eventData);
            return;
        }
        isExistSubUI(eventData);
    }

    public override void releaseUI()
    {
        // 중단
    }

    protected override void setUI()
    {
        // 초기화
        m_bWaiting = false;
    }

    protected override void startWaiting(responseListener listener)
    {
        base.startWaiting(listener);
        // ~~
    }
    protected override void stopWaiting()
    {
        base.stopWaiting();
    }

    protected override void processInputKey(inputKeyManager.S_KeyInput.KeyType type)
    {
        if (isExistSubUI(type))
            return;
    }

    public void setGameRoom(bool bMakeRoom, int nSlotIndex, string roomName, int nRoomNumber, string roomMode, bool bPublic, int nRoomLimit, List<S_RoomUserInfo> userList)
    {
        ((gameRoomUI)m_uiList[(int)INDEX_OF_CONTROLLER_UI.GAMEROOM_UI]).setGameRoom(
            bMakeRoom, nSlotIndex,roomName, nRoomNumber,roomMode, bPublic, nRoomLimit, userList, string.Empty);
    }
    
}