using UnityEngine;

public static class CassandraFactory
{
    public static Combatant CreateCassandra()
    {
        // Create a new GameObject to represent Cassandra
        GameObject cassandraObject = new GameObject("Cassandra");

        // Add the Combatant component
        Combatant cassandra = cassandraObject.AddComponent<Combatant>();

        // --- Configure Cassandra's Stats based on the design document ---

        // Core Identity
        cassandra.name = "Cassandra Blythe";
        cassandra.karenClass = KarenClass.RepentantKaren;
        // Faction and AIProfile can be set later or assigned a default
        // cassandra.faction = ...;
        // cassandra.aiProfile = ...;

        // Base Stats
        cassandra.level = 1;
        cassandra.experiencePoints = 0;
        cassandra.xpToNextLevel = 100;
        cassandra.maxEmotionalStamina = 100;
        cassandra.currentEmotionalStamina = 100;
        cassandra.maxCredibility = 120; // A bit higher due to her bureaucratic knowledge
        cassandra.currentCredibility = 120;
        cassandra.armor = 0;

        // Dual-Axis Meters
        cassandra.entitlement = 10; // Starts low
        cassandra.maxEntitlement = 100;
        cassandra.insight = 25; // Starts with some self-awareness
        cassandra.maxInsight = 100;

        // Modifiers (default for now)
        cassandra.outgoingDamageModifier = 1f;
        cassandra.incomingDamageModifier = 1f;
        cassandra.incomingHealingModifier = 1.1f; // Good at supporting
        cassandra.credibilityCostModifier = 0;

        // Rhetorical Strengths/Weaknesses (example)
        cassandra.rhetoricalResistances.Add(RhetoricalClass.Manipulation); // Knows it when she sees it
        cassandra.rhetoricalWeaknesses.Add(RhetoricalClass.Vulnerability); // Can be hurt by genuine appeals

        // The verbalLoadout will be populated with her unique abilities
        cassandra.verbalLoadout = CreateCassandraAbilities();

        Debug.Log("Cassandra character created programmatically with her full ability loadout.");

        return cassandra;
    }

    private static System.Collections.Generic.List<VerbalAbility> CreateCassandraAbilities()
    {
        var abilities = new System.Collections.Generic.List<VerbalAbility>();

        // --- Verbal Attacks ---
        var hoaNotice = ScriptableObject.CreateInstance<HOAViolationNoticeAbility>();
        hoaNotice.name = "HOA Violation Notice";
        hoaNotice.description = "According to Subsection 14-B, your outfit is against regulation. Applies 'Confidence Lost' to the target.";
        hoaNotice.cost = 15;
        hoaNotice.rhetoricalClass = RhetoricalClass.Manipulation;
        abilities.Add(hoaNotice);

        var snarkyRetort = ScriptableObject.CreateInstance<SnarkyRetortAbility>();
        snarkyRetort.name = "Snarky Retort";
        snarkyRetort.description = "\"Oh sweetie, bless your heart.\" For 1 turn, counter-attack when damaged.";
        snarkyRetort.cost = 20;
        snarkyRetort.rhetoricalClass = RhetoricalClass.Aggression;
        abilities.Add(snarkyRetort);

        var weaponizedPoliteness = ScriptableObject.CreateInstance<WeaponizedPolitenessAbility>();
        weaponizedPoliteness.name = "Weaponized Politeness";
        weaponizedPoliteness.description = "\"Of course I understand… but you’re still wrong.\" 50% chance to Stun a target with less than 50% morale.";
        weaponizedPoliteness.cost = 25;
        weaponizedPoliteness.rhetoricalClass = RhetoricalClass.Manipulation;
        abilities.Add(weaponizedPoliteness);

        // --- Support / Team Abilities ---
        var neighborhoodWatch = ScriptableObject.CreateInstance<NeighborhoodWatchAbility>();
        neighborhoodWatch.name = "Neighborhood Watch";
        neighborhoodWatch.description = "\"We keep an eye out for each other — and on each other.\" Increases your damage by 20% for 3 turns.";
        neighborhoodWatch.cost = 30;
        neighborhoodWatch.rhetoricalClass = RhetoricalClass.Vulnerability; // Represents community trust
        abilities.Add(neighborhoodWatch);

        var bakeSale = ScriptableObject.CreateInstance<BakeSaleDiplomacyAbility>();
        bakeSale.name = "Bake Sale Diplomacy";
        bakeSale.description = "\"Nothing says ‘peace offering’ like lemon bars.\" Restores 20 morale and removes one debuff from yourself.";
        bakeSale.cost = 25;
        bakeSale.rhetoricalClass = RhetoricalClass.Vulnerability;
        abilities.Add(bakeSale);

        // --- Signature Abilities ---
        var reframe = ScriptableObject.CreateInstance<ReframeAbility>();
        reframe.name = "Reframe";
        reframe.description = "Flip the narrative. The next attack against you is cancelled and the attacker becomes Insecure.";
        reframe.cost = 50;
        reframe.rhetoricalClass = RhetoricalClass.Manipulation;
        abilities.Add(reframe);

        var committeeCoup = ScriptableObject.CreateInstance<CommitteeCoupAbility>();
        committeeCoup.name = "Committee Coup";
        committeeCoup.description = "\"The motion has passed — unanimously.\" Steal all buffs from the target.";
        committeeCoup.cost = 100;
        committeeCoup.rhetoricalClass = RhetoricalClass.Manipulation;
        abilities.Add(committeeCoup);

        return abilities;
    }
}
