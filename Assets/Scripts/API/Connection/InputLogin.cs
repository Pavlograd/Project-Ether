using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InputLogin : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;
    EventSystem system;
    public Selectable firstInput;
    public InputField login;
    public InputField password;
    public Button submitButton;
    public Text notification;
    public string userToken;

    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
        notification.text = "";
    }

    void Update()
    {
        // Only for PC and tests need to change
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (previous != null)
            {
                previous.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
    }

    public void Login()
    {
        if (string.IsNullOrEmpty(login.text))
        {
            return;
        }
        if (string.IsNullOrEmpty(password.text))
        {
            return;
        }
        StartCoroutine(GetAutologin("http://projectether.francecentral.cloudapp.azure.com/api/token-auth/", login.text, password.text));
    }

    IEnumerator GetAutologin(string uri, string username, string password)
    {
        Account account = new Account();
        account.username = username;
        account.password = password;
        string jsonData = JsonUtility.ToJson(account);
        using (UnityWebRequest www = UnityWebRequest.Post(uri, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
                notification.text = www.error;
                notification.color = new Color32(234, 80, 22, 255);
            }
            else
            {
                if (www.isDone)
                {
                    Debug.Log(www.downloadHandler.text);
                    var user = JsonUtility.FromJson<API_Token>(www.downloadHandler.text);
                    userToken = user.token;

                    Debug.Log(www.downloadHandler.text);

                    if (!string.IsNullOrEmpty(user.token))
                    {
                        Debug.Log("Connected");

                        // Save token in a file
                        Token.SaveToken(user.token);
                        Token.CreateSave();

                        // redirect to main menu
                        levelLoader.LoadLevel("Main Menu");

                        notification.text = "You are connected: " + this.userToken + " " + Token.GetToken();
                        notification.color = new Color32(117, 215, 24, 255);
                    }
                    else
                    {
                        Debug.Log("Bad login/password");
                        notification.text = "Error: Bad login/password";
                        notification.color = new Color32(234, 80, 22, 255);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class Account
    {
        public string username;
        public string password;
    }
}
