using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WorldMapController : MonoBehaviour
{
    public static WorldMapController instance;
   
    void Awake()
    {
        //DontDestroyOnLoad(transform);
       
        if (PlayerPrefs.GetInt("Isquite")== 0)
        {
            LoadingScreen.SetActive(true);
            Loading_Bar.DOFillAmount(1, 2.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
             
                FadeScreen.SetActive(true);
                if (PlayerPrefs.GetInt("Level") < 1)
                {
                    StartCoroutine(Level());
                }
                else
                {
                    LoadingScreen.SetActive(false);
                }

            });
        }
        else
        {
            FadeScreen.SetActive(true);
        }
        instance = this;
      
    }
 
    IEnumerator Level()
    {
        yield return new WaitForSeconds(0.05f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public static bool NewUnlocking = false;
    public bool testLevel;
    public int mapLevelNo;
    public MapPatchScript[] mapPatchScripts;
    [Header("Extras")]
    public UnityEngine.UI.Image fadeImage;
    public GameObject playButton,LoadingScreen,FadeScreen;
    public Image Loading_Bar;


    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Isquite", 0);
    }
    public int tempLevel;
    private IEnumerator Start()
    {
        //if (PlayerPrefs.GetInt("MainLevelNo") == mapPatchScripts.Length)
        //{
         
        //    PlayerPrefs.SetInt("MainLevelNo", 0);
        //}
        if (!testLevel)
            mapLevelNo = PlayerPrefs.GetInt("MainLevelNo");
          
        if (PlayerPrefs.GetInt("Isquite") == 0)
        {
            NextCountry();
            PlayerPrefs.SetInt("Isquite", 1);
        }
        EnablePlayButton(false);

        tempLevel = mapLevelNo;
        if (NewUnlocking)
        {
            if (mapLevelNo > 0)
            {
                tempLevel = tempLevel - 1;
            }
        }
        else
        {
            EnablePlayButton(true);
        }

        for (int i = 0; i < tempLevel; i++)
        {
            Debug.Log("Preunlock");
            mapPatchScripts[i % mapPatchScripts.Length].PreUnlocked();
        }

        if (NewUnlocking)
        {

            NewUnlocking = false;

            if (mapLevelNo <= mapPatchScripts.Length)
                mapPatchScripts[tempLevel].SetUnlocked();


            yield return new WaitForSeconds(1f);
            EnablePlayButton(true);
        }
    }
    public void OnPlay()
    {
        StartCoroutine(OnPlayDelay());
      
    }
    System.Collections.IEnumerator OnPlayDelay()
    {
        EnablePlayButton(false);

        HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.Selection);
        AudioManagerNew.instance.Play("Pop");
      
        mapPatchScripts[mapLevelNo % mapPatchScripts.Length].GoInToThisCountry();
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, 0.5f).SetDelay(0.1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); 
    }
    private void EnablePlayButton(bool value)
    {
        playButton.SetActive(value);

    }
    public void Fade()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, 0.5f).SetDelay(0.5f).SetEase(Ease.Linear);
    }
    public void NextCountry()
    {
        if (mapLevelNo >= mapPatchScripts.Length)
        {
            //Debug.Log("Yek");
            mapLevelNo = 0;
            PlayerPrefs.SetInt("MainLevelNo", 0);
        }
      
        mapPatchScripts[mapLevelNo].
       transform.GetChild(0).gameObject.SetActive(false);
        mapPatchScripts[mapLevelNo].
       transform.GetChild(1).gameObject.SetActive(false);

    }
}