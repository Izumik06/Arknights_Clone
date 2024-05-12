using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

namespace Izumik
{
    public class SpawnManager : MonoBehaviour
    {
        public List<SpawnData> dispatchLog;
        public List<GameObject> enemyList;
        public Queue<Enemy> enemyPool = new Queue<Enemy>();
        private static SpawnManager instance;
        int enemyIndex;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void StartSpawnEnemy()
        {
            for(int i = 0; i < dispatchLog.Count; i++)
            {
                CreateEnemy(dispatchLog[i]);
            }
            StartCoroutine(SpawnEnemy());
        }
        //시간에 맞게 몹 생성 실행
        IEnumerator SpawnEnemy()
        {
            yield return new WaitForSeconds(dispatchLog[0].spawnTime);
            StartCoroutine(PlaceEnemy(dispatchLog[0]));

            for(int i = 1; i < dispatchLog.Count; i++)
            {
                yield return new WaitForSeconds(dispatchLog[i].spawnTime - dispatchLog[i - 1].spawnTime);
                Debug.Log(dispatchLog[i].spawnTime - dispatchLog[i - 1].spawnTime);
                StartCoroutine(PlaceEnemy(dispatchLog[i]));
            }
        }
        //GameManager에 있는 Enemy 배치
        IEnumerator PlaceEnemy(SpawnData data)
        {
            for(int i = 0; i < data.count; i++)
            {
                Enemy enemy = enemyPool.Dequeue();
                enemy.gameObject.SetActive(true);
                enemy.StartMove();
                GameManager.Instance.enemyList.Add(enemy);
                yield return new WaitForSeconds(data.spawnDelay);
            }
        }
        //몹 생성 후 게임메니저에 있는 리스트에 할당
        private void CreateEnemy(SpawnData data)
        {
            for (int i = 0; i < data.count; i++)
            {
                GameObject enemy = Instantiate(enemyList.Find(_ => _.name == data.type));
                enemy.transform.position = new Vector3(data.spawnPoint.x, 0, data.spawnPoint.y);
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.waitNodes = data.waitNodes;
                enemyScript.targetNode = data.defensePoint;

                if (data.posError.Count != 0)
                {
                    enemyScript.posError = data.posError[i % data.posError.Count];
                }
                if (data.useAstar)
                {
                    enemyScript.Findpath(data.root, data.spawnPoint);
                }
                else
                {
                    enemyScript.roots = data.root;
                }
                enemyScript.spawnBySpawnManager = true;
                enemyPool.Enqueue(enemyScript);
                enemy.SetActive(false);
            }
        }
        public static SpawnManager Instance
        {
            get
            {
                if(instance == null)
                {
                    return null;
                }
                return instance;
            }
        }
    }
}

