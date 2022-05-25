using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InputRegister : MonoBehaviour
{
    EventSystem system;
    public Selectable firstInput;
    public InputField username;
    public InputField email;
    public InputField password_1;
    public InputField password_2;
    public Button submitButton;
    public Text notification;

    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
        notification.text = "";
    }

    void Update()
    {
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

    public void RegisterPlayer()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            notification.text = "Fill in the username field";
            notification.color = new Color32(215, 148, 24, 255);
        }
        if (string.IsNullOrEmpty(email.text))
        {
            notification.text = "Fill in the email field";
            notification.color = new Color32(215, 148, 24, 255);
        }
        if (string.IsNullOrEmpty(password_1.text))
        {
            notification.text = "Fill in the password field";
            notification.color = new Color32(215, 148, 24, 255);
        }
        if (string.IsNullOrEmpty(password_2.text))
        {
            notification.text = "Fill in the password verification field";
            notification.color = new Color32(215, 148, 24, 255);
        }
        if (checkPassword(password_1.text, password_2.text) == 1)
        {
            StartCoroutine(CreateUser("http://projectether.francecentral.cloudapp.azure.com/auth/users/", username.text, email.text, password_1.text));
        }
        else
        {
            notification.text = "The 2 passwords are not identical";
            notification.color = new Color32(215, 148, 24, 255);
        }
    }

    int checkPassword(string password_1, string password_2)
    {
        if (password_1 == password_2)
            return 1;
        return 0;
    }

    IEnumerator CreateUser(string uri, string username, string email, string password)
    {
        Register register = new Register();
        register.email = email;
        register.username = username;
        register.password = password;
        string jsonData = JsonUtility.ToJson(register);
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
                    notification.text = www.downloadHandler.text;
                    notification.color = new Color32(215, 148, 24, 255);

                    // Reload scene so player can Login
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }

    [System.Serializable]
    public class Register
    {
        public string email;
        public string username;
        public string password;
    }
}
