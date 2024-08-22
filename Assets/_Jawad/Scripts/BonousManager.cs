using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonousManager : MonoBehaviour
{
    public GameObject Bonous_Level,Current;
    public int currentBonous=0;
    public static  BonousManager instance;
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("MainLevelNo", PlayerPrefs.GetInt("MainLevelNo") + 1);
        WorldMapController.NewUnlocking = true;
        GameManager.instance.AddPlayerPrefValue();
    }
  
    private void Awake()
    {
        GameManager.instance.IsBonous = true;
        UIManager.Instance.ShowBonousScreen();
        instance = this;
    }
    public  IEnumerator spawnBonous()
    {
        yield return new WaitForSeconds(3f);
        UIManager.Instance.Bonous_Parent.GetChild(currentBonous).GetChild(0).gameObject.SetActive(false);
        if (currentBonous < 2) 
        {
            Destroy(Current);
            GameObject Ob = Instantiate(Bonous_Level);
            Ob.transform.SetParent(transform);
            Current = Ob;
            currentBonous++;
        }
        else
        {
          StartCoroutine(  UIManager.Instance.ShowwinScreen());
            Debug.Log("YouWin");
        }
    }
}
