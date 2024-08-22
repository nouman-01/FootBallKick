using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
public class HapticControllerNew : MonoBehaviour
{
    public static HapticControllerNew instance;
    public bool canPlayHaptic;

    private void Awake()
    {
        instance = this;
    }
    public void PlayHaptic(HapticTypes type)
    {
        if (GetHapticStatus() == 1)
            return;
        MMVibrationManager.Haptic(type);
    }

    public void SetHapticStatus(int status) // 0 means on... 1 means off
    {
        PlayerPrefs.SetInt("isHaptic", status);
    }

    public int GetHapticStatus()
    {
        return PlayerPrefs.GetInt("isHaptic", 0);
    }

    private int haptic;
    public void HapticDiscrete(HapticTypes hapticType = HapticTypes.LightImpact, int ratio = 2)
    {
        if (haptic % ratio == 0)
        {
            TriggerHaptic(hapticType);
        }
        haptic++;
    }
    public void TriggerHaptic(HapticTypes _type)
    {
        if (GetHapticStatus() == 1)
            return;
        //if (canPlayHaptic)
        MMVibrationManager.Haptic(_type);

    }

    public void PlayLightHaptic()
    {
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }

    public void PlayRigidHaptic()
    {
        MMVibrationManager.Haptic(HapticTypes.RigidImpact);
    }
    public void PlayFailHaptic()
    {
        MMVibrationManager.Haptic(HapticTypes.Failure);
    }

}
