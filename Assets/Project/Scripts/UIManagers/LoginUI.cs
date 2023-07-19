using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginUI : MonoBehaviour
{
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public static LoginUI loginUI; 
    // Start is called before the first frame update
    void Start()
    {
        loginUI = this;
    }

    public void DestroyObj()
    {
        Debug.Log("Here");
        DestroyImmediate(this);
    }
}
