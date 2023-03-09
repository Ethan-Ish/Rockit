using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataManager
{


    private string file = "saveData.txt";

    public void Save(saveDataClass saveData)
    {
        string json = JsonUtility.ToJson(saveData);
        writeToFile(file, json);
    }

    public saveDataClass Load()
    {
        saveDataClass saveData = new saveDataClass();
        string json = ReadFromFile(file);
        JsonUtility.FromJsonOverwrite(json, saveData);
        return saveData;
    }

    private void writeToFile(string filename, string json)
    {
        string path = GetFilePath(filename);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found!");
        }
        return "";
    }

    public void printFilePath()
    {
        Debug.Log(GetFilePath("saveData.txt"));
    }

    private string GetFilePath(string filename)
    {
        return Application.persistentDataPath + "/" + filename;
    }

}
