using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PassController : MonoBehaviour
{

    public List<GameObject> objectsToTurnOn;
    public DOTweenAnimation passTween;
    // Start is called before the first frame update

    public GameObject player;
    Animator animPlayer;
    public static PassController Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        animPlayer = player.GetComponent<Animator>();
    }

    public void Shoot()
    {
        print("ShootPass");
        foreach (var objectToTurnOn in objectsToTurnOn)
        {
            objectToTurnOn.SetActive(true);
        }
        passTween.DORestartAllById("MoveToNextPosition");
        gameObject.SetActive(false);
    }
    public void PlayShootAnimation()
    {
        animPlayer.Play("Shoot");
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PlayShootAnimation();
        }
    }
}
