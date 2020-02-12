using System.Collections;
using System.Collections.Generic;
using PACKET;
using UnityEngine;

public class ingameManager : management {
    
    public override void init()
    {
        // 진우님이 만든 인게임 매니저 set
    }

    public override void release()
    {
        // 진우 님이 만든 인게임 매니저 release
    }
    protected override void processEvent(C_BasePacket curEvent)
    {
        // 진우 님이 만든 인 게임 매니저에게 대 분류 타입부분만 떼고 보내기
        // use-> byte[] realData = getData(curEvent);
    }
    byte[] getData(C_BasePacket packet)
    {
        return null;
    }
}
