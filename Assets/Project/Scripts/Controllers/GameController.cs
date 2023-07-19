using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController: MonoBehaviour
{
    
    public static GameController GM;
    private int _state;
    
    
    private AuthController _authController;

    void Start()
    {
        _authController = new AuthController();
        _state = 0;
        GM = this;
        DontDestroyOnLoad(this);
    }


    //public void DebugInput()
    //{
    //    string email = EmailInput.text;
    //    string password = PasswordInput.text;
    //    Debug.Log("Username: " + email + "\n" + "Password: " + password);
    //}

    //state 0
    public void CreateAccount()
    {
        if (_state != 0) return;
        string email = LoginUI.loginUI.EmailInput.text;
        string password = LoginUI.loginUI.PasswordInput.text;
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

    public async void LoginWithEmail()
    {
        if (_state != 0) return;

        string email = LoginUI.loginUI.EmailInput.text;
        string password = LoginUI.loginUI.PasswordInput.text;

        bool success = await _authController.LoginWithEmailAsync(email, password);

        if (success)
        {
            Debug.Log("Login success");

            UIController.UIManager.DeactiveUI(0);
            
            UIController.UIManager.ActiveUI(1);
            UIController.UIManager.FetchUser();

        }
        else
        {
            Debug.Log("Failed to login!");
        }
    }


    public void LoginWithFacebook()
    {
        if (_state != 0) return;
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

    public void SignOut()
    {
        _authController.SignOut();
        UIController.UIManager.DeactiveUI(1);
        UIController.UIManager.ActiveUI(0);
    }
}
