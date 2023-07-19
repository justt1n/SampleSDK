using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class HomeUI : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField uid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FetchUserData()
    {
        Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            username.text = user.DisplayName;
            email.text = user.Email;
            uid.text = user.UserId;
        }
    }

    public void ChangeName()
    {
        Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = username.text,
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
            });
        }
    }

    
}
