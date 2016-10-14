# SprotoDemo-Csharp
sproto demo for unity&amp;c#

# Introduction
NetManager.cs : send and receive.<br>
ProtoMsgListener.cs : proto message listenter, handle proto message from server.<br>


## testing
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
