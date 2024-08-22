using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public GameObject Portal;
    Transform Puck;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puck")
        {
            Puck = other.transform;
            Puck.GetComponent<Puck>().isMove = false;
            StartCoroutine(Teleprt());
        }
    }

    IEnumerator Teleprt()
    {
        yield return new WaitForSeconds(0.05f);

        Puck.transform.rotation = Portal.transform.rotation;
        Puck.transform.position = new Vector3(Portal.transform.position.x, Puck.transform.position.y,Portal.transform.position.z);
        Puck.GetComponent<Puck>().rb.AddForce(transform.forward *2500, ForceMode.Acceleration);
    }
}
