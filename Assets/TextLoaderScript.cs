using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLoaderScript : MonoBehaviour
{
    public TMP_Text myText;

    public void NewTextElement(string text)
    {
        myText.text = text;
    }
}
