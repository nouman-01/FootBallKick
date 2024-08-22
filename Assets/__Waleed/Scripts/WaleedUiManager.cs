using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaleedUiManager : MonoBehaviour
{
    public static WaleedUiManager instance;

    public Transform popupParentCanvas;
    public GameObject onePlusPopup;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    public void LoadPopup(Vector3 pos)
    {
        GameObject popUp = Instantiate(onePlusPopup, pos, Quaternion.identity);

        popUp.transform.parent = popupParentCanvas;
    }
}
