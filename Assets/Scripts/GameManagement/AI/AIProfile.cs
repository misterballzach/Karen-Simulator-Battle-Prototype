using UnityEngine;
using System.Collections.Generic;

public abstract class AIProfile : ScriptableObject
{
    public abstract VerbalAbility ChooseAbility(Combatant self, Combatant target, List<VerbalAbility> abilities, Dictionary<VerbalAbility, int> cooldowns);
}
