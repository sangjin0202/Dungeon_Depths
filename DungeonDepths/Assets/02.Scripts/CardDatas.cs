using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Object/CardDatas", order = int.MaxValue)]
public class CardDatas : ScriptableObject
{
	public List<CardData> cardDataList = new List<CardData>();
}

[System.Serializable]
public class CardData// 저장할 카드 데이터
{
	[SerializeField] CardID cardID; public CardID CardID { get { return cardID; } set { cardID = value; } }
	[SerializeField] CardRarity rarity; public CardRarity Rarity { get { return rarity; } set { rarity = value; } }
	[SerializeField] Class cardClass; public Class CardClass { get { return cardClass; } set { cardClass = value; } }
	[SerializeField] string cardName; public string CardName { get { return cardName; } set { cardName = value; } }
	[SerializeField] string cardDesc; public string CardDesc { get { return cardDesc; } set { cardDesc = value; } }
	[SerializeField] float value; public float Value { get { return value; } set { this.value = value; } }
	[SerializeField] Sprite sprite; public Sprite Sprite { get { return sprite; } set { sprite = value; } }

}
