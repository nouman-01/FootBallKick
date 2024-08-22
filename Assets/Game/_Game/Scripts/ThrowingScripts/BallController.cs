using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
   public List<Vector2> pointTrack;
    bool mouseDown;

    bool kicked = false;
    Vector2 direction;
   public Vector2[] curveOffsets;
    public GameObject dummyObject;
    void Start()
    {
        mouseDown = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointTrack = new List<Vector2>();
            mouseDown = true;
            StartCoroutine(SavePoints());
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }

        if (kicked)
        {
            kicked = false;

            Debug.Log("Kick overall direction: " + direction);

            foreach (Vector2 v in curveOffsets)
            {
                Debug.Log("Curve " + v);
            }
        }

    }

    IEnumerator SavePoints()
    {
        Vector2 lastPos = (Vector2)Input.mousePosition;
        Vector2 pos;
        pointTrack.Add(lastPos);

        float tolerance = 5 * 5; //5 pixel tolerance

        while (mouseDown)
        {
            pos = (Vector2)Input.mousePosition;

            if ((pos - lastPos).sqrMagnitude >= tolerance)
            {
                pointTrack.Add(pos);
                lastPos = pos;
                Instantiate(dummyObject, pos, Quaternion.identity);
                Debug.Log("jajal");
            }

            yield return null;
        }

        if (pointTrack.Count < 2)
        {
            yield return false;
        }
        kicked = true;

        direction = pointTrack[pointTrack.Count - 1] - pointTrack[0];

        if (pointTrack.Count < 3)
        {
            yield return true;
        }

        float tolerance2 = 10;
        int dirChange = -1;
        Vector2 perpDir = new Vector2(-direction.y, direction.x).normalized; //left side
        int lastSide = 0;
        int side = 0;
        float largestOffset = 0;

        List<Vector2> offsets = new List<Vector2>();

        for (int i = 1; i < pointTrack.Count - 1; i++)
        {
            Vector2 offset = (pointTrack[i] - pointTrack[0]) - (Vector2)Vector3.Project(pointTrack[i] - pointTrack[0], direction);

            side = (int)Mathf.Sign(Vector3.Dot(perpDir, offset));

            if (offset.sqrMagnitude < tolerance2 * tolerance2) continue;

            if (side != lastSide)
            {
                dirChange++;
                lastSide = side;
                largestOffset = 0;
            }

            if (offset.sqrMagnitude > largestOffset)
            {
                largestOffset = offset.sqrMagnitude;

                if (dirChange < offsets.Count)
                {
                    offsets[dirChange] = offset;
                }
                else
                {
                    offsets.Add(offset);
                }
            }
        }
        curveOffsets = offsets.ToArray();
    }
}