using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
using UnityEngine.EventSystems;

public class UIManager : SingletonDontDestroy<UIManager>
{
    [SerializeField]
    List<GameObject> windowList = new List<GameObject>();
    GameObject curWindow;
    public List<GameObject> WindowList
    {
        get => windowList;
    }
    public GameObject CurWindow
    {
        get => curWindow;
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
        var _selectCardWindow = windowList[(int)Window.SELECTCARD];
        OnWindow(Window.SELECTCARD);
        var _selectedCards = CardManager.Instance.SelectRandomCards(3);
        for (int i = 0; i < 3; i++)
        {
            var _parent = _selectCardWindow.transform.GetChild(1).GetChild(i).gameObject;
            _parent.GetComponent<Card>().cardData = _selectedCards[i];
            SetCardInfo(_selectedCards[i], _parent);
        }
    }

    public void ChooseCard()
    {
        // CardManager.Instance.GetCard(cards[0]);
        var _clickObject = EventSystem.current.currentSelectedGameObject;
        CardManager.Instance.GetCard(_clickObject.GetComponent<Card>().cardData);
        OffWindow(Window.SELECTCARD);
    }



}
