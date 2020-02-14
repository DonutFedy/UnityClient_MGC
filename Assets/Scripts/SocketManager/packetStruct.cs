using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;





namespace PACKET
{

    #region LOGIN

    public struct S_UserAccessData
    {
        public string       m_accessID;
        public string       m_accessPW;
    }

    public struct S_Channel
    {
        public string       m_channelName;
        public Int32        m_nNumberOfPeople;
        public Int32        m_nLimitOfPeople;
        public S_Channel(string name, int num, int limit)
        {
            m_channelName = name;
            m_nNumberOfPeople = num;
            m_nLimitOfPeople = limit;
        }
    }
    #endregion


    #region ROOM
    public struct S_RoomInfo
    {
        public Int16            m_number;
        public Int64 m_nCreateTime;
        public Int16            m_playerCount;
        public Int16            m_maxPlayerCount;
        public bool             m_bIsStart;
        public bool             m_password;
        public string           m_roomName;
        public S_RoomInfo(int num = 0, long nCreateTime = 0, int plycnt = 0, int maxcount = 0,bool bIsStart = false, bool pass = false, string str = "")
        {
            m_number = (Int16)num;
            m_nCreateTime = (Int64)nCreateTime;
            m_playerCount = (Int16)plycnt;
            m_maxPlayerCount = (Int16)maxcount;
            m_bIsStart = bIsStart;
            m_password = pass;
            m_roomName = str;
        }
    };

    public struct S_RoomUserInfo
    {
        public Int16        m_nSlotIndex; // 1부터 시작됨
        public bool         m_bIsMaster;
        public string       m_userNickname;
        public Int32        m_nCharacterImageIndex;
        public bool         m_bReadyState;

        public S_RoomUserInfo(int nSlotIndex, string userNickname, int nCharacterImageIndex, bool bReadyState, bool bIsMaster)
        {
            m_nSlotIndex = (Int16)nSlotIndex;
            m_userNickname = userNickname;
            m_nCharacterImageIndex = nCharacterImageIndex;
            m_bReadyState = bReadyState;
            m_bIsMaster = bIsMaster;
        }
    }

    public struct S_UserData
    {
        public UInt32       m_gpID;
        public string       m_nickName;
    }

    #endregion


}