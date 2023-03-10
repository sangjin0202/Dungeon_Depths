using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{


    public struct NodeInfo
    {
        public bool isClear;
        public NodeInfo(bool isClear)
        {
            this.isClear = isClear;
        }
    }
        private void Awake()
    {
        StageManager.Instance.registery = "Nature";
        
    }
    void Start()
    {
        NodeInfo Node1 = new NodeInfo(false);
        NodeInfo Node2 = new NodeInfo(false);
        NodeInfo Node3 = new NodeInfo(false);
        NodeInfo Node4 = new NodeInfo(false);
    }

    
    void Update()
    {
        
    }
}
