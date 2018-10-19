using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cookapps.Ads;

public class AdmobExampleMain : MonoBehaviour {

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
		if (AdmobManager.IsBannerLoaded()) AdmobManager.ShowBanner();
	}

	private void onClickHideBanner() {
		if (AdmobManager.IsBannerLoaded()) AdmobManager.HideBanner();
	}

	private void onClickInterstitial() {
		if (AdmobManager.IsInterstitialLoaded()) AdmobManager.ShowInterstitial();
	}

	private void onClickReward() {
		if (AdmobManager.IsRewardLoaded()) AdmobManager.ShowReward(this.onRewardResult);
	}

	private void onRewardResult(float reward) {
		
	}
}
