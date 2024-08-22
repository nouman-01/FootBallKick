using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class Control : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform Arrow_Parent;
    public Transform Start_point, Middle_point, LastPoint;
    public bool isMove = false;
    public Vector3[] Positions;
    public float Min,Max,speed,dragspeed;
    int index = 0;
    float MiddlePos,Lastpos;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = Positions.Length;
    }
 

    void Update()
        {
        if (!isMove)
        {
            DrawQuadraticBezierCurve(Start_point.position, Middle_point.position, LastPoint.position);
        }
        else
        {
            Vector3 destination = Positions[index];
            Vector3 newpos = Vector3.MoveTowards(transform.position, destination, speed );
            transform.position = newpos;
            float Dis = Vector3.Distance(transform.position, destination);
            if (Dis <= 0.05f)
            {
                if(index< Positions.Length-1)
                {
                index++;
                }
                else
                {
                    rb.AddForce(transform.forward * 0.5f);
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            MiddlePos = Middle_point.localPosition.x + Input.GetAxis("Mouse X") *  dragspeed;
            MiddlePos = Mathf.Clamp(MiddlePos, Min, Max);
            if (Middle_point.localPosition.x == LastPoint.localPosition.x)
            {
                Middle_point.localPosition = new Vector3(MiddlePos, Middle_point.localPosition.y, Middle_point.localPosition.z);
            }
            if (Middle_point.localPosition.x < Max && Middle_point.localPosition.x > Min)
            {
               
                LastPoint.localPosition = new Vector3(MiddlePos, Middle_point.localPosition.y, LastPoint.localPosition.z);
            }
            else
            {
                float  Lastpos= LastPoint.localPosition.x - Input.GetAxis("Mouse X") * dragspeed;
                if (Middle_point.localPosition.x > 0)
                {
                Lastpos = Mathf.Clamp(Lastpos, -1, Max);
                LastPoint.localPosition = new Vector3(Lastpos, LastPoint.localPosition.y, LastPoint.localPosition.z);
                }
                else if(Middle_point.localPosition.x < 0)
                {
                Lastpos = Mathf.Clamp(Lastpos,Min,1f);
                LastPoint.localPosition = new Vector3(Lastpos, LastPoint.localPosition.y, LastPoint.localPosition.z);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //GetComponent<LineRenderer>().enabled = false;
        }
    }
    void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {

        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < Positions.Length; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
            Positions[i] = B;
            Arrow_Parent.GetChild(i).position = B;
            if (i != Positions.Length - 1) 
            { 
            Arrow_Parent.GetChild(i).LookAt(Arrow_Parent.GetChild(i + 1));
            }
            else
            {
                Arrow_Parent.GetChild(i).LookAt(Arrow_Parent.GetChild(i - 1).position);

            }
        }
    }

}







