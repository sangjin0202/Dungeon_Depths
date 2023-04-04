using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{


    public Transform playerTr;
    public Transform thisTr;
    public Animator animator;
    

    void Awake()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        thisTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        
    }

    
    
    void Start()
    {
        
    }

    void Update()
    {
        float dist = Vector3.Distance(playerTr.position, thisTr.position);
        if (dist < 3.0f)
        {
            animator.SetBool("isOpen", true);
        }
        else
            animator.SetBool("isOpen", false);
    }
}
