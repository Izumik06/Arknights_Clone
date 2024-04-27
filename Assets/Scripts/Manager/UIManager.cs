using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Izumik
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        [Header("퍼즈 버튼")]
        public GameObject pauseIcon;
        public GameObject restartIcon;
        public GameObject pauseObj;

        [Header("게임 속도 버튼")]
        public TextMeshProUGUI gameSpeedText;
        public GameObject normalSpeedIcon;
        public GameObject highSpeedIcon;

        [Header("섬멸수 / 목표 HP")]
        public TextMeshProUGUI enemyCountText;
        public TextMeshProUGUI targetHpText;
        public GameObject deductHpUI;
        public TextMeshProUGUI deductHpUI_Text;

        [Header("코스트 / 배치가능인원수")]
        public TextMeshProUGUI costText;
        public Slider costSlider;
        public TextMeshProUGUI deploymentSlotText;

        [Header("작전 포기")]
        public GameObject exitUI;
        public Slider progressSlider;
        public TextMeshProUGUI progressText;
        public TextMeshProUGUI informationText;
        public TextMeshProUGUI useSaneText;
        // Start is called before the first frame update
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCostUI();
        }
        //게임 스피드 조절 버튼
        public void SpeedBtn()
        {
            if(GameManager.Instance.gameSpeed == 2)
            {
                gameSpeedText.text = "1X";
                normalSpeedIcon.SetActive(true);
                highSpeedIcon.SetActive(false);
                GameManager.Instance.gameSpeed = 1;
            }
            else
            {
                gameSpeedText.text = "2X";
                normalSpeedIcon.SetActive(false);
                highSpeedIcon.SetActive(true);
                GameManager.Instance.gameSpeed = 2;
            }
        }
        //Pause버튼
        public void PauseBtn()
        {
            if(GameManager.Instance.isPause)
            {
                pauseIcon.SetActive(true);
                restartIcon.SetActive(false);
                pauseObj.SetActive(false);
                GameManager.Instance.isPause = false;
            }
            else
            {
                pauseIcon.SetActive(false);
                restartIcon.SetActive(true);
                pauseObj.SetActive(true);
                GameManager.Instance.isPause = true;
            }
        }
        //작전 포기 버튼
        public void ShowExitUI()
        {
            //작전 포기 UI 정보 업데이트
            exitUI.SetActive(true);
            progressSlider.value = (float)GameManager.Instance.destroyedEnemyCount / GameManager.Instance.maxEnemyCount;
            progressText.text = Mathf.RoundToInt((float)GameManager.Instance.destroyedEnemyCount / GameManager.Instance.maxEnemyCount * 100).ToString() + "%";
            informationText.text = $"<color=orange>작전을 중도 포기</color>하시면 이성 수치가 {StageManager.Instance.stage.useSane - StageManager.Instance.stage.deductionSane} 회복됩니다. <color=orange>(이성은 0만큼만 소모)</color>:";
            useSaneText.text = "+" + (StageManager.Instance.stage.useSane - StageManager.Instance.stage.deductionSane).ToString();

            //1배속으로 변경 후 pause
            if(GameManager.Instance.gameSpeed == 2)
            {
                SpeedBtn();
            }
            else
            {
                GameManager.Instance.isPause = true;
            }
        }
        //목표 HP 감소 UI 활성화 & 업데이트
        public void ShowTargetHpdown()
        {
            deductHpUI.SetActive(true);
            deductHpUI_Text.text = "X -" + (StageManager.Instance.stage.targetHP - GameManager.Instance.targetHP).ToString();
        }
        //Exit창의 돌아가기 버튼
        public void RestartGameBtn()
        {
            exitUI.SetActive(false);
            GameManager.Instance.isPause = false;
        }
        //배치 가능 수 UI 업데이트
        public void UpdateDeploymentSlotUI()
        {
            deploymentSlotText.text = "배치 가능 인원수: " + GameManager.Instance.deploymentSlots;
        }
        //섬멸수 / 목표 HP UI 업데이트
        public void UpdateSituationUI()
        {
            enemyCountText.text = GameManager.Instance.destroyedEnemyCount.ToString() + "/" + GameManager.Instance.maxEnemyCount.ToString();
            targetHpText.text = GameManager.Instance.targetHP.ToString();
        }
        //코스트 UI 업데이트
        void UpdateCostUI()
        {
            costSlider.value = GameManager.Instance.costDelay;
            costText.text = GameManager.Instance.cost.ToString();
        }
        public static UIManager Instance
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
