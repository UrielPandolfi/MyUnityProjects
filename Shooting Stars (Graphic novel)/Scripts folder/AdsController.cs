using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsController : MonoBehaviour
{
    public static AdsController instance;
    private void Awake()
    {
        instance = this;
    }

    public string appId = "ca-app-pub-4042679109664193~4165035701";

    string bannerId = "ca-app-pub-4042679109664193/4032678925";
    string interId = "ca-app-pub-4042679109664193/9397165174";
    string rewardedId = "ca-app-pub-4042679109664193/7497776054";

    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {
            print("Ads Initialized.");
        });

        LoadBannerAd();
        LoadInterstitialAd();
        LoadRewardedAd();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ShowInterstitialAd();
        }
    }

    #region Banner Ad
    public void LoadBannerAd()
    {
        if(!PlayerPrefs.HasKey("NoAds"))
        {
            // Create banner
            if(bannerView == null)
            {
                CreateBannerAd();
            }

            // Listen to banner events
            ListenToBannerAd();
            
            // Load banner
            var adRequest = new AdRequest();
            adRequest.Keywords.Add("anime");
            adRequest.Keywords.Add("romance");
            adRequest.Keywords.Add("otome");
            adRequest.Keywords.Add("visual novel");
            adRequest.Keywords.Add("love story");
            adRequest.Keywords.Add("dating sim");

            print("Loading Banner AD!");
            bannerView.LoadAd(adRequest);
        }
    }

    void CreateBannerAd()
    {
        if(bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(bannerId, AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Bottom);    // Creamos el nuevo
    }

    public void DestroyBannerAd()
    {
        if(bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }

    void ListenToBannerAd()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}." + adValue.Value+ adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    #endregion

    #region Interstitial Ad
    public void LoadInterstitialAd() // Esta es para cargar el ad, se hace siempre nada mas arranca el juego, para que este listo para reproducirse en cierto momento el ad
    {
        if(interstitialAd != null)  // Comprobamos que no se carguen dos al mismo tiempo, si ya hay uno cargado lo borramos
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("anime");
        adRequest.Keywords.Add("romance");
        adRequest.Keywords.Add("otome");
        adRequest.Keywords.Add("visual novel");
        adRequest.Keywords.Add("love story");
        adRequest.Keywords.Add("dating sim");

        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) => {

            if(error != null || ad == null)
            {
                print("intersticial failed: " + error);
                return;
            }

            print("intersticial ad loaded! " + ad.GetResponseInfo());

            interstitialAd = ad;
            InterstitialEvent(ad);

        });
    }

    public void ShowInterstitialAd()
    {
        if(interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            print("Interstitial ad not ready.");
        }
    }

    public void InterstitialEvent(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(("Interstitial ad paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            LoadInterstitialAd();
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                        "with error : " + error);
        };
    }

    #endregion

    #region Rewarded Ad

    public void LoadRewardedAd()
    {
        if(rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("anime");
        adRequest.Keywords.Add("romance");
        adRequest.Keywords.Add("otome");
        adRequest.Keywords.Add("visual novel");
        adRequest.Keywords.Add("love story");
        adRequest.Keywords.Add("dating sim");

        RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>   // el id y el ad request da como resultado el ad y el error si es que lo hay.
        {

            if(error != null || ad == null)
            {
                print("Ad has been failed!");
                return;
            }

            rewardedAd = ad;
            RewardedEvent(rewardedAd);
        });
    }

    public void ShowRewardedAd()
    {
        if(rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward Reward) =>
            {
                // Usando la expresion lambda hacemos que esto sea el resultado del reward
                ShopController.instance.AddCrystals(2);
            });
        }
    }

    void RewardedEvent(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(("Rewarded ad paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadRewardedAd();
            ShopController.instance.RewardedAdsCooldownOn();
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                        "with error : " + error);
        };
    }

    #endregion
}
