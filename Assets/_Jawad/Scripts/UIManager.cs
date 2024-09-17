using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;
using System;

using Newtonsoft.Json.Bson;
using System.Security.Cryptography;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject Ending_ImgParent;
    CinemachineBrain cinemachine;
    public GameObject StartScreen,Keys_Parent,ShopBtn,BonousScreen;
    public Transform Bonous_Parent;
    public GameObject InGameScreen, LevelfailScreen, LevelWinScreen, SettingScreen, ShopScreen, LoadingScreen, RateusScreen, RewardScreen,PauseScreen,MainScrenn,exitScreen,rewardPannel,RevivePannel,inappPAnnel;
    [SerializeField]
    GameObject Shop_Objects;
    public Image FadeImge;
    public Transform Canvas,sublvl_indicator,Coins,CoinDstination;
    public Text Cash_text,Lvltxt1, Lvltxt2,wolletText;
    int wolletvalue = 5;
    int increment=0;
    public GameObject loadingScreen;  // Reference to your loading screen UI
    public Image loadingSlider;

    public GameObject[] inappPannels;
    public GameObject[] RewardPannels;



    public GameObject sounds;
    public GameObject sound_on;
    public GameObject sound_offbtn;

    public int temp;


    private void Awake()
    {
        Instance = this;


        if (PlayerPrefs.GetInt("OneTime") == 0)
        {
            PlayerPrefs.SetInt("Wolletcash", wolletvalue);
            PlayerPrefs.SetInt("OneTime", 1);
        }
    }

    void Start()
    {
        current_Keys();
        cinemachine = Camera.main.GetComponent<CinemachineBrain>();
        if (PlayerPrefs.GetInt("MM") == 0)
        {
            MainScrenn.SetActive(true);
            Debug.LogError("mainmenutruee");
        }
        else
        {
            MainScrenn.SetActive(false);
            Debug.LogError("mainmenufalse");

        }

        if (PlayerPrefs.GetInt("sound on") == 0)
        {
            sounds.SetActive(true);
            sound_on.SetActive(true);

        }
        else if (PlayerPrefs.GetInt("sound on") == 1)
        {
            sounds.SetActive(false);
            sound_offbtn.SetActive(true);
            sound_on.SetActive(false);


        }
       
        //temp = PlayerPrefs.GetInt("Cash");
        //Cash_text.text = temp.ToString();
        // Cash_text.text = wolletvalue.ToString();
        //Debug.LogError("coins"+wolletvalue);

        // Debug.LogError("Start");

    }

    public void Exitbtn()
    {
        exitScreen.SetActive(true);
    }
    public void ExitbtnNo()
    {
        exitScreen.SetActive(false);
    }
    public void ExitbtnYes()
    {
        Application.Quit();
        //   exitScreen.SetActive(false);
    }
    public void Sound_On()
    {
        sounds.SetActive(true);
    }
    public void sound_off()
    {
        sounds.SetActive(false);
        PlayerPrefs.SetInt("sound on", 1);


    }
    public void ShowStartscreen()
    {
        
        //if (PlayerPrefs.GetInt("Level") < 1)
        //{
        //    ShopBtn.SetActive(false);
        //}
        //else
        //{
        //ShopBtn.SetActive(true);
        //}
        Shop_Objects.SetActive(false);
        DisableScreens();
        EnableScreen(StartScreen);
        EnableScreen(InGameScreen);
        MainScrenn.SetActive(true);
        StartCoroutine(CameraSpeed());
    }

    public void ShowIngameScreen()
    {
        DisableScreens();
        EnableScreen(InGameScreen);
    }
    public void ShowBonousScreen()
    {
        DisableScreen(InGameScreen);
        DisableScreens();
        for(int i=0;i< Bonous_Parent.childCount;i++)
        {
        Bonous_Parent.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
        EnableScreen(BonousScreen);
        
    }
    
    public void ShowSettingScreen()
    {
      
        DisableScreens();
        EnableScreen(SettingScreen);
    }
    public void ShowRewardScreen()
    {
        PlayerPrefs.SetInt("RewardKeys", 0);
        PlayerPrefs.SetInt("ChestBox", PlayerPrefs.GetInt("ChestBox") + 1);
        if (PlayerPrefs.GetInt("ChestBox") > 3)
        {
            PlayerPrefs.SetInt("ChestBox", 0);
        }
        DisableScreens();
        EnableScreen(RewardScreen);

    }
    public void ShowShopScreen()
    {
        ShopManager.instance.Update_Kits(false);
        Shop_Objects.SetActive(true);
        DisableScreens();
        EnableScreen(ShopScreen);
        StartCoroutine(CameraSpeed());
    }
    void reward_coins()
    {

    }
    public void ShowRateusScreen()
    {
        DisableScreens();
        EnableScreen(RateusScreen);
    }
    int adcount;
    int inapp;

    
    public IEnumerator ShowwinScreen()
    {
        HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.Success);
        adcount++;
        if (adcount == 2)
        {
            admanager.instance.ShowGenericVideoAd();
            adcount = 0;
        }
        inapp++;
        if (inapp == 2)
        {
            inappPannels[UnityEngine.Random.Range(0, inappPannels.Length)].SetActive(true);
            //inappPAnnel.SetActive(true);
           // rewardPannel.SetActive(false);
            inapp = 0;

        }
        else
        {
            
            RewardPannels[UnityEngine.Random.Range(0, RewardPannels.Length)].SetActive(true);
            //rewardPannel.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);

    }
    
    public void remove_ads()
    {
        admanager.instance.removeAllAds = true;
        admanager.instance.RemoveAds(true);

    }
    public void onclickClosePannel()
    {
        wolletvalue = 5;
       wolletText.text = wolletvalue.ToString();

        DisableScreens();
        EnableScreen(LevelWinScreen);
        StartCoroutine(Generate_Coins());
    }
    public IEnumerator CameraSpeed()
    {
        cinemachine.m_DefaultBlend=
        new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut,0f);
        yield return new WaitForSeconds(0.1f);
        cinemachine.m_DefaultBlend =
        new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut,1f);

    }
    public IEnumerator ShowLoseScreen()
    {
        HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.Failure);
        yield return new WaitForSeconds(1.5f);
       // DisableScreens();
        RevivePannel.SetActive(true);
        // EnableScreen(LevelfailScreen);
    }
    public void onclickNoThank()
    {
        DisableScreens();
        RevivePannel.SetActive(false);
        EnableScreen(LevelfailScreen);
    }
    public void showReviveViedo()
    {
        //StartCoroutine(LevelManager.instance.Move_Next());


         admanager.instance.ShowRewardedVideAdGeneric(revive_btn);
    }

    void revive_btn()
    {

        GameManager.isRevive = true;
        GameManager.instance.RestartGame();
        RevivePannel.SetActive(false);
    }

    public void DisableScreens()
    {
        DisableScreen(StartScreen);
        DisableScreen(InGameScreen);
        DisableScreen(SettingScreen);
        DisableScreen(RewardScreen);
        DisableScreen(ShopScreen);
        DisableScreen(RateusScreen);
        DisableScreen(LevelfailScreen);
        DisableScreen(LevelWinScreen);
        DisableScreen(PauseScreen);
    }

    public void pauseBtn()
    {
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }
    public void resumeBtn()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
    public void HomeBtn()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadHomeAsynchronously(0));
        PlayerPrefs.SetInt("MM", 0);

        DisableScreens();

    }
    public void playbtn()
    {
        MainScrenn.SetActive(false);
        StartCoroutine(LoadAsynchronously(0));

        // SceneManager.LoadScene(0);
        PlayerPrefs.SetInt("MM", 1);

    }
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(0));
    }

    IEnumerator LoadHomeAsynchronously(int num)
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        // Begin to load the Scene you specify
        AsyncOperation operation = SceneManager.LoadSceneAsync(num);

        // While the scene loads, update the loading screen UI
        while (!operation.isDone)
        {
            // Get progress (0 to 1)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Update the slider value
            loadingSlider.fillAmount = progress;

            // Update the progress text (optional)
            //if (progressText != null)
            //{
            //    progressText.text = (progress * 100f).ToString("F0") + "%";
            //}

            yield return null;
        }

        // Hide the loading screen after the scene is loaded
        loadingScreen.SetActive(false);
        MainScrenn.SetActive(true);

    }
    IEnumerator LoadAsynchronously(int num)
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        // Begin to load the Scene you specify
        AsyncOperation operation = SceneManager.LoadSceneAsync(num);

        // While the scene loads, update the loading screen UI
        while (!operation.isDone)
        {
            // Get progress (0 to 1)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Update the slider value
            loadingSlider.fillAmount = progress;

            // Update the progress text (optional)
            //if (progressText != null)
            //{
            //    progressText.text = (progress * 100f).ToString("F0") + "%";
            //}

            yield return null;
        }

        // Hide the loading screen after the scene is loaded
        loadingScreen.SetActive(false);
    }
    public void OnClickRestartLevelBtn()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadAsynchronously(0));
        AudioManager.Instance.PlayButtonClickSound();

       // PlayerController.instance.RestartLevel();
        PlayerPrefs.SetInt("MM", 1);

    }
    public void OnClickContinueToNextLevelBtn()
    {
        AudioManager.Instance.PlayButtonClickSound();
        PlayerController.instance.LoadNextLevel();
    }
    public void sublvl_Indicator(int Max)
    {
        for (int i = 0; i < Max; i++)
        {
            sublvl_indicator.GetChild(i).gameObject.SetActive(true);

        }
        for (int i = 0; i < sublvl_indicator.childCount; i++)
        {
            sublvl_indicator.GetChild(i).GetChild(0).gameObject.SetActive(false);
            sublvl_indicator.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }

    }
    public void Button_Sound()
    {
        AudioManager.Instance.PlayButtonClickSound();
    }
    public void EnableScreen(GameObject Screen)
    {
        Screen.gameObject.SetActive(true);
    }
    public void DisableScreen(GameObject Screen)
    {
        Screen.gameObject.SetActive(false);
    }
    public void showCoinsViedo()
    {
        admanager.instance.ShowRewardedVideAdGeneric(show_100coins);
    }
    public void showDoubleCoinsViedo()
    {
        admanager.instance.ShowRewardedVideAdGeneric(coinsDouble_reward);
    }
    public void SkipLevel_Viedo()
    {
        admanager.instance.ShowRewardedVideAdGeneric(Skiplevel_reward);
    }
    void Skiplevel_reward()
    {
        GameManager.instance.NextLevel();
    }
    void multiplyCoinsRward()
    {
        admanager.instance.ShowRewardedVideAdGeneric(coinsDouble_reward);
    }
    void coins_reward()
    {
        if (PlayerPrefs.GetInt("Level") > 0)
        {
            increment =100;
            PlayerPrefs.SetInt("Wolletcash", PlayerPrefs.GetInt("Wolletcash") + increment);
            wolletvalue = PlayerPrefs.GetInt("Wolletcash");
            Debug.LogError("coins1....." + increment);

        }

        Debug.LogError("coins2....." + wolletvalue);


        DOTween.To(() => wolletvalue, x => wolletvalue = x, 0, 1.5f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            wolletText.text = ((int)wolletvalue).ToString();

        });
        rewardPannel.SetActive(false);
    }
    void coinsDouble_reward()
    {
        StartCoroutine(Generate_Coins());

        //if (PlayerPrefs.GetInt("Level") > 0)
        //{
        //    increment = PlayerPrefs.GetFloat("Wolletcash") * 2f;
        //    PlayerPrefs.SetFloat("Wolletcash", PlayerPrefs.GetFloat("Wolletcash") + increment);
        //    wolletvalue = PlayerPrefs.GetFloat("Wolletcash");
        //    Debug.LogError("coins1....." + increment);

        //}

        //Debug.LogError("coins2....." + wolletvalue);


        //DOTween.To(() => wolletvalue, x => wolletvalue = x, 0, 1.5f).SetEase(Ease.Linear).OnUpdate(() =>
        //{
        //    wolletText.text = ((int)wolletvalue).ToString();

        //});

    }
    void show_100coins()
    {
        StartCoroutine(Generate100_Coins());
    }

    public void dailyReward()
    {
        StartCoroutine(Generate_Coins());
    }
    public void privacyPolicy_Btn()
    {
        Application.OpenURL("https://freentostudio.blogspot.com/2023/04/freentostudioprivacypolicy.html?m=1")
;
    }

    public IEnumerator Generate_Coins()
    {
        if (PlayerPrefs.GetInt("Level") > 0)
        {
            increment = 5/*PlayerPrefs.GetInt("Wolletcash") * 1*/;
            PlayerPrefs.SetInt("Wolletcash", PlayerPrefs.GetInt("Wolletcash") + increment);
            wolletvalue = PlayerPrefs.GetInt("Wolletcash");
            Debug.LogError("coins1....." + increment);

        }

        Debug.LogError("coins2....." + wolletvalue);


        DOTween.To(() => wolletvalue, x => wolletvalue = x, 0, 1.5f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            wolletText.text = wolletvalue.ToString();

        });

        temp = PlayerPrefs.GetInt("Cash");
        ShopManager.instance.Cash(PlayerPrefs.GetInt("Wolletcash"), false);
        DOTween.To(() => temp, x => temp = x, temp + PlayerPrefs.GetInt("Wolletcash"), 1.5f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            Debug.Log(PlayerPrefs.GetInt("Wolletcash") + "Wollet Cash");
            Cash_text.text = temp.ToString();
            Debug.LogError("coins3....." + temp);

           // wolletText.text = ((int)wolletvalue).ToString();

        });

        for (int i = 0; i < 20; i++)
        {
            // HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.LightImpact);

            Transform coin = Instantiate(Coins);
            coin.SetParent(LevelWinScreen.transform);
            coin.SetAsFirstSibling();
            coin.transform.localPosition = new Vector3(0f, 90f, 0f);
            coin.transform.DOMove(CoinDstination.position, 0.2f).OnPlay(() =>
            {

                coin.SetParent(CoinDstination);
            }).OnComplete(() =>
            {

                Destroy(coin.gameObject);
            });
            //UnityEngine.Random.Range(0.1f)
            yield return new WaitForSeconds(0.04f);
        }
    }
    public IEnumerator Generate100_Coins()
    {
        if (PlayerPrefs.GetInt("Level") > 0)
        {
            increment = /*PlayerPrefs.GetInt("Wolletcash") +*/100;
            PlayerPrefs.SetInt("Wolletcash", PlayerPrefs.GetInt("Wolletcash") + increment);
            wolletvalue = PlayerPrefs.GetInt("Wolletcash");
            Debug.LogError("coins1....." + increment);

        }

       // Debug.LogError("coins2....." + wolletvalue);


        DOTween.To(() => wolletvalue, x => wolletvalue = x, 0, 1.5f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            wolletText.text = ((int)wolletvalue).ToString();

        });

        temp = (int)PlayerPrefs.GetInt("Cash");
        ShopManager.instance.Cash((int)PlayerPrefs.GetInt("Wolletcash"), false);
        DOTween.To(() => temp, x => temp = x, temp + PlayerPrefs.GetInt("Wolletcash"), 1.5f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            Debug.Log(PlayerPrefs.GetInt("Wolletcash") + "Wollet Cash");
            Cash_text.text = ((int)temp).ToString();
            Debug.LogError("coins3....." + temp);

         //   wolletText.text = ((int)wolletvalue).ToString();

        });

        for (int i = 0; i < 20; i++)
        {
            // HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.LightImpact);

            Transform coin = Instantiate(Coins);
            coin.SetParent(LevelWinScreen.transform);
            coin.SetAsFirstSibling();
            coin.transform.localPosition = new Vector3(0f, 90f, 0f);
            coin.transform.DOMove(CoinDstination.position, 0.2f).OnPlay(() => {

                coin.SetParent(CoinDstination);
            }).OnComplete(() =>
            {

                Destroy(coin.gameObject);
            });
            //UnityEngine.Random.Range(0.1f)
            yield return new WaitForSeconds(0.04f);
        }
    }
    public void Gate_Imges(bool Positve)
    {
        if (Positve)
        {

            int indx = UnityEngine.Random.Range(0, Ending_ImgParent.transform.childCount - 1);
            Ending_ImgParent.transform.GetChild(indx).GetComponent<DOTweenAnimation>().DORestart();
        }
        else
        {
            //int indx = Random.Range(0, Sad_ImgParent.transform.childCount - 1);
            //Sad_ImgParent.transform.GetChild(indx).GetComponent<DOTweenAnimation>().DORestart();
        }
    }
    public void moreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Freento+Studio&hl=mn");
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("MM", 0);
    }
    void current_Keys()
    {
        if (PlayerPrefs.GetInt("frst") == 0)
        {
            PlayerPrefs.SetInt("GKeys", -1);
            PlayerPrefs.SetInt("frst", 1);
        }
        for (int i = 0; i <= PlayerPrefs.GetInt("GKeys"); i++)
        {
            Keys_Parent.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
    }
}
