using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject Ending_ImgParent;
    CinemachineBrain cinemachine;
    public GameObject StartScreen,Keys_Parent,ShopBtn,BonousScreen;
    public Transform Bonous_Parent;
    public GameObject InGameScreen, LevelfailScreen, LevelWinScreen, SettingScreen, ShopScreen, LoadingScreen, RateusScreen, RewardScreen,PauseScreen,MainScrenn,exitScreen;
    [SerializeField]
    GameObject Shop_Objects;
    public Image FadeImge;
    public Transform Canvas,sublvl_indicator,Coins,CoinDstination;
    public Text levelNumber,Cash_text,Lvltxt1, Lvltxt2,wolletText;
    float wolletvalue = 100;
    float increment=0;
    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.GetInt("OneTime") == 0)
        {
            PlayerPrefs.SetFloat("Wolletcash", 100);
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
        }
        else
        {
            MainScrenn.SetActive(false);

        }
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
    public void ShowStartscreen()
    {
        
        if (PlayerPrefs.GetInt("Level") < 1)
        {
            ShopBtn.SetActive(false);
        }
        else
        {
        ShopBtn.SetActive(true);
        }
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
    public void ShowRateusScreen()
    {
        DisableScreens();
        EnableScreen(RateusScreen);
    }
    
    public IEnumerator ShowwinScreen()
    {
        HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.Success);

        yield return new WaitForSeconds(0.5f);
        wolletvalue = 100;
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
        DisableScreens();
        EnableScreen(LevelfailScreen);
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
        MainScrenn.SetActive(true);
        DisableScreens();
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("MM", 0);

    }
    public void playbtn()
    {
        MainScrenn.SetActive(false);
        PlayerPrefs.SetInt("MM", 1);

    }
    public void OnClickRestartLevelBtn()
    {
        AudioManager.Instance.PlayButtonClickSound();
        PlayerController.instance.RestartLevel();
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
        for(int i = 0; i < sublvl_indicator.childCount; i++)
        {
            sublvl_indicator.GetChild(i).GetChild(0).gameObject.SetActive(false);
            sublvl_indicator.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }

    }
    public void EnableScreen(GameObject Screen)
    {
        Screen.gameObject.SetActive(true);
    }
    public void DisableScreen(GameObject Screen)
    {
        Screen.gameObject.SetActive(false);
    }

    float temp;
    public IEnumerator Generate_Coins()
    {
        if (PlayerPrefs.GetInt("Level") > 0) 
        { 
         increment = PlayerPrefs.GetFloat("Wolletcash") * 0.2f;
         PlayerPrefs.SetFloat("Wolletcash", PlayerPrefs.GetFloat("Wolletcash")+increment);
            wolletvalue = PlayerPrefs.GetFloat("Wolletcash");
        }


        

        DOTween.To(() => wolletvalue, x => wolletvalue = x, 0, 1.5f).SetEase(Ease.Linear).OnUpdate(()=> 
        {
            wolletText.text =((int) wolletvalue).ToString();
        });

        temp = PlayerPrefs.GetFloat("Cash");
        ShopManager.instance.Cash(PlayerPrefs.GetFloat("Wolletcash"), false);
        DOTween.To(() => temp, x => temp = x, temp + PlayerPrefs.GetFloat("Wolletcash"), 1.5f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            Debug.Log(PlayerPrefs.GetFloat("Wolletcash") + "Wollet Cash");
            Cash_text.text = ((int)temp).ToString();
            //wolletText.text = wolletvalue.ToString();
        });

        for (int i = 0; i < 20; i++)
        {
            HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.LightImpact);

            Transform coin = Instantiate(Coins);
            coin.SetParent(LevelWinScreen.transform);
            coin.SetAsFirstSibling();
            coin.transform.localPosition = new Vector3(0f,90f,0f);
            coin.transform.DOMove(CoinDstination.position, 0.2f).OnPlay(()=> {

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
            int indx = Random.Range(0, Ending_ImgParent.transform.childCount - 1);
            Ending_ImgParent.transform.GetChild(indx).GetComponent<DOTweenAnimation>().DORestart();
        }
        else
        {
            //int indx = Random.Range(0, Sad_ImgParent.transform.childCount - 1);
            //Sad_ImgParent.transform.GetChild(indx).GetComponent<DOTweenAnimation>().DORestart();
        }
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
