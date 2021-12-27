using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GoogleMobileAds.Api;

public class OnGameStart : SingletonManager<OnGameStart>
{
    public static bool IsLoginGPGS = false;
    public static bool ShowAD = false;

    private BannerView m_BannerView;
    private InterstitialAd m_PopupAD;


    //ca-app-pub-3940256099942544/6300978111 테스트 배너
    //ca-app-pub-7998524345364346/7584837821 내 배너

    //ca-app-pub-3940256099942544/1033173712 테스트 전면 광고
    //ca-app-pub-7998524345364346/9208733073 내 전면광고

    //ca-app-pub-3940256099942544~3347511713 테스트 앱 id
    //ca-app-pub-7998524345364346~8818028442 내 앱 id
    private string m_PopupAD_UnitID = "ca-app-pub-3940256099942544/1033173712";
    private string m_BannerAD_UnitID = "ca-app-pub-3940256099942544/6300978111";

    public override void Awake()
    {
        base.Awake();
#if UNITY_ANDROID
        InitGPGS();
        if (ShowAD)
            InitAdMob();
#endif
        GotoNextScene();
    }


    public void InitAdMob()
    {

#if UNITY_ANDROID
        MobileAds.Initialize((initStatue) =>
        {
            List<string> testDeviceIds = new List<string>();
            testDeviceIds.Add("99CAF939BB7BF821B99B3CE48A752AE8");

            RequestConfiguration configuration =
                    new RequestConfiguration.Builder().SetTestDeviceIds(testDeviceIds).build();
            MobileAds.SetRequestConfiguration(configuration);
        });
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif
    }

    public void InitBannerAD()
    {
#if UNITY_ANDROID
        if (m_BannerView != null)
        {
            m_BannerView.Destroy();
        }

        try
        {
            m_BannerView = new BannerView(m_BannerAD_UnitID, AdSize.Banner, AdPosition.Bottom);
            AdRequest _request = new AdRequest.Builder().Build();
            m_BannerView.LoadAd(_request);
            ShowBannerAD();
        }
        catch (System.Exception)
        {
            throw;
        }
#endif
    }

    public void ShowBannerAD()
    {
#if UNITY_ANDROID
        m_BannerView.SetPosition(AdPosition.Bottom);
        m_BannerView.Show();
#endif
    }

    public void HideBannerAD()
    {
#if UNITY_ANDROID
        m_BannerView.Hide();
#endif
    }

    public void LoadPopupAD()
    {
#if UNITY_ANDROID
        try
        {
            m_PopupAD = new InterstitialAd(m_PopupAD_UnitID);

            AdRequest _request = new AdRequest.Builder().Build();
            m_PopupAD.LoadAd(_request);
        }
        catch (System.Exception)
        {
            throw;
        }
#endif
    }

    public void DestroyPopupAD()
    {
#if UNITY_ANDROID
        if (m_PopupAD != null)
            m_PopupAD.Destroy();
#endif
    }

    public void AddClosedCallbackPopupAD(System.EventHandler<System.EventArgs> method)
    {
        if (m_PopupAD.IsLoaded())
        {
            m_PopupAD.Show();
            m_PopupAD.OnAdClosed += method;
        }
        else
        {
            method.Invoke(null, new System.EventArgs());
        }
    }

    private void InitGPGS()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            //.RequestServerAuthCode(false)
            .Build();
        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.Activate();
        LoginGPGS();
    }

    private void LoginGPGS()
    {

        try
        {

            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                IsLoginGPGS = true;
            }
            else
            {
                PlayGamesPlatform.Instance.Authenticate((bool Success) =>
                {
                    if (Success)
                    {
                        IsLoginGPGS = Success;
                    }
                    else
                    {
                        //로그인 실패
                        IsLoginGPGS = Success;
                    }
                });
            }
        }
        catch (System.Exception)
        {
            throw;
        } 
    }

    private void GotoNextScene()
    {
        SetEncryptedPlayerPrefs();

        if (EncryptedPlayerPrefs.GetInt(Board.IsPlayingKey) == 1)
            SceneManager.LoadScene(2);
        else
            SceneManager.LoadScene(1);
    }

    public void SetEncryptedPlayerPrefs()
    {
        if (ReferenceEquals(EncryptedPlayerPrefs.keys, null))
        {
            EncryptedPlayerPrefs.keys = new string[5];

            EncryptedPlayerPrefs.keys[0] = "7z3DBe69";
            EncryptedPlayerPrefs.keys[1] = "0GDczs34";
            EncryptedPlayerPrefs.keys[2] = "GvbsI116";
            EncryptedPlayerPrefs.keys[3] = "UIVoziv4";
            EncryptedPlayerPrefs.keys[4] = "jaw3eDAs";

        }
        if (!EncryptedPlayerPrefs.HasKey(Board.RecordKey))
        {
            EncryptedPlayerPrefs.SetInt(Board.RecordKey, 0);
            EncryptedPlayerPrefs.SetInt(Board.IsPlayingKey, 0);
            EncryptedPlayerPrefs.SetInt(Board.PresetIndexKey, 0);
            EncryptedPlayerPrefs.SetInt(Board.LastScoreKey, 0);
            EncryptedPlayerPrefs.SetString(Board.BallsDataKey, "Null");
            EncryptedPlayerPrefs.SetInt(Board.SoundOnOffKey, 1);
        }

        if (!EncryptedPlayerPrefs.HasKey(Board.ShowADKey))
        {
            EncryptedPlayerPrefs.SetInt(Board.ShowADKey, Board.ShowADTerm);
        }
    }
}
