using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Landmark_Data", menuName = "Landmark/Landmark_Data", order = 1)]
public class LandmarkData : ScriptableObject
{
    public string landmarkName;
    public List<DialogueEntry> text_dialogues;

    //public List<Image> image_dialogues;

    [System.Serializable]
    public struct DialogueEntry
    {
        public string text;
        public Image dialogueImage;
    }

}
