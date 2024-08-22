using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    string Levelname;
    public GameObject CoinModel;
    public bool isBonous = false;
    private void Awake()
    {
        if (!isBonous) 
        { 
        Levelname = LevelManager.instance.name;
        if (PlayerPrefs.GetInt(transform.name + Levelname) == 1)
        {
            CoinModel.SetActive(false);
            GetComponent<SphereCollider>().enabled = false;
            //gameObject.SetActive(true);
        }
        else
        {
            //Destroy(gameObject);
        }
        }
    }
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isBonous)
            {
                PlayerPrefs.SetInt(transform.name + Levelname, 1);
            }
            var ui = WaleedUiManager.instance;
            ui.LoadPopup(Camera.main.WorldToScreenPoint(other.transform.position));
            AudioManager.Instance.CoinHitSound();
            HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
            ShopManager.instance.Cash(1,true);
            CoinModel.SetActive(false);
            GetComponent<SphereCollider>().enabled = false;
            
        }
    }
    public void Reset_Coin()
    {
        if (!isBonous)
        {
            PlayerPrefs.SetInt(transform.name +
            Levelname, 0);
        }
    }
}
