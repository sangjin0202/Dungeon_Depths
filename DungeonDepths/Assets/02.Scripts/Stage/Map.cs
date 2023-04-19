using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapData mapData;
    [SerializeField]
    Transform startPosition;    //플레이어 맵 시작 위치    
    private bool isClear = false;  //맵 클리어 여부 
    public Transform StartPosition 
    { 
        get => startPosition; 
        set => startPosition = value; 
    }
    public bool IsClear
    {
        get => isClear;
        set => isClear = value;
    }
    public virtual void Awake()
    {
        //Debug.Log("IsClear 초기 : " + isClear);
        startPosition = transform.GetChild(0).GetComponent<Transform>();
        gameObject.transform.position = mapData.Position;   // 맵 위치 초기화
        // 초기화
    }
    public virtual void CheckIsClear() { }
}
