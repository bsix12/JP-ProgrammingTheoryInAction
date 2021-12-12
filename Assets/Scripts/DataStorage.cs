using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataStorage : MonoBehaviour
{
    public static DataStorage Instance;

    public int highScoreData;
    public string playerNameInputData;

    // Start is called before the first frame update


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadDataFromDisk();
    }

    [System.Serializable]
    class SaveData
    {
        public int highScoreData;
        public string playerNameInputData;
    }

    public void SaveDataToDisk()
    {
        SaveData data = new SaveData();
        data.highScoreData = highScoreData;
        data.playerNameInputData = playerNameInputData;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadDataFromDisk()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScoreData = data.highScoreData;
            playerNameInputData = data.playerNameInputData;
        }
    }
}
