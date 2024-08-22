using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    public Camera Cam;
    public static ShopManager instance;

   

    public Material[] Kit_Mat, Helmet_Mat, puck_Mat, Hockey_Mat;
    public SkinnedMeshRenderer ShopHelmat, ShopKit;
    public MeshRenderer ShopPuck, ShopHockey;
    public Enventory_Manager[] Envetory;


    
    [SerializeField] List<SlectionInfo> Slection;
    [SerializeField] List<SlectedKits> ItemsMats;
    [SerializeField] List<ShopKits> Shop_playerMats;
    
    public Material Shop_Mat;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //Cash(5000);
    }
    private void Start()
    {

      UIManager.Instance.Cash_text.text = ((int)PlayerPrefs.GetFloat("Cash")).ToString();
    }
    [System.Serializable]
    public class SlectionInfo
    {
        public Transform Slected_Btn, Slected_panel,Cam;
        public Material ShopMat;

    } 
    [System.Serializable]
    public class SlectedKits
    {
        public Material Puck_Mat, Hockey_Mat, Kit_Mat, Helmat;
    } 
    [System.Serializable]
    public class ShopKits
    {
        public Material ShopPuck_Mat, Shop_Hockey_Mat, Shop_Kit_Mat, Shop_Helmat;
       

    }
    public void ButtonSlection(int Current)
    {
    for(int i = 0; i < Slection.Count; i++)
        {
            if (i== Current)
            {
               
                EnableScreen(Slection[i].Slected_panel.gameObject);
                EnableScreen(Slection[i].Cam.gameObject);
                Shop_Mat = Slection[i].ShopMat; /*1375f*/
                Slection[i].Slected_Btn.DOLocalMoveY(131f, 0.2f).SetEase(Ease.Linear);
                Slection[i].Slected_Btn.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
               
                DisableScreen(Slection[i].Slected_panel.gameObject);
                DisableScreen(Slection[i].Cam.gameObject);
                Slection[i].Slected_Btn.DOLocalMoveY(114f, 0.2f).SetEase(Ease.Linear);
                Slection[i].Slected_Btn.GetChild(1).gameObject.SetActive(false);
            }
        }

    }
   
    // Screens Enable/Disable--------------------------
    private void EnableScreen(GameObject screen)
    {
        screen.SetActive(true);
    }
    private void DisableScreen(GameObject screen)
    {
        screen.SetActive(false);
    }

    public void Update_Kits(bool isShop)
    {
        
        if (isShop)
        {
            ShopKit.material = Kit_Mat[PlayerPrefs.GetInt("ShopShirt")];
            ShopHelmat.material = Helmet_Mat[PlayerPrefs.GetInt("ShopShirt")];
            ShopHockey.material = Hockey_Mat[PlayerPrefs.GetInt("ShopStick")];
            ShopPuck.material = puck_Mat[PlayerPrefs.GetInt("ShopPuck")];
        }
        else
        {
            for (int i = 0; i < Envetory.Length; i++)
            {
                Envetory[i].Envetory();
            }
            PlayerPrefs.SetInt("ShopShirt", PlayerPrefs.GetInt("Shirt"));
            PlayerPrefs.SetInt("ShopStick", PlayerPrefs.GetInt("Stick"));
            PlayerPrefs.SetInt("ShopPuck", PlayerPrefs.GetInt("Puck"));
            ShopKit.material = Kit_Mat[PlayerPrefs.GetInt("ShopShirt")];
            ShopHelmat.material = Helmet_Mat[PlayerPrefs.GetInt("ShopShirt")];
            ShopHockey.material = Hockey_Mat[PlayerPrefs.GetInt("ShopStick")];
            ShopPuck.material = puck_Mat[PlayerPrefs.GetInt("ShopPuck")];
        }
    }
    // Dumy For cash Increasing ----------------------------------------------
    public void Cash(float value,bool text)
    {
        PlayerPrefs.SetFloat("Cash", PlayerPrefs.GetFloat("Cash") + value);
        if (text)
        { 
        UIManager.Instance.Cash_text.text = ((int)PlayerPrefs.GetFloat("Cash")).ToString();
        }
    }  
    public void WolletCash(int value)
    {
        PlayerPrefs.SetFloat("Cash", PlayerPrefs.GetFloat("Cash") + value);
        UIManager.Instance.Cash_text.text = PlayerPrefs.GetFloat("Cash").ToString();
    }
    public void Update_Cash(int value)
    {
        PlayerPrefs.SetFloat("Cash",PlayerPrefs.GetFloat("Cash")-value);
        UIManager.Instance.Cash_text.text = PlayerPrefs.GetFloat("Cash").ToString();
    }
}

