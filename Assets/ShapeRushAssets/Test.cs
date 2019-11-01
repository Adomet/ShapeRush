using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
 

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.rotation);
        Debug.Log(transform.eulerAngles);
        Debug.Log(transform.rotation.eulerAngles);
        Debug.Log(transform.localEulerAngles);
        Debug.Log(transform.localRotation.eulerAngles.x);
        Debug.Log("MyEulerX:" + MyEulerX(transform));
    }

    float MyEulerX(Transform myTransform)
    {
        float a = myTransform.localEulerAngles.x;

        if (myTransform.localEulerAngles.y == 180 && myTransform.localEulerAngles.z == 180)
        {
            a= 180 - myTransform.localEulerAngles.x;
        }

        return (360+a)%360;
    }
}
