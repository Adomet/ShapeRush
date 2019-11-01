using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//This Enum is for the comm between the frame and shape it self when interact
public enum ShapeType
{
    Cube, Triangle, Pentagon,CookieMan
}


public class ToolObjectController : MonoBehaviour
{
    //GameObject Stuff
    public Shape toolShape;
    public List<Shape> Shapes;
    public GameManager GM;

    public ParticleSystem pointParticleSystem;
    public ParticleSystem obstacleHitParticyleSystem;

    //Input
    public float touchPositionX = 0;
    public float rot = 0;
    public float buttonRotSpeed = 0;
    public float sens = 1;
    public float factor = 1f;
    public float wheelTempAngle =0;
    public float wheelAngle = 0;

    public GameObject CorsorUI;


    public bool isLeftLocked = false;
    public bool isLockEnabled = false;
    public bool isRightLocked = false;
    public bool isShrinked = false;
    private bool didvibrate = false;

    private Gyroscope Gyroscope;

    //UI
    public Toggle JoystickXYToggle;
    public Toggle InputToggle;
    public Toggle LockToggle;

    [System.Serializable]
    public struct Shape
    {
        public GameObject ShapeGameObject;
        public HelperLine Helper;
        public ShapeType ShapeType;

    }


    // Start is called before the first frame update
    void Start()
    {
        GM = GameManager.Instance;
        toolShape.ShapeGameObject.SetActive(true);

        Gyroscope = Input.gyro;
        Gyroscope.enabled = true;
    }


    public void LockSwitch()
    {
        isLockEnabled = LockToggle.isOn;
    }


    void ChangeState(ShapeType shapeType)
    {
        for (int i = 0; i < Shapes.Capacity; i++)
        {
            if (Shapes[i].ShapeType == shapeType)
            {
                toolShape.ShapeGameObject.SetActive(false);
                toolShape = Shapes[i];
                toolShape.ShapeGameObject.SetActive(true);
            }
        }


        GM.ToolParticle.GetComponent<ParticleSystem>().startColor = toolShape.ShapeGameObject.GetComponent<MeshRenderer>().material.color;

    }


    float MyEulerX(Transform myTransform)
    {
        if (myTransform != null)
        {
            float a = myTransform.localEulerAngles.x;

            if (myTransform.localEulerAngles.y == 180 && myTransform.localEulerAngles.z == 180)
            {
                a = 180 - myTransform.localEulerAngles.x;
            }

            return (360 + a) % 360;
        }
        else
            return 0;
    }



    void OnTriggerEnter(Collider other)
    {

        // Change this this is not effecent !!!
        // Change this this is not effecent !!!
        // Change this this is not effecent !!!
        ChangeToolItem cTI = other.GetComponent<ChangeToolItem>();
        if (other.GetComponent<FinishLine>() == null)
        {
            if (cTI == null)
            {
                if (other.gameObject.layer == 12)
                {
                    other.gameObject.SetActive(false);
                    GM.AddPoint(1);
                    pointParticleSystem.Play();

                }
                else
                {
                    GM.GotHitByObstacle();

                    obstacleHitParticyleSystem.startColor = toolShape.ShapeGameObject.GetComponent<MeshRenderer>().material.color;
                    obstacleHitParticyleSystem.Play();


                    Vector3 scale = transform.localScale;
                   // transform.localScale = new Vector3(scale.x, scale.y * 0.9f, scale.z * 0.9f);

                    isShrinked = true;
                    //  Debug.Log("Game Over");
                }

            }
            else
            {



                // Debug.Log(toolShape.Helper.nextFrame.gameObject.name);
                other.gameObject.SetActive(false);


                Transform nextFrameTransform = toolShape.Helper.nextFrame.transform;



                ChangeState(cTI.shapeType);



                transform.rotation = Quaternion.Euler(new Vector3(MyEulerX(nextFrameTransform), 0, 0));


                //transform.rotation = Quaternion.Euler(new Vector3(rotOfFrameAfterChangeItem, 0f, 0f));

                // toolShape.Helper.nextFrame= Track.Instance.GetNextFrame();
                Destroy(other.gameObject);
            }
        }
    }


    void AngleAtPerfect()
    {

       // float anglex = (360 / other.GetComponent<Frame>().cornerCount) - MyEulerX(nextFrameTransform);

        //transform.rotation = Quaternion.Euler(new Vector3(anglex, 0, 0));





        if (!didvibrate)
        {
            Vibration.Vibrate(30);
            didvibrate = true;
        }

       Color color =  toolShape.Helper.GetComponent<HelperLine>().helperEndTransform.GetComponent<MeshRenderer>().material.color;
        color.a = 0.85f;
        toolShape.Helper.GetComponent<HelperLine>().helperEndTransform.GetComponent<MeshRenderer>().material.color = color;
    }


    // Update is called once per frame
    // ReThink this code after did not optimized
    void Update()
    {


        float angleofSimmetry = 1;
        Transform nextFrameTransform = null;
        if (toolShape.Helper.nextFrame != null)
        {
            angleofSimmetry = 360 / toolShape.Helper.nextFrame.cornerCount;
            nextFrameTransform = toolShape.Helper.nextFrame.transform;
        }


        


        float difff = Mathf.Abs((MyEulerX(transform) - (MyEulerX(nextFrameTransform)))) % angleofSimmetry;
    


    
        if (Input.GetKey(KeyCode.Space))
            GameManager.Instance.RestartGame();


        if (!InputToggle.isOn)
        {
            
                if (Input.GetMouseButtonDown(0))
                {
                    touchPositionX = Input.mousePosition.x;
                }
            if (Input.GetMouseButton(0))
            {

                float Inputdiff = Input.mousePosition.x - touchPositionX;



                // rot = Mathf.Clamp(diff * diff,-700,700) * Mathf.Sign(diff);







                if (isLockEnabled)
                {
                    if ((angleofSimmetry - difff < GM.perfectAchiveAngle || difff < GM.perfectAchiveAngle))
                    {


                        if (Inputdiff > 0)
                        {
                            if (!isLeftLocked)
                            {
                                isRightLocked = true;
                                AngleAtPerfect();
                            }
                        }
                        else
                        {
                            if (!isRightLocked)
                            {
                                isLeftLocked = true;
                                AngleAtPerfect();

                            }
                        }




                        if (isRightLocked)
                            isLeftLocked = false;


                        if (isLeftLocked)
                            isRightLocked = false;






                        if (isRightLocked && !isLeftLocked)
                        {
                            if (Inputdiff > 0)
                                Inputdiff = 0;

                        }

                        if (isLeftLocked && !isRightLocked)
                        {
                            if (Inputdiff < 0)
                                Inputdiff = 0;

                        }


                        

                    

                    }
                    else
                    {
                        isRightLocked = false;
                        isLeftLocked = false;

                        didvibrate = false;


                        Color color = toolShape.Helper.GetComponent<HelperLine>().helperEndTransform.GetComponent<MeshRenderer>().material.color;
                        color.a = 0.25f;
                        toolShape.Helper.GetComponent<HelperLine>().helperEndTransform.GetComponent<MeshRenderer>().material.color = color;

                    }

                    factor = 0.5f;
                }
                else
                {
                    //soft lock algorithm


                    sens = difff/angleofSimmetry;
                   


                    factor = 0.5f;
                }


                rot = Inputdiff;
                sens = Mathf.Clamp(sens, 0.05f,1f);
                transform.Rotate(new Vector3(rot *sens*factor, 0, 0));
                touchPositionX = Input.mousePosition.x;

            }
          

        }
        else
        {

             if (Input.GetMouseButton(0))
             {
                
                     if (Screen.width / 2 < Input.mousePosition.x)
                     {
                         transform.Rotate(new Vector3(buttonRotSpeed *2f* Time.deltaTime, 0, 0));
                     }
                     else
                         transform.Rotate(new Vector3(-buttonRotSpeed *2f* Time.deltaTime, 0, 0));
                 
            
             }

            //if (Input.GetMouseButton(0))
            //{
            //
            //    Vector2 wheelCenter = CorsorUI.transform.position;
            //
            //    float wheelAngle = Vector2.Angle(Input.mousePosition, wheelCenter);
            //    transform.Rotate(new Vector3(wheelAngle,0,0));
            //
            //
            //}
        }
    }





}

