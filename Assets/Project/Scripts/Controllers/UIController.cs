using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject homeUI;
    public GameObject loginUI;

    public GameObject[] UIs;
    public static UIController UIManager;
    // Start is called before the first frame update
    void Start()
    {
        UIManager = this;
    }

    public void DestroyUIbyIndex(int index)
    {
        Destroy(UIs[index]);
    }

    public void LoadUIbyPath()
    {
        Instantiate(homeUI);
    }

    public void ActiveUI(int uiIndex)
    {
        Debug.Log("active" + uiIndex);
        if (uiIndex == 0) //loginUI
        {
            loginUI.SetActive(true);
        } else if (uiIndex == 1) //homeUI
        {
            Debug.Log("Vo day roi");
            homeUI.SetActive(true);
        } else if (uiIndex == 2)
        {
            //
        }
        
    }

    public void DeactiveUI(int uiIndex)
    {
        Debug.Log("deactive" + uiIndex);
        if (uiIndex == 0) //loginUI
        {
            loginUI.SetActive(false);
        }
        else if (uiIndex == 1) //homeUI
        {
            homeUI.SetActive(false);
        }
        else if (uiIndex == 2)
        {
            //
        }
    }

    public void FetchUser()
    {
        homeUI.GetComponent<HomeUI>().FetchUserData();
    }

}
