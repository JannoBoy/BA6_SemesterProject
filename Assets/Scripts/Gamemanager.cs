using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NatSuite.Examples.Components;
using System;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    [Header("Settings")]
    //public GameObject Menu_MainMenu;
    public Camera camera_Map;
    public Camera camera_Selfie;
    public GameObject Canvas_Selfie;
    public GameObject Canvas_Login;
    public GameObject Canvas_Landmarks;
    public GameObject Camera_CaptureEffect;
    public GameObject Camera_Buttons;
    public GameObject Map_Controls;
    public string UI_Sound_Plobb, UI_Sound_Sht;

    [Header("Landmark Main")]
    public GameObject Menu_Landmark_Main;
    public TMP_Text text_LandmarkTitle;
    public Image image_LandmarkImage;

    [Header("Landmark Slideshow")]
    public GameObject Menu_Landmark_Slideshow;
    public Image image_Slideshow_Frogo;
    public Image image_Slideshow;
    public TMP_Text text_smallDialogue;
    public TMP_Text text_bigDialogue;
    public GameObject btn_Next;
    public GameObject btn_Previous;
    public GameObject btn_InteractionMenu;
    public TMP_Text text_PageCounter;

    [Header("Landmark BigDialogue")]
    public GameObject Container_Bigdialogue;
    public GameObject Container_Smalldialogue;
    //public GameObject btn_Next;
    //public GameObject btn_Previous;
    //public GameObject btn_InteractionMenu;
    //public TMP_Text text_PageCounter;

    [Header("Landmark Interaction")]
    public GameObject Menu_Landmark_Interaction;
    public Image image_Frogo;

    public Landmark[] Landmarks;

    [Header("Message Interaction")]
    public PointOfInterestManagerScript landmark_Manager;
    public GameObject textHolder;
    public TMP_Text txt_preview;

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
        //camera_Map.enabled = true;
        //camera_Depth.enabled = true;

        //Menu_MainMenu.SetActive(false);

        //StartCoroutine(InitWebcam());
    }

    public void Btn_StartCamera()
    {
        SoundManager.instance.Play(UI_Sound_Plobb);
        StartCoroutine(InitWebcam());
    }
    

    public void Btn_TakeScreenshot()
    {
        SoundManager.instance.Play(UI_Sound_Plobb);
        StartCoroutine(CaptureProcess());
       
    }

    public void Btn_SendMessage()
    {
        string sendText = txt_preview.text;
        landmark_Manager.WriteMessage(sendText);
    }

    IEnumerator CaptureProcess()
    {
        //disable the camera UI buttons and enable capture effect
        Camera_Buttons.SetActive(false);
        //capture effect
        yield return StartCoroutine(CaptureEffect());
        Capture.instance.TakeScreenShot();
        Camera_Buttons.SetActive(true);
    }

    IEnumerator CaptureEffect()
    {

        Camera_CaptureEffect.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        Camera_CaptureEffect.SetActive(false);
        yield return new WaitForSeconds(0.1f);

    }

    public void Btn_CloseCamera()
    {
        SoundManager.instance.Play(UI_Sound_Sht);
        DisableSelfiePreview();
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
        camera_Selfie.enabled = true;

        Canvas_Login.SetActive(false);
        Canvas_Landmarks.SetActive(false);
        camera_Map.enabled = false;
        yield return null;
    }

    void DisableSelfiePreview()
    {
        CameraPreview.instance.StopWebcam();
        camera_Map.enabled = true;
        Canvas_Landmarks.SetActive(true);

        camera_Selfie.enabled = false;
        Canvas_Selfie.SetActive(false);
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
            SoundManager.instance.Play(UI_Sound_Plobb);
            //set current landmark shared variables
            currentLandmark = myLandmark;
            currentLandmarkName = name_Landmark;
            //fill landmark title and image
            UpdateLandmarkMenuInfo(currentLandmark.data);
            //initialize index for slideshow
            _slideshowIndex = 1;
            //display landmark menu
            Menu_Landmark_Main.SetActive(true);
            Map_Controls.SetActive(false);
        }


    }
    //exit landmark and go back to map view
    public void Btn_CloseLandmarkMenu()
    {
        SoundManager.instance.Play(UI_Sound_Sht);
        currentLandmarkName = "";
        currentLandmark = new Landmark { name = "", data = null };
        _slideshowIndex = 0;
        Menu_Landmark_Main.SetActive(false);
        Menu_Landmark_Slideshow.SetActive(false);
        Menu_Landmark_Interaction.SetActive(false);
        Map_Controls.SetActive(true);
    }

    public void Btn_OpenLandmarkSlideshowMenu()
    {
        SoundManager.instance.Play(UI_Sound_Plobb);
        UpdateLandmarkSlideshowInfo(currentLandmark.data);
        Menu_Landmark_Slideshow.SetActive(true);
        Menu_Landmark_Main.SetActive(false);
    }

    //open interaction menu
    public void Btn_OpenLandmarkInteractionMenu()
    {
        SoundManager.instance.Play(UI_Sound_Plobb);
        Menu_Landmark_Interaction.SetActive(true);
        Menu_Landmark_Slideshow.SetActive(false);
    }
    //go to next dialogue/image entry for the landmark
    public void Btn_LandmarkSlideshow_Next()
    {
        SoundManager.instance.Play(UI_Sound_Sht);
        _slideshowIndex++;
        UpdateLandmarkSlideshowInfo(currentLandmark.data);
    }
    //go to previous dialogue/image entry for the landmark
    public void Btn_LandmarkSlideshow_Previous()
    {
        SoundManager.instance.Play(UI_Sound_Sht);
        _slideshowIndex--;
        UpdateLandmarkSlideshowInfo(currentLandmark.data);
    }

    public void Btn_NextCamera()
    {
        SoundManager.instance.Play(UI_Sound_Plobb);
        CameraPreview.instance.NextWebcam();
    }

    public void Btn_Messages()
    {
        SoundManager.instance.Play(UI_Sound_Plobb);
        landmark_Manager.SpawnMessages();
    }

    void UpdateLandmarkMenuInfo(LandmarkData _data)
    {
        Debug.Log("filling landmark info menu for: " + _data.landmarkName);
        text_LandmarkTitle.SetText(_data.landmarkName);
        image_LandmarkImage.sprite = _data.text_dialogues[0].dialogueImage;

    }

    void UpdateLandmarkSlideshowInfo(LandmarkData _data)
    {
        if (_data.text_dialogues[_slideshowIndex].dialogueImage != null)
        {
            Container_Bigdialogue.SetActive(false);
            image_Slideshow.sprite = _data.text_dialogues[_slideshowIndex].dialogueImage;
            text_smallDialogue.SetText(_data.text_dialogues[_slideshowIndex].text);
            image_Slideshow.gameObject.SetActive(true);
            Container_Smalldialogue.SetActive(true);
        }
        else
        {
            Container_Smalldialogue.SetActive(false);
            image_Slideshow.gameObject.SetActive(false);
            text_bigDialogue.SetText(_data.text_dialogues[_slideshowIndex].text);
            Container_Bigdialogue.SetActive(true);

        }

        text_PageCounter.SetText((_slideshowIndex).ToString() + "/" + (currentLandmark.data.text_dialogues.Count - 1).ToString());

        //buttons visibility
        //if index >1
        //if at end
        //else only one page
        //at end
        if (_slideshowIndex == currentLandmark.data.text_dialogues.Count - 1)
        {
            //if more than one page
            if (_slideshowIndex > 1)
            {
                btn_Previous.SetActive(true);
            }
            //only one page
            else
            {
                btn_Previous.SetActive(false);
            }
            btn_Next.SetActive(false);
            btn_InteractionMenu.SetActive(true);
        }
        else
        {
            if (_slideshowIndex == 1)
            {
                btn_Previous.SetActive(false);
                btn_Next.SetActive(true);
                btn_InteractionMenu.SetActive(false);
            }
            else
            {
                btn_Previous.SetActive(true);
                btn_Next.SetActive(true);
                btn_InteractionMenu.SetActive(false);
            }
        }

        //if (_slideshowIndex < currentLandmark.data.text_dialogues.Count - 1)
        //{
        //    btn_Previous.SetActive(false);
        //    btn_Next.SetActive(true);
        //    btn_InteractionMenu.SetActive(false);
        //}
        //else if (_slideshowIndex == currentLandmark.data.text_dialogues.Count - 1)
        //{
        //    if (_slideshowIndex > 1)
        //    {
        //        btn_Previous.SetActive(true);
        //    }
        //    else
        //    {
        //        btn_Previous.SetActive(false);
        //    }

        //    btn_Next.SetActive(false);
        //    btn_InteractionMenu.SetActive(true);
        //}
        //else
        //{
        //    btn_Previous.SetActive(true);
        //    btn_Next.SetActive(true);
        //    btn_InteractionMenu.SetActive(false);
        //}

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

    }
}
