using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InputReset : MonoBehaviour
{
    EventSystem system;
    public Selectable firstInput;
    public InputField email;
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
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift)) {
            Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (previous != null) {
                previous.Select();
            }
        } else if (Input.GetKeyDown(KeyCode.Tab)) {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null) {
                next.Select();
            }
        } else if (Input.GetKeyDown(KeyCode.Return)) {
            if (string.IsNullOrEmpty(email.text)) {
                return;
            }
            submitButton.onClick.Invoke();
            StartCoroutine(resetPassword("http://projectether.francecentral.cloudapp.azure.com/auth/users/reset_password/", email.text));
        }
    }

    IEnumerator resetPassword(string uri, string email)
    {
        Reset reset = new Reset();
        reset.email = email;
        string jsonData = JsonUtility.ToJson(reset);
        using (UnityWebRequest www = UnityWebRequest.Post(uri, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log(www.error);
                notification.text = www.error;
                notification.color = new Color32(234, 80, 22, 255);
            } else {
                if (www.isDone) {
                    Debug.Log(www.downloadHandler.text);
                    Debug.Log("Email to reset password sent");
                    notification.text = "Email to reset password sent";
                    notification.color = new Color32(117, 215, 24, 255);
                }
            }
        }
    }

    [System.Serializable]
    public class Reset
    {
        public string email;
    }
}
