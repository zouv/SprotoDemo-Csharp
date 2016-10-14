/***
* Author: zouv
* Date: 2016-07-01
* Doc: 网络管理
***/

using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using Sproto;
using System.Collections.Generic;

public class NetManager
{
    private static NetManager _instance;

    private Socket _socket;

    private SprotoStream _receStream = new SprotoStream();
    private int _session;
    public delegate void ProtoMsgHandler(SprotoTypeBase msg);
    private Dictionary<long, int> _dictSessionTag;
    private Dictionary<long, ProtoMsgHandler> _dictSessionResponHandler;
    private List<ProtoMsg> _listReceiveMsg;

    public static NetManager GetInstance()
    {
        if (_instance != null)
            return _instance;

        _instance = new NetManager();
        return _instance;
    }

    public NetManager()
    {
        _session = 1;
        _dictSessionTag = new Dictionary<long, int>();
        _dictSessionResponHandler = new Dictionary<long, ProtoMsgHandler>();
        _listReceiveMsg = new List<ProtoMsg>();
    }

    public void Connect()
    {
        AddressFamily af = AddressFamily.InterNetwork;
        _socket = new Socket(af, SocketType.Stream, ProtocolType.Tcp);
        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);

        string host = "192.168.4.111";
        int port = 8810;
        _socket.BeginConnect(host, port, new AsyncCallback(ConnectCallback), _socket);
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            _socket.EndConnect(ar);
        }
        catch (SocketException e)
        {
            Debug.LogError("Connect Callback Error: " + e.ToString());
            return;
        }

        byte[] receBuffer = new byte[1 << 16];
        _receStream.Write(receBuffer, 0, receBuffer.Length);
        _receStream.Seek(0, SeekOrigin.Begin);
        _socket.BeginReceive(_receStream.Buffer, 0, _receStream.Buffer.Length, 0, new AsyncCallback(ReceiveCallback), _socket);
    }

    public void Send<T>(SprotoTypeBase msg, ProtoMsgHandler handler = null)
    {
        int tag = Protocol.Instance.Protocol[typeof(T)];
        int session = GetProtoSession();
        Send(msg, tag, session);

        if (handler != null)
        {
            _dictSessionTag[session] = tag;
            _dictSessionResponHandler[session] = handler;
        }
    }

    private void Send(SprotoTypeBase msg, int tag, int session)
    {
        var pkg = new SprotoType.package();
        pkg.type = tag;
        pkg.session = session;
        var rpcStream = new SprotoStream();
        int len = pkg.encode(rpcStream);
        len += msg.encode(rpcStream);
        var sendPack = new SprotoPack();
        byte[] data = sendPack.pack(rpcStream.Buffer, len);
        byte[] lenBytes = BitConverter.GetBytes(data.Length);
        var sendStream = new SprotoStream();
        int headSize = 4;
        for (int i = 1; i <= headSize; i++)
        {
            sendStream.WriteByte(lenBytes[headSize - i]);
        }
        sendStream.Write(data, 0, data.Length);
        _socket.BeginSend(sendStream.Buffer, 0, data.Length + headSize, 0, new AsyncCallback(SendCallback), _socket);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            var handler = (Socket)ar.AsyncState;
            handler.EndSend(ar);
        }
        catch (SocketException e)
        {
            Debug.LogError("Send Callback Error: " + e.ToString());
        }
    }

    public void ReceiveCallback(IAsyncResult ar = null)
    {
        int headSize = 2;
        int size = 0;
        for (int i = _receStream.Position; i < headSize; i++)
        {
            size |= _receStream[i] << 8 * (headSize - i - 1);
        }
        int length = NetUtil.ReadBytes(_receStream.Buffer, 0, headSize);
        byte[] data = new byte[length];
        _receStream.Seek(headSize, SeekOrigin.Current);
        _receStream.Read(data, 0, length);
        var pkg = new SprotoType.package();
        SprotoPack recePack = new SprotoPack();
        byte[] unpackData = recePack.unpack(data);
        int offset = pkg.init(unpackData);
        long session = pkg.session;

        int tag;
        if (pkg.HasType)
        {
            tag = (int)pkg.type;
            ProtocolFunctionDictionary.typeFunc GenRequest = Protocol.Instance.Protocol[tag].Request.Value;
            SprotoTypeBase rpcResp = GenRequest(unpackData, offset);
            var receMsg = new ProtoMsg(rpcResp, tag);
            _listReceiveMsg.Add(receMsg);
        }
        else
        {
            if (_dictSessionTag.TryGetValue(session, out tag))
            {
                ProtocolFunctionDictionary.typeFunc GenResponse = Protocol.Instance.Protocol[tag].Response.Value;
                SprotoTypeBase rpcResp = GenResponse(unpackData, offset);
                ProtoMsgHandler responseHandler = _dictSessionResponHandler[session];
                var receMsg = new ProtoMsg(rpcResp, responseHandler);
                _dictSessionTag.Remove(session);
                _dictSessionResponHandler.Remove(session);
                _listReceiveMsg.Add(receMsg);
            }
            else
            {
                Debug.LogError("receive callback Warning! not found tag by session : " + session);
            }
        }

        _receStream.Seek(0, SeekOrigin.Begin);
        _socket.BeginReceive(_receStream.Buffer, 0, _receStream.Buffer.Length, 0, new AsyncCallback(ReceiveCallback), _socket);
    }

    public int GetProtoTag(System.Type findType)
    {
        System.Type t = typeof(Protocol);
        int tag = 0;
        foreach (System.Type type in t.GetNestedTypes())
        {
            if (findType.Name.Equals(type.Name))
            {
                tag = (int)type.GetField("Tag").GetValue(t);
                break;
            }
        }

        return tag;
    }

    public int GetProtoSession()
    {
        _session += 1;
        return _session;
    }

    public void Update()
    {
        HandleReceiveMsg();
    }

    private void HandleReceiveMsg()
    {
        for (; _listReceiveMsg.Count > 0;)
        {
            ProtoMsg protoMsg = _listReceiveMsg[0];
            protoMsg.Handle();
            _listReceiveMsg.RemoveAt(0);
        }
    }
}
