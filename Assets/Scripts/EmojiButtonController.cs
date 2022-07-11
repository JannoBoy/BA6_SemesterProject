using UnityEngine;
using TMPro;

public class EmojiButtonController : MonoBehaviour
{
    public TMP_Text previeTxt;

    public void EmojiButton(string myEmoji)
    {
        if(previeTxt.text.Length + myEmoji.Length < 70)
        previeTxt.text = previeTxt.text + myEmoji;
    }

    public void EmojiButtonClear()
    {
        previeTxt.text = "";
    }

}
