using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        NetManager.GetInstance().Connect();             // 连接服务器
        var goTestWin =  GameObject.Find("TestWin");    // 打开测试界面
        goTestWin.AddComponent<TestWin>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        NetManager.GetInstance().Update();
	}
}
