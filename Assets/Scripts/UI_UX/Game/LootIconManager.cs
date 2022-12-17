using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LootIconManager : MonoBehaviour, IPointerClickHandler
{
    private string _name = "";
    private Color _initialColor = Color.white;
    [SerializeField] private TMP_Text _quantity;
    [SerializeField] private Image _icon;
    [SerializeField] private UiTooltips _tooltips;

    private void Start() {
        _initialColor = _quantity.color;
    }

    public void SetIcon(Sprite sprite, string quantity, string name)
    {
        _icon.sprite = sprite;
        _quantity.text = quantity;
        _name = name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _tooltips.ShowToolTip(_name);
    }

    public void ChangeTextColor(Color color)
    {
        _quantity.color = color;
    }

    public void ResetColor()
    {
        _quantity.color = _initialColor;
    }

    public void ChangeQuantity(string quantity)
    {
        _quantity.text = quantity;
    }
}
