using System.Collections.Generic;

public class LevelData : Singleton<LevelData>
{
    // Script should be reset at the end of a dungeon when getting back to menu ?

    // Dungeon data
    // Environment
    /* Oponent: {
            Id
            name
            UnitsData [{
                position,
                prefab,
                ... ?,
            }]
        }
    */

    // Player data
    public Ability playerDefaultAttack = null;
    public List<Ability> playerAbilities = new List<Ability>(4);
}