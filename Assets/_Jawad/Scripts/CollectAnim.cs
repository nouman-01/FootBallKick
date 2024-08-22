using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAnim : MonoBehaviour
{
    public static CollectAnim instance;

    public Transform endTarget;
    public GameObject itemPrefab;
    //public Transform startPos;
    public Transform parentTransform;

    public int noOfItemsToGenerate = 1;
    public float spawnRadius = 2f;
    public float moveRadius = 200f;
    public float timeToAnimate = 1f;

    public bool isCanvasCamera;
    public int NoToAddInPlayerPref = 50;
    List<GameObject> prefabsReference;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Z))
        //{
        //    ShowAnim();
        //}
    }

    public void ShowAnim(Transform startPos)
    {
        prefabsReference = new List<GameObject>();

        for (int i = 0; i < noOfItemsToGenerate; i++)
        {
            Vector2 positionToSpawn = Random.insideUnitCircle * spawnRadius;
            GameObject objInstantiated = Instantiate(itemPrefab, startPos.position, Quaternion.identity);
            objInstantiated.transform.parent = startPos;
            objInstantiated.transform.localPosition = positionToSpawn;
            prefabsReference.Add(objInstantiated);
        }
        if (!isCanvasCamera)
        {
            for (int i = 0; i < noOfItemsToGenerate; i++)
            StartCoroutine(MoveRoutine2DCanvas(prefabsReference[i].transform, Random.Range(0f, 0.2f), timeToAnimate));
        }
        else
        {
            for (int i = 0; i < noOfItemsToGenerate; i++)
            StartCoroutine(MoveRoutine3DCanvas(prefabsReference[i].transform, Random.Range(0f, 0.2f), timeToAnimate));
        }
    }

    #region 2DCanvas
    IEnumerator MoveRoutine2DCanvas(Transform obj, float waitTime,float duration)
    {
        yield return new WaitForSeconds(waitTime);

        float current = 0;

        Vector2 parentPosition = transform.position;
        Vector2 moveToPos = parentPosition + Random.insideUnitCircle * moveRadius;

        while (current < duration)
        {
            Vector2 objPosition = obj.position;

            if (objPosition == moveToPos)
                break;

            obj.position = Vector2.Lerp(obj.position, moveToPos, current / duration);

            //PlayerController.instance.currentScore++;
            //UiManager.instance.scoreText.text = PlayerController.instance.currentScore.ToString();
            current += Time.deltaTime;
            yield return null;
        }
       
        StartCoroutine(MoveToTarget2D(obj, 0f, 2f));
    }
    IEnumerator MoveToTarget2D(Transform obj, float waitTime, float duration)
    {
        yield return new WaitForSeconds(waitTime);

        float current = 0;
        Vector3 endPos = endTarget.position;

        while (current < duration)
        {
            obj.position = Vector2.Lerp(obj.position, endPos, current / duration);

            current += Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region 3DCanvas
    IEnumerator MoveRoutine3DCanvas(Transform obj, float waitTime, float duration)
    {
        yield return new WaitForSeconds(waitTime);

        float current = 0;

        Vector2 parentPosition = transform.position;
        Vector2 moveToPos = parentPosition + Random.insideUnitCircle * moveRadius;

        while (current < duration)
        {
            Vector2 objPosition = obj.localPosition;

            if (objPosition == moveToPos)
                break;

            obj.localPosition = Vector2.Lerp(obj.localPosition, moveToPos, current / duration);

            current += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(MoveToTarget3D(obj, 0f, 2f));
    }

    IEnumerator MoveToTarget3D(Transform obj, float waitTime, float duration)
    {
        yield return new WaitForSeconds(waitTime);

        float current = 0;
        Vector3 endPos = endTarget.position;

        while (current < duration)
        {
            obj.position = Vector3.Lerp(obj.position, endPos, current / duration);

            current += Time.deltaTime;
            yield return null;
        }
        Destroy(obj.gameObject);
    }
    #endregion
}
