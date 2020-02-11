﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PACKET
{

    public abstract class C_BasePacket
    {
        public BasePacketType   m_basicType;
        public float            m_fEventTime;
        public bool             m_bResponse;


        /// <summary>
        /// 직렬화
        /// 데이터 -> byte[]
        /// </summary>
        /// <returns></returns>
        public abstract C_Buffer serialize();

        /// <summary>
        /// 역 직렬화
        /// byte[] -> 데이터
        /// </summary>
        /// <param name="buf"></param>
        public abstract void deserialize(C_Buffer buf);
        
    }


    #region LOGIN


    public abstract class C_LoginPacket : C_BasePacket
    {
        public LoginPacketType m_loginType;

        protected void setType(LoginPacketType type)
        {
            m_basicType = BasePacketType.basePacketTypeLogin;
            m_loginType = type;
        }
    }

    public class C_LoginRequestPacket : C_LoginPacket
    {
        public string       m_accountID;
        public string       m_accountPW;
        public C_LoginRequestPacket()
        {
            setType(LoginPacketType.loginPacketTypeLoginRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_accountID);
            buf.get(ref m_accountPW);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            buf.set(m_accountID);
            buf.set(m_accountPW);
            return buf;
        }
    }
    public class C_LoginResponsePacket : C_LoginPacket
    {
        public bool         m_bFlag;
        public string       m_nickname;
        public C_LoginResponsePacket()
        {
            setType(LoginPacketType.loginPacketTypeLoginResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_bFlag);
            if( m_bFlag)
            {
                buf.get(ref m_nickname);
            }
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            buf.set(m_bFlag);
            if(m_bFlag)
            {
                buf.set(m_nickname);
            }
            return buf;
        }
    }

    public class C_ChannelListRequestPacket : C_LoginPacket
    {
        public C_ChannelListRequestPacket()
        {
            setType(LoginPacketType.loginPacketTypeShowChannelRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            return buf;
        }
    }
    public class C_ChannelListResponsePacket : C_LoginPacket
    {
        const int           CHANNEL_SIZE = 4;
        // CHANNEL_SIZE
        public List<S_Channel> m_channelList = new List<S_Channel>();
        public C_ChannelListResponsePacket()
        {
            setType(LoginPacketType.loginPacketTypeShowChannelResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            for (int i = 0; i < CHANNEL_SIZE; ++i)
            {
                S_Channel newChannel = new S_Channel();
                buf.get(ref newChannel.m_channelName);
                buf.get(ref newChannel.m_nNumberOfPeople);
                buf.get(ref newChannel.m_nLimitOfPeople);
                m_channelList.Add(newChannel);
            }
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            return buf;
        }
    }
    public class C_LogoutRequestPacket : C_LoginPacket
    {
        public C_LogoutRequestPacket()
        {
            setType(LoginPacketType.loginPacketTypeLogoutRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);

            return buf;
        }
    }

    public class C_ChannelInRequestPacket : C_LoginPacket
    {
        public byte         m_nChannelNumber;
        public C_ChannelInRequestPacket()
        {
            setType(LoginPacketType.loginPacketTypeChannelInRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            buf.set(m_nChannelNumber);
            return buf;
        }
    }
    public class C_ChannelInResponsePacket : C_LoginPacket
    {
        public bool         m_bResult;
        public string       m_ip;
        public Int16        m_port;
        public C_ChannelInResponsePacket()
        {
            setType(LoginPacketType.loginPacketTypeChannelInResonse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_bResult);
            if (m_bResult)
            {
                buf.get(ref m_ip);
                buf.get(ref m_port);
            }
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            return buf;
        }
    }
    public class C_DeleteRequestPacket : C_LoginPacket
    {
        public C_DeleteRequestPacket()
        {
            setType(LoginPacketType.loginPacketTypeDeleteRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            return buf;
        }
    }
    public class C_DeleteResponsePacket : C_LoginPacket
    {
        public bool         m_bResult;
        public C_DeleteResponsePacket()
        {
            setType(LoginPacketType.loginPacketTypeDeleteResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_bResult);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            buf.set(m_bResult);
            return buf;
        }
    }
    public class C_SignupRequestPacket : C_LoginPacket
    {
        public string       m_accountID;
        public string       m_accountPW;
        public string       m_nickname;
        public C_SignupRequestPacket()
        {
            setType(LoginPacketType.loginPacketTypeSignupRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_accountID);
            buf.get(ref m_accountPW);
            buf.get(ref m_nickname);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            buf.set(m_accountID);
            buf.set(m_accountPW);
            buf.set(m_nickname);
            return buf;
        }
    }
    public class C_SignupResponsePacket : C_LoginPacket
    {
        public bool         m_bResult;
        public C_SignupResponsePacket()
        {
            setType(LoginPacketType.loginPacketTypeSignupResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_bResult);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_loginType);
            buf.set(m_bResult);
            return buf;
        }
    }



    #endregion


    #region ROOM



    public abstract class C_BaseRoomPacket : C_BasePacket
    {
        public RoomPacketType       m_roomType;


        protected void setType(RoomPacketType type)
        {
            m_basicType = BasePacketType.basePacketTypeRoom;
            m_roomType = type;
        }
    }

    public class C_RoomPacketMakeRoomRequest : C_BaseRoomPacket
    {
        public string       m_roomName;
        public Int16        m_maxPlayer = 0;
        public Int16        m_password = 0;

        public C_RoomPacketMakeRoomRequest()
        {
            setType(RoomPacketType.roomPacketTypeMakeRoomRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_roomName);
            buf.get(ref m_maxPlayer);
            buf.get(ref m_password);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            buf.set(m_roomName);
            buf.set(m_maxPlayer);
            buf.set(m_password);
            return buf;
        }
    }

    public class C_RoomPacketMakeRoomResponse : C_BaseRoomPacket
    {
        public bool         m_success = false;
        public Int32        m_roomNumber = 0;

        public C_RoomPacketMakeRoomResponse()
        {
            setType(RoomPacketType.roomPacketTypeMakeRoomResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_success);
            buf.get(ref m_roomNumber);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            buf.set(m_success);
            if(m_success)
                buf.set(m_roomNumber);
            return buf;
        }
    }

    public class C_RoomPacketRoomListResponse : C_BaseRoomPacket
    {
        public Int16                m_listCount = 0;
        public List<S_RoomInfo>     m_roomList = new List<S_RoomInfo>();

        public C_RoomPacketRoomListResponse()
        {
            setType(RoomPacketType.roomPacketTypeRoomListResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_listCount);

            for(int i = 0; i < m_listCount; ++i)
            {
                S_RoomInfo newRoomInfo = new S_RoomInfo();
                buf.get(ref newRoomInfo.m_number);
                buf.get(ref newRoomInfo.m_nCreateTime);
                buf.get(ref newRoomInfo.m_playerCount);
                buf.get(ref newRoomInfo.m_maxPlayerCount);
                buf.get(ref newRoomInfo.m_bIsStart);
                buf.get(ref newRoomInfo.m_password);
                buf.get(ref newRoomInfo.m_roomName);

                m_roomList.Add(newRoomInfo);
            }
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            buf.set(m_listCount);
            for(int i = 0; i < m_listCount; ++i)
            {
                buf.set(m_roomList[i].m_number);
                buf.set(m_roomList[i].m_nCreateTime);
                buf.set(m_roomList[i].m_playerCount);
                buf.set(m_roomList[i].m_maxPlayerCount);
                buf.set(m_roomList[i].m_bIsStart);
                buf.set(m_roomList[i].m_password);
                buf.set(m_roomList[i].m_roomName);
            }
            return buf;
        }
    }

    public class C_RoomPacketRoomListRequest : C_BaseRoomPacket
    {
        public Int16            m_page;
        public C_RoomPacketRoomListRequest()
        {
            setType(RoomPacketType.roomPacketTypeRoomListRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_page);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            buf.set(m_page);
            return buf;
        }
    }


    public class C_RoomPacketEnterRoomRequest : C_BaseRoomPacket
    {
        public Int32 m_roomNumber = 0;
        public Int64 m_nRoomTime = 0;
        public Int16 m_password = 0;

        public C_RoomPacketEnterRoomRequest()
        {
            setType(RoomPacketType.roomPacketTypeEnterRoomRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_roomNumber);
            buf.get(ref m_nRoomTime);
            buf.get(ref m_password);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            buf.set(m_roomNumber);
            buf.set(m_nRoomTime);
            buf.set(m_password);
            return buf;
        }
    }

    public class C_RoomPacketEnterRoomResponse : C_BaseRoomPacket
    {
        public bool                 m_bIsSuccess;
        public Int16                m_nSlotIndex;
        public ErrorTypeEnterRoom   m_errorType;

        public C_RoomPacketEnterRoomResponse()
        {
            setType(RoomPacketType.roomPacketTypeEnterRoomResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_bIsSuccess);
            buf.get(ref m_nSlotIndex);
            buf.get(ref m_errorType);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            buf.set(m_bIsSuccess);
            buf.set(m_nSlotIndex);
            buf.set(m_errorType);
            return buf;
        }
    }

    public class C_RoomPacketLeaveRoomRequest : C_BaseRoomPacket
    {
        public C_RoomPacketLeaveRoomRequest()
        {
            setType(RoomPacketType.roomPacketTypeLeaveRoomRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            return buf;
        }
    }

    public class C_RoomPacketLeaveRoomResponse : C_BaseRoomPacket
    {
        public bool m_bIsSuccess;

        public C_RoomPacketLeaveRoomResponse()
        {
            setType(RoomPacketType.roomPacketTypeLeaveRoomResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_bIsSuccess);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            buf.set(m_bIsSuccess);
            return buf;
        }
    }
    
    public class C_RoomPacketRoomInfoRequest : C_BaseRoomPacket
    {
        public C_RoomPacketRoomInfoRequest()
        {
            setType(RoomPacketType.packetTypeRoomRoomInfoRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            return buf;
        }
    }
    public class C_RoomPacketRoomInfoResponse : C_BaseRoomPacket
    {
        public Int16                    m_listCount;
        public List<S_RoomUserInfo>     m_userList = new List<S_RoomUserInfo>();
        public C_RoomPacketRoomInfoResponse()
        {
            setType(RoomPacketType.packetTypeRoomRoomInfoResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_listCount);

            for(int i = 0; i < m_listCount; ++i)
            {
                S_RoomUserInfo info = new S_RoomUserInfo();
                buf.get(ref info.m_nSlotIndex);
                buf.get(ref info.m_bIsMaster);
                buf.get(ref info.m_userNickname);
                buf.get(ref info.m_nCharacterImageIndex);
                buf.get(ref info.m_bReadyState);
                m_userList.Add(info);
            }
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            buf.set(m_listCount);
            
            foreach(S_RoomUserInfo info in m_userList)
            {
                buf.set(info.m_nSlotIndex);
                buf.set(info.m_bIsMaster);
                buf.set(info.m_userNickname);
                buf.set(info.m_nCharacterImageIndex);
                buf.set(info.m_bReadyState);
            }

            return buf;
        }
    }

    public class C_RoomPacketToogleReadyRequest : C_BaseRoomPacket
    {
        public C_RoomPacketToogleReadyRequest()
        {
            setType(RoomPacketType.packetTypeRoomToggleReadyRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_roomType);
            return buf;
        }
    }


    #endregion


    #region SOCIAL

    public abstract class C_BaseSocialPacket : C_BasePacket {
        public SocialPacketType     m_socialType;

        protected void setType(SocialPacketType type)
        {
            m_basicType = BasePacketType.basePacketTypeSocial;
            m_socialType = type;
        }
    }

    public class C_SocialPacketChatRornalRequest : C_BaseSocialPacket {
        public string               m_message;
        public C_SocialPacketChatRornalRequest()
        {
            setType(SocialPacketType.packetTypeSocialChatNormalRequest);
            m_bResponse = false;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_message);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_socialType);
            buf.set(m_message);
            return buf;
        }
    }
    public class C_SocialPacketChatRornalResponse : C_BaseSocialPacket
    {
        public string       m_nickname;
        public string       m_message;
        public C_SocialPacketChatRornalResponse()
        {
            setType(SocialPacketType.packetTypeSocialChatNormalResponse);
            m_bResponse = true;
        }
        public override void deserialize(C_Buffer buf)
        {
            buf.get(ref m_nickname);
            buf.get(ref m_message);
        }

        public override C_Buffer serialize()
        {
            C_Buffer buf = new C_Buffer();
            buf.set((byte)m_basicType);
            buf.set((byte)m_socialType);
            buf.set(m_nickname);
            buf.set(m_message);
            return buf;
        }
    }


    #endregion
    public class C_Anomaly : C_BasePacket
    {
        public C_Anomaly()
        {
            m_basicType = BasePacketType.basePacketTypeSize;
        }
        public AnomalyType  m_type;
        public override void deserialize(C_Buffer buf)
        {
            throw new System.NotImplementedException();
        }

        public override C_Buffer serialize()
        {
            throw new System.NotImplementedException();
        }
        public void setType(AnomalyType type)
        {
            m_type = type;
        }
    }

}