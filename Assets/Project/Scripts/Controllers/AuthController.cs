using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using System;
using Facebook.Unity;
using Firebase.Auth;

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

    public void LoginWithEmail(string email, string password, Action<bool> callback)
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

    public void LoginWithFacebook(Action<bool> callback)
    {
        var perms = new List<string>() { "public_profile", "email" };
        
        FB.LogInWithReadPermissions(perms, AuthCallback =>
        {
            if (FB.IsLoggedIn)
            {
                // AccessToken class will have session details
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                Debug.Log(aToken.UserId);
                // Print current access token's granted permissions
                foreach (string perm in aToken.Permissions)
                {
                    Debug.Log(perm);
                }
            }
            else
            {
                Debug.Log("User cancelled login");
            }
        });

        
        
        Firebase.Auth.Credential credential =
        Firebase.Auth.FacebookAuthProvider.GetCredential(Facebook.Unity.AccessToken.CurrentAccessToken.UserId);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }

    
}
