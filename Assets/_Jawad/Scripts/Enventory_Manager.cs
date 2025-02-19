using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enventory_Manager : MonoBehaviour
{
    public int indx;
    ShopManager shopManager;
    public GameObject PurchaseBtn;
    public GameObject PurchaseBtn1;
    //Use Name For Playerprefs use in game
    public string Name;
    public List<ItemInfo> Items;
    [System.Serializable]
    public class ItemInfo
    {
        public Text Price_Text;
        public int Price;
        public Button Button;
        public Image LockImg;
    }
    private void Start()
    {
        
        shopManager = ShopManager.instance;
      //  PurchaseBtn.SetActive(true);
        Envetory();


    }
   
    private void OnEnable()
    {
        Envetory();


    }
    public void BuyItme(int CurrentBtn_indx)
    {



        HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        indx = CurrentBtn_indx;
        if (indx >= 0 && indx <= 5 && name == "shirt")
        {
            foreach (var kit in shopManager.Kit_Mat)
            {
                kit.SetActive(false);
            }
            Debug.LogError("shirt click");
            shopManager.Kit_Mat[indx].SetActive(true);
        }
        if (indx >= 0 && indx <= 5 && name == "ball")
        {
            foreach (var puck in shopManager.puck_Mat)
            {
                puck.SetActive(false);
            }

            shopManager.puck_Mat[indx].SetActive(true);
        }
        if (indx >= 0 && indx <= 5 && name == "shoes")
        {
            foreach (var hockey in shopManager.Hockey_Mat)
            {
                hockey.SetActive(false);
            }

            shopManager.Hockey_Mat[indx].SetActive(true);
        }
        PlayerPrefs.SetInt("Shop" + Name, indx);
        ShopManager.instance.Update_Kits(true);
                PurchaseBtn1.SetActive(false);
        PurchaseBtn.SetActive(false);
        Sletedoff();
        Items[indx].Button.GetComponent<ButtonInfo>().Slected.SetActive(true);
        if (PlayerPrefs.GetInt(Items[CurrentBtn_indx].Button.name) == 0)
        {
            Debug.Log(Items[CurrentBtn_indx].Price + "Items Price Bug");
            if (PlayerPrefs.GetInt("Cash") >= Items[CurrentBtn_indx].Price)
            {
                Debug.Log(PlayerPrefs.GetInt("Cash") + "Cash Bug");
                PurchaseBtn.SetActive(true);
                PurchaseBtn1.SetActive(false);
                PurchaseBtn.GetComponent<Button>().interactable = true;
                Debug.LogError("click1");
            }
            else if (PlayerPrefs.GetInt(Items[CurrentBtn_indx].Button.name) == 0)
            {
                Debug.Log(Items[CurrentBtn_indx].Price + "Items Price Bug");
                if (PlayerPrefs.GetInt("Cash") <=Items[CurrentBtn_indx].Price)
                {
                    Debug.LogError("wokkkk" + PlayerPrefs.GetInt("Cash") + "Cash Bug");
                    PurchaseBtn1.SetActive(true);
                    PurchaseBtn.SetActive(false);
                    PurchaseBtn.GetComponent<Button>().interactable = false;
                    Debug.LogError("click2");


                }


                // PurchaseBtn.SetActive(false);
                //  PurchaseBtn.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            PlayerPrefs.SetInt(Name, indx);
            LevelManager.instance.UpdateShop();
        }

    }
    public void purchaseBtn1()
    {
        UIManager.Instance.rewardPannel.SetActive(true);
    }
    public void Purchase_Item()
    {
            Debug.LogError(PlayerPrefs.GetInt("Cash") + "Cash Bug");


            shopManager.Update_Cash(Items[indx].Price);
            PlayerPrefs.SetInt(Items[indx].Button.name, 1);
        PurchaseBtn.SetActive(false);
        PurchaseBtn1.SetActive(false);
        Debug.LogError("click3");

        // PurchaseBtn.GetComponent<Button>().interactable = false;
        PlayerPrefs.SetInt(Name, indx);
            LevelManager.instance.UpdateShop();
            Items[indx].LockImg.gameObject.SetActive(false);
        

     }
        public void Start_Enventory()
    {
        for(int i=0;i< Items.Count; i++)
        {
            if (PlayerPrefs.GetInt(Items[i].Button.name) == 0)
            {
                if (i == 0)
                {
                    PlayerPrefs.SetInt(Items[i].Button.name,1);
                    Items[i].LockImg.gameObject.SetActive(false);
                  
                }
                else
                {
                    Items[i].LockImg.gameObject.SetActive(true);
                    Items[i].Price_Text.text =Items[i].Price.ToString();
                }
            }
            else
            {
                Items[i].LockImg.gameObject.SetActive(false);
            }
        }
    }
    public void Sletedoff()
    {
        for (int i=0;i< Items.Count; i++)
        {
            Items[i].Button.GetComponent<ButtonInfo>().Slected.SetActive(false);
        }
    }
    public void Envetory()
    {
        Sletedoff();
        Items[PlayerPrefs.GetInt(Name)].Button.GetComponent<ButtonInfo>().Slected.SetActive(true);
        Start_Enventory();
    }
    
}
