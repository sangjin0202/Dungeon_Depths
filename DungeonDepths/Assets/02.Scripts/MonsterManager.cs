using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    public static MonsterManager monsterManager;
    [SerializeField] List<Transform> spawnPoints;
    WaitForSeconds ws;
    void Awake() 
    {
        ws = new WaitForSeconds(1.0f);
        if(monsterManager == null) 
        {
            monsterManager = this;
        }

        GameObject SpawnPointsGroup = GameObject.Find("SpawnPoints");
        if(SpawnPointsGroup != null) 
        {
            SpawnPointsGroup.GetComponentsInChildren(spawnPoints);
        }
        spawnPoints.RemoveAt(0);
        StartCoroutine(Spawn());
    }
    
    IEnumerator Spawn() {
        yield return ws;
        int idx = Random.Range(0, spawnPoints.Count);
        GameObject enemy = Instantiate(enemyPrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
    }
}
