using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe1 : MonoBehaviour
{
   
   
    private Vector3 startPos; //mouse slide movement start pos
    private Vector3 endPos; //mouse slide movement end pos
    public float zDistance = 30.0f;//z distance
    private bool isThrown;
    public bool done;

    

    void Start()
    {
       
           
        
        //InstantiateGameManager.instance.block();
        isThrown = false;
       
    }
   

    void Update()
    {

        
        if (isThrown)
        {
            return; 
        }


         if (Input.GetMouseButtonDown(0))
         {


         Vector3 mousePos = Input.mousePosition * -1.0f;
         mousePos.z = zDistance;

         startPos = Camera.main.ScreenToWorldPoint(mousePos);

             
         }

        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine("throwBall");
        }
    }

   
    void call()
    {
        done = true;
      
   
    }

    IEnumerator throwBall()
    {
        Vector3 mousePos = Input.mousePosition * -1.0f;
        mousePos.z = zDistance;

        endPos = Camera.main.ScreenToWorldPoint(mousePos);
        endPos.z = Camera.main.nearClipPlane;

        Vector3 throwDir = (startPos - endPos).normalized;
        
        if (throwDir.y > 0.1f)
        {
            
            StartCoroutine("stopAnim");
            yield return new WaitForSeconds(0.4f);
           
            this.gameObject.GetComponent<Rigidbody>().AddForce(throwDir * (startPos - endPos).sqrMagnitude);
            isThrown = true;
            //once = 0;
           
            Destroy(gameObject, 2.5f);
            Invoke("call", 2.4f);
        }
    }

    IEnumerator stopAnim()
    {
        yield return new WaitForSeconds(0.6f);
       

    }
}


//Vector3 mousePos = Input.mousePosition * -1.0f;
//mousePos.z = zDistance; 
            
//            endPos = Camera.main.ScreenToWorldPoint(mousePos);
//            endPos.z = Camera.main.nearClipPlane; 

//            Vector3 throwDir = (startPos - endPos).normalized;

//            this.gameObject.GetComponent<Rigidbody>().AddForce(throwDir* (startPos - endPos).sqrMagnitude);

//isThrown = true;
////once = 0;
//Destroy(gameObject, 2.5f);
//Invoke("call", 2.4f);