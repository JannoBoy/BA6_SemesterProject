using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class PlayerDataLoaderScript : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference dBReference;
    public FirebaseAuthManager authManager;


    private IEnumerator LoadUserUnlocks()
    {
        Debug.Log(User.UserId);

        var DBTask = dBReference.Child("users").Child(User.UserId).Child("unlocks").Child("0").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        DataSnapshot snapshot = DBTask.Result;

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            if(snapshot.Value == null)
            {
                StartCoroutine(CreateNewUserDatabase());
            }
        }
    }

    public void CreateUserDataBase()
    {
        auth = authManager.auth;
        User = authManager.User;
        dBReference = authManager.dBReference;
        StartCoroutine(LoadUserUnlocks());
    }

    private IEnumerator CheckForNewUserDatabase()
    {
        var DBTask = dBReference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        DataSnapshot snapshot = DBTask.Result;

        if (snapshot.Value == null)
        {

        }
        else
        {
            Debug.Log("Is not a new User");
        }
    }

    private IEnumerator CreateNewUserDatabase()
    {
        var DBTask = dBReference.Child("users").Child(User.UserId).Child("unlocks").Child("0").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        DataSnapshot snapshot = DBTask.Result;

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            var DBTaskSet = dBReference.Child("users").Child(User.UserId).Child("unlocks").Child("0").SetValueAsync(false);
            var DBTaskSetTwo = dBReference.Child("users").Child(User.UserId).Child("unlocks").Child("1").SetValueAsync(false);
            var DBTaskSetThree = dBReference.Child("users").Child(User.UserId).Child("unlocks").Child("2").SetValueAsync(false);

            yield return new WaitUntil(predicate: () => DBTaskSetThree.IsCompleted);
        }
    }

        void Start()

    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
