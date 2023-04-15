using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	public CardData cardData;
	[SerializeField]
	Image cardIma;
	private void Awake()
	{
		cardIma = transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
	}

	private void Start()
	{
		cardIma.sprite = cardData.Sprite;
	}
}
