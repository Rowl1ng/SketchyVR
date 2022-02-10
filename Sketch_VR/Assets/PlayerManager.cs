using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using Dummiesman;



public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI saveinfo;

    //Default settings for debugging vrpaint scene, you need replace them with your paths
    public static string model_dir = @"..\demo_dataset";
    public static string save_dir = @"..\demo_savedir";
    public static string namelist_path = @"..\demo_namelist.txt";

    public static string player_id = "Sketcher";
    public static string model_id;
    public static int index = 0;
    public static float load_ref_time;
    public static bool memory = false;
    
    //Default countdown time is 30 seconds
    public static float countdown = 30;

    public List<string> namelist;
    public TextMeshProUGUI username;
    public TextMeshProUGUI modelname;

    private GameObject shape_space;

    public Slider sizeSlider;
    private float size;
    private GameObject line_manager;
    private GameObject SaveSketchLogic;

    private GameObject loadedObject;

    private void Start()
    {
        line_manager = GameObject.Find("PointLineManager");
        SaveSketchLogic = GameObject.Find("SaveSketchLogic");

        shape_space = GameObject.Find("shape_space");

        username.text = "Welcome! " + player_id;
        if (!File.Exists(namelist_path))
        {
            Debug.LogError("File doesn't exist: " + namelist_path);
        }
        else
        {
            //Read the text from directly from the test.txt file
            using (StreamReader sr = new StreamReader(namelist_path))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    namelist.Add(line);
                }
            }
        }
        model_id = namelist[index];
        modelname.text = "Model: "+ (index + 1) + "/" + namelist.Count + "  " + model_id;
        size = sizeSlider.value;
        loadModel();
        saveinfo.text = "Model: " + (index + 1) + "/" + namelist.Count;

    }

    private void loadModel()
    {
        string targetPath = model_dir + Path.DirectorySeparatorChar + model_id + ".obj";

        //file path
        if (!File.Exists(targetPath))
        {
            Debug.LogError("File doesn't exist: " + targetPath);
        }
        else
        {
            if (loadedObject != null)
                Destroy(loadedObject);
            loadedObject = new OBJLoader().Load(targetPath);
            loadedObject.tag = "reference";
            GameObject shape_anchor = GameObject.Find("shape_ReferenceAnchor");
            loadedObject.transform.SetParent(shape_space.transform);

            loadedObject.transform.SetPositionAndRotation(shape_anchor.transform.position, shape_anchor.transform.rotation);
            loadedObject.transform.localScale = new Vector3(size, size, size);
            load_ref_time = Time.time;
        }

    }
    public void NextModel()
    {
        if (SaveSketchLogic.GetComponent<SaveSketchLogic>().saved)
        {
            if (index < namelist.Count - 1)
            {
                index += 1;
                model_id = namelist[index];
                modelname.text = "Model: " + (index + 1) + "/" + namelist.Count + "  " + model_id;
                saveinfo.text = "Model: " + (index + 1) + "/" + namelist.Count;

                GameObject line_manager = GameObject.Find("PointLineManager");
                //Destroy(line_manager.GetComponent<PointLineManager>()); //toggle this script to re-invoke it
                line_manager.GetComponent<PointLineManager>().init();
                //GameObject[] reference = GameObject.FindGameObjectsWithTag("reference");
                //for (int i = 0; i < reference.Length; i++)
                //{
                //    Destroy(reference[i]);
                //}
                loadModel();
                GameObject[] sketch = GameObject.FindGameObjectsWithTag("Dynamic_Line");
                for (int i = 0; i < sketch.Length; i++)
                {
                    Destroy(sketch[i]);
                }

                //line_manager.AddComponent<PointLineManager>();

                //GameObject count_down = GameObject.Find("CountDown");
                //count_down.GetComponent<CountDownScript>().init(); 

            }
            SaveSketchLogic.GetComponent<SaveSketchLogic>().saved = false;
        }
    }
    public void PreviousModel()
    {
        if (index > 0)
        {
            index -= 1;
            model_id = namelist[index];
            modelname.text = "Model: " + (index + 1) + "/" + namelist.Count + "  " + model_id;
            saveinfo.text = "Model: " + (index + 1) + "/" + namelist.Count;

            //Destroy(line_manager.GetComponent<PointLineManager>()); //toggle this script to re-invoke it
            line_manager.GetComponent<PointLineManager>().init();

            //GameObject[] reference = GameObject.FindGameObjectsWithTag("reference");
            //for (int i = 0; i < reference.Length; i++)
            //{
            //    Destroy(reference[i]);
            //}
            loadModel();

            GameObject[] sketch = GameObject.FindGameObjectsWithTag("Dynamic_Line");
            for (int i = 0; i < sketch.Length; i++)
            {
                Destroy(sketch[i]);
            }

            //line_manager.AddComponent<PointLineManager>();

            //GameObject count_down = GameObject.Find("CountDown");
            //count_down.GetComponent<CountDownScript>().init();
        }
    }
    public void OnSliderValueChanged(float value)
    {
        size = value;
        loadedObject.transform.localScale = new Vector3(size, size, size);

    }
}
