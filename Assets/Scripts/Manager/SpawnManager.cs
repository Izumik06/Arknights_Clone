using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Izumik
{
    public class SpawnManager : MonoBehaviour
    {
        public List<SpawnData> dispatchLog;
        public List<GameObject> enemyList;
        private static SpawnManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        public void StartSpawnEnemy()
        {
            StartCoroutine(SpawnEnemy());
        }
        //矫埃俊 嘎霸 各 积己 角青
        IEnumerator SpawnEnemy()
        {
            yield return new WaitForSeconds(dispatchLog[0].spawnTime);
            StartCoroutine(CreateEnemy(dispatchLog[0]));
            for(int i = 1; i < dispatchLog.Count; i++)
            {
                yield return new WaitForSeconds(dispatchLog[i].spawnTime - dispatchLog[i - 1].spawnTime);
                Debug.Log(dispatchLog[i].spawnTime - dispatchLog[i - 1].spawnTime);
                StartCoroutine(CreateEnemy(dispatchLog[i]));
            }
        }
        //各 积己
        IEnumerator CreateEnemy(SpawnData data)
        {
            for(int j = 0; j < data.count; j++)
            {
                GameObject enemy = Instantiate(enemyList.Find(_ => _.GetComponent<Enemy>().enemyType == data.type));
                enemy.transform.position = new Vector3(data.spawnPoint.x, 0, data.spawnPoint.y);
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.waitNodes = data.waitNodes;
                enemyScript.targetNode = data.defensePoint;

                if(data.posError.Count != 0)
                {
                    enemyScript.posError = data.posError[j % data.posError.Count];
                }
                if (data.useAstar)
                {
                    enemyScript.Findpath(data.root, data.spawnPoint);
                }
                else
                {
                    enemyScript.roots = data.root;
                }
                yield return new WaitForSeconds(data.spawnDelay);
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

