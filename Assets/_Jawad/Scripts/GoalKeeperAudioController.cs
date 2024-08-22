using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalKeeperAudioController : MonoBehaviour
{
    public bool Ismoving = false;
    public DOTweenAnimation Tween;
    public void PlayKeeperGoalSavedSound()
    {
        AudioManager.Instance.PlayGoalSavedKeeperSound(0);
    }

    public void PlayKeeperGoaledSound()
    {
        AudioManager.Instance.PlayGoalKeeperSound(1);
    }
}
