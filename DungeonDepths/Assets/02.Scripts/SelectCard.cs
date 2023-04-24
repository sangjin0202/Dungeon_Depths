using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectCard : MonoBehaviour
{
	public CardData cardData;
	[SerializeField]
	Image cardImg;
	[SerializeField]
	Text cardDesc;

	private void Awake()
	{
		cardImg = transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
		cardImg.sprite = cardData.Sprite;
		cardDesc = transform.parent.parent.GetChild(1).GetChild(0).GetComponent<Text>();
	}
}
