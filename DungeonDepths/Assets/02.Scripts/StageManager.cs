using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    private static StageManager instance = null;


    [HideInInspector]
    public int stageIdx = 0; //  스테이지 인덱스
    [HideInInspector]
    public string registery; // 노드 속성
    [HideInInspector]
    public bool isClear = false;
    Stage1 stage1;

    public GameObject _player = null;
    public bool isAporPortal = false;// 포탈에 접근 했는지
    public GameObject portal;

    public static StageManager Instance
    {

        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<StageManager>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<StageManager>();
                    instance = newObj;
                }
            }

            return instance;
        }


    }


    public void Awake()
    {
        var objs = FindObjectsOfType<StageManager>();
        if(objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);



        portal = GameObject.Find("Portal");

        _player = GameObject.FindWithTag("Player");
        stage1 = GetComponent<Stage1>();

    }




    public void Move(Vector3 Target)
    {
        _player.transform.position = Target;
    }



}
