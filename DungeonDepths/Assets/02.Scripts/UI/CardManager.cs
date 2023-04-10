using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardRarity { NOMAL, RARE };
public class CardManager : MonoSingleton<CardManager>
{
	public List<GameObject> cardObjList = new List<GameObject>(); // ������ ����Ʈ
	public GameObject parent; // ������ �θ�
	public List<CardData> cardList = new List<CardData>(); // ��ũ���ͺ� ������Ʈ���� �̾ƿ� ������ ����Ʈ
	public CardDatas cardDatas; // ��ũ���ͺ� ������Ʈ

	public void Awake()
	{
		SetData();
	}

	private void Start()
	{
		InitCardData();
	}

	private void InitCardData() // Ǯ�Ŵ������� �����Ҵ��� �����տ� (��ũ���ͺ� ������Ʈ���� �ű� ����Ʈ)������ �Ű��ֱ�
	{
		for (int i = 0; i < cardList.Count; i++)
		{
			GameObject _list = parent.transform.GetChild(i).gameObject;
			var _cardObjList = _list.GetComponent<CardInfo>();
			_cardObjList.CardName = cardList[i].CardName;
			_cardObjList.CardDesc = cardList[i].CardDesc;
			_cardObjList.Rarity = cardList[i].Rarity;
			_cardObjList.Sprite = cardList[i].Sprite;
			cardObjList.Add(_list);
		}
	}

	private void SetData() // ��ũ���ͺ� �����͸� ����Ʈ�� �ű��
	{
		foreach (var _data in cardDatas.cardDataList)
		{
			cardList.Add(_data);
		}
	}
}