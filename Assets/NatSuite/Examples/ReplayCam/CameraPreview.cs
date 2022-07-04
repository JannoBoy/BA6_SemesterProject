/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Examples.Components
{

    using UnityEngine;
    using UnityEngine.Android;
    using UnityEngine.UI;
    using System.Collections;

    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public class CameraPreview : MonoBehaviour
    {
        public static CameraPreview instance;
        public WebCamTexture cameraTexture { get; private set; }
        private RawImage rawImage;
        private AspectRatioFitter aspectFitter;
        public WebCamDevice[] devices;
        public string _ChosenDeviceName = "";
        public int deviceIndex = 0;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        void Start()
        {
            rawImage = GetComponent<RawImage>();
            aspectFitter = GetComponent<AspectRatioFitter>();
        }
        IEnumerator Startt()
        {
            rawImage = GetComponent<RawImage>();
            aspectFitter = GetComponent<AspectRatioFitter>();
            // Request camera permission
            if (Application.platform == RuntimePlatform.Android)
            {
                if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
                {
                    Permission.RequestUserPermission(Permission.Camera);
                    yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.Camera));
                }
            }
            else
            {
                yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
                if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
                    yield break;
            }
            // Start the WebCamTexture
            cameraTexture = new WebCamTexture(null, 1920, 1080, 30);
            cameraTexture.Play();
            yield return new WaitUntil(() => cameraTexture.width != 16 && cameraTexture.height != 16); // Workaround for weird bug on macOS
            // Setup preview shader with correct orientation
            rawImage.texture = cameraTexture;
            rawImage.material.SetFloat("_Rotation", cameraTexture.videoRotationAngle * Mathf.PI / 180f);
            //rawImage.material.SetFloat("_Scale", cameraTexture.videoVerticallyMirrored ? -1 : 1);
            Debug.Log(cameraTexture.videoVerticallyMirrored.ToString());
            rawImage.material.SetFloat("_Scale", -1);
            // Scale the preview panel
            if (cameraTexture.videoRotationAngle == 90 || cameraTexture.videoRotationAngle == 270)
                aspectFitter.aspectRatio = (float)cameraTexture.height / cameraTexture.width;
            else
                aspectFitter.aspectRatio = (float)cameraTexture.width / cameraTexture.height;
        }

        public IEnumerator InitWebcam()
        {
            Debug.Log("start initwebcam on webcam script");
            // Request camera permission
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
                yield break;

            devices = WebCamTexture.devices;
            for (int i = 0; i < devices.Length; i++)
            {
                Debug.Log(devices[i].name);
            }

            if (devices.Length > 0)
            {
                deviceIndex = 0;
                _ChosenDeviceName = devices[deviceIndex].name;
            }
            else
            {
                //show message that there are no devices installed
                Debug.Log("No Devices Installed");
            }
            //Debug.Log("finished initwebcam on webcam script");

        }

        public IEnumerator StartWebcam(string _deviceName)
        {
            if (_deviceName == "")
            {
                Debug.Log("empty device name");
                yield break;
            }
            // Start the WebCamTexture
            cameraTexture = new WebCamTexture(_deviceName, 1080, 2340, 30);
            cameraTexture.Play();
            yield return new WaitUntil(() => cameraTexture.width != 16 && cameraTexture.height != 16); // Workaround for weird bug on macOS
            // Setup preview shader with correct orientation
            rawImage.texture = cameraTexture;
            rawImage.material.SetFloat("_Rotation", cameraTexture.videoRotationAngle * Mathf.PI / 180f);
            rawImage.material.SetFloat("_Scale", cameraTexture.videoVerticallyMirrored ? 1 : -1);
            Debug.Log(cameraTexture.videoVerticallyMirrored.ToString());
            // Scale the preview panel
            if (cameraTexture.videoRotationAngle == 90 || cameraTexture.videoRotationAngle == 270)
                aspectFitter.aspectRatio = (float)cameraTexture.height / cameraTexture.width;
            else
                aspectFitter.aspectRatio = (float)cameraTexture.width / cameraTexture.height;
        }

        public void StopWebcam()
        {
            if (cameraTexture != null)
            {
                cameraTexture.Stop();
            }
        }

        public void NextWebcam()
        {
            if (devices.Length <= 1)
            {
                return;
            }
            else
            {
                StopWebcam();
                if (deviceIndex < devices.Length - 1)
                {
                    deviceIndex++;
                }
                else
                {
                    deviceIndex = 0;
                }
                _ChosenDeviceName = devices[deviceIndex].name;
                StartCoroutine(StartWebcam(_ChosenDeviceName));
            }

        }
    }
}