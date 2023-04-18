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
            Debug.Log("상자가까이 감");
            if (Input.GetButtonDown("Interaction") && !isOpen)
            {
                Debug.Log("상호작용");
                OpenChest();
                Invoke("DisableChest", 2f);
            }
        }
    }

    public void OpenChest()
    {
        isOpen = true;
        anim.SetTrigger("Open");
        // 인보크로 특성카드 UI 1개띄우고 습득시키기~
    }

    public void DisableChest()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
