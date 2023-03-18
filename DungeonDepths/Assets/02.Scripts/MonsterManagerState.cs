using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MonsterManagerState {
    class Wait : State<MonsterManager> {
        public override void Enter(MonsterManager m) {
            Debug.Log("Enter");
            MonsterManager.Instance.isGameOver = false;
            m.isPlayerDead = false;

        }
        public override void Execute(MonsterManager m) {

        }
        public override void Exit(MonsterManager m) {

        }
    }
    #region 各 贸澜 积己
    class Spawn : State<MonsterManager> {

        public override void Enter(MonsterManager m) {
            SpawnAtStart(m);

        }

        private static void SpawnAtStart(MonsterManager m) {
            for (int i = 0; i < m.totalMeleeNum; i++) {
                int index = Random.Range(0, m.spawnPoints.Count);
                var newMonster = PoolManager.ins.Instantiate("Enemy", m.spawnPoints[index].position, Quaternion.identity);
                m.meleeMonsters.Add(newMonster);

                //for(int i = 0; i < totalRangeNum; i++) {
                //    int index = Random.Range(0, spawnPoints.Count);
                //    rangeMonsters.Add(PoolManager.ins.Instantiate("Enemy", spawnPoints[index].position, Quaternion.identity));
                //}
            }
        }

        public override void Execute(MonsterManager m) {
            int index = m.deadSpots.Count;
            for (int i = 0; i < index; i++) {
                var newMonster = PoolManager.ins.Instantiate("Enemy", m.deadSpots[0], Quaternion.identity);
                m.deadSpots.Remove(m.deadSpots[0]);
                m.meleeMonsters.Add(newMonster);
            }
            m.isRespawn = false;
        }

        public override void Exit(MonsterManager m) {
            
        }
        #endregion
    }
    class Finish : State<MonsterManager> {
        public override void Enter(MonsterManager m) {
            foreach (GameObject monster in m.meleeMonsters) {
                monster.SetActive(false);
            }
            m.meleeMonsters.Clear();
            foreach(GameObject monster in m.rangeMonsters) {
                monster.SetActive(false);
            }
            m.rangeMonsters.Clear();
        }
        public override void Execute(MonsterManager m) {
        }
        public override void Exit(MonsterManager m) {

        }
    }
}