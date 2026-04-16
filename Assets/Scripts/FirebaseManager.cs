using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    [Header("Login UI")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;

    [Header("Signup UI")]
    public TMP_InputField signupUsername;
    public TMP_InputField signupEmail;
    public TMP_InputField signupPassword;

    [Header("Status UI")]
    public TMP_Text statusText;

    private FirebaseAuth auth;
    private DatabaseReference dbRef;
    private bool firebaseReady = false;
    private FirebaseUser currentUser;

    private const string DATABASE_URL = "https://echosight-6d3ce-default-rtdb.europe-west1.firebasedatabase.app/";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            DependencyStatus dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;

                // Use the database URL directly
                dbRef = FirebaseDatabase.GetInstance(DATABASE_URL).GetReference("Users");

                firebaseReady = true;
                SetStatus("Firebase Ready");
                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                firebaseReady = false;
                SetStatus("Firebase Error: " + dependencyStatus);
                Debug.LogError("Could not resolve Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void SignUp()
    {
        if (!firebaseReady)
        {
            SetStatus("Firebase not ready");
            return;
        }

        string username = signupUsername.text.Trim();
        string email = signupEmail.text.Trim();
        string password = signupPassword.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            SetStatus("Please fill all signup fields");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                SetStatus("Signup failed");
                Debug.LogError(task.Exception);
                return;
            }

            currentUser = task.Result.User;

            UserData userData = new UserData(username, email, 0);
            string json = JsonUtility.ToJson(userData);

            dbRef.Child(currentUser.UserId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(saveTask =>
            {
                if (saveTask.IsCanceled || saveTask.IsFaulted)
                {
                    SetStatus("Signup succeeded but save failed");
                    Debug.LogError(saveTask.Exception);
                    return;
                }

                SetStatus("Signup Success");
                Debug.Log("User saved to Realtime Database.");
            });
        });
    }

    public void Login()
    {
        if (!firebaseReady)
        {
            SetStatus("Firebase not ready");
            return;
        }

        string email = loginEmail.text.Trim();
        string password = loginPassword.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            SetStatus("Please fill login fields");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                SetStatus("Login failed");
                Debug.LogError(task.Exception);
                return;
            }

            currentUser = task.Result.User;
            SetStatus("Login Success");
            LoadUserData();
        });
    }

    public void Logout()
    {
        if (!firebaseReady)
            return;

        auth.SignOut();
        currentUser = null;
        SetStatus("Logged Out");
    }

    public void LoadUserData()
    {
        if (!firebaseReady || currentUser == null)
            return;

        dbRef.Child(currentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                SetStatus("Failed to load user data");
                Debug.LogError(task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                string username = snapshot.Child("username").Value?.ToString();
                string email = snapshot.Child("email").Value?.ToString();
                string highScore = snapshot.Child("highScore").Value?.ToString();

                Debug.Log("Username: " + username);
                Debug.Log("Email: " + email);
                Debug.Log("High Score: " + highScore);

                SetStatus("Welcome " + username);
            }
            else
            {
                SetStatus("No user data found");
            }
        });
    }

    public void SaveHighScore(int newScore)
    {
        if (!firebaseReady || currentUser == null)
            return;

        dbRef.Child(currentUser.UserId).Child("highScore").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Failed to read high score: " + task.Exception);
                return;
            }

            int existingHighScore = 0;
            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists && snapshot.Value != null)
            {
                int.TryParse(snapshot.Value.ToString(), out existingHighScore);
            }

            if (newScore > existingHighScore)
            {
                dbRef.Child(currentUser.UserId).Child("highScore").SetValueAsync(newScore).ContinueWithOnMainThread(saveTask =>
                {
                    if (saveTask.IsCanceled || saveTask.IsFaulted)
                    {
                        Debug.LogError("Failed to save high score: " + saveTask.Exception);
                        return;
                    }

                    Debug.Log("New high score saved: " + newScore);
                });
            }
            else
            {
                Debug.Log("Score not higher than saved high score");
            }
        });
    }

    public bool IsLoggedIn()
    {
        return currentUser != null;
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;

        Debug.Log(message);
    }
}