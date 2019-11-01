using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{

   public GameObject TestObject;

    public void SwitchMenuState()
    {
        TestObject.SetActive(!TestObject.activeSelf);
    }
}
