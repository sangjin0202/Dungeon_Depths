using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Portal : MonoBehaviour
{
    string playerTag = "Player";
	bool isInteraction;
    private void Update()
    {
        if (Input.GetButtonDown("Interaction"))
        { 
            if(isInteraction)
                UIManager.Instance.OnWindowWithPause(Window.MAP);
        }
    }

    private void OnTriggerEnter(Collider _other)
	{
		if(_other.CompareTag(playerTag))
			isInteraction = true;
	}
    private void OnTriggerExit(Collider other)
    {
		isInteraction = false;
    }

}
