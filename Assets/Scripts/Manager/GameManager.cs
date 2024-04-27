using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Izumik
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public int deploymentSlots;

        [Header("�����/��ǥHP")]
        public int targetHP;
        public int maxEnemyCount;
        public int destroyedEnemyCount;

        [Header("�ڽ�Ʈ ����")]
        public int cost;
        public float costDelay;
        public float costSpeed;

        [Header("���� ���� ����")]
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
        //���� ����, ���� ������ ���� �ð����� ����
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
        //�ڽ�Ʈ ȸ��
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
