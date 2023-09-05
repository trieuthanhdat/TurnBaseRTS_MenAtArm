using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    public GameObject goBusy;
    private void Start() {
        UnitAction.instance.onBusyChange += OnBusyChange;
    }

    private void OnBusyChange(object sender, bool e)
    {
        if(e == true)
        {
            OnShow();
        }else
        {
            OnHide();
        }
    }

    public void OnShow()
    {
        if(goBusy)goBusy.SetActive(true);
    }
    public void OnHide()
    {
        if(goBusy)goBusy.SetActive(false);
    }
}
