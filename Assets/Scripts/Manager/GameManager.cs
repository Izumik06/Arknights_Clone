using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Izumik
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public int deploymentSlots;
        public int cost;
        public int targetHP;

        public bool isStartGame;
        private void Awake()
        {
            if(instance == null )
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            StartGame();
        }
        public void StartGame()
        {
            isStartGame = true;
            Time.timeScale = 2;
            StageManager.Instance.SetStage();
            SpawnManager.Instance.StartSpawnEnemy();
        }
        public static GameManager Instance
        {
            get
            {
                if ( instance == null)
                {
                    return null;
                }
                return instance;
            }
        }
    }
}
