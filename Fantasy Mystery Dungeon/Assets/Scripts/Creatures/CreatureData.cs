using UnityEngine;

[CreateAssetMenu(fileName = "NewCreatureData", menuName = "ScriptableObjects/CreatureData", order = 1)]
public class CreatureData : ScriptableObject
{
    private const string defaultName = "unnamed";
    public string creatureName = defaultName;
    public CreatureType creatureType = CreatureType.Human;
    
    public int baseHealth = 3;
    public int baseAttack = 3;
    public int baseDefense = 3;
    //All base stats should add to 8 for monsters and 10 for characters
    public int statTotal
    {
        get
        {
            return baseHealth + baseAttack + baseDefense;
        }
    }

    private void OnValidate()
    {
        creatureName = creatureName.Trim();
        if(creatureName == "")
        {
            Debug.LogWarning("No name set, defaulting to '" + defaultName + "'.", this);
            creatureName = defaultName;
        }
        
        baseHealth  = Mathf.Clamp(baseHealth,  1, 5);
        baseAttack  = Mathf.Clamp(baseAttack,  1, 5);
        baseDefense = Mathf.Clamp(baseDefense, 1, 5);

        if(statTotal < 8)
        {
            Debug.LogWarning("All stats for " + ToString() + " should add up to at least 8.", this);
        }
    }

    public override string ToString()
    {
        return "'" + creatureName + "' Data";
    }
}
