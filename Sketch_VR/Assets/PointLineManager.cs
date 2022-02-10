﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using System.IO;
using UnityEngine.UI;

public class PointLineManager : MonoBehaviour
{
    public Material lmat;
    public Slider widthSlider;

    public GameObject rightController;
    private float lineWidth;

    public float Max_width;
    public float Min_width;

    //public bool with_reference;

    private bool pressing;
    private TubeRenderer currLine;
    private LineRenderer lr;
    private List<Vector3> verts;
    public List<List<float>> timestamps;
    public List<List<List<float>>> all_timestamps;
    private float start_time;
    private bool visible;

    // Use this for initialization
    private GameObject sketch_space;

    private Material material;
    private GameObject laser;
    private GameObject ColorManager;

    void Start()
    {
        ColorManager = GameObject.Find("ColorPicker");
        rightController = GameObject.Find("RightHandAnchor");
        sketch_space = GameObject.Find("sketch_space");
        laser = GameObject.Find("LaserPointer");
        init();
    }


    public void init()
    {
        lineWidth = widthSlider.value;
        start_time = 0.0f;
        verts = new List<Vector3>();
        timestamps = new List<List<float>>();
        all_timestamps = new List<List<List<float>>>();
        pressing = false;
        visible = true;

    }
    // Update is called once per frame
    void Update()
    {
        float RI = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        bool undoX = OVRInput.GetDown(OVRInput.RawButton.X);
        bool hideY = OVRInput.GetDown(OVRInput.RawButton.Y);

        //Undo last stroke
        if (pressing == false && undoX == true)
        {
            GameObject[] delete = GameObject.FindGameObjectsWithTag("Dynamic_Line");
            int deleteCount = delete.Length;
            if (deleteCount > 0)
            {
                Destroy(delete[deleteCount - 1]);
                if (all_timestamps.Count > 0)
                    all_timestamps.RemoveAt(all_timestamps.Count - 1);
            }

        }

        //Hide reference
        if (pressing == false && hideY == true)
        {
            GameObject countdown = GameObject.Find("CountDown");
            
            if (!countdown.GetComponent<CountDownScript>().doOnce)
            {
                GameObject[] reference = GameObject.FindGameObjectsWithTag("reference");

                for (int i = 0; i < reference.Length; i++)
                {
                    MeshRenderer[] meshrenderer_obj = reference[i].transform.GetComponentsInChildren<MeshRenderer>();
                    for (int j = 0; j < meshrenderer_obj.Length; j++)
                        meshrenderer_obj[j].enabled = !meshrenderer_obj[j].enabled;
                }
                visible = !visible;
            }

        }

        bool draw = !PlayerManager.memory || (PlayerManager.memory && !visible);
        //Debug.LogError("memory: " + PlayerManager.memory + draw);

        //press-start a new stroke
        if (pressing == false && RI > 0 && draw)
        {
            pressing = true;
            GameObject go = new GameObject();
            go.transform.SetParent(sketch_space.transform);
            go.tag = "Dynamic_Line";
            //lr = go.AddComponent<LineRenderer>();
            //lr.useWorldSpace = false;
            //lr.material = material;
            //lr.material.color = ColorManager.GetComponent<ColorManager>().GetCurrentColor();
            //lr.startWidth = lineWidth;
            //lr.endWidth = lineWidth;
            //lr.positionCount = 0;
            //lr.sortingLayerName = "ForeGround";
            //lr.sortingOrder = 2000;

            currLine = go.AddComponent<TubeRenderer>();
            currLine.lmat = lmat;
            currLine.lmat.color = ColorManager.GetComponent<ColorManager>().GetCurrentColor();
            currLine.setWidth(lineWidth);
            start_time = Time.time;
        }

        // release
        if (pressing == true && RI == 0 && draw)
        {
            pressing = false;

            //add stroke info to list of strokes
            if (timestamps.Count > 0)
            {
                all_timestamps.Add(new List<List<float>>(timestamps));
                timestamps.Clear();
            }

            verts.Clear();
            start_time = 0.0f;
        }

        //press-keep drawing on this stroke
        if (pressing == true && RI > 0 && draw)
        {
            Vector3 pos = rightController.transform.position;
            if (verts.Count == 0 || verts[verts.Count - 1] != pos)
            {
                verts.Add(pos);
                Vector3 relaPt = sketch_space.transform.InverseTransformPoint(pos);

                //timestamps.Add(new List<float>{ relaPt[0], relaPt[1], relaPt[2], Time.time - start_time });
                timestamps.Add(new List<float> { relaPt[0], relaPt[1], relaPt[2], Time.time - start_time, Time.time - PlayerManager.load_ref_time });

            }

            //lr.positionCount = verts.Count;
            //lr.SetPositions(verts.ToArray());
            currLine.positionCount = verts.Count;
            currLine.SetPositions(verts.ToArray());

        }

        if (pressing == false)
        {
            laser.GetComponent<LineRenderer>().enabled = true;

            float moveHOrizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (moveVertical > 0 && lineWidth < Max_width)
            {
                lineWidth += 0.001f;
                rightController.gameObject.transform.localScale = new Vector3(lineWidth, lineWidth, lineWidth);
            }

            if (moveVertical < 0 && lineWidth > Min_width)
            {
                lineWidth -= 0.001f;
                rightController.gameObject.transform.localScale = new Vector3(lineWidth, lineWidth, lineWidth);
            }
        }
        else
            laser.GetComponent<LineRenderer>().enabled = false;

    }
    public void OnSliderValueChanged(float value)
    {
        lineWidth = value;
    }
}