using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MapPatchScript : MonoBehaviour
{


    public GameObject greyArea;
    public GameObject colorArea;
    public GameObject myCam;
    public ParticleSystem confity;


    public void PreUnlocked()
    {

        greyArea.SetActive(false);
        colorArea.SetActive(true);
        colorArea.GetComponent<SpriteRenderer>().DOFade(1, 0f);
        //WorldMapController.instance.NextCountry();
    }
   
    public void SetUnlocked()
    {
      StartCoroutine(SetUnlockedDelay());
    }

   System.Collections.IEnumerator SetUnlockedDelay()
   {
    
        greyArea.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        confity.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        colorArea.SetActive(true);
        colorArea.GetComponent<SpriteRenderer>().DOFade(1, 0.4f).SetEase(Ease.Linear).OnComplete(()=> {
            WorldMapController.instance.NextCountry();
        });
    }
    

   public void GoInToThisCountry()
   {
      WorldMapController.instance.Fade();
      myCam.SetActive(true);
   }

   
   
}
