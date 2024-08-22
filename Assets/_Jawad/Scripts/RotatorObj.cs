using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorObj : MonoBehaviour
{
   public bool isRotate = true;
    float Speed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
        {
            transform.Rotate(new Vector3(0f, Speed, 0f));
        }
    }
}
