using System;
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

    //공통 스텟
    [Serializable]
    public class Stat
    {
        public string name;
        public int hp;
        public int atk;
        public int def;
        public int sdef; //마저
        public float attackspeed;
        public AttackType attacktype;

        public Stat(string name, int hp, int atk, int def, int sdef, float attackspeed, AttackType attacktype)
        {
            this.name = name;
            this.hp = hp;
            this.atk = atk;
            this.def = def;
            this.sdef = sdef;
            this.attackspeed = attackspeed;
            this.attacktype = attacktype;
        }
    }
    //적 스텟
    [Serializable]
    public class EnemyStat : Stat
    {
        public bool isFlying;
        public int weight;
        public int edef; //원소 내성
        public int eresistance; //원소 피해 저항
        public float speed;
        public float range;
        public int lifePointPenalty;
        public EnemyType enemyType;

        public EnemyStat(string name, int hp, int atk, int def, int sdef, int edef, int eresistance, float attackspeed, float speed, bool isFlying, int weight, float range, EnemyType enemyType, AttackType attacktype, int lifePointPenalty)
            : base(name, hp, atk, def, sdef, attackspeed, attacktype)
        {
            this.isFlying = isFlying;
            this.weight = weight;
            this.edef = edef;
            this.eresistance = eresistance;
            this.speed = speed;
            this.range = range;
            this.enemyType = enemyType;
            this.lifePointPenalty = lifePointPenalty;
        }
    }
}
