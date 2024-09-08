using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using HedgehogTeam.EasyTouch;
public class Puck : MonoBehaviour
{
    public bool IsBonous = false; bool isHit = false,Isgamestart=false;
    public static Puck instance;
    public Animator GoalKeeperAnim;
    public string KeeperAnim_positionName;
    Vector3 lastVelocity;
    [SerializeField]
    Transform Trail_Parent;
    
    // Controller++++++++++++++++++++++++++
    public LineRenderer lineRenderer;
    public Transform Arrows_Parent;
    public Transform Start_point, Middle_point, LastPoint;
    public bool isMove = false, puck = true,Iswin=false,isControl=true;
    bool ShowParent = true;
    Vector3[] Positions=new Vector3[18];
    public float Min, Max;
    float speed = 5.7f, dragspeed =4f, MiddlePos, Rot_value = 13f;
    int index = 0;
   
    public Rigidbody rb;
    GameManager gameManager;
    int Goal_indx;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        Goal_indx = UnityEngine.Random.Range(LevelManager.instance.AImin,LevelManager.instance.AImax);
        if (LevelManager.instance.IsGoal)
        {
            Trail_Parent.GetChild(1).gameObject.SetActive(true);
            Trail_Parent.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Trail_Parent.GetChild(0).gameObject.SetActive(true);
            Trail_Parent.GetChild(1).gameObject.SetActive(false);
        }
        gameManager = GameManager.instance;
        rb = GetComponent<Rigidbody>();
        lineRenderer.positionCount = Positions.Length;
        //if (PlayerPrefs.GetInt("Level") < 15)
        //{
        // Goal_indx = UnityEngine.Random.Range(7, 10);
        //}
        //else
        //{
        //Goal_indx = UnityEngine.Random.Range(1 , 4);
        //}


    }

    void Update()
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
                    lastVelocity = transform.localPosition;
                    float Dis = Vector3.Distance(transform.localPosition, destination);
                    if (Dis <= 0.05f)
                    {
                        if (index < Positions.Length - 1)
                        {
                            index++;
                            if (index == Goal_indx && GoalKeeperAnim!=null)
                            {
                                KeeperAnim();
                            }
                        }
                        else
                        {
                            if (!LevelManager.instance.isScndplayer)
                            {
                                puck = false;
                                transform.rotation = Arrows_Parent.GetChild(Arrows_Parent.childCount - 3).transform.rotation;
                                rb.AddForce(transform.forward */*lastVelocity.magnitude*/2500, ForceMode.Acceleration);
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
         
        
            if (Input.GetMouseButtonUp(0)&& Isgamestart)
            {
            isControl = false;
            Arrows_Parent.gameObject.SetActive(false);
            PlayerController.instance.PlayShootAnimation();

            }
        }
      
    }
    
    public void ShootTheBall()
    {
        LevelManager.instance.CamerShake();
        isMove = true;
        CheckIfMissed();
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "new_ice_end_vibrant")
            AudioManager.Instance.PlayPuckHitSoundSound();
        if (collision.gameObject.CompareTag("GoalKeeper") &&!Iswin)
        {
            
            isMove = false;
            collision.gameObject.tag = "Untagged";
            StopAllCoroutines();
            Directional_force(collision);
            StartCoroutine(waitBeforeLevelFail(3));
            
        } if (collision.gameObject.CompareTag("StaticKeeper") &&!Iswin)
        {
            collision.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            isMove = false;
            collision.gameObject.tag = "Untagged";
            StopAllCoroutines();
            Directional_force(collision);
            StartCoroutine(waitBeforeLevelFail(3));
            
        }

        if (collision.collider.tag == "Objects" && !Iswin)
        {
            collision.gameObject.tag = "Untagged";
            isHit = true;
            Directional_force(collision);
        }
        if (collision.collider.tag == "MoveKeeper" && !Iswin)
        {
            
            Directional_force(collision);
            isMove = false;
            if (collision.transform.GetComponent<DOTweenAnimation>())
            {
                collision.transform.GetComponent<DOTweenAnimation>().DOPause();
              //  collision.transform.GetChild(1).GetComponent<Animator>().enabled = true;
                collision.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().enabled = true;
                collision.transform.GetChild(0).GetComponent<Animator>().enabled = false;

                //collision.transform.GetComponent<DOTweenAnimation>().GetComponent<Animator>().enabled = false;
                Debug.LogError("collide");
            }

            if (collision.transform.GetComponent<Moving>()) 
            { 
            collision.transform.GetComponent<Moving>().isMove = false;
                collision.transform.GetComponent<Moving>().Animdye.enabled = true;
                 collision.transform.GetComponent<Moving>().Anim.enabled=false;
            }
            collision.gameObject.tag = "Untagged";
            isHit = true;
            Directional_force(collision);
        }

        if (collision.gameObject.tag == "SnowMan" && !Iswin)
        {
            collision.gameObject.tag = "Untagged";
            Directional_force(collision);
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            for(int i = 0; i < collision.transform.childCount; i++)
            {
                collision.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            }

        }

        if(collision.gameObject.tag=="Obs" && !Iswin)
        {
            collision.transform.parent.GetComponent<DOTweenAnimation>().DORestart();
            collision.gameObject.tag = "Untagged";
            isHit = true;
            Directional_force(collision);
        }

        if (collision.gameObject.tag == "Goalwall" && !Iswin)
        {
            Debug.Log("Yes wall");  
            isMove = false;
            var speed = 5;
            //var speed = lastVelocity.magnitude;

            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(speed, 0f);

        }

        if (collision.gameObject.tag == "Enemy" && !Iswin)
        {
            if (collision.transform.GetComponent<DOTweenAnimation>())
            {
                collision.transform.GetComponent<DOTweenAnimation>().DOPause();
            }
            collision.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().enabled = true;

            collision.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            collision.gameObject.tag = "Untagged";
            isHit = true;
            Directional_force(collision);
        }
        if (collision.gameObject.tag == "Enemy1" && !Iswin)
        {
            if (collision.transform.GetComponent<OrbitAroundTree>())
            {
                collision.transform.GetComponent<OrbitAroundTree>().isRotate=false;
            }
            collision.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().enabled = true;

            collision.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            collision.gameObject.tag = "Untagged";
            isHit = true;
            Directional_force(collision);
        }


        if (collision.gameObject.tag == "RotatorEnemy" && !Iswin)
        {
            collision.gameObject.tag = "Untagged";
            collision.transform.parent.GetComponent<RotatorObj>().isRotate = false;
            collision.transform.GetChild(0).GetComponent<Animator>().enabled=false;
            collision.gameObject.tag = "Untagged";
            isHit = true;
            Directional_force(collision);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall" && !Iswin)
        {
            if (GameManager.instance.gameState == GameManager.GameState.play && !Iswin)
            {
                Iswin = true;
                GameManager.instance.gameState = GameManager.GameState.failed;
                PlayerController.instance.OnGoalStopped();
                StartCoroutine(UIManager.Instance.ShowLoseScreen());
            }
        }

        if (other.gameObject.CompareTag("Goal"))
        {
            Iswin = true;

            if (GameManager.instance.gameState == GameManager.GameState.play)
            {
                LevelManager.instance.IsGoal = true;
                UIManager.Instance.Gate_Imges(true);


                other.tag = "Untagged";

                StopAllCoroutines();
                other.transform.GetChild(0).gameObject.SetActive(true);
                GameManager.instance.gameState = GameManager.GameState.completed;
                StartCoroutine(LevelManager.instance.Move_Next());
                AudioManager.Instance.BooSound();
                if (GoalKeeperAnim != null)
                {
                GoalKeeperAnim.ResetTrigger("Lose");
                }
                HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.Success);
                AudioManager.Instance.PlayGoalSound();
                AudioManager.Instance.PlayGoalKeeperSound(1f);
                PlayerController.instance.OnGoaled();
            }
        }
        if (other.tag == "Tree")
        {
         other.transform.GetChild(0).GetChild(0).DOMove(new Vector3(0.3f, 0f,0.2f), 0.2f).SetRelative(true).SetLoops(2, LoopType.Yoyo);
        }

        if (other.tag == "Portal" && !Iswin)
        {
            other.tag = "Untagged";
            
            StartCoroutine(Teleprt(other.transform.GetComponent<Teleport>().Portal));
        }

        if (other.tag == "Scndplayer" && !Iswin)
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(3).gameObject.SetActive(false);
            HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.Success);
            UIManager.Instance.Gate_Imges(true);
            other.tag = "Untagged";
            LevelManager.instance.isScndplayer = true;
            StopAllCoroutines();
            StartCoroutine(LevelManager.instance.Move_Next());
        }

        if(other.tag== "GoalHitTargets")
        {
            KeeperAnim_positionName = other.name;
        }

        //if (other.tag == "Coin")
        //{
        //    //var ui = WaleedUiManager.instance;
        //    //ui.LoadPopup(Camera.main.WorldToScreenPoint(other.transform.position));
        //    //AudioManager.Instance.CoinHitSound();
        //    //HapticControllerNew.instance.PlayHaptic(MoreMountains.NiceVibrations.HapticTypes.LightImpact);
        //    //Debug.Log("Coin");
        //    //ShopManager.instance.Cash(20);
        //    //Destroy(other.gameObject);
        //}

        //if (other.gameObject.tag == "key")
        //{
        //    //Debug.Log("Keys");
        //    //Destroy(other.gameObject);
        //    //if (PlayerPrefs.GetInt("GKeys") < 2)
        //    //{
        //    //    PlayerPrefs.SetInt("GKeys", PlayerPrefs.GetInt("GKeys") + 1);
        //    //    UIManager.Instance.Keys_Parent.transform.GetChild(PlayerPrefs.GetInt("GKeys")).GetChild(0).gameObject.SetActive(true);
        //    //}
        //}
        if (other.gameObject.tag == "SnowMan" && !Iswin)
        {
            other.gameObject.tag = "Untagged";
           
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            for (int i = 0; i < other.transform.childCount; i++)
            {
                other.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            }

        }


    }
  
    public void CheckIfMissed()
    {
        StartCoroutine(waitBeforeLevelFail(3));
    }
    IEnumerator waitBeforeLevelFail(float waitTime)
    {


        yield return new WaitForSeconds(3f);
        if (GameManager.instance.gameState == GameManager.GameState.play && !Iswin)
        {
            UIManager.Instance.Gate_Imges(false);
            Iswin = true;
            GameManager.instance.gameState = GameManager.GameState.failed;
            AudioManager.Instance.BooSound();
         
            if (GoalKeeperAnim != null)
            {
                GoalKeeperAnim.SetTrigger("Win");
            }

            PlayerController.instance.OnGoalStopped();
      
            AudioManager.Instance.PlayGoalSavedPlayerSound(1);
            StartCoroutine(UIManager.Instance.ShowLoseScreen());

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
    IEnumerator Teleprt(GameObject Portal)
    {
        isMove = false;
        transform.localEulerAngles = Portal.transform.eulerAngles;
        yield return new WaitForSeconds(0.05f);
        transform.position = new Vector3(Portal.transform.position.x, transform.position.y, Portal.transform.position.z);
        rb.AddForce(transform.forward * 50f);
    }
    bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
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
        if (isControl) { 
            if (ShowParent)
            {
            
            ShowParent = false;
            
            }
         
            if ((gesture.deltaPosition.x > 6f || gesture.deltaPosition.x< -6f) &&!Isgamestart )
            {
           
                GameManager.instance.TaptoPlay();
                Isgamestart = true;
                Arrows_Parent.gameObject.SetActive(true);
            }
            MiddlePos = Middle_point.localPosition.x+(gesture.deltaPosition.x * dragspeed*Time.deltaTime);
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
    void KeeperAnim()
    {
        if (GoalKeeperAnim.transform.GetComponent<GoalKeeperAudioController>().Tween != null)
        {
            Destroy(GoalKeeperAnim.transform.GetComponent<GoalKeeperAudioController>().Tween);
        }
        if (GoalKeeperAnim != null)
        {
            switch (KeeperAnim_positionName)
            {
                case "TopRight":
                    GoalKeeperAnim.SetInteger("MovementHorizontal", 1);
                    Invoke("DefaultAnim", 0.5f);

                    break;
                case "BottomRight":
                    GoalKeeperAnim.SetInteger("MovementHorizontal", 2);
                    Invoke("DefaultAnim", 0.5f);

                    break;
                case "TopCenter":
                    GoalKeeperAnim.SetInteger("MovementVertical", 1);
                    Invoke("DefaultAnim", 0.5f);

                    break;
                case "BottomCenter":
                    GoalKeeperAnim.SetInteger("MovementVertical", -1);
                    Invoke("DefaultAnim", 0.5f);

                    break;
                case "TopLeft":
                    GoalKeeperAnim.SetInteger("MovementHorizontal", -1);
                    Invoke("DefaultAnim", 0.5f);

                    break;
                case "BottomLeft":
                    GoalKeeperAnim.SetInteger("MovementHorizontal", -2);
                    Invoke("DefaultAnim", 0.5f);
                    
                    break;
                default:
                    GoalKeeperAnim.SetInteger("MovementVertical", 0);
                    GoalKeeperAnim.SetInteger("MovementHorizontal", 0);
                    
                    break;
            }
            //GoalKeeperAnim.SetInteger("MovementVertical", 0);
            //GoalKeeperAnim.SetInteger("MovementHorizontal", 0);
            KeeperAnim_positionName = "";
        }
    }
    void DefaultAnim()
    {
        GoalKeeperAnim.SetInteger("MovementVertical", 0);
        GoalKeeperAnim.SetInteger("MovementHorizontal", 0);
    }
}
