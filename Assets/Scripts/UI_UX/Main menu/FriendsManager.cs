using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendsManager : MonoBehaviour
{
    private API_Users _users;
    private API_FriendsList _friendsList;
    [SerializeField] private TMP_InputField _field;
    [SerializeField] private GameObject _friendPrefab;
    [SerializeField] private GameObject _friendsContent;
    [SerializeField] private RectTransform _rt;
    [SerializeField] private Scrollbar _sb;

    public void InitializingFriends()
    {
        FetchUserList();
        _friendsList = API.AddRemoveFriend("");
        RefreshFriends();
        // _rt = _friendsContent.GetComponent<RectTransform>();
    }

    private void FetchUserList()
    {
        _users = API.GetUsers();
    }

    public void AddRemoveFriend()
    {
        foreach (API_User user in _users.users)
            if (user.username == _field.text)
            {
                _friendsList = API.AddRemoveFriend(user.id);
            }

        RefreshFriends();
    }

    public void RefreshFriends()
    {
        EmptyList();
        ResetScrollPosition();
        
        _rt.sizeDelta = new Vector2(0f, 247.94f);
        int i = 0;
        if (_friendsList.friends_username.Count >= 1 && _friendsList.friends_username[0] != "")
        {
            foreach (string username in _friendsList.friends_username)
            {
                Vector3 pos = new Vector3(0f, 0f, 0f);
                pos.x = pos.x + 175 * i;
                _rt.sizeDelta = new Vector2(0, 150 * i);

                i++;

                GameObject friend = Instantiate(_friendPrefab, pos, transform.rotation);
                friend.transform.SetParent(GameObject.FindGameObjectWithTag("FriendItemList").transform, false);
                friend.SetActive(true);
                TMP_Text itemID = friend.transform.Find("ID").GetComponent<TMP_Text>();
                itemID.SetText(username);
            }
        }
    }

    private void EmptyList()
    {
        foreach (Transform child in _friendsContent.transform)
            Destroy(child.gameObject);
    }

    private void ResetScrollPosition()
    {
        _sb.value = 1;
    }
}