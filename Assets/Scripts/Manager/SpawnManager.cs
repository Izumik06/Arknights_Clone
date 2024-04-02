using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Izumik
{
    public class SpawnManager : MonoBehaviour
    {
        public string[,] dispatchLog;
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

        }
        IEnumerator SpawnEnemy()
        {
            for(int i = 1; i < dispatchLog.Length; i++)
            {
                for(int j = 0; j < dispatchLog[i, 1])
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

