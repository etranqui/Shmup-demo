using UnityEngine;
using System.Collections;
using System;

// Adds the ability to create this asset from the Unity Meny
[CreateAssetMenu(menuName = "Abilities/PowerUpAbility")]


public class PowerUpAbility : Ability {

    public int weaponNumber;        // WeaponNumber can only be 2 or 4
    public WeaponType[] weaponsInit;

    public override void Initialize()
    {
        weaponsInit = new WeaponType[5];

        for (int i = 0; i < 5; i++)
            weaponsInit[i] = WeaponType.none;
    }

    public override void TriggerAbility()
    {
        weaponsInit[0] = Hero.S.weapons[0].type;

        for (int i = 1; i <= weaponNumber; i++)
        {
            weaponsInit[i] = Hero.S.weapons[i].type;
            Hero.S.weapons[i].type = Hero.S.weapons[0].type;
        }
    }

    public override void Deactivate()
    {
        for (int i = 0; i < 5; i++)
            Hero.S.weapons[i].type = weaponsInit[i];
    }
}

