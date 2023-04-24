using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
using UnityEngine.EventSystems;

public class UIManager : SingletonDontDestroy<UIManager>
{
    [SerializeField]
    private List<GameObject> windowList = new List<GameObject>();
    Stack<GameObject> curWindows = new Stack<GameObject>();
    public List<GameObject> WindowList
    {
        get => windowList;
    }
    public GameObject CurWindow
    {
        get => curWindows.Peek();
    }
    public Stack<GameObject> CurWindows
    {
        get => curWindows;
    }
    protected override void OnAwake()
    {
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
        curWindows.Push(windowList[(int)Window.MAINMENU]);
        curWindows.Peek().SetActive(true);
    }
    public void OnWindowWithPause(Window _name) 
    {
        OnWindow(_name);
        GameManager.Instance.Pause();
    }
    public void OnWindow(Window _name) 
    {
        if (curWindows.Contains(windowList[(int)_name])) return;
        curWindows.Push(windowList[(int)_name]);
        curWindows.Peek().SetActive(true);
    }
    
    public void OffWindowWithResume() 
    {
        OffWindow();
        GameManager.Instance.Resume();
    }
    public void OffWindow() 
    {
        curWindows.Pop().SetActive(false);
    }
    public void OnClickOptionBtn()  // 옵션
    {
        OnWindowWithPause(Window.OPTION);
    }
    public void OnClickRestartBtn()   // 게임 오버-> restart 버튼 
    {
        CloseAllWindows();
        GameManager.Instance.LoadMenuScene();
        OnWindowWithPause(Window.MAINMENU);
    }
    public void OnClickMainMenuPlayBtn() // 메인메뉴에서 플레이 하는 버튼
    {
        OffWindowWithResume();
        StartCoroutine(LoadingWindow());
        GameManager.Instance.LoadPlayScene();
    }
    IEnumerator LoadingWindow()
    {
        windowList[(int)Window.LOADING].SetActive(true);
        yield return null;
        windowList[(int)Window.LOADING].SetActive(false);
    }
    public IEnumerator ShowCardInfo(CardData _cardData)
    {
        var _card = windowList[(int)Window.GETCARD];
        SetCardInfo(_cardData, _card);
        _card.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _card.SetActive(false);
    }
    public void SetCardInfo(CardData _cardData, GameObject _card)
    {
        _card.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = _cardData.Sprite;
        _card.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = _cardData.CardName;
        if (_cardData.Rarity == CardRarity.NOMAL)
            _card.transform.GetChild(1).GetChild(1).GetComponent<Image>().gameObject.SetActive(false);
        else
            _card.transform.GetChild(1).GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
        _card.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = _cardData.CardDesc;
    }
    public void ShowSelectCardInfo()
    {
        StartCoroutine(ShowSelect());
    }

    IEnumerator ShowSelect()
    {
        yield return new WaitForSeconds(2.0f);
        var _selectCardWindow = windowList[(int)Window.SELECTCARD];
        OnWindowWithPause(Window.SELECTCARD);
        var _selectedCards = CardManager.Instance.SelectRandomCards(3);
        for(int i = 0; i < 3; i++)
        {
            var _parent = _selectCardWindow.transform.GetChild(1).GetChild(i).gameObject;
            _parent.GetComponent<SelectCard>().cardData = _selectedCards[i];
            SetCardInfo(_selectedCards[i], _parent);
        }
    }

    public void ChooseCard()
    {
        // CardManager.Instance.GetCard(cards[0]);
        var _clickObject = EventSystem.current.currentSelectedGameObject;
        CardManager.Instance.GetCard(_clickObject.GetComponent<SelectCard>().cardData);
        OffWindowWithResume();
    }
    public void CloseAllWindows()
    {
        while (curWindows.Count != 0)
            OffWindow();
    }
}
