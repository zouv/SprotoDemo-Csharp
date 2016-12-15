/***
* Author: zouv
* Date: 2015-08-11
* Doc: 协议事件封装
***/

using UnityEngine;
using System.Collections;
using Sproto;

public class ProtoMsg
{
    private SprotoTypeBase _msg;
    private NetManager.ProtoMsgHandler _respHandler;
    private int _tag;

    public ProtoMsg(SprotoTypeBase msg, NetManager.ProtoMsgHandler respHandler)
    {
        _msg = msg;
        _respHandler = respHandler;
    }

    public ProtoMsg(SprotoTypeBase msg, int tag)
    {
        _msg = msg;
        _tag = tag;
    }

    public void Handle()
    {
        if (_respHandler != null)
        {
            _respHandler(_msg);
        }
        else
        {
            NetManager.ProtoMsgHandler handler = ProtoMsgListener.GetInstance().Get(_tag);
            handler(_msg);
        }
    }
}
