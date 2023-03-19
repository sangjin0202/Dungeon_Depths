using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stage1 : MonoBehaviour
{
    public Transform[] spawnPoints; // spawn 
    public Transform BMSpawnPoints;
    public GameObject monsterPrefab;
    public GameObject bossMonsterPrefab;
    public float createTime;
    public int maxMonster = 10;
    public float dist = 5f;
    public Vector3 playerDist;
    public float portalRange = 3f;
    public Transform[] Portals;
    private int clearCnt;
    public bool isStageClear = false;
    public GameObject portal;
    public ParticleSystem particles;
    public struct NodeInfo
    {
        public bool isClear;
        
        public NodeInfo(bool isClear)
        {
            this.isClear = isClear;
        }
    }


   public NodeInfo newNode1 = new NodeInfo(false);
   public NodeInfo newNode2 = new NodeInfo(false);
   public NodeInfo newNode3 = new NodeInfo(false);
   public NodeInfo newNode4 = new NodeInfo(false);


    void Awake()
    {
        playerDist = GameObject.FindWithTag("Player").transform.position;
        particles = GameObject.Find("Portal").GetComponent<ParticleSystem>();
        particles.Stop();
        
        
    }
   
    void Update()
    {
        if (clearCnt == 4)
        {
            CallBossMonster();
            
        }
        if (isStageClear)
        {
            CallPortal();
        }
        
    }
    public void  CallBossMonster()
    {                                                 
     Instantiate(bossMonsterPrefab, BMSpawnPoints.position, BMSpawnPoints.rotation);                    
    }

    public void CallPortal()
    {
        particles.Play();
        foreach (Transform i in Portals)
        {
            float dist = Vector3.Distance(playerDist, i.position);
        }
        if (dist < portalRange)
        {
            StageManager.Instance.isAporPortal = true;// 포탈에 접근하면
        }
    }
}
