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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        InitWindowList();
    }
    void InitWindowList()   // 초기화
    {
        var windows = transform.GetChild(0);
        for (int i = 0; i < windows.transform.childCount; i++)
        {
            windowList.Add(windows.GetChild(i).gameObject);
            windowList[i].SetActive(false);
        }
        curWindow = windowList[0];
        curWindow.SetActive(true);
    }
    public void OnWindow(Window _name) // Full Window 켜기
    {
        windowList[(int)_name].SetActive(true);
        curWindow = windowList[(int)_name];
        GameManager.Instance.Pause();
    }
    public void OffWindow(Window _name) // Full Window 끄기
    {
        windowList[(int)_name].SetActive(false);
        curWindow = null;
        GameManager.Instance.Resume();
    }
    public void OnClickCloseBtn()   // UI 종료 버튼
    {
        if (curWindow != null)
            curWindow.SetActive(false);
        GameManager.Instance.Resume();
    }
    public void OnClickOptionBtn()  // 옵션
    {
        OnWindow(Window.OPTION);
    }
    public void OnClickRestartBtn()   // 게임 오버-> restart 버튼 
    {
        OffWindow(Window.GAMEOVER);
        GameManager.Instance.LoadMenuScene();
        OnWindow(Window.MAINMENU);
    }
    public void OnClickMainMenuPlayBtn() // 메인메뉴에서 플레이 하는 버튼
    {
        OffWindow(Window.MAINMENU);
        StartCoroutine(LoadingWindow());
        GameManager.Instance.LoadPlayScene();

    }

    IEnumerator LoadingWindow() 
    {
        windowList[(int)Window.LOADING].SetActive(true);
        yield return null;
        windowList[(int)Window.LOADING].SetActive(false);
    }
    public void ShowMapInfo()
    {

    }

    public void ShowCardInfo(CardData _cardData)
    {

    }
    
}
