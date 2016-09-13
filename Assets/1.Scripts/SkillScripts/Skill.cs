using UnityEngine;
using System.Collections;


public enum SkillType {
	WeaponSkill,
	MagicSkill,
	Buff,
	Debuff
}

public enum SkillName {
	Foresight,
	InnerBeast,
	Berserk,
	ThrillOfBattle,
}

public enum SkillState {
	Ready,
	Casting,
	OnStartUp,
	Active,
}


[System.Serializable]
public class SkillDefinition {
	public SkillType	type;
	public SkillName	skillName;
	public float		castTime;
	public float		startUpTime;
	public float		activeTime;
	public float		coolDownTime;
}


public class Skill : MonoBehaviour {
	// static parameter attributes (definition)

	public SkillDefinition		def;
	public SkillName			sName;

	// dynamic parameters (executed at runtime)
	public SkillState	state = SkillState.Ready;
	public float		castTimer;
	public float 		activeTimer;
	public float 		coolDownTimer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		switch (state) {
		// If Ready, do nothing
		case SkillState.OnStartUp:
			break;

		// if Casting, update castTimer, State and
		case SkillState.Casting:
			castTimer -= Time.deltaTime;
			if (castTimer < 0) {
				state = SkillState.Active;
			}
			break;
		
		// if Active, update Active timer, state and perform action
		case SkillState.Active:

			activeTimer -= Time.deltaTime;

			// if we're in start up frames, do nothing
			if (activeTimer > def.activeTime) {
				break;
			}
			break;
		}

		// update cooldown timer and state
		coolDownTimer -= Time.deltaTime;
		if (coolDownTimer < 0) {
			state = SkillState.Ready;
		}
	}

	// Activate skill. Will be called from a delegate function
	public void Activate() {

		coolDownTimer = def.coolDownTime;
		activeTimer = def.activeTime + def.startUpTime;
		state = SkillState.Active;

		if (def.type == SkillType.MagicSkill) {
			castTimer = def.castTime;
			state = SkillState.Casting;
		}
	}
}
