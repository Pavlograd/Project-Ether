using System.Collections.Generic;

public struct ItemData
{
    public int id;
    public string name;
}

public enum Items {
    Cobalt,
    Iron,
    Bronze,
    Souls,
    AbiliyDust,
    MinorFireRune,
    MinorEarthRune,
    MinorIceRune,
    MinorElectricRune,
    MinorWindRune,
    MinorNeutralRune,
    Coin,
}

static public class ItemsDictionary
{
    static private readonly Dictionary<Items, ItemData> _items = new Dictionary<Items, ItemData>() {
        {Items.Iron, new ItemData() { id = 0, name = "Iron" }},
        {Items.Cobalt, new ItemData() { id = 1, name = "Cobalt" }},
        {Items.Bronze, new ItemData() { id = 2, name = "Bronze" }},
        {Items.Souls, new ItemData() { id = 3, name = "Souls" }},
        {Items.AbiliyDust, new ItemData() { id = 4, name = "Ability dust" }},
        {Items.MinorEarthRune, new ItemData() { id = 5, name = "Minor Earth Rune" }},
        {Items.MinorElectricRune, new ItemData() { id = 6, name = "Minor Electric Rune" }},
        {Items.MinorFireRune, new ItemData() { id = 7, name = "Minor Fire Rune" }},
        {Items.MinorIceRune, new ItemData() { id = 8, name = "Minor Ice Rune" }},
        {Items.MinorWindRune, new ItemData() { id = 9, name = "Minor Wind Rune" }},
        {Items.Coin, new ItemData() { id = 10, name = "Coin" }},
        {Items.MinorNeutralRune, new ItemData() { id = 11, name = "Minor Neutral Rune" }},
    };

    static public bool TryGetItem(Items item, out ItemData data)
    {
        return _items.TryGetValue(item, out data);
    }

    static public string GetNameById(int id)
    {
        foreach (KeyValuePair<Items, ItemData> key in _items) {
            if (key.Value.id == id) {
                return key.Value.name;
            }
        }
        return "";
    }
}
