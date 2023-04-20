using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EnumTypes;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private bool isPlaying = false;
    private bool isPause;
    [SerializeField]
    private bool isGameOver;
    //TODO 추후 구현
    private bool isGameClear;
    public bool IsPause
    {
        get => isPause;
    }
    public bool IsGameOver
    {
        get => isGameOver;
        set => isGameOver = value;
    }
    private void Awake()
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
    }
    private void Update()
    {
        if (isPlaying)
        {  //Scene1에서 검사할 내용

            if (isGameOver)
            {
                Debug.Log("게임종료");
                Invoke("GameOver", 3f);
                isPlaying = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPause)
                UIManager.Instance.OnWindow(Window.OPTION);
            else
                UIManager.Instance.OffWindow(Window.OPTION);
        }
    }
    public void Pause()
    {
        if (!isPause)
        {
            Time.timeScale = 0f;
            isPause = true;
        }
    }
    public void Resume()
    {
        if (isPause)
        {
            Time.timeScale = 1f;
            isPause = false;
        }
    }
    public void GameOver()
    {
        UIManager.Instance.OnWindow(Window.GAMEOVER);
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
        isGameOver = false;
    }
    public void LoadPlayScene() // 메인 메뉴 -> 시작 버튼
    {
        SceneManager.LoadScene(1);
        isPlaying = true;
        Resume();
    }
}
