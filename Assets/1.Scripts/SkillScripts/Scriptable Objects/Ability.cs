using UnityEngine;
using System.Collections;

public abstract class Ability : ScriptableObject {

    public string aName = "New Ability";
    public Sprite aSprite;
    public AudioClip aSound;
    public float aBaseCoolDown = 2.5f;
    public float aDuration = 20f;

    public abstract void Initialize();
    public abstract void TriggerAbility();
    public abstract void Deactivate();
    
}
