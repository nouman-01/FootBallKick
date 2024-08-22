using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    string LevelName;
    public GameObject keyModel;
    public GameObject vfx;
    private void Start()
    {
        LevelName = LevelManager.instance.name;
        if (PlayerPrefs.GetInt("RewardKeys") == 3 || PlayerPrefs.GetInt(transform.name+ LevelName) ==1)
        {
            keyModel.SetActive(false);
            GetComponent<SphereCollider>().enabled = false;
        
        }
        else
        {
            gameObject.SetActive(true);
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIManager.Instance.Keys_Parent.SetActive(true);
            PlayerPrefs.SetInt(transform.name +
            LevelManager.instance.name, 1);
            HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.RigidImpact);
            gameObject.GetComponent<SphereCollider>().enabled = false;
            if (keyModel)
                keyModel.SetActive(false);

            if (vfx)
                vfx.SetActive(true);
            AudioManager.Instance.KeyHitSound();
            if (PlayerPrefs.GetInt("GKeys") < 2)
            {
                PlayerPrefs.SetInt("GKeys", PlayerPrefs.GetInt("GKeys") + 1);
                UIManager.Instance.Keys_Parent.transform.GetChild(PlayerPrefs.GetInt("GKeys")).GetChild(0).gameObject.SetActive(true);
            }


            if (PlayerPrefs.GetInt("RewardKeys") != 3)
                PlayerPrefs.SetInt("RewardKeys", PlayerPrefs.GetInt("RewardKeys") + 1);

            //if (UIKeysCanvas.instance)
            //    UIKeysCanvas.instance.EnableKey();




        }
    }
    public void Reset_Key()
    {
        PlayerPrefs.SetInt(transform.name +
            LevelManager.instance.name, 0);
    }
}
