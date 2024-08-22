using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crowdOffsetController : MonoBehaviour
{
    public float offsetSpeed;

    public float reactionTime;
    public float reactionSpeed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DefaultCrownReaction());
    }

    // Update is called once per frame
    void Update()
    {
    }

    [ContextMenu("ShowCrownReaction")]
    public void ShowCrownReaction()
    {
        StopAllCoroutines();
        StartCoroutine(CrownReaction());
    }
    
    IEnumerator CrownReaction()
    {
        Vector2 offset = GetComponent<Renderer>().material.mainTextureOffset;
        float currentT = 0;
        while (currentT < reactionTime)
        {
            if(Random.value <= 0.5f)
                GetComponent<Renderer>().material.mainTextureOffset += new Vector2(reactionSpeed * Time.deltaTime, 0);
            else
                GetComponent<Renderer>().material.mainTextureOffset -= new Vector2(reactionSpeed * Time.deltaTime, 0);
            yield return null;
            currentT += Time.deltaTime;
        }
        // currentT = 0;
        // while (currentT < reactionTime)
        // {
        //     GetComponent<Renderer>().material.mainTextureOffset -= new Vector2(reactionSpeed * Time.deltaTime, 0);
        //     yield return null;
        //     currentT += Time.deltaTime;
        // }
        StartCoroutine(DefaultCrownReaction());
    }

    IEnumerator DefaultCrownReaction()
    {
        while (true)
        {
            GetComponent<Renderer>().material.mainTextureOffset += new Vector2(offsetSpeed * Time.deltaTime, 0);

            yield return null;
        }
    }
}
