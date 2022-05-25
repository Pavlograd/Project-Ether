using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GenerationData", menuName = "GenerationData/DonjonGenerator")]
public class DonjonGeneratorData : ScriptableObject
{
    public GenerationRoom[] _randomSizeRooms;
    public TexturePack[] _texturesPacks;
    public GameObject portal;
    public GenerationMob[] mobs;
    public GenerationTrap[] traps;
    public Pattern[] _patterns;
    public GameObject boss;
    public List<Ability> _abilitiesAvailableData;// TEMPORAIRE : en attente de la BDD pour stocker les abilities que le joueur peut �quiper
}
