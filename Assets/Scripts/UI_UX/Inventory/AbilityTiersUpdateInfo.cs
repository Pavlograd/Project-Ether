using System.Collections.Generic;

public class TierRequirements
{
    public int quantity = 0;
    public ItemData itemData = new ItemData() { id = -1, name = "" };

    public TierRequirements(int _quantity, int _id, string _name)
    {
        this.quantity = _quantity;
        itemData.id = _id;
        itemData.name = _name;
    }
}

static public class AbilityTiersUpdateInfo
{
    static private readonly Dictionary<States, ItemData> _runesDictionary = new Dictionary<States, ItemData>() {
        {States.FIRE, new ItemData() {id = 7, name = "Minor Fire Rune"}},
        {States.ICE, new ItemData() {id = 8, name = "Minor Ice Rune"}},
        {States.ELECTRIC, new ItemData() {id = 6, name = "Minor Electric Rune"}},
        {States.WIND, new ItemData() {id = 9, name = "Minor Wind Rune"}},
        {States.EARTH, new ItemData() {id = 5, name = "Minor Earth Rune"}},
        {States.NEUTRAL, new ItemData() { id = 11, name = "Minor Earth Rune" }},
    };

    static private readonly Dictionary<int, float> _rateDict = new Dictionary<int, float>() {
        {1, .75f},
        {2, .4f},
        {3, .25f},
        {4, .1f}
    };

    static public List<TierRequirements> GetTier(int tier, States state)
    {
        switch (tier) {
            case 1: return GetTier1(state);
            case 2: return GetTier2(state);
            case 3: return GetTier3(state);
            case 4: return GetTier4(state);
            default: return null;
        }
    }

    static public float GetTierRate(int tier)
    {
        return _rateDict.GetValueOrDefault(tier, -1);
    }

    static public List<TierRequirements> GetTier1(States states)
    {
        ItemData rune = _runesDictionary.GetValueOrDefault(states, new ItemData() { id = -1, name = "" });
        return new List<TierRequirements>() {
            new TierRequirements(500, 10, "Gold"),
            new TierRequirements(1, rune.id, rune.name),
            new TierRequirements(25, 4, "Ability dust"),
        };
    }

    static public List<TierRequirements> GetTier2(States states)
    {
        ItemData rune = _runesDictionary.GetValueOrDefault(states, new ItemData() { id = -1, name = "" });
        return new List<TierRequirements>() {
            new TierRequirements(1000, 10, "Gold"),
            new TierRequirements(2, rune.id, rune.name),
            new TierRequirements(50, 4, "Ability dust"),
        };
    }

    static public List<TierRequirements> GetTier3(States states)
    {
        ItemData rune = _runesDictionary.GetValueOrDefault(states, new ItemData() { id = -1, name = "" });
        return new List<TierRequirements>() {
            new TierRequirements(1750, 10, "Gold"),
            new TierRequirements(4, rune.id, rune.name),
            new TierRequirements(100, 4, "Ability dust"),
        };
    }

    static public List<TierRequirements> GetTier4(States states)
    {
        ItemData rune = _runesDictionary.GetValueOrDefault(states, new ItemData() { id = -1, name = "" });
        return new List<TierRequirements>() {
            new TierRequirements(3000, 10, "Gold"),
            new TierRequirements(8, rune.id, rune.name),
            new TierRequirements(200, 4, "Ability dust"),
        };
    }
};