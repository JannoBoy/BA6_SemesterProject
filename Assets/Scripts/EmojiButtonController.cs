using UnityEngine;
using TMPro;

public class EmojiButtonController : MonoBehaviour
{
    public TMP_Text previeTxt;

    public void EmojiButton(string myEmoji)
    {
        previeTxt.text = previeTxt.text + myEmoji;
    }
        
}
