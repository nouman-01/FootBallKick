using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Moving : MonoBehaviour
{
    
    public float[] positions;
    public int Moveposindx;
    Vector3 Nextpos;
    public float objectspeed;
    public bool isMove = false/*,IsX_axis=true*/;

    public Animator Anim;
    public Animator Animdye;

    private void Start()
    {
       // Nextpos =new Vector3( positions[0],transform.position.y,transform.position.z);
    }
    private void Update()
    {
        if (isMove)
        { 
        MoveObject();
        }
    }
    void MoveObject()
    {
        if (transform.position == Nextpos)
        {
            //Anim.SetBool("Yes", true);
            Moveposindx++;
            if (Moveposindx >= positions.Length)
            {
                Anim.SetBool("Yes", false);

                Moveposindx =0;
            }
            else
            {
                Anim.SetBool("Yes", true);
            }
          
            Nextpos = new Vector3(positions[Moveposindx], transform.position.y, transform.position.z);
            
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Nextpos, objectspeed * Time.deltaTime);
        }
    }

    //[SerializeField] float Speed;
    //public float Area=5;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (transform.position.x <= -Area)
    //    {
    //        //transform.DOScale(new Vector3(-1, 1f, 1f), 0.3f);
    //        //transform.localScale=new Vector3(-1,1,1);
    //        Debug.Log("Yes");
    //        Speed = 5;
    //    }
    //    if (transform.position.x >= Area)
    //    {
    //        //transform.DOScale(new Vector3(1, 1f, 1f), 0.3f);
    //        //transform.localScale=new Vector3(1,1,1);
    //        Speed = -Speed;
    //    }
    //    transform.Translate(Speed * Time.deltaTime,0f,0f);
    //}
}
