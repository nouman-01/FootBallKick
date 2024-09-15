using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showBannerAd : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        admanager.instance.showBoxBanner();
    }
    private void OnDisable()
    {
        admanager.instance.hideBoxBanner();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
