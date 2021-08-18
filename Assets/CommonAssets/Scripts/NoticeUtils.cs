using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NoticeUtils : MonoBehaviour
{
    public static NoticeUtils ins;
    void Awake() { ins = this; }

    public bool internetCheck;
    
    public Transform alert;
    public Transform oneBtnAlert;
    public Transform twoBtnAlert;
    public GameObject loadingAlert;
    public GameObject internetNotice;


    private Action<int> action;

    void Start()
    {
        DontDestroyOnLoad(this);
        if (internetCheck) { StartCoroutine("InternetCheck"); }
    }

    IEnumerator InternetCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!internetCheck) { yield break; }
            if (Application.internetReachability == NetworkReachability.NotReachable /*&& !Application.isEditor*/)
            {internetNotice.SetActive(true);}
            else
            {internetNotice.SetActive(false);}
        }
    }

    public void ShowAlert(string notice,float disappearTime = 3)
    {
        alert.gameObject.SetActive(true);
        alert.GetChild(1).GetComponent<Text>().text = notice;
        StopCoroutine("ShowAlertCoroutine");
        StartCoroutine("ShowAlertCoroutine", disappearTime);
    }

    IEnumerator ShowAlertCoroutine(float disappearTime)
    {
        yield return new WaitForSeconds(disappearTime);
        alert.gameObject.SetActive(false);
    }

    public void ShowOneBtnAlert(string notice, Action<int> callBack = null, string btnTxt = "Ok")
    {
        action = null;
        oneBtnAlert.gameObject.SetActive(true);
        oneBtnAlert.GetChild(2).GetComponent<Text>().text = notice;
        oneBtnAlert.GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>().text = btnTxt;
        if (callBack != null) { action = callBack; }

    }

    public void ShowTwoBtnAlert(string notice, Action<int> callBack, string firstBtnTxt = "Ok", string secondBtnTxt = "Cancel")
    {
        action = null;
        twoBtnAlert.gameObject.SetActive(true);
        twoBtnAlert.GetChild(2).GetComponent<Text>().text = notice;
        twoBtnAlert.GetChild(3).GetChild(0).GetChild(1).GetComponent<Text>().text = firstBtnTxt;
        twoBtnAlert.GetChild(3).GetChild(1).GetChild(1).GetComponent<Text>().text = secondBtnTxt;
        
        if (callBack != null) { action = callBack; }
    }

    public void NotificationBtn(Transform btnTransform)
    {
        GameUtils.ins.PlayBtnSound();
        btnTransform.parent.parent.gameObject.SetActive(false);
        if (action != null ) { action.Invoke(btnTransform.GetSiblingIndex()); }
    }

    public GameObject ShowLoadingAlert(string notice)
    {
        StopCoroutine("LoadingAlert");
        StartCoroutine("LoadingAlert", notice);
        return loadingAlert;
    }

    public void HideLoadingAlert()
    {
        StopCoroutine("LoadingAlert");
        loadingAlert.SetActive(false);
    }

    IEnumerator LoadingAlert(string notice)
    {
        float alertHideTime = 15;
        loadingAlert.SetActive(true);
        while (loadingAlert.activeInHierarchy && alertHideTime > 0)
        {
            string dots = "";
            for (int i = 0; i < 4; i++)
            {
                dots += ".";
                loadingAlert.transform.GetChild(2).GetComponent<Text>().text = notice + dots;
                alertHideTime -= .5f;
                yield return new WaitForSeconds(.5f);
            }
        }
        loadingAlert.SetActive(false);
    }
}
