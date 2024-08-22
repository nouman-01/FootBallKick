using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using HedgehogTeam.EasyTouch;
public class BonousPuck : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform Arrows_Parent;
    public Transform Start_point, Middle_point, LastPoint;
    public Vector3 startPos;
    GameManager gameManager;
    Vector3 lastVelocity;
    Rigidbody rb;
    int index = 0;
    public Vector3[] Positions = new Vector3[18];
    public bool isMove = false, puck = true, Iswin = false, Isgamestart = false, isControl=true;
    float speed = 5.7f, dragspeed = 4f, MiddlePos, Rot_value = 13f;
    public float Min, Max;
    private void Start()
    {
        lineRenderer.positionCount = Positions.Length;
        gameManager = GameManager.instance;
        rb = GetComponent<Rigidbody>();
        startPos = transform.localPosition;
    }
    private void Update()
    {
        if (!IsPointerOverUIObject() && gameManager.isStart && !Iswin)
        {
            if (!isMove)
            {
                DrawQuadraticBezierCurve(Start_point.position, Middle_point.position, LastPoint.position);
            }
            else
            {
                if (puck)
                {
                    Vector3 destination = Positions[index];
                    Vector3 newpos = Vector3.MoveTowards(transform.localPosition, destination, speed);
                    transform.localPosition = newpos;

                    float Dis = Vector3.Distance(transform.localPosition, destination);
                    if (Dis <= 0.05f)
                    {
                        if (index < Positions.Length - 1)
                        {
                            index++;
                        }
                        else
                        {
                        puck = false;
                        transform.rotation = Arrows_Parent.GetChild(Arrows_Parent.childCount - 3).transform.rotation;
                        rb.AddForce(transform.forward *2500, ForceMode.Acceleration);
                          
                        }
                    }
                }
            }

        }
        if (Input.GetMouseButtonUp(0) && Isgamestart)
        {
            isControl = false;
            Arrows_Parent.gameObject.SetActive(false);
            PlayerController.instance.PlayShootAnimation();

        }
    }
    void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
        {
            float t = 0;
            Vector3 B = new Vector3(0, 0, 0);
            for (int i = 0; i < Positions.Length; i++)
            {
                float X = point1.x * 2.5f;
                B = (1 - t) * (1 - t) * point0 + 2f * (1 - t) * t * new Vector3(X, point1.y, point1.z) + t * t * point2;
                //lineRenderer.SetPosition(i, B);
                t += (1 / (float)lineRenderer.positionCount);
                Positions[i] = new Vector3(B.x, transform.localPosition.y, B.z);
                Arrows_Parent.GetChild(i).position = Vector3.Lerp(Arrows_Parent.GetChild(i).position, B, Time.deltaTime * Rot_value);
                if (i != Positions.Length - 1)
                {
                    Arrows_Parent.GetChild(i).LookAt(Arrows_Parent.GetChild(i + 1));
                }
                else
                {
                    Arrows_Parent.GetChild(i).LookAt(-Arrows_Parent.GetChild(i - 1).position);
                }
            }
        }
    bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    public void ShootTheBall()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            //Invoke("ResetGame", 5f);
            isMove = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obs" && !Iswin)
        {
            collision.transform.parent.GetComponent<DOTweenAnimation>().DORestart();
            collision.gameObject.tag = "Untagged";
          
            Directional_force(collision);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Coin")
        //{
        //    var ui = WaleedUiManager.instance;

        //    ui.LoadPopup(Camera.main.WorldToScreenPoint(other.transform.position));
        //    AudioManager.Instance.CoinHitSound();

        //    HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.LightImpact);

        //    Debug.Log("Other Gameobject");
        //    ShopManager.instance.Cash(1);
        //    Destroy(other.gameObject);
        //}
        if (other.gameObject.tag == "SnowMan")
        {
            other.gameObject.tag = "Untagged";

            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            for (int i = 0; i < other.transform.childCount; i++)
            {
                other.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            }

        }
    }  
    void OnEnable()
    {
        EasyTouch.On_TouchDown += On_Drag;
    }
    void OnDisable()
    {
        EasyTouch.On_TouchDown -= On_Drag;
    }
    void OnDestroy()
    {
        EasyTouch.On_TouchDown -= On_Drag;
    }
    public void On_Drag(Gesture gesture)
    {
        if (isControl)
        {

            if ((gesture.deltaPosition.x > 6f || gesture.deltaPosition.x < -6f) && !Isgamestart)
            {

                GameManager.instance.TaptoPlay();
                Isgamestart = true;
                Arrows_Parent.gameObject.SetActive(true);
            }
            MiddlePos = Middle_point.localPosition.x + (gesture.deltaPosition.x * dragspeed * Time.deltaTime);
            MiddlePos = Mathf.Clamp(MiddlePos, Min, Max);
            if (Middle_point.localPosition.x == LastPoint.localPosition.x)
            {
                Middle_point.localPosition = new Vector3(MiddlePos, Middle_point.localPosition.y, Middle_point.localPosition.z);
            }
            if (Middle_point.localPosition.x < Max && Middle_point.localPosition.x > Min)
            {
                LastPoint.localPosition = new Vector3(MiddlePos, LastPoint.localPosition.y, LastPoint.localPosition.z);
            }
            else
            {
                float Lastpos = LastPoint.localPosition.x - (gesture.deltaPosition.x * dragspeed * Time.deltaTime);
                if (Middle_point.localPosition.x > 0)
                {
                    Lastpos = Mathf.Clamp(Lastpos, -9, Max);
                    LastPoint.localPosition = new Vector3(Lastpos, LastPoint.localPosition.y, LastPoint.localPosition.z);
                }
                else if (Middle_point.localPosition.x < 0)
                {
                    Lastpos = Mathf.Clamp(Lastpos, Min, 9f);
                    LastPoint.localPosition = new Vector3(Lastpos, LastPoint.localPosition.y, LastPoint.localPosition.z);
                }
            }
        }
    }
    public void Directional_force(Collision col)
    {
        isMove = false;
        var speed = 30;
        //var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, col.contacts[0].normal);
        rb.velocity = direction * Mathf.Max(speed, 0f);
    }
}
