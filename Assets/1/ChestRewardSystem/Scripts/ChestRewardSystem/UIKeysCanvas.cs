using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIKeysCanvas : MonoBehaviour
{
    public static UIKeysCanvas instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Chest Reward Keys")]
    public GameObject keysParent;
    public Image[] keysUI;

    private void OnEnable()
    {
        Init();
    }


    private void Init()
    {
        if (GetKeys() == 0)
        {
            keysParent.SetActive(false);
        }
        else
        {
            keysParent.SetActive(true);
        }

        ResetKeys();
    }


    #region KeysReward

    public void EnableKey()
    {
        keysParent.SetActive(true);

        int index = GetKeys() - 1;
        KeyFilling(keysUI[index], 0, 1);
    }

    public void DisableKey()
    {
        keysParent.SetActive(false);
    }

    void ResetKeys()
    {
        foreach (var item in keysUI)
        {
            item.fillAmount = 0;
        }

        for (int i = 0; i < GetKeys(); i++)
        {
            keysUI[i].fillAmount = 1f;
        }
    }

    int GetKeys()
    {
        return PlayerPrefs.GetInt("RewardKeys");
    }

    bool CheckChestRewardAvaiable()
    {
        return PlayerPrefs.GetInt("RewardKeys") == 3 ? true : false;
    }


    void KeyFilling(Image image, float startValue, float target = 0, float delay = 0)
    {

        float scale = 0.3f;
        image.transform.parent.DOPunchScale(new Vector3(scale, scale, scale), 1);
        float fill = startValue;

        DOTween.To(() => fill, x => fill = x, target, 0.5f).SetDelay(delay)
    .OnUpdate(() =>
    {
        image.fillAmount = fill;
    }
    );
    }

    #endregion
}
