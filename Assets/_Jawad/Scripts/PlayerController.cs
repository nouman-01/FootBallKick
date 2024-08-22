using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool Isbonospuck = false;
    // Start is called before the first frame update
    //public Transform PuckParent,HockeyParent,HelmatParent,KitParent;
    public SkinnedMeshRenderer Helmat, Kit;
    public MeshRenderer Hockey, Puck;


    public static PlayerController instance;
    public Puck puck;
    public BonousPuck Bonous_puck;
    public Animator animPlayer;
    public static int CurrentLevel = 0;
    
    //public Transform Kit_Mat, Helmat,Hockey;
    ShopManager shopManager;
    void OnEnable()
    {
        instance = this;
    }
    private void Start()
    {
        animPlayer = GetComponent<Animator>();
        shopManager = ShopManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    public void LoadNextLevel()
    {
        PlayerPrefs.SetInt("CurrentLevelGlobal",PlayerPrefs.GetInt("CurrentLevelGlobal",1)+1);
        CurrentLevel++;
        CurrentLevel %= SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(CurrentLevel);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(CurrentLevel);
    }
    public void Shoot()
    {
        if (!Isbonospuck)
        {
        puck.ShootTheBall();

            AudioManager.Instance.PlayHitSound();
            HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);
        }
        else
        {
            StartCoroutine(BonousManager.instance.spawnBonous());
            Bonous_puck.ShootTheBall();
        }

    }
    public void PauseGame()
    {
        //Time.timeScale = 0;
        //UIManager.Instance.tutorial.SetActive(true);
    }
    public void ResumeGame()
    {
        //Time.timeScale = 1;
    }
    public void PlayShootAnimation()
    {
        //Invoke("Reset", 2f);
        //ResumeGame();
        animPlayer.Play("Shoot");
        //animPlayer.StopPlayback
    }
    private void Reset()
    {
        animPlayer.SetTrigger("Idle");
    }
    public void OnGoaled()
    {
        GetComponent<DOTweenAnimation>().DORestartById("rotatetoOrigin");
        Camera.main.GetComponent<DOTweenAnimation>().DORestartById("WinTween");
        animPlayer.SetTrigger("Win");
    }
    public void OnGoalStopped()
    {
        GetComponent<DOTweenAnimation>().DORestartById("rotatetoOrigin");
        Camera.main.GetComponent<DOTweenAnimation>().DORestartById("FailTween");
        animPlayer.SetTrigger("Lose");
    }
    public   void ShopObj()
    {
        Kit.material=     ShopManager.instance.Kit_Mat[PlayerPrefs.GetInt("Shirt")];
        Helmat.material= ShopManager.instance.Helmet_Mat[PlayerPrefs.GetInt("Shirt")];
        Hockey.material = ShopManager.instance.Hockey_Mat[PlayerPrefs.GetInt("Stick")];
        Puck.material = ShopManager.instance.puck_Mat[PlayerPrefs.GetInt("Puck")];
    }
}
