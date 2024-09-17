using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public bool IsMap = true;
    GameObject[] coins, Keys;
    public static LevelManager instance;
    public PlayerController playerController;
    public int Total_index = 0, current_index = 0;
    public bool isScndplayer = false,IsGoal=false;
    public List<Sublvl_Info> Sublvl;
    public int AImin = 8,AImax=16;
    [System.Serializable]
    public class Sublvl_Info
    {
        public Puck puck;
        public PlayerController playerController;
        public GameObject Cameras;

    }
    private void Awake()
    {
        GameManager.instance.IsBonous = false;
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
       GameManager.instance. IsMap = IsMap;
        UIManager.Instance.sublvl_Indicator(Total_index);
        UIManager.Instance.sublvl_indicator.GetChild(current_index).GetChild(1).gameObject.SetActive(true);
        UpdateShop();
      
        //EnableCurrent_subLevl();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator Move_Next()
    {

        UIManager.Instance.sublvl_indicator.GetChild(current_index).GetChild(0).gameObject.SetActive(true);
        if (isScndplayer)
        {
            Transform Puck1 = Sublvl[current_index].puck.transform;
            Puck1.GetComponent<Puck>().puck = false;
            Puck1.GetComponent<Puck>().rb.constraints = RigidbodyConstraints.FreezeAll;
            Puck1.DOMove(Sublvl[current_index + 1].puck.transform.position, 0.8f).OnComplete(() =>
            {
                isScndplayer = false;
                Sublvl[current_index].puck.enabled = false;

                Destroy(Puck1.gameObject, 1.5f);
            });
        }
        yield return new WaitForSeconds(0.8f);

        Sublvl[current_index].puck.enabled = false;
        current_index++;
        GameManager.revive_Index = current_index;
        UIManager.Instance.sublvl_indicator.GetChild(current_index).GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        if (current_index < Total_index)
        {
            //CamerShake(true, true);
            GameManager.instance.gameState = GameManager.GameState.play;
            Sublvl[current_index].puck.transform.parent.gameObject.SetActive(true);
            Sublvl[current_index].puck.gameObject.SetActive(true);
            Sublvl[current_index].puck.enabled = true;
            Sublvl[current_index].playerController.enabled = true;
            playerController = Sublvl[current_index].playerController;
            Sublvl[current_index].playerController.GetComponent<Animator>().enabled = true;
            Sublvl[current_index].Cameras.SetActive(true);
        }
        else
        {

            StartCoroutine(UIManager.Instance.ShowwinScreen());

            //UIManager.Instance.ShowWinScreen();  
            Debug.Log("You Win");
        }
    }
    public IEnumerator MyNextLevel(int Lvlindex)
    {
        UIManager.Instance.sublvl_indicator.GetChild(Lvlindex).GetChild(0).gameObject.SetActive(true);
        if (isScndplayer)
        {
            Transform Puck1 = Sublvl[Lvlindex].puck.transform;
            Puck1.GetComponent<Puck>().puck = false;
            Puck1.GetComponent<Puck>().rb.constraints = RigidbodyConstraints.FreezeAll;
            Puck1.DOMove(Sublvl[Lvlindex + 1].puck.transform.position, 0.8f).OnComplete(() =>
            {
                isScndplayer = false;
                Sublvl[Lvlindex].puck.enabled = false;

                Destroy(Puck1.gameObject, 1.5f);
            });
        }
        yield return new WaitForSeconds(0.8f);

        Sublvl[Lvlindex].puck.enabled = false;

        Lvlindex++;
        current_index = Lvlindex;
       // current_index++;

        UIManager.Instance.sublvl_indicator.GetChild(Lvlindex).GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        if (Lvlindex < Total_index)
        {
            //CamerShake(true, true);
            GameManager.instance.gameState = GameManager.GameState.play;
            Sublvl[Lvlindex].puck.transform.parent.gameObject.SetActive(true);
            Sublvl[Lvlindex].puck.gameObject.SetActive(true);
            Sublvl[Lvlindex].puck.enabled = true;
            Sublvl[Lvlindex].playerController.enabled = true;
            playerController = Sublvl[Lvlindex].playerController;
            Sublvl[Lvlindex].playerController.GetComponent<Animator>().enabled = true;
            Sublvl[Lvlindex].Cameras.SetActive(true);
        }
        else
        {

            StartCoroutine(UIManager.Instance.ShowwinScreen());

            //UIManager.Instance.ShowWinScreen();  
            Debug.Log("You Win");
        }
    }
    public void CamerShake()
    {
        if (current_index < Sublvl.Count)
        {
            Sublvl[current_index].Cameras.GetComponent<DOTweenAnimation>().DORestartById("Goal");

        }
        else
        {
            Sublvl[current_index - 1].Cameras.GetComponent<DOTweenAnimation>().DORestartById("Goal");

        }
    }
    
    public void UpdateShop()
    {
        for (int i = 0; i < Sublvl.Count; i++)
        {
            Sublvl[i].playerController.ShopObj();

            //Sublvl[i]..GetComponent<MeshRenderer>().material.mainTexture =
            //       ShopManager.instance.Puck[PlayerPrefs.GetInt("Puck")];
        }

    }
    public void EnableCurrent_subLevl()
    {
        for(int i = 0; i < Sublvl.Count; i++)
        {
            if (i == 0)
            {
                Sublvl[i].puck.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                Sublvl[i].puck.transform.parent.gameObject.SetActive(false);

            }

        }

    }
    public void KeyReset()
    {
        Keys = GameObject.FindGameObjectsWithTag("key");
        foreach (GameObject Key in Keys)
        {
            Key.GetComponent<KeyScript>().Reset_Key();
            ////Debug.Log("Reset Keys Here");
            //PlayerPrefs.SetInt(Key.name+transform.name, 0);
            //Debug.Log("YesKeyFalse Here Playerpref Here ==" +
            //  transform.name + LevelManager.instance.name +
            //  PlayerPrefs.GetInt(transform.name + LevelManager.instance.name));
        }
    }
    public void CoinsReset()
    {
        coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach(GameObject coin in coins)
        {
            coin.GetComponent<Coin>().Reset_Coin();
        }

    }
}
