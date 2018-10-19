using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cookapps.Analytics;

public class AppEventExampleMain : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//이벤트만 남기기
		AppEventManager.sendAppEvent("이벤트 이름");

		//이벤트와 함께 값도 남기기
		Dictionary<string, object> param = new Dictionary<string, object>();
		param["user_name"] = "게임잼";
		param["score"] = 1024768;
	
		AppEventManager.sendAppEvent("이벤트 이름", param);
	}
}
