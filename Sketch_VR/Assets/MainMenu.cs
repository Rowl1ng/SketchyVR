using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SFB;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public class MainMenu : MonoBehaviour
{
    public Toggle MemoryMode;
    public TMP_InputField username;

    public TMP_InputField rootdir;
    //public TMP_InputField modeldir;
    //public TMP_InputField namelist;

    public TMP_InputField subsetID;
    public TMP_InputField index;
    public TextMeshProUGUI message;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    
    //public void selectModelDir()
    //{

    //    PlayerManager.model_dir = StandaloneFileBrowser.OpenFolderPanel("Select Model Folder", "", true)[0];
    //    modeldir.text = PlayerManager.model_dir;

    //}
    public void selectRootDir()
    {

        rootdir.text = StandaloneFileBrowser.OpenFolderPanel("Select Root Folder", "", true)[0];
        //savedir.text = PlayerManager.root_dir;


    }

    //public void selectFile()
    //{
    //    PlayerManager.namelist_path = StandaloneFileBrowser.OpenFilePanel("Open Namelist File", "", "txt", true)[0];
    //    namelist.text = PlayerManager.namelist_path;
    //}

    public void startSketch()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            message.text = "Username is Null!";
            Debug.LogError("Username is Null!");
            
        }
        else if (string.IsNullOrEmpty(subsetID.text))
        {
            message.text = "subsetID is Null!";
            Debug.LogError("subsetID is Null!");

        }

        else
        {
            //if (string.IsNullOrEmpty(namelist.text))
            //{
            //    //message.text = "Name List is Null!";
            //    //Debug.LogError("Name List is Null!");
            //    PlayerManager.namelist_path = rootdir.text + Path.DirectorySeparatorChar + "namelist" + Path.DirectorySeparatorChar + subsetID.text + ".txt";
            //}
            //if (string.IsNullOrEmpty(savedir.text))
            //{
            //    //message.text = "Save Directory is Null!";
            //    //Debug.LogError("Save Directory is Null!");
            //    PlayerManager.save_dir = "C://Users//iflyvrtest//Documents//projects//formal_dataset//save_dir";
            //}

            //if (string.IsNullOrEmpty(modeldir.text))
            //{
            //    //message.text = "Model Directory is Null!";
            //    //Debug.LogError("Model Directory is Null!");
            //    PlayerManager.model_dir = "C://Users//iflyvrtest//Documents//projects//formal_dataset//models";
            //}
            PlayerManager.model_dir = rootdir.text + Path.DirectorySeparatorChar + "models";
            PlayerManager.namelist_path = rootdir.text + Path.DirectorySeparatorChar + "namelist" + Path.DirectorySeparatorChar + subsetID.text + ".txt";
            PlayerManager.save_dir = rootdir.text + Path.DirectorySeparatorChar + "save_dir" + Path.DirectorySeparatorChar + subsetID.text;
            if (!Directory.Exists(PlayerManager.save_dir))
            {
                Directory.CreateDirectory(PlayerManager.save_dir);
            }
            PlayerManager.memory = MemoryMode.isOn;
            PlayerManager.player_id = username.text;
            //if (float.TryParse(countdown.text, out float number))
            //    PlayerManager.countdown = number;
            if (int.TryParse(index.text, out int number_))
                PlayerManager.index = number_ - 1;
            SceneManager.LoadScene(1);
        }

    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
