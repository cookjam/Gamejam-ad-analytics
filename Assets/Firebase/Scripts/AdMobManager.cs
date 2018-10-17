using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Firebase;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour {

	public static AdMobManager Instance; 
	public string appIdAndroid;
	// private string appIdIOS;
	public string bannerIdAndroid;
	// private string bannerIdIOS;
	
	public string interstitialIdAndroid;
	// private string interstitialIdIOS;

	public string rewardIdAndroid;
	// private string rewardIdIOS;

	private InterstitialAd interstitialAd;
	private RewardBasedVideoAd rewardAd;
	private BannerView bannerAd;
	
	public delegate void RewardCallback(string type, float reward);

	private RewardCallback rewardComplete;
	private RewardCallback rewardFail;
	
	void Awake() {
        DontDestroyOnLoad(transform.gameObject);
		if(Instance) return;
		Instance = this;
    }
	void Start () {
		MobileAds.Initialize(this.appIdAndroid);
		this.loadBanner();
		this.loadInterstitial();
		this.loadReward();
	}
	private void loadBanner() {
		if(this.bannerIdAndroid == null) return;
		AdRequest req = new AdRequest.Builder().Build();
		this.bannerAd = new BannerView(this.bannerIdAndroid, AdSize.SmartBanner, AdPosition.Top);
		this.bannerAd.LoadAd(req);
		this.bannerAd.Hide();
	}	
	private void loadInterstitial() {
		if(this.interstitialIdAndroid == null) return;
		AdRequest req = new AdRequest.Builder().Build();
		this.interstitialAd = new InterstitialAd(this.interstitialIdAndroid);
		this.interstitialAd.LoadAd(req);
	}
	private void loadReward() {
		if(this.rewardIdAndroid == null) return;
		AdRequest req = new AdRequest.Builder().Build();
		this.rewardAd = RewardBasedVideoAd.Instance;
		this.rewardAd.LoadAd(req, this.rewardIdAndroid);
	}

	
	private void showBanner() {
		if(this.bannerAd == null) return;
		this.bannerAd.OnAdOpening += this.onClickBanner;
		this.bannerAd.Show();
	}

	private void onClickBanner(object sender, EventArgs args) {
		//트래킹 남기기
	}

	private void hideBanner() {
		if(this.bannerAd == null) return;
		this.bannerAd.OnAdOpening -= this.onClickBanner;
		this.bannerAd.Hide();
	}



	private void showInterstitial() {
		if(this.interstitialAd == null) return;
		if(!this.interstitialAd.IsLoaded()) return;
		this.interstitialAd.OnAdClosed += this.onCloseInterstitial;
		this.interstitialAd.OnAdLeavingApplication += this.onClickInterstitial;
		this.interstitialAd.Show();
	}

	private void onClickInterstitial(object sender, EventArgs args) {
		this.interstitialAd.OnAdClosed -= this.onClickInterstitial;
		this.interstitialAd.Destroy();
		this.loadInterstitial();
		//트래킹 남기기
	}

	private void onCloseInterstitial(object sender, EventArgs args) {
		this.interstitialAd.OnAdClosed -= this.onCloseInterstitial;
		this.interstitialAd.Destroy();
		this.loadInterstitial();
	}



	private void showReward(RewardCallback onComplete, RewardCallback onFail) {
		if(this.rewardAd == null || !this.rewardAd.IsLoaded()) {
			onFail("fail", 0);
			return;
		}
		this.rewardComplete = onComplete;
		this.rewardFail = onFail;
		this.rewardAd.OnAdClosed += this.onCloseReward;
		this.rewardAd.OnAdRewarded += this.onCompleteReward;
		this.rewardAd.Show();
	}

	private void onCloseReward(object sender, EventArgs args) {
		this.rewardComplete = null;
		this.rewardFail = null;
		this.rewardAd.OnAdClosed -= this.onCloseReward;
		this.rewardAd.OnAdRewarded -= this.onCompleteReward;
		this.rewardFail("fail", 0);
		this.loadReward();
	}

	private void onCompleteReward(object sender, Reward args) {
		this.rewardComplete = null;
		this.rewardFail = null;
		this.rewardAd.OnAdClosed -= this.onCloseReward;
		this.rewardAd.OnAdRewarded -= this.onCompleteReward;
		this.rewardComplete(args.Type, (float)args.Amount);
		this.loadReward();
		//트래킹 남기기
	}



	public static void ShowBanner() {
		Instance.showBanner();
	}

	public static void HideBanner() {
		Instance.hideBanner();
	}

	public static void ShowInterstitial() {
		Instance.showInterstitial();
	}
	
	public static void ShowReward(RewardCallback onComplete, RewardCallback onFail) {
		Instance.showReward(onComplete, onFail);
	}

	
}
