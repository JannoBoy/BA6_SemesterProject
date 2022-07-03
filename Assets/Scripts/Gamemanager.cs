using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject Menu_MainMenu;
    public Camera camera_Map, camera_Depth;

    [Header("Landmark Main")]
    public GameObject Menu_Landmark_Main;
    public TMP_Text text_LandmarkTitle;
    public Image image_LandmarkImage;

    [Header("Landmark Slideshow")]
    public GameObject Menu_Landmark_Slideshow;
    public Image image_Slideshow_Frogo;
    public Image image_Slideshow;
    public TMP_Text text_Slideshow;
    public GameObject btn_Next;
    public GameObject btn_Previous;
    public TMP_Text text_PageCounter;

    [Header("Landmark Interaction")]
    public GameObject Menu_Landmark_Interaction;
    public Image image_Frogo;

    public Landmark[] Landmarks;

    public void StartGame()
    {
        camera_Map.enabled = true;
        camera_Depth.enabled = true;

        Menu_MainMenu.SetActive(false);
    }

    public void Btn_OpenLandmarkMenu(string name_Landmark)
    {
        for (int i = 0; i < Landmarks.Length; i++)
        {
            if (Landmarks[i].name.Equals(name_Landmark))
            {
                //fill landmark title and image
                FillLandmarkInfo(Landmarks[i].data);
                break;
            }
        }
    }
    //exit landmark and go back to map view
    public void Btn_CloseLandmarkMenu()
    {

    }
    //open interaction menu
    public void Btn_OpenLandmarkInteractionMenu()
    {

    }
    //go to next dialogue/image entry for the landmark
    public void Btn_LandmarkInteraction_Next()
    {

    }
    //go to previous dialogue/image entry for the landmark
    public void Btn_LandmarkInteraction_Previous()
    {

    }

    void FillLandmarkInfo(LandmarkData _data)
    {
        Debug.Log("The location name from entry is: " + _data.landmarkName);
    }

    [System.Serializable]
    public struct Landmark
    {
        public string name;
        public LandmarkData data;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Btn_OpenLandmarkMenu("Dom");
        }
    }
}
