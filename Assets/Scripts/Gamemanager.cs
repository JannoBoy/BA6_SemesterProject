using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{

    public GameObject Menu_MainMenu;

    public Camera camera_Map, camera_Depth;

    public GameObject Menu_Landmark;
    public TMP_Text text_LandmarkTitle;
    public Image image_LandmarkImage;


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
