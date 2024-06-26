using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SqlClient;
using System;
using System.Data;

namespace Izumik
{
    public class StageManager : MonoBehaviour
    {
        private static StageManager instance;
        public List<GameObject> stages = new List<GameObject>();
        public GameObject curStageObj;
        public Stage stage;
        public int curStage;

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
        private void Start()
        {

        }
        //맵 불러오고 정보 입력
        public void SetStage()
        {
            curStageObj = stages[curStage - 1];
            curStageObj.SetActive(true);
            stage = curStageObj.GetComponent<Stage>();

            SpawnManager.Instance.dispatchLog = LoadDispatch();

            Camera.main.transform.position = stage.cameraPos;
            Camera.main.transform.eulerAngles = stage.cameraRot;

            GameManager.Instance.cost = stage.initialCost;
            GameManager.Instance.costSpeed = stage.costSpeed;
            GameManager.Instance.targetHP = stage.targetHP;
            GameManager.Instance.deploymentSlots = stage.deploymentSlots;
            GameManager.Instance.maxEnemyCount = stage.enemyCount;
        }

        //DB에서 맵 로그 로드
        public List<SpawnData> LoadDispatch()
        {
            string connectString = "Server=localhost;Database=Stages;uid=sa;pwd=1234;";

            List<SpawnData> dispatchLog = new List<SpawnData>();

            using (SqlConnection conn = new SqlConnection(connectString))
            {
                //DB에 있는 행 수만큼 반복
                conn.Open();
                SqlDataReader dr = new SqlCommand("select count(*) from [" + stage.stageName + "]", conn).ExecuteReader();
                dr.Read();
                int dispatchCount = dr.GetInt32(0);
                dr.Close();
                dr = new SqlCommand("select * from [" + stage.stageName + "]", conn).ExecuteReader();
                for (int i = 0; i < dispatchCount; i++)
                {
                    //튜플 읽어와서 SpawnData로 변환
                    dr.Read();
                    float spawnTime = (float)dr.GetDouble(0);
                    string type = dr.GetString(1);
                    int count = dr.GetInt32(2);
                    float spawnDelay = (float)dr.GetDouble(3);
                    Node spawnPoint = new Node(dr.GetString(4));
                    List<Node> root = TypeTranslator.Instance.StringToRoot(dr.GetString(5));
                    List<WaitNode> waitNode;
                    if (!dr.IsDBNull(6))
                    {
                        waitNode = TypeTranslator.Instance.StringToWaitnode(dr.GetString(6));
                    }
                    else
                    {
                        waitNode = new List<WaitNode>();
                    }
                    List<float> posError;
                    if (!dr.IsDBNull(7))
                    {
                        posError = TypeTranslator.Instance.StringToPosError(dr.GetString(7));
                    }
                    else
                    {
                        posError = new List<float>();
                    }
                    Node targetNode = new Node(dr.GetString(8));
                    bool useAstar = dr.GetBoolean(9);

                    dispatchLog.Add(new SpawnData(spawnTime, type, count, spawnDelay, spawnPoint, root, waitNode, posError, targetNode, useAstar));
                }
            }
            return dispatchLog;
        }
        public static StageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                return instance;
            }
        }
    }
    [Serializable]
    public class WaitNode
    {
        public int x, y;
        public float time;

        public WaitNode(int _x, int _y, float _time)
        {
            this.x = _x;
            this.y = _y;
            this.time = _time;
        }
    }
    [Serializable]
    public class SpawnData
    {
        public string type;
        public int count;
        public float spawnTime;
        public float spawnDelay;
        public List<Node> root;
        public Node defensePoint;
        public bool useAstar;
        public List<WaitNode> waitNodes;
        public Node spawnPoint;
        public List<float> posError;

        public SpawnData(float spawnTime, string type, int count, float spawnDelay, Node spawnPoint, List<Node> root, List<WaitNode> _waitNodes, List<float> _PosError, Node defensePoint, bool useAstar)
        {
            this.type = type;
            this.count = count;
            this.spawnTime = spawnTime;
            this.spawnDelay = spawnDelay;
            this.root = root;
            this.defensePoint = defensePoint;
            this.useAstar = useAstar;
            this.waitNodes = _waitNodes;
            this.posError = _PosError;
            this.spawnPoint = spawnPoint;
        }
    }
}
