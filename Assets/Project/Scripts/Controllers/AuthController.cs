using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using System;
using Facebook.Unity;
using Firebase.Auth;
using System.Threading.Tasks;

public class AuthController
{
    private Firebase.Auth.FirebaseAuth auth;

    public AuthController()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        if (!FB.IsInitialized)
        {
            FB.Init(OnInitComplete);
        }
        else
        {
            OnInitComplete();
        }
    }

    private void OnInitComplete()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.LogError("Failed to initialize the Facebook SDK");
        }
    }

    public void CreateAccount(string email, string password, Action<bool> callback)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                callback(false);
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                callback(false);
            }
            else
            {
                // Firebase user has been created successfully.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

                callback(true);
            }
        });
    }

    public void LoginWithEmailAsync(string email, string password, Action<bool> callback)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                callback(false);
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                callback(false);
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            callback(true);
        });
    }

    public async Task<bool> LoginWithEmailAsync(string email, string password)
    {
        try
        {
            Firebase.Auth.AuthResult result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + ex);
            return false;
        }
    }


    public void LoginWithFacebook(Action<bool> callback)
    {
        FacebookSDKController fbController = new FacebookSDKController();
        fbController.LoginWithFacebook();
    }

    public void SignOut()
    {
        auth.SignOut();
    }
}
