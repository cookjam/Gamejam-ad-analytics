using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Cookapps.Analytics;
using GoogleMobileAds.Api;

namespace Cookapps.Ads {

public class AdmobManager : MonoBehaviour {

	public static AdmobManager Instance; 
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
	
	private bool bannerLoaded;

	public delegate void RewardCallback(float reward);

	private RewardCallback rewardComplete;
	private Reward reward;

	void Awake() {
        DontDestroyOnLoad(this.gameObject);
		if (Instance) return;
		Instance = this;
    }
	void Start () {
		if(this.appIdAndroid == "") return;
		MobileAds.Initialize(this.appIdAndroid);
		this.loadBanner();
		this.loadInterstitial();
		this.loadReward();
	}
	private void loadBanner() {
		if (this.bannerIdAndroid == "") return;
		Debug.Log("loadBanner");
		this.sendAppEvent("ca_ad_banner_requested");
		AdRequest req = new AdRequest.Builder().TagForChildDirectedTreatment(true).Build();
		this.bannerAd = new BannerView(this.bannerIdAndroid, AdSize.SmartBanner, AdPosition.Top);
		this.bannerAd.LoadAd(req);
		this.bannerAd.OnAdLoaded += onLoadBanner;
		this.bannerAd.Hide();
		this.bannerLoaded = false;
	}	
	
	public void showBanner(string position) {
		if (!this.isBannerLoaded()) return;
		Debug.Log("showBanner");
		this.sendAppEvent("ca_ad_banner_initiated");
		this.sendAppEvent("ca_ad_banner_impression");
		if(position == "top") this.bannerAd.SetPosition(AdPosition.Top);
		else this.bannerAd.SetPosition(AdPosition.Bottom);
		this.bannerAd.OnAdOpening += this.onClickBanner;
		this.bannerAd.Show();
	}

	private void onLoadBanner(object sender, EventArgs args) {
		Debug.Log("onLoadBanner");
		this.bannerAd.OnAdLoaded -= this.onLoadBanner;
		this.bannerLoaded = true;
	}

	private void onClickBanner(object sender, EventArgs args) {
		Debug.Log("onClickBanner");
		this.sendAppEvent("ca_ad_banner_click");
	}

	private void hideBanner() {
		if (!this.isBannerLoaded()) return;
		Debug.Log("hideBanner");
		this.bannerAd.OnAdLoaded -= this.onLoadBanner;
		this.bannerAd.OnAdOpening -= this.onClickBanner;
		this.bannerAd.Hide();
	}

	private bool isBannerLoaded() {
		if (this.bannerAd == null) return false;
		if (this.bannerLoaded == false) return false;
		return true;
	}




	private void loadInterstitial() {
		if (this.interstitialIdAndroid == "") return;
		Debug.Log("loadInterstitial");
		this.sendAppEvent("ca_ad_is_requested");	
		AdRequest req = new AdRequest.Builder().TagForChildDirectedTreatment(true).Build();
		this.interstitialAd = new InterstitialAd(this.interstitialIdAndroid);
		this.interstitialAd.LoadAd(req);
	}

	public void showInterstitial() {
		if (!this.isInterstitialLoaded()) return;
		Debug.Log("showInterstitial");
		this.sendAppEvent("ca_ad_is_initiated");
		this.sendAppEvent("ca_ad_is_impression");
		this.interstitialAd.OnAdClosed += this.onCloseInterstitial;
		this.interstitialAd.OnAdLeavingApplication += this.onClickInterstitial;
		this.interstitialAd.Show();
	}

	private void onClickInterstitial(object sender, EventArgs args) {
		Debug.Log("onClickInterstitial");
		this.sendAppEvent("ca_ad_is_click");
		this.resetInterstitial();
	}

	private void onCloseInterstitial(object sender, EventArgs args) {
		Debug.Log("onCloseInterstitial");
		this.sendAppEvent("ca_ad_is_exit");
		this.resetInterstitial();
	}

	public void resetInterstitial() {
		this.interstitialAd.OnAdClosed -= this.onClickInterstitial;
		this.interstitialAd.Destroy();
		this.loadInterstitial();
	}

	private bool isInterstitialLoaded() {
		if (this.interstitialAd == null) return false;
		if (!this.interstitialAd.IsLoaded()) return false;
		return true;
	}



	private void loadReward() {
		if (this.rewardIdAndroid == "") return;
		Debug.Log("loadReward");
		this.sendAppEvent("ca_ad_rv_requested");
		AdRequest req = new AdRequest.Builder().TagForChildDirectedTreatment(true).Build();
		this.rewardAd = RewardBasedVideoAd.Instance;
		this.rewardAd.OnAdFailedToLoad += this.onFailReward;
		this.rewardAd.LoadAd(req, this.rewardIdAndroid);
	}

	private void onFailReward (object sender, AdFailedToLoadEventArgs args)
    {
		Debug.Log("onFailReward");
        Debug.Log(args.Message);
    }

	public void showReward(RewardCallback onComplete) {
		if (!this.isRewardLoaded()) {
			if (onComplete != null) onComplete(0);
			return;
		}
		Debug.Log("showReward");
		this.sendAppEvent("ca_ad_rv_initiated");
		this.sendAppEvent("ca_ad_rv_impression");
		this.rewardComplete = onComplete;
		this.rewardAd.OnAdClosed += this.onCloseReward;
		this.rewardAd.OnAdRewarded += this.onCompleteReward;
		this.rewardAd.OnAdLeavingApplication += this.onClickReward;
		this.rewardAd.Show();
		this.reward = null;
	}

	private void onClickReward(object sender, EventArgs args) {
		Debug.Log("onClickReward");
		this.sendAppEvent("ca_ad_rv_click");
	}

	private void onCloseReward(object sender, EventArgs args) {
		Debug.Log("onCloseReward");
		Debug.Log(this.reward);
		if (this.rewardComplete != null) {
			if (this.reward != null) this.rewardComplete((float)this.reward.Amount);
			else this.rewardComplete(0);
		}
		this.resetReward();
	}

	private void onCompleteReward(object sender, Reward args) {
		Debug.Log("onCompleteReward");
		this.sendAppEvent("ca_ad_rv_completed");
		this.reward = args;
	}

	public void resetReward() {
		Debug.Log("resetReward");
		this.rewardComplete = null;
		this.rewardAd.OnAdClosed -= this.onCloseReward;
		this.rewardAd.OnAdRewarded -= this.onCompleteReward;
		this.rewardAd.OnAdLeavingApplication -= this.onClickReward;
		this.reward = null;
		this.loadReward();
	}

	private bool isRewardLoaded() {
		if (this.rewardAd == null) return false;
		if (!this.rewardAd.IsLoaded()) return false;
		return true;
	}


	private void sendAppEvent(string name) {
		AppEventManager.sendAppEvent(name);
	}



	public static void ShowBanner(string position = "top") {
		Instance.showBanner(position);
	}

	public static void HideBanner() {
		Instance.hideBanner();
	}

	public static void ShowInterstitial() {
		Instance.showInterstitial();
	}
	
	public static void ShowReward(RewardCallback onComplete = null) {
		Instance.showReward(onComplete);
	}

	public static bool IsBannerLoaded() {
		return Instance.isBannerLoaded();
	}

	public static bool IsInterstitialLoaded() {
		return Instance.isInterstitialLoaded();
	}

	public static bool IsRewardLoaded() {
		return Instance.isRewardLoaded();
	}
}
}