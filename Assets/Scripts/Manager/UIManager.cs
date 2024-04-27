using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Izumik
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        [Header("���� ��ư")]
        public GameObject pauseIcon;
        public GameObject restartIcon;
        public GameObject pauseObj;

        [Header("���� �ӵ� ��ư")]
        public TextMeshProUGUI gameSpeedText;
        public GameObject normalSpeedIcon;
        public GameObject highSpeedIcon;

        [Header("����� / ��ǥ HP")]
        public TextMeshProUGUI enemyCountText;
        public TextMeshProUGUI targetHpText;
        public GameObject deductHpUI;
        public TextMeshProUGUI deductHpUI_Text;

        [Header("�ڽ�Ʈ / ��ġ�����ο���")]
        public TextMeshProUGUI costText;
        public Slider costSlider;
        public TextMeshProUGUI deploymentSlotText;

        [Header("���� ����")]
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
        //���� ���ǵ� ���� ��ư
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
        //Pause��ư
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
        //���� ���� ��ư
        public void ShowExitUI()
        {
            //���� ���� UI ���� ������Ʈ
            exitUI.SetActive(true);
            progressSlider.value = (float)GameManager.Instance.destroyedEnemyCount / GameManager.Instance.maxEnemyCount;
            progressText.text = Mathf.RoundToInt((float)GameManager.Instance.destroyedEnemyCount / GameManager.Instance.maxEnemyCount * 100).ToString() + "%";
            informationText.text = $"<color=orange>������ �ߵ� ����</color>�Ͻø� �̼� ��ġ�� {StageManager.Instance.stage.useSane - StageManager.Instance.stage.deductionSane} ȸ���˴ϴ�. <color=orange>(�̼��� 0��ŭ�� �Ҹ�)</color>:";
            useSaneText.text = "+" + (StageManager.Instance.stage.useSane - StageManager.Instance.stage.deductionSane).ToString();

            //1������� ���� �� pause
            if(GameManager.Instance.gameSpeed == 2)
            {
                SpeedBtn();
            }
            else
            {
                GameManager.Instance.isPause = true;
            }
        }
        //��ǥ HP ���� UI Ȱ��ȭ & ������Ʈ
        public void ShowTargetHpdown()
        {
            deductHpUI.SetActive(true);
            deductHpUI_Text.text = "X -" + (StageManager.Instance.stage.targetHP - GameManager.Instance.targetHP).ToString();
        }
        //Exitâ�� ���ư��� ��ư
        public void RestartGameBtn()
        {
            exitUI.SetActive(false);
            GameManager.Instance.isPause = false;
        }
        //��ġ ���� �� UI ������Ʈ
        public void UpdateDeploymentSlotUI()
        {
            deploymentSlotText.text = "��ġ ���� �ο���: " + GameManager.Instance.deploymentSlots;
        }
        //����� / ��ǥ HP UI ������Ʈ
        public void UpdateSituationUI()
        {
            enemyCountText.text = GameManager.Instance.destroyedEnemyCount.ToString() + "/" + GameManager.Instance.maxEnemyCount.ToString();
            targetHpText.text = GameManager.Instance.targetHP.ToString();
        }
        //�ڽ�Ʈ UI ������Ʈ
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
