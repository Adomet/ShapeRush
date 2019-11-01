using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColTest : MonoBehaviour
{
 
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Adomet");
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Adomet");
        Destroy(this.gameObject);
    }
}
