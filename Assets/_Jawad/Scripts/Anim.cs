using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    
    private void Start()
    {
       int RandomNo= Random.Range(1, 4);
        if (RandomNo == 1)
        {
            GetComponent<Animator>().SetTrigger("D1");
        }
        else if (RandomNo == 2)
        {
            GetComponent<Animator>().SetTrigger("D2");

        }
        else
        {
            GetComponent<Animator>().SetTrigger("D3");

        }
        
    }

}
