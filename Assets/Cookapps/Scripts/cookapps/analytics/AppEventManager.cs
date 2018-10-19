using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase;

namespace Cookapps.Analytics {

public class AppEventManager {

	public static void sendAppEvent(string name, Dictionary<string, object> param = null) {
		AppEventManager.sendFirebaseAppEvent(name, param);
	}

	public static void sendFirebaseAppEvent(string name, Dictionary<string, object> param = null) {
		Debug.Log("Firebase AppEvent : " + name);
		if(param != null) Firebase.Analytics.FirebaseAnalytics.LogEvent(name, FirebaseUtil.ParseParams(param));
		else Firebase.Analytics.FirebaseAnalytics.LogEvent(name);
	}
}
}