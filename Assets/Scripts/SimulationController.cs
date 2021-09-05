using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationController : MonoBehaviour
{
    [Header("Bodies array")]
    [SerializeField] private GameObject[] Bodies;

    [SerializeField] private Rect BodiesWindow = new Rect(10, 10, 160, 350);
    [SerializeField] private Rect UIWindow = new Rect(10, 360, 160, 140);
    [SerializeField] private Rect[] PropertyWindows;

    private Vector2 scrollViewVector = Vector2.zero;
    private string[] BodyCount = { "2", "3", "4", "5", "6", "7", "8", "9", "10" };
    private int n;
    private int i;
    private int BodyIndex;
    private int BodyPropertyWindowYPosition = 50;

    void Start()
    {
        n = 0;
        i = 0;
        BodyIndex = 0;
    }

    private void Awake()
    {
        PropertyWindows = new Rect[10];

        UnityEngine.Screen.SetResolution(UnityEngine.Screen.currentResolution.width, UnityEngine.Screen.currentResolution.width, true);

        for(int j = 0; j < Bodies.Length; j++)
        {
            PropertyWindows[j] = new Rect(10, BodyPropertyWindowYPosition, 140, 25);
            BodyPropertyWindowYPosition += 30;
        }

        Time.timeScale = 0;
    } 

    private void OnGUI()
    {
        BodiesWindow = GUI.Window(0, BodiesWindow, BodyCountWindow, "Bodies");
        UIWindow = GUI.Window(1, UIWindow, SimulationUI, "UI");
    }

    void BodyPropertyWindowArray()
    {
        for (int j = 0; j < Bodies.Length; j++)
        {
            if(Bodies[j].activeSelf == true)
            {
                GUI.Button(PropertyWindows[j], "");
            }
        }
    }

    void SimulationUI(int WindowID)
    {
        if (GUI.Button(new Rect(30, 30, 100, 20), "Run"))
        {
            Time.timeScale = 1;
        }

        if (GUI.Button(new Rect(30, 55, 100, 20), "Stop"))
        {
            Time.timeScale = 0;
        }

        if (GUI.Button(new Rect(30, 80, 100, 20), "Reset"))
        {
            Time.timeScale = 0;

            for (int i = 2; i < Bodies.Length; i++)
            {
                Bodies[i].GetComponent<TrailRenderer>().Clear();
                Bodies[i].SetActive(false);
            }

            Bodies[0].transform.position = new Vector3(-15, 0, -10);
            Bodies[0].GetComponent<TrailRenderer>().Clear();

            Bodies[1].transform.position = new Vector3(15, 0 ,10);
            Bodies[1].GetComponent<TrailRenderer>().Clear();
        }

        if (GUI.Button(new Rect(30, 105, 100, 20), "Quit"))
        {
            Application.Quit();
        }
    }

    void BodyCountWindow(int WindowID)
    {
        if (GUI.Button(new Rect(25, 20, 125, 25), "Select Body Count"))
        {
            if (n == 0)
            {
                n = 1;
            }

            else
            {
                n = 0;
            }
        }

        if (n == 1)
        {
            scrollViewVector = GUI.BeginScrollView(new Rect(25, 45, 125, 115), scrollViewVector, new Rect(0, 0, 80, 225));

            for (i = 0; i < BodyCount.Length; i++)
            {
                if (GUI.Button(new Rect(0, i * 25, 300, 25), ""))
                {
                    n = 0;
                    BodyIndex = i;

                    for(int j = 0; j < (BodyCount.Length + 1); j++)
                    {
                        if(j < int.Parse(BodyCount[BodyIndex]))
                        {
                            Bodies[j].SetActive(true);
                        }

                        else
                        {
                            Bodies[j].SetActive(false);
                        }
                    }
                }

                GUI.Label(new Rect(0, i * 25, 300, 25), BodyCount[i]);
            }

            GUI.EndScrollView();
        }

        else
        {
            GUI.Label(new Rect(5, 20, 300, 25), BodyCount[BodyIndex]);
        }

        BodyPropertyWindowArray();
    }
}
