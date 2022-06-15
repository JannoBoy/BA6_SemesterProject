using UnityEngine;
using System.Collections;

public class Capture : MonoBehaviour
{
    public static Capture instance;
    public Camera myCamera;
    public string designatedPath;
    private bool takeScreenShot = false;
    public RenderTexture rt;
    public Texture2D screenShot;

    public string ScreenShotName(int width, int height)
    {
        //return string.Format("{0}/screen_{1}x{2}_{3}.png",
        //                     Application.dataPath,
        //                     width, height,
        //                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        return string.Format("{0}/screen_{1}x{2}_{3}.jpg",
                             designatedPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
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

    public void TakeScreenShot()
    {
        takeScreenShot = true;
    }

    void LateUpdate()
    {
        if (takeScreenShot)
        {
            screenShot = ScreenCapture.CaptureScreenshotAsTexture(1);
            Debug.Log("capturing");

            byte[] bytes = screenShot.EncodeToJPG(100);
            string filename = ScreenShotName(Screen.width, Screen.height);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            //here we should set the latest file path incase we wish to open the last screenshot taken
            takeScreenShot = false;

        }
    }


    public Texture2D rotateTexture(Texture2D image)
    {

        Texture2D target = new Texture2D(image.height, image.width, image.format, false);    //flip image width<>height, as we rotated the image, it might be a rect. not a square image

        Color32[] pixels = image.GetPixels32(0);
        pixels = rotateTextureGrid(pixels, image.width, image.height);
        target.SetPixels32(pixels);
        target.Apply();

        return target;
    }

    public Color32[] rotateTextureGrid(Color32[] tex, int wid, int hi)
    {
        Color32[] ret = new Color32[wid * hi];      //reminder we are flipping these in the target

        for (int y = 0; y < hi; y++)
        {
            for (int x = 0; x < wid; x++)
            {
                ret[(hi - 1) - y + x * hi] = tex[x + y * wid];         //juggle the pixels around

            }
        }

        return ret;
    }
}