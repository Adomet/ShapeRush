using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance = null;

    public float acc = 1f;
    public float speed = 1f;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if(!GameManager.Instance.isGameOver)
        speed += acc;

        transform.Translate(Vector3.left * speed * Time.deltaTime);
        
    }
}
