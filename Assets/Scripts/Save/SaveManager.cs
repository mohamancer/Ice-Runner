using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; } }
    private static SaveManager instance;

    // fields
    public SaveState save;
    private const string saveFileName = "data.ir";
    private BinaryFormatter formatter;


    // Actions
    public Action<SaveState> OnLoad;
    public Action<SaveState> OnSave;

    private void Awake()
    {
        instance = this;
        formatter = new BinaryFormatter();

        // Try and load previous save state
        Load();

    }

    public void Load()
    {
        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + saveFileName, FileMode.Open, FileAccess.Read);
            // Deserialize
            save = (SaveState)formatter.Deserialize(file);
            file.Close();
            OnLoad?.Invoke(save);
        }
        catch
        {
            Debug.Log("Save not found, create new.");
            Save();
        }

    }

    public void Save()
    {
        // If there's no previous state, create new
        if (save == null)
            save = new SaveState();

        // set the time at which we've tried saving
        save.LastSaveTime = DateTime.Now;

        // open a file and write to it
        FileStream file = new FileStream(Application.persistentDataPath + saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(file, save);
        file.Close();

        OnSave?.Invoke(save);

    }
}
