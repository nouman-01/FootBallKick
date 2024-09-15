using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screen_check : MonoBehaviour
{
    public Canvas manincanvas;
    // Start is called before the first frame update
    void Start()
    {



    }
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("MM") == 0)
        {
            this.gameObject.SetActive(true);
            manincanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            Debug.LogError("mainmenutruee");
        }
        else if (PlayerPrefs.GetInt("MM") == 1)
        {
            this.gameObject.SetActive(false);
            manincanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            Debug.LogError("mainmenufalse");

        }
        Debug.LogError("Start");
    }
    private void OnDisable()
    {
            manincanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnApplicationQuit()
    {
        
    }
}
