using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class PointOfInterestManagerScript : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference dBReference;
    public FirebaseAuthManager authManager;

    public string myName;
    public string uI_Name;
    public int messageNumber;

    private bool iCalled;

    private MeshRenderer myMesh;

    [SerializeField]
    private Material playerEnteredMaterial;

    public string[] myMessages;

    public GameObject textElement;

    public GameObject textHolder;

    public Camera _mainCamera;

    private Material myBasicMaterial;

    private void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        authManager = GameObject.FindGameObjectWithTag("FirebaseAuthManager").GetComponent<FirebaseAuthManager>();
        AssignMessageComponent(Gamemanager.instance.textHolder);


        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                //InitializeFirebase();
                auth = authManager.auth;
                User = authManager.User;
                dBReference = authManager.dBReference;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void LoadMessages()
    {
        auth = authManager.auth;
        User = authManager.User;
        dBReference = authManager.dBReference;
        StartCoroutine(GetMessages());
    }

    public void WriteMessage(string message)
    {
        StartCoroutine(WriteMessages(message));
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        dBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private IEnumerator GetMessages()
    {

        var DBTask = dBReference.Child("pointOfInterest").Child(myName).Child("messages").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            messageNumber = (int)DBTask.Result.ChildrenCount;

            myMessages = new string[messageNumber];

            for (int i = 0; i < messageNumber; i++)
            {
                myMessages[i] = snapshot.Child(i.ToString()).Value.ToString();
            }

            if (iCalled)
            {
                SpawnMessages();
                iCalled = false;
            }
        }
    }

    private IEnumerator WriteMessages(string message)
    {

        var DBTask = dBReference.Child("pointOfInterest").Child(myName).Child("messages").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {

        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            messageNumber = (int)DBTask.Result.ChildrenCount;

            myMessages = new string[messageNumber];

            string messagePlace = (messageNumber).ToString();

            var DBTaskUpdate = dBReference.Child("pointOfInterest").Child(myName).Child("messages").Child(messagePlace).SetValueAsync(message);

            

            StartCoroutine(GetMessages());

            iCalled = true;
        }
    }

    public void SpawnMessages()
    {
        foreach (Transform child in textHolder.transform)
        {
            Destroy(child.gameObject);
        }

        int i = 0;

        foreach(string newText in myMessages)
        {
            GameObject newTextBox = Instantiate(textElement, textHolder.transform);
            newTextBox.GetComponent<TextLoaderScript>().NewTextElement(myMessages[i]);
            i++;
            Debug.Log(i);
        }
    }

    public void AssignMessageComponent(GameObject _textHolder)
    {
        textHolder = _textHolder;
    }

    private void ClickOnObject()
    {
        if(_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

       
        Ray _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
       // for (int i = 0; i < Input.touchCount; ++i)
        //{
          //  if (Input.GetTouch(i).phase == TouchPhase.Began)
           // {
               // Ray _ray = _mainCamera.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit _hit;
                if (Physics.Raycast(_ray, out _hit, 1000f))
                {
                    if (_hit.transform == transform)
                    {
                        Debug.Log("Clicked On Me");
                        Gamemanager.instance.landmark_Manager = GetComponent<PointOfInterestManagerScript>();
                        Gamemanager.instance.Btn_OpenLandmarkMenu(uI_Name);
                    }
                }
           // }
      //  }
    }
    

    void Start()
    {
        LoadMessages();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickOnObject();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            myMesh.material = playerEnteredMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            myMesh.material = myBasicMaterial;
        }
    }
}
