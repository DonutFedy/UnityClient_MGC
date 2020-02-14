using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PACKET
{
    public enum BasePacketType : byte
    {
        basePacketTypeNone = 0,
        // client to server packet
        basePacketTypeLogin,
        basePacketTypePreLoad,
        basePacketTypeRoom,
        basePacketTypeGame,
        basePacketTypeMarket,
        basePacketTypeRanking,
        basePacketTypeSocial,
        basePacketTypeSize,
    };

    public enum LoginPacketType : byte
    {
        loginPacketTypeNone = 0,

        loginPacketTypeLoginResponse, 
        loginPacketTypeLoginRequest,  // d

        loginPacketTypeLogoutRequest,  // d

        loginPacketTypeSignupResponse,
        loginPacketTypeSignupRequest,  // d

        loginPacketTypeDeleteResponse, 
        loginPacketTypeDeleteRequest,  // d

        loginPacketTypeShowChannelResponse, 
        loginPacketTypeShowChannelRequest,  // d

        loginPacketTypeChannelInResonse,
        loginPacketTypeChannelInRequest,    // d

        loginPacketTypeSize,
    };

    public enum PreLoadType : byte
    {
        packetTypeNone = 0,

        preLoadPlayerInfo,

        packetTypeCount,
    }

    public enum RoomPacketType : byte
    {
        roomPacketTypeNone = 0,

        roomPacketTypeMakeRoomRequest , // c->s
        roomPacketTypeMakeRoomResponse , // s -> c

        roomPacketTypeRoomListRequest, // c -> s
        roomPacketTypeRoomListResponse, // s->c

        roomPacketTypeEnterRoomRequest,  // c -> s
        roomPacketTypeEnterRoomResponse,  // s->c

        roomPacketTypeLeaveRoomRequest, // c->s
        roomPacketTypeLeaveRoomResponse, // s->c

        packetTypeRoomRoomInfoRequest, // c->s
        packetTypeRoomRoomInfoResponse, //GameServer -> Client Broadcast (It will be send when new player enter the room)

        packetTypeRoomToggleReadyRequest, //Client -> GameServer

        roomPacketTypeCount, // limit
    }

    public enum SocialPacketType : byte
    {
        packetTypeSocialNone,

        //chat
        packetTypeSocialChatNormalRequest,
        packetTypeSocialChatNormalResponse,

        packetTypeSocialChatFriendRequest,
        packetTypeSocialChatFriendResponse,

        packetTypeSocialChatGuildRequest,
        packetTypeSocialChatGuildResponse,

        //Friend
        packetTypeSocialAddFriendRequest, //친추 요청
        packetTypeSocialAddFriendResponse,

        packetTypeSocialConfirmFriendRequest, //받은 친추 목록
        packetTypeSocialConfirmFriendResponse,

        packetTypeSocialAcceptFriendRequest, //친구 요청 수락
        packetTypeSocialAcceptFriendResponse,
        packetTypeSocialFriendListRequest, //친구 리스트
        packetTypeSocialFriendListResponse,

        packetTypeSocialCount,
    }


    public enum ErrorTypeAddFriend : byte
    {
        none = 0,

	    notExistPlayer,
	    srcFriendListIsFull, 
	    destFriendListIsFull,
	    destFriendRequestListIsFull,
	    alreadySendRequest,

	    count,
    };


    public enum ErrorTypeAcceptFriend : byte
    {
        none = 0,

        srcFriendListIsFull,
        destFriendListIsFull,

        count,
    }


    public enum ErrorTypeEnterRoom : byte
    {
        errorTypeNone = 0,

        errorTypeNotExistRoom,
        errorTypeWrongPassword,
        errorTypeAlreadyIncluded,
        errorTypeGameStart,
        errorTypeMaxPlayer,
        errorTypeCanNotEnterRoom,
        errorTypePlayerLogout,

        errorTypeCount,
    };


    public enum AnomalyType : byte
    {
        loginServer_Reconnect,
        loginServer_Disconnect,
        mainServer_Reconnect,
        mainServer_Disconnect
    }
}
