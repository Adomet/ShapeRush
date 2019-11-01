using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperLine : MonoBehaviour
{

    public Transform helperEndTransform;

    public Frame nextFrame;
    public Transform helperTransform;
    public GameManager gm;
    private float defaultacc = 0.02f;

    private void Start()
    {
        gm = GameManager.Instance;
        //Pop first of list
        nextFrame = Track.Instance.GetNextFrame(true);

        defaultacc = PlayerController.Instance.acc;
    }

    public void FindNextFrame(bool pass)
    {
        nextFrame = Track.Instance.GetNextFrame(pass);
    }

    private void Update()
    {
        if (nextFrame != null)
            SetHelperLineEndPoint(nextFrame.transform.position);
        else
            SetHelperLineEndPoint(transform.position);
    }
    // Get the closest obstacle and extend the helper line object to its position
    void SetHelperLineEndPoint(Vector3 targetPosition)
    {
        float diff = Mathf.Abs(transform.position.x - targetPosition.x);
        Vector3 scale = helperTransform.localScale;
        scale.x = diff;
        helperTransform.localScale =scale ;

    }

    void PerfectTrigger(Collider other)
    {
        ToolObjectController toolController = gm.Tool.GetComponent<ToolObjectController>();
        if (toolController.isShrinked)
        {
            NormalPassTrigger(other);

        }
        else
        {
            Color MyColor = GetComponentInParent<MeshRenderer>().material.color;
            MyColor = new Color(MyColor.r, MyColor.g, MyColor.b, MyColor.a / 2f);
            other.GetComponent<Frame>().PerfectAnim(MyColor);
            gm.PerfectAnimTrigger();
            PlayerController.Instance.speed *= 1.075f;

        }
       
    }

    void NormalPassTrigger(Collider other)
    {
        Color MyColor = GetComponentInParent<MeshRenderer>().material.color;
        MyColor = new Color(MyColor.r, MyColor.g, MyColor.b, MyColor.a / 2f);
        other.GetComponent<Frame>().NormalPassAnim(MyColor);
        gm.NormalPassAnimTrigger();


        PlayerController.Instance.speed *= 1.05f;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Frame>() != null)
        {
            Frame frame = other.GetComponent<Frame>();
            if (!frame.isPassed)
            {
                //Gets tool object by parenting and check perfect the rot by angle of simmetyr
                Transform transformOfTool = transform.parent.parent.transform;
                float angleofSimmetry = 180 / other.GetComponent<Frame>().cornerCount;


                float diff = Mathf.Abs((MyEulerX(transformOfTool) - (MyEulerX(other.transform)))) % angleofSimmetry;
                float diff2 = Mathf.Abs((MyEulerX(transformOfTool) - (angleofSimmetry - MyEulerX(other.transform)))) % angleofSimmetry;

               
               

                if (angleofSimmetry - diff < gm.perfectAchiveAngle || diff < gm.perfectAchiveAngle)
                {
                    // Debug.Log("Perfectoo");
                    PerfectTrigger(other);

                    // Debug.Log((transformOfTool.rotation.eulerAngles.x % angleofSimmetry));
                    // Debug.Log((other.transform.rotation.eulerAngles.x % angleofSimmetry));
                }

                else if (angleofSimmetry - diff2 < gm.perfectAchiveAngle || diff2 < gm.perfectAchiveAngle)
                {
                    PerfectTrigger(other);

                }


                else
                {
                    NormalPassTrigger(other);
                }


                //New Perfect Trigger for New Frame Rotattion Types

               
                


                
            }

            //Debug.Log((other.transform.eulerAngles.x));
            //Debug.Log((transformOfTool.transform.rotation.eulerAngles.x));
            //Debug.Log((angleofSimmetry));
            //Debug.Log((transformOfTool.rotation.eulerAngles.x % angleofSimmetry));
            //Debug.Log((other.transform.rotation.eulerAngles.x % angleofSimmetry));
            //Debug.Log("diff"+diff);

            if (frame.isPassed)
                FindNextFrame(false);
            else
                FindNextFrame(true);


        }

        PlayerController.Instance.acc = defaultacc;

        Vector3 scale = gm.Tool.transform.localScale;
        gm.Tool.transform.localScale = new Vector3(0.3f, 1f, 1f);

        gm.Tool.GetComponent<ToolObjectController>().isShrinked = false;




        if (gm.AutoPlay && nextFrame!=null)
        {
            Transform nextFrameTransform = nextFrame.transform;

           // Debug.Log("y,z:" + nextFrameTransform.localEulerAngles.y + "," + nextFrameTransform.localEulerAngles.z);
            if (nextFrameTransform.localEulerAngles.y == 180 || nextFrameTransform.localEulerAngles.z == 180)
            {
                float anglex = (360 / other.GetComponent<Frame>().cornerCount) - MyEulerX(nextFrameTransform);
               
                Debug.Log(anglex);
             
                gm.Tool.transform.rotation = Quaternion.Euler(new Vector3(anglex, 0, 0));
            }
            else
            {
                gm.Tool.transform.rotation = Quaternion.Euler(new Vector3(MyEulerX(nextFrameTransform), 0, 0));
            }
            
        }

    }


    float MyEulerX(Transform myTransform)
    {
        float a = myTransform.localEulerAngles.x;

        if (myTransform.localEulerAngles.y == 180 && myTransform.localEulerAngles.z == 180)
        {
            a = 180 - myTransform.localEulerAngles.x;
        }

        return (360 + a) % 360;
    }

}
