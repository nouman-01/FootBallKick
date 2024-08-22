using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIChestItem : MonoBehaviour
{
    public int rewardAmount;
    public GameObject chestModel;
    public TMPro.TextMeshProUGUI rewardAmountText;
    public UnityEngine.UI.Button chestButton;
    public Image SkinImg;


    public GameObject rewardDetail;
    public GameObject vfx_Unlock;



    private void OnEnable()
    {
        rewardDetail.gameObject.SetActive(false);
        vfx_Unlock.gameObject.SetActive(false);
        SkinImg.gameObject.SetActive(false);
        chestButton.interactable = true;
        rewardAmountText.text = rewardAmount.ToString("n0");
    }



    public void PlayOpenChest()
    {
        
        //chestModel.GetComponent<DOTweenAnimation>().DOPlayById("Close");
        chestModel.transform.DOScale(Vector3.zero,0.7f).SetEase(Ease.InOutElastic);
        
        //chestModel.gameObject.SetActive(false);
    }

    public void PlayShakeChest()
    {
        chestModel.GetComponent<DOTweenAnimation>().DORestartById("Shake");
        //chestModel.GetComponent<Animator>().SetTrigger("Shake");

    }

    public void EnableRewardDetail()
    {
        rewardDetail.gameObject.SetActive(true);

    }

    public void EnableUnlockVFX()
    {
        vfx_Unlock.gameObject.SetActive(true);
    }

    public void DisableButtonInteration()
    {
        chestButton.interactable = false;
    }

    public int GetRewardAmount()
    {
        return rewardAmount;
    }

}
