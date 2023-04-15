using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardRarity { NOMAL, RARE };
public class CardManager : MonoBehaviour
{
	public List<GameObject> cardObjList = new List<GameObject>(); // 프리팹 리스트
	public GameObject parent; // 프리팹 부모
	public List<CardData> cardList = new List<CardData>(); // 스크립터블 오브젝트에서 뽑아온 데이터 리스트
	public CardDatas cardDatas; // 스크립터블 오브젝트

	public void Awake()
	{
	}

	private void Start()
	{
		InitCardData();
	}

	private void InitCardData() 
	{
		foreach (var _data in cardDatas.cardDataList) // 스크립터블 오브젝트 데이터를 리스트로 옮겨 담은 후
		{
			cardList.Add(_data);
		}

		for (int i = 0; i < cardList.Count; i++) // 풀매니저에서 동적할당한 프리팹에 (스크립터블 오브젝트에서 옮긴 리스트)데이터 옮겨주기
		{
			GameObject _list = parent.transform.GetChild(i).gameObject;
			var _cardObjList = _list.GetComponent<Card>();
			_cardObjList.cardData.CardName = cardList[i].CardName;
			_cardObjList.cardData.CardDesc = cardList[i].CardDesc;
			_cardObjList.cardData.Rarity = cardList[i].Rarity;
			_cardObjList.cardData.Value = cardList[i].Value;
			_cardObjList.cardData.Sprite = cardList[i].Sprite;
			cardObjList.Add(_list);
		}
	}

}
