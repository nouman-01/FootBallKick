using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIChestReward : MonoBehaviour
{
    public static UIChestReward instance;
    public Image BestReward_Img;

    private void Awake()
    {
        instance = this;
    }

    public int remainingKeys = 3;
    public bool isKit = true;
    public int indx, chestItm_indx;
    public bool Get_Prize = false;
    public Enventory_Manager[] enventory_manager;
    public UIChestItem[] uiChestItems;


    public Image[] keysImages;


    public GameObject GetKeyBtn;
    public GameObject nextBtn;
    public GameObject noThanks;





    public AudioClip sfx_addKey;
    public AudioClip sfx_openChest;
    public AudioClip sfx_chestLocked;
    public AudioClip sfx_keyCollect;
    public AudioClip sfx_buttonClick;



    AudioSource audioSource;
    int openedBoxes;
    private void OnEnable()
    {

        indx = Random.Range(1, 3);

        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();

        openedBoxes = 0;
        remainingKeys = 3;
        Chestskin_Reward(BestReward_Img, false);
        //chestRoomCamera.SetActive(true);

    }

    private void OnDisable()
    {
        GetKeyBtn.SetActive(false);
        nextBtn.SetActive(false);
        noThanks.SetActive(false);
        //chestRoomCamera.SetActive(false);

        for (int i = 0; i < keysImages.Length; i++)
        {
            keysImages[i].fillAmount = 1;
        }

        PlayerPrefs.SetInt("RewardKeys", remainingKeys);

    }


    //open chests based upon there index
    public void OpenChest(int index)
    {
        chestItm_indx = index;
        StartCoroutine(OpenChestDelay(index));

    }

    IEnumerator OpenChestDelay(int index)
    {
        if (remainingKeys > 0)
        {
            KeyFilling(keysImages[remainingKeys - 1], 1, 0);

            remainingKeys--;
            openedBoxes++;

            audioSource.PlayOneShot(sfx_addKey);
            audioSource.PlayOneShot(sfx_openChest);
            uiChestItems[index].PlayOpenChest();
            uiChestItems[index].DisableButtonInteration();
            yield return new WaitForSeconds(0.5f);
            uiChestItems[index].EnableUnlockVFX();
            StartCoroutine(AddCoinToMainCash(uiChestItems[index].GetRewardAmount(), 0.5f));

            yield return new WaitForSeconds(0.3f);
            if (isKit && indx == remainingKeys && PlayerPrefs.GetInt("ChestBox")==3)
            {

                Chestskin_Reward(uiChestItems[index].SkinImg,true);
            }
            else
            {
                
                CollectAnim.instance.ShowAnim(uiChestItems[index].transform);
                uiChestItems[index].EnableRewardDetail();
            }
            CheckBoxesAndKeys();

        }
        else
        {
            uiChestItems[index].PlayShakeChest();
            audioSource.PlayOneShot(sfx_chestLocked);
        }

    }
    public void Chestskin_Reward(Image Rewad_img,bool Isplayerpref)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int a = 0; a < enventory_manager[i].Items.Count; a++)
            {
                if (PlayerPrefs.GetInt(enventory_manager[i].Items[a].Button.name) == 0 && a != 0)
                {
                    BestReward_Img.sprite = enventory_manager[i].Items[a].Button.transform.GetChild(0).GetComponent<Image>().sprite;

                    if (Isplayerpref)
                    { 
                    Get_Prize = true;
                    PlayerPrefs.SetInt(enventory_manager[i].Items[a].Button.name, 1);
                    Rewad_img.gameObject.SetActive(true);
                    Rewad_img.sprite = enventory_manager[i].Items[a].Button.transform.GetChild(0).GetComponent<Image>().sprite;
                    }
                    goto Defauld;
                }

            }
        }
        Defauld:
        if (!Get_Prize&& Isplayerpref)
        {
            uiChestItems[chestItm_indx].EnableRewardDetail();
            CollectAnim.instance.ShowAnim(uiChestItems[chestItm_indx].transform);
        }
    }

    void KeyFilling(Image image, float startValue, float target = 0)
    {

        float scale = 0.3f;
        image.transform.parent.DOPunchScale(new Vector3(scale, scale, scale), 1);

        float fill = startValue;

        DOTween.To(() => fill, x => fill = x, target, 0.5f)
    .OnUpdate(() =>
    {
        image.fillAmount = fill;
    }
    );
    }

    void CheckBoxesAndKeys()
    {
        if (openedBoxes != 9)
        {
            if (remainingKeys == 0)
            {

                EnableGetKeyAdButton();

                // if (S_Ads.instance.Is_RewardedVideoAvalaible())
                // {
                //     int missionID = PlayerPrefs.GetInt("LevelForEndReward");
                //     S_Ads.instance.SendRVImpression("chestReward_getExtraKeys", missionID);
                // }
                // else
                // {
                //     EnableNextButton();
                // }

            }
        }
        else
        {
            EnableNextButton();
        }
    }

    public void OnExtraKeys()
    {

        Click();

        GetKeyBtn.SetActive(false);
        noThanks.SetActive(false);

        ClaimedReward();

    }

    void Click()
    {
        audioSource.PlayOneShot(sfx_buttonClick);
    }

    void ClaimedReward()
    {
        if (openedBoxes != 9)
        {
            remainingKeys = 3;
            audioSource.PlayOneShot(sfx_keyCollect);

            for (int i = 0; i < keysImages.Length; i++)
            {
                KeyFilling(keysImages[i], 0, 1);

            }
        }
    }

    void NotClaimed()
    {

        // if (S_Ads.instance.Is_RewardedVideoAvalaible())
        // {
        //     EnableGetKeyAdButton();
        // }
        // else
        // {
        //     EnableNextButton();
        // }
    }

    public void OnNext()
    {
        Click();


    }


    void EnableGetKeyAdButton()
    {
        //nextBtn.SetActive(true);
        //GetKeyBtn.SetActive(true);
        //noThanks.SetActive(true);
        nextBtn.SetActive(true);
    }

    void EnableNextButton()
    {
        GetKeyBtn.SetActive(false);
        noThanks.SetActive(false);
        nextBtn.SetActive(true);
    }


    int counter = 0;
    private IEnumerator AddCoinToMainCash(int noOfCoins, float duration)
    {

        counter = 0;
        float current = 0;
        int currentCoins = PlayerPrefs.GetInt("currentCash");
        int targetCoins = currentCoins + noOfCoins;

        PlayerPrefs.SetInt("currentCash", targetCoins);




        while (current < duration)
        {

            string temp = Mathf.Lerp(currentCoins, targetCoins, (current / duration) * 1f).ToString("n0");


            // if (UiManager.instance)
            //     UiManager.instance.SetMainCashText(temp);

            current += Time.deltaTime;
            counter++;
            yield return null;
        }

        // if (UiManager.instance)
        //     UiManager.instance.SetMainCashText(PlayerPrefs.GetInt("currentCash").ToString("n0"));



    
}


}
