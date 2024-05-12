using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

namespace Izumik
{
    public class Enemy : MonoBehaviour
    {
        public bool moveLeft;
        public bool isWaiting;

        public EnemyStat baseStat;
        public EnemyStat currentStat;

        public Node targetNode;
        public List<Node> roots;
        public List<WaitNode> waitNodes;

        public float posError;
        public int deductTargetHp;

        public bool spawnBySpawnManager;

        private void Start()
        {
            GetStat();
        }
        private void GetStat()
        {
            if (baseStat.name == "Root") { return; }
            string connectString = "Server=localhost;Database=Datas;uid=sa;pwd=1234;";
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                SqlDataReader sd = new SqlCommand($"select * from [EnemyStats] where Type='{baseStat.name}'", conn).ExecuteReader();
                sd.Read();
                string name = sd.GetString(0);
                int hp = sd.GetInt32(1);
                int atk = sd.GetInt32(2);
                int def = sd.GetInt32(3);
                int sdef = sd.GetInt32(4);
                int edef = sd.GetInt32(5);
                int eresistance = sd.GetInt32(6);
                float attackspeed = (float)sd.GetDouble(7);
                float speed = (float)sd.GetDouble(8);
                bool isFlying = sd.GetBoolean(9);
                int weight = sd.GetInt32(10);
                float range = (float)sd.GetDouble(11);
                EnemyType enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), sd.GetString(12));
                AttackType attackType = range > 0 ? AttackType.ranged : AttackType.melee;
                int lifePointPenalty = sd.GetInt32(13);

                baseStat = new EnemyStat(name, hp, atk, def, sdef, edef, eresistance, attackspeed, speed, isFlying, weight, range, enemyType, attackType, lifePointPenalty);
                currentStat = new EnemyStat(name, hp, atk, def, sdef, edef, eresistance, attackspeed, speed, isFlying, weight, range, enemyType, attackType, lifePointPenalty);
            }
        }
        //target를 거쳐가는 경로 탐색 
        public void Findpath(List<Node> targets, Node spawnPoint)
        {
            roots = new List<Node>();

            FathFinder fathFinder = GetComponent<FathFinder>();

            roots.Add(spawnPoint);
            for (int i = 0; i < targets.Count; i++)
            {
                List<Node> fath = fathFinder.PathFinding(roots[roots.Count - 1], targets[i]);
                for (int j = 0; j < fath.Count; j++)
                {
                    roots.Add(fath[j]);
                }
            }
        }
        public void StartMove()
        {
            Debug.Log("이동 시작");
            StartCoroutine(Move());
        }
        IEnumerator Move()
        {
            Debug.Log("코루틴 실행");
            //이동
            for (int i = 0; i < roots.Count; i++)
            {
                isWaiting = false;
                while (Vector2.Distance(new Vector2(roots[i].x, roots[i].y), new Vector2(transform.position.x, transform.position.z)) >= 0.02)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(roots[i].x, transform.position.y, roots[i].y), currentStat.speed * 0.0125f);
                    moveLeft = (new Vector3(roots[i].x, transform.position.y, roots[i].y) - transform.position).x < 0;
                    yield return new WaitForSeconds(0.02f);
                }
                WaitNode waitNode = waitNodes.Find(_ => _.x == roots[i].x && _.y == roots[i].y);
                if (waitNode != null)
                {
                    isWaiting = true;
                    yield return new WaitForSeconds(waitNode.time);
                }
                else
                {
                    yield return null;
                }
            }

            //목표 지점에 도달하면 목표 Hp감소 / 자기 삭제
            GameManager.Instance.enemyList.Remove(this);

            if (!this.gameObject.tag.Contains("Root"))
            {
                if (spawnBySpawnManager)
                {
                    GameManager.Instance.destroyedEnemyCount++;
                }
                GameManager.Instance.TargetHpDeduct(deductTargetHp);

            }

            Destroy(gameObject);
        }
    }

    public enum AttackType
    {
        melee, ranged
    }
    public enum EnemyType
    {
        artsCreation, drone, infectedCreature, machina, possessed, sarkaz, seaMonster, none
    }
}
