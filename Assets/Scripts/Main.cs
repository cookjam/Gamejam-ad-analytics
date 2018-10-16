using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Firebase.Analytics.FirebaseAnalytics
		.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
