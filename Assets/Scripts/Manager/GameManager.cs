using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Izumik
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public int deploymentSlots;

        [Header("섬멸수/목표HP")]
        public int targetHP;
        public int maxEnemyCount;
        public int destroyedEnemyCount;

        [Header("코스트 관련")]
        public int cost;
        public float costDelay;
        public float costSpeed;

        [Header("게임 진행 관련")]
        public List<Enemy> enemyList;
        public bool isStartGame;
        public bool isPause;
        public int gameSpeed;
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
        private void Update()
        {
            GetCost();
            Pause();
        }
        public void TargetHpDeduct(int lifePointPenalty)
        {
            targetHP -= lifePointPenalty;
            UIManager.Instance.ShowTargetHpdown();
            UIManager.Instance.UpdateSituationUI();
        }
        //퍼즈 실행, 퍼즈 해제시 원래 시간으로 실행
        private void Pause()
        {
            if (isPause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = gameSpeed;
            }
        }
        //코스트 회복
        private void GetCost()
        {
            costDelay += Time.deltaTime * costSpeed;
            if(costDelay >= 1)
            {
                costDelay = 0;
                cost++;
            }
        }
        public void StartGame()
        {
            isStartGame = true;
            StageManager.Instance.SetStage();
            UIManager.Instance.UpdateSituationUI();
            UIManager.Instance.UpdateDeploymentSlotUI();
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
