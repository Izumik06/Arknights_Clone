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

        [Header("�ڽ�Ʈ / ��ġ�����ο���")]
        public TextMeshProUGUI costText;
        public Slider costSlider;
        public TextMeshProUGUI deploymentSlotText;
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
            Debug.Log("Ŭ����");
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
        //��ġ ���� �� UI ������Ʈ
        public void UpdateDeploymentSlotUI()
        {
            deploymentSlotText.text = "��ġ ���� �ο���: " + GameManager.Instance.deploymentSlots;
        }
        //����� / ��ǥ HP UI ������Ʈ
        public void UpdateSituationUI()
        {
            enemyCountText.text = GameManager.Instance.curEnemyCount.ToString() + "/" + GameManager.Instance.maxEnemyCount.ToString();
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
