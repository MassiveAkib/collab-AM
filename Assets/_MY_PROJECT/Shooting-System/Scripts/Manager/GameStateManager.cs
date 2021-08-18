using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public enum eMazeMode
{
    NORMAL = 0, 
    SPEED = 1, 
    ALLKILLENEMY = 2
}

public enum eMazeSize
{
    NORMAL = 0, 
    LARGE = 1
}

public class SaveData
{
    public int playerHP;
    public int playerDef;
    public int bulletCount;
    public float playerDamage;
    public float time;
    public bool autoGun;
    public int stage;
    public int highScoreStage;

    public eMazeMode mazeMode = eMazeMode.ALLKILLENEMY;
    public eMazeSize mazeSize = eMazeSize.NORMAL;

    public SaveData(int playerHP, int playerDef, int bulletCount, float playerDamage, float time, bool autoGun, int stage, int highScoreStage, eMazeMode mazeMode, eMazeSize mazeSize)
    {
        this.playerHP = playerHP;
        this.playerDef = playerDef;
        this.bulletCount = bulletCount;
        this.playerDamage = playerDamage;
        this.time = time;
        this.autoGun = autoGun;
        this.stage = stage;
        this.highScoreStage = highScoreStage;
        this.mazeMode = mazeMode;
        this.mazeSize = mazeSize;
    }
}

public class GameStateManager : MonoBehaviour
{
    public int playerHP;
    public int playerDef;
    public int bulletCount;
    public float playerDamage;
    public float time;
    public bool autoGun;

    public eMazeMode mazeMode;
    public eMazeSize mazeSize;

    private int stage;
    public int highScoreStage;

    private string filePath;

    public int Stage
    {
        get { return stage; }
        set
        {
            stage = value;

            if (stage > highScoreStage)
            {
                highScoreStage = stage;
            }
        }
    }

    private static GameStateManager instance = null;

    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameStateManage의 instance가 없습니다.");
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("GameStateManager의 instance가 이미 존재하므로 " + gameObject.name + "을 지웠습니다.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        GameStateManager[] obj = FindObjectsOfType<GameStateManager>(); 
        if (obj.Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);

        filePath = string.Concat(Application.persistentDataPath, "/", "Save");
        Debug.Log(Application.persistentDataPath);

        Load();
    }

    public void DataClear()
    {
        playerHP = 100;
        playerDef = 0;
        bulletCount = 30;
        playerDamage = 10;
        autoGun = false;
        stage = 1;
        time = 0f;
        mazeMode = eMazeMode.ALLKILLENEMY;
        mazeSize = eMazeSize.NORMAL;
    }

    public void Save()
    {
        SaveData saveData = new SaveData(playerHP, playerDef, bulletCount, playerDamage, time, autoGun, stage, highScoreStage, mazeMode, mazeSize);
        string savedJson = JsonUtility.ToJson(saveData);
        byte[] bytes = Encoding.UTF8.GetBytes(savedJson);
        string code = Convert.ToBase64String(bytes);
        File.WriteAllText(filePath, code);
    }

    public void Load()
    {
        if (File.Exists(filePath))
        {
            string code = File.ReadAllText(filePath);
            byte[] bytes = Convert.FromBase64String(code);
            string savedJson = Encoding.UTF8.GetString(bytes);
            SaveData saveData = JsonUtility.FromJson<SaveData>(savedJson);

            playerHP = saveData.playerHP;
            playerDef = saveData.playerDef;
            bulletCount = saveData.bulletCount;
            playerDamage = saveData.playerDamage;
            time = saveData.time;
            autoGun = saveData.autoGun;
            mazeMode = saveData.mazeMode;
            mazeSize = saveData.mazeSize;
            stage = saveData.stage;
            highScoreStage = saveData.highScoreStage;
        }
        else
        {
            highScoreStage = 1;
            DataClear();
        }
    }
}
