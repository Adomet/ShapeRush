using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;
    public GameObject goSceneObjects,windGameSceneObjects,Player,Tool,FinishLine,VirtualCamera,ToolParticle;
    public bool isGameOver=false;
    public float perfectAchiveAngle = 3f;
    public int myPoints = 0;


    public int levelNumber = 0;



    public ToolObjectController toolController;

    public TextMeshProUGUI GamePointText;
    public TextMeshProUGUI GameLevelText;
    public Toggle InputToggle;
    public Toggle AutoPlayToggle;
    public Toggle LockToggle;




    PlayerController playerController;

    public Animator Perfectanimator;
    public Animator FinishCamAnim;
    public Animator FinishToolAnim;


    public Slider GameLevelSlider;
    private float Leveldistance;

    public bool AutoPlay= false;



    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        if (PlayerPrefs.HasKey("InputToggle"))
        {
            InputToggle.isOn = bool.Parse(PlayerPrefs.GetString("InputToggle"));
            AutoPlayToggle.isOn = bool.Parse(PlayerPrefs.GetString("AutoPlayToggle"));
            LockToggle.isOn = bool.Parse(PlayerPrefs.GetString("LockToggle"));
            levelNumber = PlayerPrefs.GetInt("LevelNumber");
        }

        Leveldistance = Mathf.Abs(FinishLine.transform.position.x - Tool.transform.position.x);


        Application.targetFrameRate = 61;

        toolController = Tool.GetComponent<ToolObjectController>();

        GameLevelText.text = ("Level " +(levelNumber+1));
    }


    public void AutoSwitch()
    {
        AutoPlay = AutoPlayToggle.isOn;

    }

    public void RestartGame()
    {

        levelNumber = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("LevelNumber", levelNumber+1);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex+1));
    }

    public void StartScene(int sceneint)
    {
        if (sceneint != SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneint);
        }
        else
        {
            levelNumber = 0;
            PlayerPrefs.SetInt("LevelNumber", levelNumber);
            SceneManager.LoadScene(levelNumber);

        }

    }


    // Start is called before the first frame update
    void Start()
    {
        playerController= PlayerController.Instance;
    }

    public void GotHitByObstacle()
    {
        playerController.speed = -7f;
        playerController.acc   =  0.2f;




       // Debug.Log("Got stuck");
    }

  
    public void PerfectAnimTrigger()
    {
        
      
            Perfectanimator.SetTrigger("trigger");
            toolController.isLeftLocked = false;
            toolController.isRightLocked = false;
            Vibration.Vibrate(40);
        

    }

    

    public void NormalPassAnimTrigger()
    {
        toolController.isLeftLocked = false;
        toolController.isRightLocked = false;

    }

    public void LateUpdate()
    {
        GameLevelSlider.value = 1+(FinishLine.transform.position.x - Tool.transform.position.x) / Leveldistance;
        //Debug.Log(1+(FinishLine.transform.position.x - Tool.transform.position.x) / Leveldistance);
    }


    //Make game Over UI objects active
    public void GameOver()
    {

        goSceneObjects.SetActive(true);
        Player.GetComponent<PlayerController>().speed = 0;
        isGameOver = true;
    }

    //Make game Over UI objects active
    public void WinGame()
    {
        PlayerPrefs.SetString("InputToggle",(InputToggle.isOn).ToString());
        PlayerPrefs.SetString("AutoPlayToggle", (AutoPlayToggle.isOn).ToString());
        PlayerPrefs.SetString("LockToggle", (LockToggle.isOn).ToString());
        PlayerPrefs.SetInt("LevelNumber", levelNumber+1);



        if(windGameSceneObjects!=null)
        windGameSceneObjects.SetActive(true);
        Player.GetComponent<PlayerController>().speed = 0;
        isGameOver = true;
        ToolParticle.GetComponent<ParticleSystem>().Stop();

        //play End animation

        Tool.GetComponent<Animator>().enabled = true;
        VirtualCamera.GetComponent<CinemachineVirtualCamera>().enabled = false;
        FinishCamAnim.SetTrigger("Finish");
        FinishToolAnim.SetTrigger("Finish");


        Vibration.Vibrate(500);


    }

    public void AddPoint(int points)
    {
        myPoints += points;
        GamePointText.text = myPoints.ToString();
    }
}
