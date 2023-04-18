//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//namespace MonsterManagerState
//{
//    #region Wait 상태 : 플레이어가 처음 스테이지(노드)에 입장한 상태
//    class Wait : State<MonsterManager>
//    {
//        public override void Enter(MonsterManager m)
//        {
//            //Debug.Log("Enter");
//            MonsterManager.Instance.isGameClear = false; // ?
//            //m.isPlayerDead = false;

//        }
//        public override void Execute(MonsterManager m)
//        {

//        }
//        public override void Exit(MonsterManager m)
//        {

//        }
//    }
//    #endregion

//    #region Spawn 상태 : 몹 생성 상태
//    class Spawn : State<MonsterManager>
//    {

//        public override void Enter(MonsterManager m)
//        {
//            SpawnAtStart(m);

//        }

//        private static void SpawnAtStart(MonsterManager m)
//        {
//            for (int i = 0; i < m.curMapMonsterCount; i++)
//            {
//                var newMonster = PoolManager.Instance.Instantiate("Enemy", m.enemySpawnPoints[i], Quaternion.identity);

//            }
//        }

//        public override void Execute(MonsterManager m)
//        {
//            int iter = m.deadSpots.Count; // 몬스터가 죽은 수 만큼 반복할 것
//            for (int i = 0; i < iter; i++)
//            {
//                var newMonster = PoolManager.Instance.Instantiate("Enemy", m.deadSpots[0], Quaternion.identity);
//                m.deadSpots.Remove(m.deadSpots[0]); // 몬스터를 새로 불러왔으므로 하나씩 삭제
//                m.meleeMonsters.Add(newMonster);
//            }
//            m.isRespawn = false;
//        }

//        public override void Exit(MonsterManager m)
//        {

//        }
//        #endregion
//    }

//    #region Finish 상태 : 스테이지(노드)클리어 혹은 플레이어 사망
//    class Finish : State<MonsterManager>
//    {
//        public override void Enter(MonsterManager m)
//        {
//            foreach (GameObject monster in m.monsterList)
//            {
//                monster.SetActive(false);
//            }
//            m.meleeMonsters.Clear();
//            //foreach(GameObject monster in m.rangeMonsters) {
//            //    monster.SetActive(false);
//        }
//        //m.rangeMonsters.Clear();
//    }
//    public override void Execute(MonsterManager m)
//    {
//    }
//    public override void Exit(MonsterManager m)
//    {

//    }
//}
//    #endregion
//}