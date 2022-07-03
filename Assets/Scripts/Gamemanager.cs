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

    string currentLandmarkName;
    Landmark currentLandmark;

    public int _slideshowIndex = 0;

    public void StartGame()
    {
        camera_Map.enabled = true;
        camera_Depth.enabled = true;

        Menu_MainMenu.SetActive(false);
    }

    public void Btn_OpenLandmarkMenu(string name_Landmark)
    {
        Landmark myLandmark = GetLandmark(name_Landmark);
        if (myLandmark.data == null)
        {
            Debug.Log("Landmark " + name_Landmark + " not found");
            return;

        }
        else
        {
            //set current landmark shared variables
            currentLandmark = myLandmark;
            currentLandmarkName = name_Landmark;
            //fill landmark title and image
            UpdateLandmarkMenuInfo(currentLandmark.data);
            //initialize index for slideshow
            _slideshowIndex = 1;
            //display landmark menu
            Menu_Landmark_Main.SetActive(true);

        }


    }
    //exit landmark and go back to map view
    public void Btn_CloseLandmarkMenu()
    {
        currentLandmarkName = "";
        currentLandmark = new Landmark { name = "", data = null };
        _slideshowIndex = 0;
        Menu_Landmark_Main.SetActive(false);
        Menu_Landmark_Slideshow.SetActive(false);
        Menu_Landmark_Interaction.SetActive(false);
    }

    public void Btn_OpenLandmarkSlideshowMenu()
    {
        UpdateLandmarkSlideshowInfo(currentLandmark.data);
        Menu_Landmark_Slideshow.SetActive(true);
        Menu_Landmark_Main.SetActive(false);
    }

    //open interaction menu
    public void Btn_OpenLandmarkInteractionMenu()
    {

    }
    //go to next dialogue/image entry for the landmark
    public void Btn_LandmarkSlideshow_Next()
    {
        _slideshowIndex++;
        UpdateLandmarkSlideshowInfo(currentLandmark.data);
    }
    //go to previous dialogue/image entry for the landmark
    public void Btn_LandmarkSlideshow_Previous()
    {
        _slideshowIndex--;
        UpdateLandmarkSlideshowInfo(currentLandmark.data);
    }

    void UpdateLandmarkMenuInfo(LandmarkData _data)
    {
        Debug.Log("filling landmark info menu for: " + _data.landmarkName);
        text_LandmarkTitle.SetText(_data.landmarkName);
        image_LandmarkImage.sprite = _data.text_dialogues[0].dialogueImage;

    }

    void UpdateLandmarkSlideshowInfo(LandmarkData _data)
    {
        image_Slideshow.sprite = _data.text_dialogues[_slideshowIndex].dialogueImage;
        text_Slideshow.SetText(_data.text_dialogues[_slideshowIndex].text);
        text_PageCounter.SetText((_slideshowIndex).ToString() + "/" + (currentLandmark.data.text_dialogues.Count -1).ToString());

        //buttons visibility
        if (_slideshowIndex < currentLandmark.data.text_dialogues.Count - 1)
        {
            btn_Previous.SetActive(false);
            btn_Next.SetActive(true);
        }
        else if (_slideshowIndex == currentLandmark.data.text_dialogues.Count - 1)
        {
            if (_slideshowIndex>1)
            {
                btn_Previous.SetActive(true);
            }
            else
            {
                btn_Previous.SetActive(false);
            }
            
            btn_Next.SetActive(false);
        }
        else
        {
            btn_Previous.SetActive(true);
            btn_Next.SetActive(true);
        }

    }

    Landmark GetLandmark(string name_Landmark)
    {
        Landmark result = new Landmark { name = "", data = null };
        for (int i = 0; i < Landmarks.Length; i++)
        {
            if (Landmarks[i].name.Equals(name_Landmark))
            {
                currentLandmark = Landmarks[i];

                result = Landmarks[i];
                break;
            }
        }
        return result;
    }

    [System.Serializable]
    public struct Landmark
    {
        public string name;
        public LandmarkData data;
    }

    private void Update()
    {
        //Testing UI flow
        if (Input.GetKeyDown(KeyCode.D))
        {
            Btn_OpenLandmarkMenu("Dom");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Btn_OpenLandmarkMenu("WDR");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Btn_CloseLandmarkMenu();
        }
    }
}
