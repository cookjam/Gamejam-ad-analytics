using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using UnityEngine.UI;
using GoogleMobileAds.Api;


public class Main : MonoBehaviour {

	public Button interstitialBtn;
	public Button bannerShowBtn;
	public Button bannerHideBtn;
	public Button rewardBtn;

	// Use this for initialization
	void Start () {
		this.interstitialBtn.onClick.AddListener(this.onClickInterstitial);
		this.bannerShowBtn.onClick.AddListener(this.onClickShowBanner);
		this.bannerHideBtn.onClick.AddListener(this.onClickHideBanner);
		this.rewardBtn.onClick.AddListener(this.onClickReward);
	}
	
	
	private void onClickShowBanner() {
		if (AdMobManager.IsBannerLoaded()) AdMobManager.ShowBanner();
	}

	private void onClickHideBanner() {
		if (AdMobManager.IsBannerLoaded()) AdMobManager.HideBanner();
	}

	private void onClickInterstitial() {
		if (AdMobManager.IsInterstitialLoaded()) AdMobManager.ShowInterstitial();
	}

	private void onClickReward() {
		if (AdMobManager.IsRewardLoaded()) AdMobManager.ShowReward(this.onRewardResult);
	}

	private void onRewardResult(float reward) {
		
	}
}
