using PROTOCOL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using PACKET;

public class friendUI : UI
{
    public enum INDEX_OF_FRIEND_UI
    {
        RESULT_UI,
    }

    bool                    m_bSetting;

    #region ListZone
    [SerializeField]
    GameObject              m_ListZoneOBJ;
    [SerializeField]
    GameObject              m_PlzAddFriendText;
    [SerializeField]
    RectTransform           m_ParentOfFriendList;
    [SerializeField]
    GameObject              m_FriendSlotPrefab;
    [SerializeField]
    List<friendSlot>        m_FriendList = new List<friendSlot>();
    [SerializeField]
    int                     m_SlotSizeOffsetY;

    
    #endregion


    #region SearchZone
    [SerializeField]
    GameObject              m_SearchZoneOBJ;

    [SerializeField]
    InputField              m_TargetUserNicknameIF;
    [SerializeField]
    Text                    m_TargetUserInfoText;
    bool                    m_bSuccessSearch;
    //S_FRIEND_INFO           m_TargetUserInfo;
    #endregion




    #region Basic UI
    public override void initUI(UI manager, int uiType)
    {
        base.initUI(manager, uiType);
        // 기본 세팅
    }
    public override void update(C_BasePacket eventData)
    {
        // 만약 서브 ui가 열려있다면 그쪽으로 throw
        if (isExistSubUI(eventData) == true)
            return;
        else if (m_listener != null)
            m_listener(eventData);
    }

    public override void releaseUI()
    {
        // 중단
        m_bSetting = false;
    }

    protected override void setUI()
    {
        m_bSetting = false;
        // 초기화
        m_bWaiting = false;

        openListZone();
        m_bSetting = true;
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

    }

    /// <summary>
    /// 친구 리스트 초기화
    /// </summary>
    void clearFriendList() 
    {
        // clear 작업
        while(m_FriendList.Count>0)
            removeSlot_list(0);
        m_FriendList.Clear();
    }

    #endregion


    #region CHAT BOX WHISPER
    public delegate void setWhisperTarget(string nickname);
    public setWhisperTarget m_dWhisper;

    #endregion


    #region EVENT

    /// <summary>
    /// 길드원 귓속말 BTN 클릭시 
    /// </summary>
    /// <param name="nickname"></param>
    void clickWhisper(string nickname)
    {
        if (m_dWhisper != null)
            m_dWhisper(nickname);
    }

    /// <summary>
    /// 친구 리스트의 slot을 삭제한다.
    /// </summary>
    /// <param name="nIndex"></param>
    void removeSlot_list(int nIndex)
    {
        if (m_FriendList.Count <= nIndex) return;
        DestroyImmediate(m_FriendList[nIndex].gameObject);
        m_FriendList.RemoveAt(nIndex);
        m_ParentOfFriendList.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10 + m_SlotSizeOffsetY * m_FriendList.Count);
    }

    void makeFriendSlot(object info)
    {
        friendSlot newSlot = (Instantiate(m_FriendSlotPrefab,
            m_ParentOfFriendList).GetComponent<friendSlot>());  // slot 생성
        
        newSlot.setSlot(info, requestDeleteFriend,clickWhisper);          // 각 slot에 deleteFriend함수 연결시켜줘야함
        m_FriendList.Add(newSlot);                              // add list
    }
    void clearSearchZone()
    {
        // clear part
        m_bSuccessSearch = false;
        m_TargetUserNicknameIF.text = "";        // clear IF
        m_TargetUserInfoText.text = "";             // clear target User Info
    }


    #endregion

    #region Server REQ / RES

    void requestFriendList()
    {
        responseFriendList(null);
        //startWaiting(responseFriendList);
        //// 서버에 유저의 친구 리스트 req
        //eventData data = new friendListRequest();
        //data.deserialize(null);
        //GameManager.m_Instance.makePacket(data);
    }

    /// <summary>
    /// 친구 리스트 response process
    /// </summary>
    /// <param name="curEventData"></param>
    void responseFriendList(C_BasePacket curEventData)
    {
        //stopWaiting();
        //socialData curSocialData = (socialData)curEventData;
        //if (curSocialData.m_type != SOCIAL_TYPE.FRIEND_LIST_RESPONSE) return; // 현재 이벤트에 맞는 타입이 아니면 return

        //friendListResponse data = (friendListResponse)curSocialData; // 실제 데이터

        if (true)    // 성공 recv 시 리스트 추가
        {
            m_PlzAddFriendText.SetActive(false);
            for (int i = 0; i < 5; ++i)                                     // 일단, 임시로 5명의 친구를 추가
            {
                //S_FRIEND_INFO newInfo = new S_FRIEND_INFO();            // info 객체 생성
                //newInfo.m_nickname = "TestNickname_" + i;
                //makeFriendSlot(newInfo);                                // 객체 생성
            }
            // recttransform resizing
            m_ParentOfFriendList.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10 + m_SlotSizeOffsetY * m_FriendList.Count);
        }
        else
        {
            // 실패 recv 시 plzAddFriendText setActive(true)
            m_PlzAddFriendText.SetActive(true);
        }

    }

    void requestDeleteFriend(string userNickname)
    {
        responseDeleteFriend(null);
        return;
        startWaiting(responseDeleteFriend);
        // 서버에 선택된 유저의 아이디 전송
        int slotIndex = -1;
        try
        {
            slotIndex = m_FriendList.FindIndex(x => { return (x.compareNickname(userNickname) == 0); });
        }
        catch(Exception ex)
        {
            GameManager.m_Instance.writeErrorLog("해당 Slot을 찾을 수 없음\n" + ex.Message);
            ((ResultUI)m_uiList[(int)INDEX_OF_FRIEND_UI.RESULT_UI]).setResultMSG("해당 Slot을 찾을 수 없음");
            openUI((int)INDEX_OF_FRIEND_UI.RESULT_UI);
        }

        //eventData data = new deleteFriendRequest();
        //data.deserialize((byte)slotIndex);
        //GameManager.m_Instance.makePacket(data);

    }

    void responseDeleteFriend(C_BasePacket curEventData)
    {
        //stopWaiting();
        //socialData curSocialData = (socialData)curEventData;
        //if (curSocialData.m_type != SOCIAL_TYPE.DELETE_FRIEND_RESPONSE) return; // 현재 이벤트에 맞는 타입이 아니면 return

        //deleteFriendResponse data = (deleteFriendResponse)curSocialData; // 실제 데이터

        if (false)    // 성공 recv 시 성공 팝업 및 현재 친구 리스트에서 제거
        {
            //removeSlot_list((byte)data.m_eventData);
        }
        else        // 실패 recv 시 실패 팝업
        {
            ((ResultUI)m_uiList[(int)INDEX_OF_FRIEND_UI.RESULT_UI]).setResultMSG("삭제 실패");
            openUI((int)INDEX_OF_FRIEND_UI.RESULT_UI);
        }
    }


    /// <summary>
    /// 유저 검색 try
    /// </summary>
    public void requestSearchFriend()
    {
        responseSearchFriend(null);
        return;
        // user nickname text check
        if (m_TargetUserNicknameIF.text.Length <= 0)
            return;

        startWaiting(responseSearchFriend);
        // 서버에 전송
        //eventData data = new searchFriendRequest();
        //S_FRIEND_INFO info = new S_FRIEND_INFO();
        //info.m_nickname = m_TargetUserNicknameIF.text;
        //data.deserialize(info);
        //GameManager.m_Instance.makePacket(data);
    }
    void responseSearchFriend(C_BasePacket curEventData)
    {
        //stopWaiting();
        //socialData curSocialData = (socialData)curEventData;
        //if (curSocialData.m_type != SOCIAL_TYPE.SEARCH_FRIEND_RESPONSE) return; // 현재 이벤트에 맞는 타입이 아니면 return

        //searchFriendResponse data = (searchFriendResponse)curSocialData; // 실제 데이터

        if (false)
        {
            // 검색된 유저 데이터 저장
            //m_TargetUserInfo = (S_FRIEND_INFO)data.m_eventData;
            // info text에 검색된 유저 데이터 저장
            //m_TargetUserInfoText.text = "대충 " + m_TargetUserInfo.m_nickname + "이라는 유저의 데이터";
            m_bSuccessSearch = true;
        }
        else    // data 가 null 이면 실패.
        {
            ((ResultUI)m_uiList[(int)INDEX_OF_FRIEND_UI.RESULT_UI]).setResultMSG("존재 하지 않는 유저입니다.");  // 실패 recv 시 실패 팝업 return
            openUI((int)INDEX_OF_FRIEND_UI.RESULT_UI);
        }
        m_TargetUserNicknameIF.text = "";
    }

    public void requestAddFriend()
    {
        responseAddFriend(null);
        //if (m_bSuccessSearch == false)  // 현재 검색된 유저가 없으면 return
        //    return;
        

        //startWaiting(responseAddFriend);
        //// 현재 검색된 유저의 데이터 동봉해서 서버에 req
        //eventData data = new addFriendRequest();
        //data.deserialize(null);
        //GameManager.m_Instance.makePacket(data);
    }

    void responseAddFriend(C_BasePacket curEventData)
    {
        //stopWaiting();
        //socialData curSocialData = (socialData)curEventData;
        //if (curSocialData.m_type != SOCIAL_TYPE.ADD_FRIEND_RESPONSE) return; // 현재 이벤트에 맞는 타입이 아니면 return

        //addFriendResponse data = (addFriendResponse)curSocialData; // 실제 데이터

        if (true)   // 현재는 무조건 생성
        {
            //makeFriendSlot(m_TargetUserInfo);                       // 생성
            // recttransform resizing
            m_ParentOfFriendList.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10 + m_SlotSizeOffsetY * m_FriendList.Count);
            clearSearchZone();
            ((ResultUI)m_uiList[(int)INDEX_OF_FRIEND_UI.RESULT_UI]).setResultMSG("친구 추가 성공!");
            openUI((int)INDEX_OF_FRIEND_UI.RESULT_UI);
        }
        else        // 실패 recv 시 실패 팝업
        {
            ((ResultUI)m_uiList[(int)INDEX_OF_FRIEND_UI.RESULT_UI]).setResultMSG("친구 추가 할 수 없습니다.");  // 실패 recv 시 실패 팝업 return
            openUI((int)INDEX_OF_FRIEND_UI.RESULT_UI);
        }
    }
    #endregion

    #region UI OPEN / CLOSE
    /// <summary>
    /// 현재 UI 닫기 BTN
    /// </summary>
    public void exitUI()
    {
        m_uiManager.closeUI(1);
        m_uiManager.setOnCloseSubUI();
    }


    public void openListZone()
    {
        m_SearchZoneOBJ.SetActive(false);
        m_ListZoneOBJ.SetActive(true);

        // if bSet == false
        if(m_bSetting == false)
        {
            clearFriendList();          // 리스트 초기화
            requestFriendList();        // 리스트 요청
        }
        else // else if change list -> list sorting
        {

        }
    }

    public void openSearchZone()
    {
        m_SearchZoneOBJ.SetActive(true);
        m_ListZoneOBJ.SetActive(false);
        clearSearchZone();
    }
    
    #endregion
}