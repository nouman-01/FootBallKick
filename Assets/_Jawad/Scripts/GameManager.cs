using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public bool IsBonous = false;
    public bool isStart = false;
    [SerializeField]
    GameObject InGameScreen;
    public string HolderName = "GenerateLevels";
    Transform StageHolder;
    public Transform level;
    UIManager uIManager;
    public int TotalLevel;
    public bool IsMap = false;
    int lvlnumber;

    public enum GameState
    {
        play,
        completed,
        failed
    };
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Isquite", 0);
    }
    public GameState gameState;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        uIManager =UIManager.Instance;
        StartGame();

    }
    public void TaptoPlay()
    {
        if (!isStart)
        {
            if (!IsBonous)
            { 
        uIManager.ShowIngameScreen();
            }
            isStart = true;
        }
    }

    public void GenerateLevels()
    {
   
        UIManager.Instance.Keys_Parent.gameObject.SetActive(false);
        IsMap = true;
        uIManager.ShowStartscreen();
        isStart = false;
        if (PlayerPrefs.GetInt("Level") == TotalLevel)
        {
            Debug.Log("Yes Levels Again Here");
            PlayerPrefs.SetInt("Level", 0); //4
        }
        //Debug.LogError("genetare levelsss");
        if (GameObject.Find(HolderName))
        {
            DestroyImmediate(GameObject.Find(HolderName));
        }
        StageHolder = new GameObject(HolderName).transform;
        StageHolder.parent = this.transform;
        level = Instantiate(Resources.Load<Transform>("Levels/Level " + (PlayerPrefs.GetInt("Level") + 1).ToString())) as Transform;
        level.transform.parent = StageHolder;
      
        lvlnumber = PlayerPrefs.GetInt("Stage") + 1;
        
        uIManager.Lvltxt1.text = lvlnumber.ToString();
        uIManager.Lvltxt2.text = (lvlnumber+1).ToString();
        if (IsBonous)
        {
            
        }
    }
    public void StartGame()
    {
      
        GenerateLevels();
    }
    public void NextLevel()
    {
        if (LevelManager.instance)
        { 
        LevelManager.instance.KeyReset();
        LevelManager.instance.CoinsReset();
        }
        uIManager.DisableScreens();
        if (PlayerPrefs.GetInt("GKeys") < 2)
        {
            if (IsMap)
            {
                AddPlayerPrefValue();
                MapSystem();
            }
            else
            {
                gameState = GameState.play;
                AddPlayerPrefValue();
                GenerateLevels();
            }


        }
        else
        {
            PlayerPrefs.SetInt("GKeys", -1);
            UIManager.Instance.ShowRewardScreen();
        }
        //AddPlayerPrefValue();
        //GenerateLevels();
    }
    public void SkipLevel()
    {
        AddPlayerPrefValue();
        GenerateLevels();
   
    }
    public void _SkipLevel()
    {
       
    }
    public void RestartGame()
    {
        AudioManager.Instance.boo.Stop();
        gameState = GameState.play;
        uIManager.DisableScreens();
        GenerateLevels();

    }
  public  void AddPlayerPrefValue()
    {
        PlayerPrefs.SetInt("Level", (PlayerPrefs.GetInt("Level") + 1));
        if (IsMap )
        {
            PlayerPrefs.SetInt("Stage", (PlayerPrefs.GetInt("Stage") + 1));
        }
    }
    public void MapSystem()
    {
        gameState = GameState.play;
        uIManager.FadeImge.gameObject.SetActive(true);
        uIManager.FadeImge.DOFade(0, 0.5f).SetDelay(0.5f).SetEase(Ease.Linear);
        PlayerPrefs.SetInt("MainLevelNo", PlayerPrefs.GetInt("MainLevelNo") + 1);
        WorldMapController.NewUnlocking = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
