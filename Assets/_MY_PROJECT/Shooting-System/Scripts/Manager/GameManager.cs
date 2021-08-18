using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] private Text timeText = null;
    [SerializeField] private Text gameOverTimeText = null;
    [SerializeField] private Text gunStateText = null;
    [SerializeField] private string[] gunStateTexts;

    public CanvasGroup gameOverCanvasGroup;
    /// <summary>
    /// 0: RestartBtn
    /// 1: EndBtn
    /// </summary>
    public Button[] gameOverButton;

    public float time = 0f;

    private StringBuilder sb = new StringBuilder(8);

    private GameStateManager gameStateManager = null;

    private PlayerMove playerMove = null;

    public GameObject enemys = null;
    public GameObject traps = null;

    private bool isSpeedMode = false;

    public float limitTime = 30f;
    private float startTime = 0f;

    public Text bulletUI = null;
    public Image bulletImage = null;
    public Text attackText = null;
    public Text defText = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManagerÀÇ instance°¡ ¾ø½À´Ï´Ù.");
            }

            return instance;
        }
    }

    public bool isPlay;

    public AudioSource sound;
    public AudioClip clip;

    public Text stageText;
    public Text mazeStateText;
    public Image mazeState;
    public string[] mazeStateTexts;

    public GameObject damageTextObject = null;
    public Transform damageTexts = null;
    public GameObject itemText = null;
    public Transform itemTexts = null;

    private int enemyKillCount = 0;
    private int enemyCount = 0;

    private ClearCheckManager clearCheckManager = null;

    public Text mazeModeText = null;
    public Text mazeModeValue = null;

    public int EnemyKillCount
    {
        get { return enemyKillCount; }
        set
        {
            if (gameStateManager.mazeMode == eMazeMode.ALLKILLENEMY)
            {
                enemyKillCount = value;
                ShowMazeEnemy(enemyCount - enemyKillCount);

                if (enemyKillCount >= enemyCount)
                {
                    clearCheckManager.Clear();
                }
            }
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("GameManagerÀÇ instance°¡ ÀÌ¹Ì Á¸ÀçÇÏ¹Ç·Î " + gameObject.name + "À» Áö¿ü½À´Ï´Ù.");
            Destroy(gameObject);
            return;
        }

        if (timeText == null)
        {
            Debug.LogError("timeText¿¡ Text°¡ ¾ø½À´Ï´Ù.");
        }

        if (gameOverTimeText == null)
        {
            Debug.LogError("gameOverTimeText¿¡ Text°¡ ¾ø½À´Ï´Ù.");
        }

        if (gunStateText == null)
        {
            Debug.LogError("gunStateText¿¡ Text°¡ ¾ø½À´Ï´Ù.");
        }

        if (gameOverCanvasGroup == null)
        {
            Debug.LogError("gameOverCanvasGroupÀÌ ¾ø½À´Ï´Ù.");
        }

        if (gameOverButton[0] == null)
        {
            Debug.LogError("ReSetButtonÀÌ ¾ø½À´Ï´Ù.");
        }

        if (gameOverButton[1] == null)
        {
            Debug.LogError("EndButtonÀÌ ¾ø½À´Ï´Ù.");
        }

        if (sound == null)
        {
            Debug.LogError("sound°¡ ¾ø½À´Ï´Ù.");
        }

        if (clip == null)
        {
            Debug.LogError("clipÀÌ ¾ø½À´Ï´Ù.");
        }

        if (stageText == null)
        {
            Debug.LogError("stageText°¡ ¾ø½À´Ï´Ù.");
        }

        if (damageTextObject == null)
        {
            Debug.LogError("damageTextObject°¡ ¾ø½À´Ï´Ù.");
        }

        if (damageTexts == null)
        {
            Debug.LogError("damageTexts°¡ ¾ø½À´Ï´Ù.");
        }

        gameStateManager = FindObjectOfType<GameStateManager>();

        if (gameStateManager == null)
        {
            Debug.LogError("gameStateManager°¡ ¾ø½À´Ï´Ù.");
        }

        playerMove = FindObjectOfType<PlayerMove>();

        if (playerMove == null)
        {
            // Debug.LogError("playerMove°¡ ¾ø½À´Ï´Ù.");
        }

        if (enemys == null)
        {
            Debug.LogError("enemys°¡ ¾ø½À´Ï´Ù.");
        }

        if (traps == null)
        {
            Debug.LogError("traps°¡ ¾ø½À´Ï´Ù.");
        }

        clearCheckManager = FindObjectOfType<ClearCheckManager>();

        if (clearCheckManager == null)
        {
            Debug.LogError("clearCheckManager°");
        }

        if (bulletUI == null)
        {
            Debug.LogError("bulletUI°");
        }

        if (bulletImage == null)
        {
            Debug.LogError("bulletImage°");
        }

        if (attackText == null)
        {
            Debug.LogError("attackText°");
        }

        if (defText == null)
        {
            Debug.LogError("defText°.");
        }

        if (itemText == null)
        {
            Debug.LogError("itemtext°");
        }

        if (itemTexts == null)
        {
            Debug.LogError("itemTexts°");
        }

        if (mazeState == null)
        {
            Debug.LogError("mazeState°");
        }

        PoolManager.CreatePool<DamageText>(damageTextObject, damageTexts, 20);
        PoolManager.CreatePool<ItemText>(itemText, itemTexts, 5);

        gameOverButton[0].onClick.AddListener(() =>//Restart
        {
            Time.timeScale = 1f;

            PoolManager.pool.Clear();
            PoolManager.prefabDictionary.Clear();

            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        gameOverButton[1].onClick.AddListener(() =>
        {
            //Application.Quit();
            SceneManager.LoadScene("Menu");
        });

        instance = this;
        isPlay = true;

        sb.Remove(0, sb.Length);
        sb.Append(gameStateManager.Stage);
        sb.Append(" ½ºÅ×ÀÌÁö");

        stageText.text = sb.ToString();
        time = gameStateManager.time;
        timeText.text = TimeDisplay();

        if (gameStateManager.mazeMode == eMazeMode.SPEED)
        {
            traps.SetActive(false);
            enemys.SetActive(false);

            isSpeedMode = true;
            startTime = time;
        }

        enemyCount = enemys.transform.childCount;

        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        ShowMazeState();

        if (gameStateManager.mazeMode == eMazeMode.SPEED)
        {
            mazeModeText.text = "³²Àº ½Ã°£";
        }
        else if (gameStateManager.mazeMode == eMazeMode.ALLKILLENEMY)
        {
            mazeModeText.text = "³²Àº Àû";
            ShowMazeEnemy(enemyCount - enemyKillCount);
        }
        else
        {
            mazeModeText.text = "Å»ÃâÇÏ¼¼¿ä!";
            sb.Remove(0, sb.Length);
            sb.Append("ÃÖ°í±â·Ï: ");
            sb.Append(gameStateManager.highScoreStage);
            sb.Append("½ºÅ×ÀÌÁö");
            mazeModeValue.text = sb.ToString();
        }
    }

    private void Update()
    {
        if (!isPlay) return;

        time += Time.deltaTime;

        timeText.text = TimeDisplay();

        if (isSpeedMode)
        {
            ShowMazeTime();

            if (startTime + limitTime <= time)
            {
                playerMove.GameOver();
            }
        }
    }

    private string timeCheck(int time)
    {
        if (time < 10)
        {
            return "0" + time;
        }

        return time.ToString();
    }

    public void GameOver()
    {
        gameStateManager.DataClear();
        gameStateManager.Save();

        isPlay = false;
        //gameOverTimeText.text = "Time: " + TimeDisplay();

        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.Confined;

        gameOverCanvasGroup.blocksRaycasts = true;
        gameOverCanvasGroup.interactable = true;

        sound.clip = clip;
        sound.loop = false;
        sound.Play();

        gameOverCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
        {
            Time.timeScale = 0f;
        });
    }

    public string TimeDisplay(float time = 0)
    {
        time = time == 0 ? this.time : time;

        int minute = (int)time / 60;
        int second = (int)time - minute * 60;
        int millisecond = (int)((time - (minute * 60 + second)) * 100);

        sb.Remove(0, sb.Length);
        sb.Append(timeCheck(minute));
        sb.Append(':');
        sb.Append(timeCheck(second));
        sb.Append(':');
        sb.Append(timeCheck(millisecond));

        return sb.ToString();
    }

    public void GunModeUIChange(int textNumber)
    {
        gunStateText.text = gunStateTexts[textNumber];
    }

    public void BulletCountUI(int bulletCount)
    {
        sb.Remove(0, sb.Length);
        sb.Append("");
        sb.Append(bulletCount);
        sb.Append("/30");

        bulletUI.text = sb.ToString();

        bulletImage.fillAmount = (float)bulletCount / 30;
    }

    public void AttackTextUI(int attack)
    {
        sb.Remove(0, sb.Length);
        sb.Append("°ø°Ý·Â: ");
        sb.Append(attack);

        attackText.text = sb.ToString();
    }

    public void DefTextUI(int def)
    {
        sb.Remove(0, sb.Length);
        sb.Append("¹æ¾î·Â: ");
        sb.Append(def);

        defText.text = sb.ToString();
    }

    private void ShowMazeState()
    {
        // mazeStateText.text = mazeStateTexts[(int)gameStateManager.mazeMode];
        //
        // if (gameStateManager.mazeSize == eMazeSize.LARGE)
        // {
        //     mazeStateText.text += " (´ëÇü ¹Ì·Î)";
        // }
        //
        // mazeStateText.DOColor(new Color(1f, 1f, 1f, 0f), 2f).SetDelay(1f);
        // mazeState.DOColor(new Color(1f, 1f, 1f, 0f), 2f).SetDelay(1f).OnComplete(() => {
        //     mazeState.gameObject.SetActive(false);
        //     mazeStateText.gameObject.SetActive(false);
        // });
    }

    private void ShowMazeTime()
    {
        mazeModeValue.text = TimeDisplay(startTime + limitTime - time);
    }

    private void ShowMazeEnemy(int value)
    {
        mazeModeValue.text = value.ToString();
    }
}
