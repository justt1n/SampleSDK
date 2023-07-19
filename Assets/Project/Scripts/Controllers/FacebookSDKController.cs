using UnityEngine;
using Facebook.Unity;
using Facebook.MiniJSON;
using Firebase;
using Firebase.Auth;
using System.Collections.Generic;
using System;
using System.Linq;

public class FacebookSDKController
{
    private FirebaseAuth auth;
    private string facebookAccessToken;

    public FacebookSDKController()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                }
                else
                {
                    Debug.LogError("Failed to initialize the Facebook SDK");
                }
            });
        }
    }

    public void LoginWithFacebook()
    {
        FB.LogInWithReadPermissions(new List<string> { "public_profile", "email" }, HandleResult);
    }

    private void HandleResult(ILoginResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
        }
        else if (result.Cancelled)
        {
            Debug.Log("Login cancelled");
        }
        else
        {
            facebookAccessToken = AccessToken.CurrentAccessToken.TokenString;
            FB.API("/me?fields=email", HttpMethod.GET, FetchUserData);
        }
    }

    private void FetchUserData(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
        }
        else
        {
            string email = (string)result.ResultDictionary["email"];
            CheckEmailExists(email);
        }
    }

    private void CheckEmailExists(string email)
    {
        auth.FetchProvidersForEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Email check was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Email check encountered an error: " + task.Exception);
                return;
            }
            if (task.Result.Count() > 0)
            {
                Debug.Log("Email already exists on Firebase");
            }
            else
            {
                Debug.Log("Email does not exist on Firebase. Creating a new account...");
                CreateAccount(email);
            }
        });
    }

    private void CreateAccount(string email)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, GenerateRandomPassword())
            .ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Account creation was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Account creation encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Account created successfully on Firebase");
            });
    }

    private string GenerateRandomPassword()
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] password = new char[10];
        System.Random random = new System.Random();

        for (int i = 0; i < password.Length; i++)
        {
            password[i] = characters[random.Next(characters.Length)];
        }

        return new string(password);
    }
}
