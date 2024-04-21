using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveSystem : Singleton<SaveSystem>
{
    public static string EDITOR_SAVE_PATH = "Assets/Resources/Database/";
    public static string APP_SAVE_PATH = Application.persistentDataPath + "/Save/";

    public static void _Editor_Save(string filename, object json)
    {
        if (!Directory.Exists(EDITOR_SAVE_PATH)) Directory.CreateDirectory(EDITOR_SAVE_PATH);

        string jsonString = JsonUtility.ToJson(json);
        File.WriteAllText(EDITOR_SAVE_PATH + filename + ".json", jsonString);
    }

    public static T _Editor_Load<T>(string filename)
    {
        string loadJson = File.ReadAllText(EDITOR_SAVE_PATH + filename + ".json");
        return JsonUtility.FromJson<T>(loadJson);
    }

    public static bool Have(string filename)
    {
        if (!Directory.Exists(APP_SAVE_PATH)) Directory.CreateDirectory(APP_SAVE_PATH);

        string filePath = Path.Combine(APP_SAVE_PATH, filename + ".json");
        return File.Exists(filePath);
    }

    public static void Save(string filename, object obj)
    {
        if (!Directory.Exists(APP_SAVE_PATH)) Directory.CreateDirectory(APP_SAVE_PATH);

        string jsonString = JsonUtility.ToJson(obj);

        Debug.Log($"Save: {jsonString}");
        File.WriteAllText(APP_SAVE_PATH + filename + ".json", jsonString);
    }

    public static T Load<T>(string filename)
    {
        string fullPath = APP_SAVE_PATH + filename + ".json";

        if (!Directory.Exists(APP_SAVE_PATH))
        {
            Directory.CreateDirectory(APP_SAVE_PATH);
        }

        if (!File.Exists(fullPath))
        {
            return default(T);
        }

        string loadJson = File.ReadAllText(fullPath);
        Debug.Log($"Load: {loadJson}");
        return JsonUtility.FromJson<T>(loadJson);
    }
}
