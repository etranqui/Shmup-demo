using UnityEngine;
using System.Collections;
using System;


// Adds the ability to create this asset from the Unity Meny
[CreateAssetMenu(menuName = "Abilities/SpeedUpAbility")]

public class SpeedUpAbility : Ability {


    public float speedMultiplier;
    private float speedInit;

    public override void Initialize()
    {

    }

    public override void TriggerAbility()
    {
        Debug.Log("Ability Triggered");
        speedInit = Hero.S.speed;
        Hero.S.speed *= speedMultiplier;
    }

    public override void Deactivate()
    {
        Debug.Log("Ability Deactivated");
        Hero.S.speed = speedInit;
    }

}
