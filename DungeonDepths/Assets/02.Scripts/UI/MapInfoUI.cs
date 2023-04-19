using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInfoUI : MonoBehaviour
{
    public List<Sprite> mapPreviewList = new List<Sprite>();
    Text mapName;
    Text mapDifficulty;
    Image mapImage;
    Map selectedMap;

    public List<Button> buttonList = new List<Button>(); // 버튼 리스트

    void Awake()
    {
        mapName = GameObject.Find("Text-ThemeTitle").GetComponent<Text>();
        mapDifficulty = GameObject.Find("Text-DifficultyTitle").GetComponent<Text>();
        mapImage = GameObject.Find("MapPreviewImg").GetComponent<Image>(); 
        for (int i = 0; i < buttonList.Count; i++)
        {
            // 버튼과 맵 정보를 연결
            int mapIndex = i;
            buttonList[i].onClick.AddListener(() =>
            {
                OnClick(mapIndex);
            });
        }
    }

    private void OnClick(int _mapIndex)
    {
        // 맵 정보 리스트에서 선택된 맵 정보를 가져옴
        selectedMap = StageManager.Instance.GetMapInfoList()[_mapIndex];

        // 선택된 맵 정보를 사용하여 맵 정보를 표시
        string _mapName = selectedMap.mapData.MapName;
        var _difficulty = selectedMap.mapData.Difficulty;
        mapName.text = "Name : " + _mapName;
        mapDifficulty.text = "Difficulty : " + _difficulty;

        mapImage.sprite = mapPreviewList[_mapIndex];
    }

    public void OnClickSelectBtn()
    {
        selectedMap.gameObject.SetActive(true);
        StageManager.Instance.StartStageMap(selectedMap);
    }


   
}
