using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Consequence List", menuName = "KAREN/Consequences/Consequence List")]
public class ConsequenceList : ScriptableObject
{
    public List<Consequence> consequences;

    public Consequence GetRandomConsequence()
    {
        if (consequences == null || consequences.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, consequences.Count);
        return consequences[randomIndex];
    }
}
