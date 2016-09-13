using UnityEngine;
using System.Collections;
using System;


// Adds the ability to create this asset from the Unity Meny
[CreateAssetMenu(menuName = "Abilities/FireUpAbility")]

public class FireUpAbility : Ability {

    public float fireRateMultiplier;
    private float fireRateInit;

    public override void Initialize()
    {

    }

    public override void TriggerAbility()
    {
        fireRateInit = Hero.S.weapons[0].def.delayBetweenShots;
        Hero.S.weapons[0].def.delayBetweenShots *= fireRateMultiplier;
    }

    public override void Deactivate()
    {
        Hero.S.weapons[0].def.delayBetweenShots = fireRateInit;
    }
}
