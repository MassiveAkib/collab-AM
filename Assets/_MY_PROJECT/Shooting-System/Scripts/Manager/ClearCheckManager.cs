using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ClearCheckManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup clearCanvasGroup;
    [SerializeField] private Text timeText;

    /// <summary>
    /// 0: ExitBtn
    /// 1: NextBtn
    /// </summary>
    public Button[] clearButton;

    //public AudioSource sound;
    //public AudioClip clip;

    private PlayerMove playerMove = null;
    private GameStateManager gameStateManager = null;

    public int largeMazeProbability = 50;
    public int speedModeProbability = 20;
    public int allKillEnemyModeProbability = 30;

    private bool isAllEnemyKillMode = false;

    private bool isGameClear = false;

    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>();

        if (playerMove == null)
        {
            Debug.LogError("playerMove°¡ ¾ø½À´Ï´Ù.");
        }

        gameStateManager = GameStateManager.Instance;

        if (gameStateManager == null)
        {
            Debug.LogError("gameStateManager°¡ ¾ø½À´Ï´Ù.");
        }

        if (clearButton[0] == null)
        {
            Debug.LogError("exitButtonÀÌ ¾ø½À´Ï´Ù.");
        }

        if (clearButton[1] == null)
        {
            Debug.LogError("clearButtonÀÌ ¾ø½À´Ï´Ù.");
        }

        if (clearCanvasGroup == null)
        {
            Debug.LogError("clearCanvasGroupÀÌ ¾ø½À´Ï´Ù.");
        }

        if (timeText == null)
        {
            Debug.LogError("timeText¿¡ Text°¡ ¾ø½À´Ï´Ù.");
        }

        // if (sound == null)
        // {
        //     Debug.LogError("sound°¡ ¾ø½À´Ï´Ù.");
        // }
        //
        // if (clip == null)
        // {
        //     Debug.LogError("clipÀÌ ¾ø½À´Ï´Ù.");
        // }

        clearButton[0].onClick.AddListener(() =>
        {
            Application.Quit();
            //SceneManager.LoadScene("Menu");
        });

        clearButton[1].onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            PoolManager.pool.Clear();
            PoolManager.prefabDictionary.Clear();

            DOTween.KillAll();
            
            SceneManager.LoadScene("Menu");

            // if (gameStateManager.mazeSize == eMazeSize.LARGE) 
            // {
            //     SceneManager.LoadScene("Maze2");
            // }
            // else
            // {
            //     SceneManager.LoadScene("Maze");
            // }
        });
        isAllEnemyKillMode = true;

        // if (GameStateManager.Instance.mazeMode == eMazeMode.ALLKILLENEMY)
        // {
        //     isAllEnemyKillMode = true;
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAllEnemyKillMode)
        {
            if (other.CompareTag("PLAYER"))
            {
                Clear();
            }
        }
    }

    public void Clear()
    {
        if (isGameClear) return;

        isGameClear = true;

        GameManager.Instance.isPlay = false;
        timeText.text = "Time: " + GameManager.Instance.TimeDisplay();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        gameStateManager.playerHP = playerMove.Hp;
        gameStateManager.playerDef = playerMove.def;
        gameStateManager.bulletCount = playerMove.currentBulletCount;
        gameStateManager.playerDamage = playerMove.damage;
        gameStateManager.autoGun = playerMove.autoGun;
        gameStateManager.Stage++;
        gameStateManager.time = GameManager.Instance.time;

        // int randomNum = Random.Range(0, 100);
        //
        // if (randomNum < speedModeProbability)
        // {
        //     gameStateManager.mazeMode = gameStateManager.mazeMode != eMazeMode.SPEED ? eMazeMode.SPEED : eMazeMode.NORMAL;
        // }
        // else if (randomNum < allKillEnemyModeProbability + speedModeProbability)
        // {
        //     gameStateManager.mazeMode = gameStateManager.mazeMode != eMazeMode.ALLKILLENEMY ? eMazeMode.ALLKILLENEMY : eMazeMode.NORMAL;
        // }
        // else
        // {
        //     gameStateManager.mazeMode = eMazeMode.NORMAL;
        // }
        //
        // randomNum = Random.Range(0, 100);
        //
        // if (randomNum < largeMazeProbability)
        // {
        //     gameStateManager.mazeSize = GameStateManager.Instance.mazeSize != eMazeSize.LARGE ? eMazeSize.LARGE : eMazeSize.NORMAL;
        // }
        // else
        // {
        //     gameStateManager.mazeSize = eMazeSize.NORMAL;
        // }

        //gameStateManager.Save();

        clearCanvasGroup.blocksRaycasts = true;
        clearCanvasGroup.interactable = true;

        // sound.clip = clip;
        // sound.loop = false;
        // sound.Play();

        if (clearCanvasGroup != null)
        {
            clearCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
            {
                Time.timeScale = 0f;
            });
        }
    }
}
