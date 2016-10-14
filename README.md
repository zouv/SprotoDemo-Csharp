# SprotoDemo-Csharp
sproto demo for unity&amp;c#

# Introduction
NetManager.cs : send and receive.<br>
ProtoMsgListener.cs : proto message listenter, handle proto message from server.<br>
Sproto-Csharp is a C# implementation of sproto https://github.com/lvzixun/sproto-Csharp

## testing
###### listener:
file: Assets/Script/UI/TestWin.cs
```javascript
void Start ()
{
  // 监听服务端主推的协议
  ProtoMsgListener.GetInstance().Add<Protocol.sc_map_enter>(ProtoMapEnterHandler);
}

/**
 * 服务端通知进入场景
 **/
private void ProtoMapEnterHandler(SprotoTypeBase msg)
{
  var scMapEnterReq = (SprotoType.sc_map_enter.request)msg;
  Debug.Log("ProtoMapEnterHandler___" + scMapEnterReq.map_id);
}
```

###### send:
file: Assets/Script/UI/TestWin.cs
```javascript
/**
 * 发送登录协议
 **/
private void OnClickLogin(GameObject go, PointerEventData ed)
{
  _txtTips.text = "login ..";
  var msg = new SprotoType.cs_login.request();
  msg.account = "zouv1";
  msg.password = "mypassword";
  NetManager.GetInstance().Send<Protocol.cs_login>(msg, ProtoLoginResponseHandler);
}

/**
 * 发送创建角色协议
 **/
private void OnClickCreate(GameObject go, PointerEventData ed)
{
  _txtTips.text = "create role ..";
  var msg = new SprotoType.cs_create.request();
  msg.name = "test1";
  NetManager.GetInstance().Send<Protocol.cs_create>(msg, ProtoCreateResponseHandler);
}
```
