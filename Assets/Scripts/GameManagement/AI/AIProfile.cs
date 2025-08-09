using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class AIProfile : ScriptableObject
{
    // Returns a tuple of the chosen ability and its intended target.
    public abstract Tuple<VerbalAbility, Combatant> ChooseAbility(Combatant self, List<Combatant> allies, List<Combatant> enemies, List<VerbalAbility> abilities, Dictionary<VerbalAbility, int> cooldowns);
}
