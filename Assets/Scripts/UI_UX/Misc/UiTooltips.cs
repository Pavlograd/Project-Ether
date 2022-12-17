using UnityEngine;
using TMPro;

public class UiTooltips : MonoBehaviour
{
    private TMP_Text _textObject = null;

    private void Start() {
        _textObject = transform.GetChild(0).GetComponent<TMP_Text>();
        gameObject.SetActive(false);
    }

    public void ShowToolTip(string itemName)
    {
        if (!gameObject.activeSelf) {
            itemName = itemName.Replace("Template(Clone)", "").Trim();
            _textObject.text = itemName;
            RectTransform rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(
                _textObject.preferredWidth + 15,
                rect.sizeDelta.y
            );
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
}
