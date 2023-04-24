using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator anim;
    bool isOpen;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider _other)
    {
        if(_other.CompareTag("Player"))
        {
            // UI 띄우기 상호작용;
            if (Input.GetButtonDown("Interaction") && !isOpen)
            {
                OpenChest();
                Invoke("DisableChest", 2f);
            }
        }
    }

    public void OpenChest()
    {
        isOpen = true;
        anim.SetTrigger("Open");
        var _card = CardManager.Instance.NormalCard();
        CardManager.Instance.GetCard(_card);
        StartCoroutine(UIManager.Instance.ShowCardInfo(_card));
    }

    public void DisableChest()
    {
        isOpen = false;
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
