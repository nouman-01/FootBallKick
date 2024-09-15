using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
//using GameAnalyticsSDK;
public enum BannerBoxPos // your custom enumeration
{
    bottomleft,
    bottomRight,
    TopLeft,
    TopRight,
    Center,
    Top,
    Bottom,
    CenterLeft,
    CenterRight
};

public class admanager : MonoBehaviour
{
    public static admanager instance;

    #region Variables

    [Header("REMOVE ADS")] public bool removeAllAds;
    [Header("PRIORITY CHECK")] public bool AdmobPriorityInter;
 //   public bool UnityPriorityInter;
    public bool AdmobPriorityRewarded;
   // public bool UnityPriorityRewarded;
    public bool ReviveVideoWatched;
    [Header("ADMOB IDS")] public string AppID = "ca-app-pub-4985960383417873~9579619428";
    public string bannerAdId1 = "ca-app-pub-4985960383417873/1399900592";
    public string bannerAdId2 = "ca-app-pub-4985960383417873/9086818921";
    public string bannerAdId3 = "ca-app-pub-4985960383417873/6260510882";
    public string bannerAdId4 = "ca-app-pub-4985960383417873/8270407183";
    public string bannerAdId5 = "ca-app-pub-4985960383417873/5209129570";
    public string InterstitialAdID = "ca-app-pub-4985960383417873/1773620905";
    public string rewarded_Ad_ID = "ca-app-pub-4985960383417873/4387002492";

    
    public bool BannerIsLoaded;
    [Header("BOX BANNER POSITIONING")] public BannerBoxPos boxbannerpos = BannerBoxPos.bottomleft;

    private Action completeAction;
    private BannerView bannerAdBottomLeft,
        bannerAdBottomRight,
        bannerAdTopLeft,
        bannerAdTopCentre,
        bannerAdBottomCentre,
        bannerAdTopRight,
        BannerAdBox;

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    private const string removeAds = "REMOVEADS";
    public bool isInitialized = false;
    public bool NextFunc;
    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (removeAllAds)
            RemoveAds(removeAllAds);
        else if (!removeAllAds)
            RemoveAds(removeAllAds);
        PlayerPrefs.SetInt("FirstTimeShop", 0);

        instance = this;
        DontDestroyOnLoad(this);

    }

    void Start()
    {
        //  Advertisement.AddListener(this);
        //GameAnalytics.Initialize();
        //    Invoke("Initialization", 3f);
         //Initialization();
       // Advertisement.Initialize(UnityAdId, UnityTestMood, this);
    }

    #region Initialization


    public void Initialization()
    {
        if (!isInitialized)
        {
            MobileAds.Initialize(initStatus => {
                isInitialized = true;
                RequestInterstital();
                Debug.Log("Initializati11111");
            });
        }
        RequestRewardVideo();
    }

    #endregion

    #region Show Functions

    
    public void ShowGenericVideoAd()
    {
        ////AppOpen.instance.isAdShown = false;
        if (AdmobPriorityInter)
        {
            if (interstitial != null)
            {
                if (interstitial.CanShowAd())
                {
                    ShowAdmobInterstitialAd();
                }
            }
            else
            {
                RequestInterstital();
                //ShowUnityInterstitial();
            }
            
        }
       
    }


    public void ShowBoxBanner(int i)
    {
        if (i == 0)
            boxbannerpos = BannerBoxPos.CenterLeft;
        if (i == 1)
            boxbannerpos = BannerBoxPos.CenterRight;
        if (i == 2)
            boxbannerpos = BannerBoxPos.Top;
        if (i == 3)
            boxbannerpos = BannerBoxPos.Bottom;
        if (i == 4)
            boxbannerpos = BannerBoxPos.bottomleft;
        if (i == 5)
            boxbannerpos = BannerBoxPos.bottomRight;
        if (i == 6)
            boxbannerpos = BannerBoxPos.TopLeft;
        if (i == 7)
            boxbannerpos = BannerBoxPos.TopRight;
        showBoxBanner();
    }

    #endregion

    #region Remove Ads

    public bool CanShowAds()
    {
        if (!PlayerPrefs.HasKey(removeAds))
            return true;
        else if (PlayerPrefs.GetInt(removeAds) == 0)
            return true;

        return false;
    }

    public void RemoveAds(bool remove)
    {
        if (remove == true)
        {
            PlayerPrefs.SetInt(removeAds, 1);
            hideBottomLeftBanner();
            hideBottomRightBanner();
            hideBannerBottomCentre();
            hideTopLeftBanner();
            hideTopRightBanner();
            hideBannerTopCentre();
        }
        else
            PlayerPrefs.SetInt(removeAds, 0);
    }

    #endregion

    #region rewarded Video Ads

    public void RequestRewardVideo()
    {
        Debug.Log("Loading the rewarded ad.11111");
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        RewardedAd.Load(rewarded_Ad_ID, adRequest,
            (RewardedAd ad, LoadAdError error) => {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(rewardedAd);
            });

    }
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) => {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () => {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () => {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () => {
            Debug.Log("Rewarded ad full screen content opened.");
            //AppOpen.instance.isAdShown = false;
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () => {
            RequestRewardVideo();
            //AppOpen.instance.//AppOpenAdShow();
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) => {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    public void ShowRewardedVideAdGeneric(Action action)
    {
        //AppOpen.instance.isAdShown = false;
        completeAction = action;
        //ShowUnityRewardedVideo();
        //ShowAdmobRewarded(action);
        if (AdmobPriorityRewarded)
        {
            if (rewardedAd != null)
            {
                if (rewardedAd.CanShowAd())
                {
                    ShowAdmobRewarded(action);
                }
            }
            else
            {
                RequestRewardVideo();
                //ShowUnityRewardedVideo();

            }
        }
       
    }

    
   
    private void ShowAdmobRewarded(Action action)
    {
        if (rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) => {
                //    //AppOpenAdManager.instance.//AppOpenAdShow();
                action?.Invoke();
                Debug.Log(String.Format(reward.Type, reward.Amount));
            });
        }
        else
        {
            RequestRewardVideo();
            Debug.Log("Rewarded Video ad not loaded");
        }
    }

    #endregion

    #region Banner

    


    public void showbannerbottomLeft()
    {
        if (CanShowAds())
        {
            if (bannerAdBottomLeft == null)
            {
                bannerAdBottomLeft = new BannerView(bannerAdId1, AdSize.Banner, AdPosition.BottomLeft);

                // Called when an ad request has successfully loaded.
                //    bannerAdBottomLeft.OnAdLoaded += HandleOnAdLoaded;
                // Called when an ad request failed to load.
                //     bannerAdBottomLeft.OnAdFailedToLoad += HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                bannerAdBottomLeft.LoadAd(request);
            }
            else
            {
                hideBottomLeftBanner();
                showbannerbottomLeft();
            }
        }
    }

    public void hideBottomLeftBanner()
    {
        if (bannerAdBottomLeft != null)
        {
            //bannerAdBottomLeft.Destroy();
            bannerAdBottomLeft.Hide();
            bannerAdBottomLeft = null;
        }
        //}
    }

    public void showbannerbottomRight()
    {
        if (CanShowAds())
        {
            if (bannerAdBottomRight == null)
            {
                bannerAdBottomRight = new BannerView(bannerAdId2, AdSize.Banner, AdPosition.BottomRight);

                //Called when an ad request has successfully loaded.
                //      bannerAdBottomRight.OnAdLoaded += HandleOnAdLoaded;
                // Called when an ad request failed to load.
                //       bannerAdBottomRight.OnAdFailedToLoad += HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                bannerAdBottomRight.LoadAd(request);
               // ShowUnityBannerAd();
            }
            else
            {
                hideBottomRightBanner();
                showbannerbottomRight();
            }
        }
    }

    public void hideBottomRightBanner()
    {
        if (bannerAdBottomRight != null)
        {
            //bannerAdBottomRight.Destroy();
            bannerAdBottomRight.Hide();
            bannerAdBottomRight = null;
        }
    }

    public void showBannerAdBottomCentre()
    {
        if (CanShowAds())
        {
            if (bannerAdBottomCentre == null)
            {
                bannerAdBottomCentre = new BannerView(bannerAdId2, AdSize.Banner, AdPosition.Bottom);

                // Called when an ad request has successfully loaded.
                //      bannerAdBottomCentre.OnAdLoaded += HandleOnAdLoaded;
                // Called when an ad request failed to load.
                //       bannerAdBottomCentre.OnAdFailedToLoad += HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                bannerAdBottomCentre.LoadAd(request);
            }
            else
            {
                hideBannerBottomCentre();
                showBannerAdBottomCentre();
            }
        }
    }

    public void hideBannerBottomCentre()
    {
        if (bannerAdBottomCentre != null)
        {
            bannerAdBottomCentre.Hide();
            bannerAdBottomCentre = null;
        }
    }

    public void showBannerAdTopCentre()
    {
        if (CanShowAds())
        {
            if (bannerAdTopCentre == null)
            {
                bannerAdTopCentre = new BannerView(bannerAdId2, AdSize.Banner, AdPosition.Top);

                // Called when an ad request has successfully loaded.
                //   bannerAdTopCentre.OnAdLoaded += HandleOnAdLoaded;
                // Called when an ad request failed to load.
                //   bannerAdTopCentre.OnAdFailedToLoad += HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                bannerAdTopCentre.LoadAd(request);
            }
            else
            {
                hideBannerTopCentre();
                showBannerAdTopCentre();
            }
        }
    }

    public void hideBannerTopCentre()
    {
        if (bannerAdTopCentre != null)
        {
            //bannerAdBottomRight.Destroy();
            bannerAdTopCentre.Hide();
            bannerAdTopCentre = null;
        }
    }

    public void showbannerTopLeft()
    {
        if (CanShowAds())
        {
            if (bannerAdTopLeft == null)
            {
                bannerAdTopLeft = new BannerView(bannerAdId3, AdSize.Banner, AdPosition.TopLeft);

                // Called when an ad request has successfully loaded.
                //    bannerAdTopLeft.OnAdLoaded += HandleOnAdLoaded;
                // Called when an ad request failed to load.
                //    bannerAdTopLeft.OnAdFailedToLoad += HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                bannerAdTopLeft.LoadAd(request);
            }
            else
            {
                hideTopLeftBanner();
                showbannerTopLeft();
            }
        }
    }

    public void hideTopLeftBanner()
    {
        if (bannerAdTopLeft != null)
        {
            //bannerAdTopLeft.Destroy();
            bannerAdTopLeft.Hide();
            bannerAdTopLeft = null;
        }
    }

    public void showbannerTopRight()
    {
        if (CanShowAds())
        {
            if (bannerAdTopRight == null)
            {
                bannerAdTopRight = new BannerView(bannerAdId4, AdSize.Banner, AdPosition.TopRight);

                // Called when an ad request has successfully loaded.
                //      bannerAdTopRight.OnAdLoaded += HandleOnAdLoaded;
                // Called when an ad request failed to load.
                //       bannerAdTopRight.OnAdFailedToLoad += HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                bannerAdTopRight.LoadAd(request);
            }
            else
            {
                hideTopRightBanner();
                showbannerTopRight();
            }
        }
    }

    public void hideTopRightBanner()
    {
        if (bannerAdTopRight != null)
        {
            bannerAdTopRight.Hide();
            bannerAdTopRight = null;
        }
    }

    public void showBoxBanner()
    {
        if (CanShowAds())
        {
            if (BannerAdBox == null)
            {
                if (boxbannerpos == BannerBoxPos.bottomleft)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.BottomLeft);
                else if (boxbannerpos == BannerBoxPos.bottomRight)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.BottomRight);
                else if (boxbannerpos == BannerBoxPos.TopLeft)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.TopLeft);
                else if (boxbannerpos == BannerBoxPos.TopRight)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.TopRight);
                else if (boxbannerpos == BannerBoxPos.Center)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.Center);
                else if (boxbannerpos == BannerBoxPos.Top)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.Top);
                else if (boxbannerpos == BannerBoxPos.Bottom)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.Bottom);
                else if (boxbannerpos == BannerBoxPos.CenterLeft)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.BottomLeft);
                else if (boxbannerpos == BannerBoxPos.CenterRight)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.BottomRight);


                //Called when an ad request has successfully loaded.
                //    BannerAdBox.OnAdLoaded += HandleOnAdLoaded;
                //Called when an ad request failed to load.
                //    BannerAdBox.OnAdFailedToLoad += HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                BannerAdBox.LoadAd(request);
            }
            else
            {
                hideBoxBanner();
                showBoxBanner();
            }
        }
    }

    public void hideBoxBanner()
    {
        if (BannerAdBox != null)
        {
            BannerAdBox.Hide();
            BannerAdBox = null;
        }
    }

    #endregion

    #region interstitial

    public void RequestInterstital()
    {
        // Clean up the old ad before loading a new one.
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        InterstitialAd.Load(InterstitialAdID, adRequest,
            (InterstitialAd ad, LoadAdError error) => {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());
                interstitial = ad;
                RegisterEventHandlers(interstitial);
            });
    }
    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) => {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () => {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () => {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () => {
            Debug.Log("Interstitial ad full screen content opened.");
            //AppOpen.instance.isAdShown = false;
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () => {
            RequestInterstital();
            //AppOpen.instance.//AppOpenAdShow();
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) => {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

   
    private void ShowAdmobInterstitialAd()
    {
        if (CanShowAds())
        {
            if (interstitial.CanShowAd())
            {
                interstitial.Show();
            }
        }
    }

    #endregion

    #region Admob Delegates

    #region Interstital

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        BannerIsLoaded = true;
        Debug.Log("Ad Loaded");
    }
    public void HandleOnAdLoadedInter(object sender, EventArgs args)
    {

        Debug.Log("Ad Loaded");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        BannerIsLoaded = false;
        Debug.Log("couldn`t load ad");
    }
    public void HandleOnAdFailedToLoadInter(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("couldn`t load ad");
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {

        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Ad Closed");
        RequestInterstital();
    }

    #endregion

    #region Rewarded



   

    #endregion

    #endregion



   

    
}