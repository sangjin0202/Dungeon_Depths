using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    List<GameObject> windowList = new List<GameObject>();
    GameObject curWindow;

    
    void Awake()
    {
        InitWindowList();
    }
    void InitWindowList()
    {
        var windows = transform.GetChild(0);
        for (int i = 0; i < windows.transform.childCount; i++)
        {
            windowList.Add(windows.GetChild(i).gameObject);
            windowList[i].SetActive(false);
        }
        curWindow = windowList[0];
    }
    public void OnWindow(Window _name)
    {
        windowList[(int)_name].SetActive(true);
        curWindow = windowList[(int)_name];
    }
    public void OffWindow(Window _name)
    {
        windowList[(int)_name].SetActive(false);
        curWindow = null;
    }
    public void OnClickOff()
    {
        if (curWindow != null)
            curWindow.SetActive(false);
    }
    public void OnClickOptionBtn()
    {
        OnWindow(Window.OPTION);
    }

    public void ShowMapInfo()
    {

    }
}
