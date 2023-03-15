using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    private static StageManager instance = null;



 
    [HideInInspector]
    public int StageIdx = 0; //  스테이지 인덱스
    [HideInInspector]
    public string registery; // 노드 속성
    [HideInInspector]
    public bool IsClear = false;
    [SerializeField]
    GameObject _player = null;
    [SerializeField]
    bool isAproPotal=false;// 포탈에 접근 했는지

    public static StageManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }




    



    public void clear()
    {
       
        switch (StageIdx)
        {
            case 0:

                break;
            case 1:
                break;
            case 2:
                break;
        }
        
    }

   
}
