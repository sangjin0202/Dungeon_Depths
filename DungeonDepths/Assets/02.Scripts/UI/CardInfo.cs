using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
	[SerializeField] CardRarity rarity; public CardRarity Rarity { get { return rarity; } set { rarity = value; } }
	[SerializeField] string cardName; public string CardName { get { return cardName; } set { cardName = value; } }
	[SerializeField] string cardDesc; public string CardDesc { get { return cardDesc; } set { cardDesc = value; } }
	[SerializeField] Sprite sprite; public Sprite Sprite { get { return sprite; } set { sprite = value; } }


	private void Awake()
	{
		
	}

	private void Start()
	{
		
	}
}
