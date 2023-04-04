using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string playerTag = "Player";
    // Start is called before the first frame update
    public Transform moveTr;
    private void Awake()
    {
        moveTr = GameObject.FindWithTag("stage1").GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == playerTag)
        {

            StageManager.Instance.Move(moveTr.position);
            // 여기서 Map UI 발생
        }
    }
}
