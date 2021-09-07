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
    [SerializeField] private Rect PropertiesModalWindow = new Rect(175, 150, 270, 125);

    private Vector2 scrollViewVector = Vector2.zero;
    private string[] BodyCount = { "2", "3", "4", "5", "6", "7", "8", "9", "10" };
    private int n;
    private int i;
    private int k;
    private int BodyIndex;
    private int BodyPropertyWindowYPosition = 50;
    private bool ShowProperties = false;
    private string Mass = "";
    private string Pitch = "";
    private string Yaw = "";
    private string Roll = "";
    private string XPosition = "";
    private string YPosition = "";
    private string ZPosition = "";

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

        if(ShowProperties == true)
        {
            PropertiesModalWindow = GUI.ModalWindow(2, new Rect(PropertiesModalWindow.x, PropertyWindows[k].y, PropertiesModalWindow.width, PropertiesModalWindow.height), ModalWindow, "Set Properties");
        }
    }

    void BodyPropertyWindowArray()
    {
        for (int j = 0; j < Bodies.Length; j++)
        {
            if(Bodies[j].activeSelf == true)
            {
                if(GUI.Button(PropertyWindows[j], "Body " + j))
                {
                    k = j;
                    ShowProperties = true;
                }
            }
        }
    }

    void ModalWindow(int WindowID)
    {
        GUI.Label(new Rect(10, 20, 45, 20), "Mass: ");
        Mass = GUI.TextField(new Rect(50, 20, 95, 20), Mass, 6);

        GUI.Label(new Rect(10, 45, 45, 20), "Pitch: ");
        Pitch = GUI.TextField(new Rect(50, 45, 45, 20), Pitch, 6);
        GUI.Label(new Rect(100, 45, 45, 20), "Yaw: ");
        Yaw = GUI.TextField(new Rect(135, 45, 45, 20), Yaw, 6);
        GUI.Label(new Rect(185, 45, 45, 20), "Roll: ");
        Roll = GUI.TextField(new Rect(215, 45, 45, 20), Roll, 6);

        GUI.Label(new Rect(30, 70, 45, 20), "X: ");
        XPosition = GUI.TextField(new Rect(50, 70, 45, 20), XPosition, 6);
        GUI.Label(new Rect(115, 70, 45, 20), "Y: ");
        YPosition = GUI.TextField(new Rect(135, 70, 45, 20), YPosition, 6);
        GUI.Label(new Rect(200, 70, 45, 20), "Z: ");
        ZPosition = GUI.TextField(new Rect(215, 70, 45, 20), ZPosition, 6);

        if (GUI.Button(new Rect(70, 95, 140, 25),"Set Property"))
        {
            Bodies[k].GetComponent<Rigidbody>().mass = float.Parse(Mass);
            Bodies[k].transform.position = new Vector3(float.Parse(XPosition), float.Parse(YPosition), float.Parse(ZPosition));
            Bodies[k].transform.rotation = Quaternion.Euler(float.Parse(Pitch), float.Parse(Yaw), float.Parse(Roll));

            Mass = "";
            Pitch = "";
            Yaw = "";
            Roll = "";
            XPosition = "";
            YPosition = "";
            ZPosition = "";

            ShowProperties = false;
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
