using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class SaveSketchLogic : MonoBehaviour
{
    private GameObject PointLineManager;
    public bool save_meta_info;
    public bool saved;
    [SerializeField] private TextMeshProUGUI saveinfo;

    private void Start()
    {
        PointLineManager = GameObject.Find("PointLineManager");
        saved = false;
    }

    public void SaveSketch()
    {
        GameObject[] sketch = GameObject.FindGameObjectsWithTag("Dynamic_Line");
        if (sketch.Length == 0)
        {
            saveinfo.text = "Sketch doesn't exist.";
            Debug.Log("Sketch doesn't exist.");
        }
        else
        {
            string folder = PlayerManager.save_dir;
            string filename = PlayerManager.model_id.Replace(Path.DirectorySeparatorChar + "", "_") + "_" + PlayerManager.player_id + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            ObjExporter ObjExporter = GameObject.Find("ObjExporter").GetComponent<ObjExporter>();


            ObjExporter.DoExportsPointsFromGame(sketch, folder, filename);

            Debug.Log("Save a Sketch with " + sketch.Length + " strokes!");

            if (save_meta_info)
            {
                ObjExporter.DoExportsMetaInfo(PointLineManager.GetComponent<PointLineManager>().all_timestamps, folder, filename);
            }
            saveinfo.text = "Save:" + filename;
            saved = true;
        }
    }

    public void ClearSketch()
    {
        Debug.Log("clear all");
        GameObject[] delete = GameObject.FindGameObjectsWithTag("Dynamic_Line");
        int deleteCount = delete.Length;//.Length();
        for (int i = deleteCount - 1; i >= 0; i--)
            Destroy(delete[i]);

        //Clear timestamps
        PointLineManager.GetComponent<PointLineManager>().all_timestamps.Clear();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
