using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct View
{
    public string name;
    public GameObject viewPanel;

    public void SwitchView(View viewToShow)
    {
        Hide();
        viewToShow.Show();
        /* 
        viewPanel.SetActive(false);
        viewToShow.viewPanel.SetActive(true); */
    }

    public void Show()
    {
        viewPanel.SetActive(true);
    }

    public void Hide() {
        viewPanel.SetActive(false);
    }
}