using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    public ShapeType FrameShapeType;
    public bool isFirst=false;
    public bool isPassed = false;

    // add how many 360/corner ratio this frame have
    public int cornerCount;
    public GameObject FructuredObject;

    public void Awake()
    {
        FructuredObject.transform.position = gameObject.transform.position;
        FructuredObject.transform.rotation = Quaternion.Euler(new Vector3(0, gameObject.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z));
        FructuredObject.transform.localScale = gameObject.transform.localScale;




    }


    public void PerfectAnim(Color mcolor)
    {
        isPassed = true;
        float mySpeed = GameManager.Instance.Player.GetComponent<PlayerController>().speed;

        if (FructuredObject != null)
        {
            FructuredObject.SetActive(true);
            for (int i = 0; i < FructuredObject.transform.childCount; i++)
            {
                Rigidbody rb = FructuredObject.transform.GetChild(i).GetComponent<Rigidbody>();

                rb.GetComponent<MeshRenderer>().material.color = mcolor;
                rb.AddExplosionForce(Random.Range(150f,200f), transform.position, Random.Range(7f,10f));
                rb.AddForce(Vector3.left * mySpeed*Random.Range(0.5f,1.1f),ForceMode.Impulse);



            }

        }


        gameObject.transform.parent.gameObject.SetActive(false);

    }

    public void NormalPassAnim()
    {
        isPassed = true;
        GetComponent<Animator>().SetTrigger("trigger");

    }
    public void NormalPassAnim(Color mcolor)
    {
        isPassed = true;
        gameObject.GetComponent<MeshRenderer>().material.color = mcolor;
       // GetComponent<Animator>().SetTrigger("trigger");

    }

}
