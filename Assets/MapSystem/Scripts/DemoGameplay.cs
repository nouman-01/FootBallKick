using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoGameplay : MonoBehaviour
{
   public void CompleteLevel()
   {
      PlayerPrefs.SetInt("MainLevelNo",PlayerPrefs.GetInt("MainLevelNo")+1);
      
      WorldMapController.NewUnlocking = true;
      UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);

   }
       
}
