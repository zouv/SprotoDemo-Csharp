/***
* Author: zouv
* Date: 2015-08-11
* Doc: 协议事件监听接口
***/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProtoMsgListener
{
    private static ProtoMsgListener _instance;
    private Dictionary<int, NetManager.ProtoMsgHandler> _dictMsghandler;

    public static ProtoMsgListener GetInstance()
    {
        if (_instance != null)
            return _instance;

        _instance = new ProtoMsgListener();
        return _instance;
    }

    public ProtoMsgListener()
    {
        _dictMsghandler = new Dictionary<int, NetManager.ProtoMsgHandler>();
    }

    public void Add<T>(NetManager.ProtoMsgHandler handle)
    {
        int tag = Protocol.Instance.Protocol[typeof(T)];
        _dictMsghandler[tag] = handle;
    }

    public void Remove<T>()
    {
        int tag = Protocol.Instance.Protocol[typeof(T)];
        _dictMsghandler.Remove(tag);
    }

    public NetManager.ProtoMsgHandler Get(int tag)
    {
        return _dictMsghandler[tag];
    }
}
