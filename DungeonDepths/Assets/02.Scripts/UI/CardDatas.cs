using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObject/CardDatas", order = int.MaxValue)]
public class CardDatas : ScriptableObject
{
	public List<CardData> cardDataList = new List<CardData>();
}
