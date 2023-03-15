using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private static Portal instance = null;

    [SerializeField]
     GameObject _player = null;
    [HideInInspector]
     GameObject home;
    [HideInInspector]
     GameObject Stage1;
    [HideInInspector]
     GameObject node1_1;
    [HideInInspector]
     GameObject node1_2;
    [HideInInspector]
     GameObject node1_3;
    [HideInInspector]
     GameObject node1_4;
    //[HideInInspector]
    // GameObject Stage2;
    //[HideInInspector]
    // GameObject node2_1;
    //[HideInInspector]
    // GameObject node2_2;
    //[HideInInspector]
    // GameObject node2_3;
    //[HideInInspector]
    //GameObject node2_4;
    //[HideInInspector]
    // GameObject Stage3;
    //[HideInInspector]
    // GameObject node3_1;
    //[HideInInspector]
    // GameObject node3_2;
    //[HideInInspector]
    // GameObject node3_3;
    //[HideInInspector]
    // GameObject node3_4;





    public static Portal Instance
    {
        get
        {
            if (null==instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Start()
    {
        home = GameObject.Find("Home");
        Stage1 = GameObject.Find("Stage1");
        node1_1 = GameObject.Find("node1");
        node1_2 = GameObject.Find("node2");
        node1_3 = GameObject.Find("node3");
        node1_4 = GameObject.Find("node4");
    }

   
    void Update()
    {

        
    }

    public void MoveToHome()
    {
        _player.transform.position = home.transform.position;
    }
    public void MoveToStage1()
    {
        _player.transform.position = Stage1.transform.position;
    }
    public void MoveToNode1_1()
    {
        _player.transform.position = node1_1.transform.position;
    }

    public void MoveToNode1_2()
    {
        _player.transform.position = node1_2.transform.position;
    }
    public void MoveToNode1_3()
    {
        _player.transform.position = node1_3.transform.position;
    }

}
