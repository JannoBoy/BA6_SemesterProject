using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NatSuite.Examples.Components;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    [Header("Settings")]
    public GameObject Menu_MainMenu;
    public Camera camera_Map, camera_Depth;
    public GameObject SelfiePreview;
    public GameObject Canvas_Selfie;
    public GameObject camera_Selfie;
    public GameObject Canvas_Landmarks;
    public GameObject Canvas_Debug;

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
    public GameObject btn_InteractionMenu;
    public TMP_Text text_PageCounter;

    [Header("Landmark Interaction")]
    public GameObject Menu_Landmark_Interaction;
    public Image image_Frogo;

    public Landmark[] Landmarks;

    [Header("Camera settings")]

    string currentLandmarkName;
    Landmark currentLandmark;

    public int _slideshowIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        camera_Map.enabled = true;
        camera_Depth.enabled = true;

        Menu_MainMenu.SetActive(false);

        StartCoroutine(InitWebcam());
    }

    IEnumerator InitWebcam()
    {
        yield return StartCoroutine(EnableSelfiePreview());
        yield return StartCoroutine(CameraPreview.instance.InitWebcam());
        yield return StartCoroutine(CameraPreview.instance.StartWebcam(CameraPreview.instance._ChosenDeviceName));

    }

    IEnumerator EnableSelfiePreview()
    {
        Canvas_Selfie.SetActive(true);
        //SelfiePreview.SetActive(true);
        Canvas_Landmarks.SetActive(false);
        Canvas_Debug.SetActive(false);
        camera_Map.enabled = false;
        camera_Depth.enabled = false;
        yield return null;
    }

    public void DisableSelfiePreview()
    {
        camera_Map.enabled = true;
        camera_Depth.enabled = true;
        Canvas_Landmarks.SetActive(true);
        Canvas_Debug.SetActive(true);
        Canvas_Selfie.SetActive(false);
        //SelfiePreview.SetActive(false);
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
        Menu_Landmark_Interaction.SetActive(true);
        Menu_Landmark_Slideshow.SetActive(false);
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

    public void Btn_NextCamera()
    {
        CameraPreview.instance.NextWebcam();
    }

    public void Btn_CloseCameraPreview()
    {

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
        text_PageCounter.SetText((_slideshowIndex).ToString() + "/" + (currentLandmark.data.text_dialogues.Count - 1).ToString());

        //buttons visibility
        if (_slideshowIndex < currentLandmark.data.text_dialogues.Count - 1)
        {
            btn_Previous.SetActive(false);
            btn_Next.SetActive(true);
        }
        else if (_slideshowIndex == currentLandmark.data.text_dialogues.Count - 1)
        {
            if (_slideshowIndex > 1)
            {
                btn_Previous.SetActive(true);
            }
            else
            {
                btn_Previous.SetActive(false);
            }

            btn_Next.SetActive(false);
            btn_InteractionMenu.SetActive(true);
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
        if (Input.GetKeyDown(KeyCode.N))
        {
            CameraPreview.instance.NextWebcam();
        }
    }
}
