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

    public string myName;
    public int messageNumber;

    private MeshRenderer myMesh;

    [SerializeField]
    private Material playerEnteredMaterial;

    public string[] myMessages;

    private Material myBasicMaterial;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void LoadMessages()
    {

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
        }
    }

    void Start()
    {

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
