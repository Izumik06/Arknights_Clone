using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            SetStage();
        }
        //맵 불러오고 정보 입력
        void SetStage()
        {
            curStageObj = stages[curStage - 1];
            curStageObj.SetActive(true);
            stage = curStageObj.GetComponent<Stage>();

            SpawnManager.Instance.dispatchLog = GameObject.Find("Util").GetComponent<ExcelReader>().Decoding(stage.dispatchLog);

            GameManager.Instance.cost = stage.initialCost;
            GameManager.Instance.targetHP = stage.targetHP;
            GameManager.Instance.canPlaceNum = stage.canPlaceNum;
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
}
