using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController: MonoBehaviour
{
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public static GameController GM;

    private AuthController _authController;
    // Start is called before the first frame update
    void Start()
    {
        _authController = new AuthController();
        GM = this;
        DontDestroyOnLoad(this);
    }


    public void DebugInput()
    {
        string email = EmailInput.text;
        string password = PasswordInput.text;
        Debug.Log("Username: " + email + "\n" + "Password: " + password);
    }

    public void CreateAccount()
    {
        string email = EmailInput.text;
        string password = PasswordInput.text;
        _authController.CreateAccount(email, password, success =>
        {
            if (success)
            {
                Debug.Log("Created");
            }
            else
            {
                Debug.Log("Error");
            }
        });
    }

    public void LoginWithEmail()
    {
        string email = EmailInput.text;
        string password = PasswordInput.text;
        _authController.LoginWithEmail(email, password, success =>
        {
            if (success)
            {
                Debug.Log("Login success");
            }
            else
            {
                Debug.Log("False to login!");
            }
        });
    }
    public void LoginWithFacebook()
    {
        string email = EmailInput.text;
        string password = PasswordInput.text;
        _authController.LoginWithFacebook(success =>
        {
            if (success)
            {
                Debug.Log("Login success");
            }
            else
            {
                Debug.Log("False to login!");
            }
        });
    }
}
