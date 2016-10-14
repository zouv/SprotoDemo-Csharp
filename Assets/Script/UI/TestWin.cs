using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sproto;

public class TestWin : MonoBehaviour 
{
    private Text _txtTips;

	void Start ()
    {
        Button btnLogin = transform.Find("BtnLogin").GetComponent<Button>();
        Button btnCreate = transform.Find("BtnCreate").GetComponent<Button>();
        _txtTips = transform.Find("TxtTips").GetComponent<Text>();

        UIEventTriggerListener.Get(btnLogin.gameObject).onClick = OnClickLogin;
        UIEventTriggerListener.Get(btnCreate.gameObject).onClick = OnClickCreate;

        ProtoMsgListener.GetInstance().Add<Protocol.sc_map_enter>(ProtoMapEnterHandler); // 监听服务端主推的协议
	}
	
	void Update () 
    {
	
	}

    void Destory()
    {
        ProtoMsgListener.GetInstance().Remove<Protocol.sc_map_enter>();
    }

    /**
     * 发送登录协议
     **/
    private void OnClickLogin(GameObject go, PointerEventData ed)
    {
        _txtTips.text = "login ..";
        var msg = new SprotoType.cs_login.request();
        msg.account = "zouwei1";
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

    /**
     * 登录协议返回
     */
    private void ProtoLoginResponseHandler(SprotoTypeBase msg)
    {
        var csLoginResp = (SprotoType.cs_login.response)msg;
        Debug.Log("ProtoLoginResponseHandler___" + csLoginResp.result);
    }

    /**
     * 创建角色协议返回
     */
    private void ProtoCreateResponseHandler(SprotoTypeBase msg)
    {
        var csCreateResp = (SprotoType.cs_create.response)msg;
        Debug.Log("ProtoCreateResponseHandler___" + csCreateResp.result);
    }

    /**
     * 服务端通知进入场景
     **/
    private void ProtoMapEnterHandler(SprotoTypeBase msg)
    {
        var scMapEnterReq = (SprotoType.sc_map_enter.request)msg;
        Debug.Log("ProtoMapEnterHandler___" + scMapEnterReq.map_id);
    }

}
