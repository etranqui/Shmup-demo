using UnityEngine;
using System.Collections;
using System.Collections.Generic;		// Required to use Dictionary<>

public class SkillBook : MonoBehaviour {


	static public SkillBook 								S; 			// Singleton
	static public Dictionary<SkillName, SkillDefinition>	SKILL_DEF;	// Skill definition
	public SkillDefinition[]		skillDefinitions;


	void Awake() {

		S = this;

		SKILL_DEF = new Dictionary<SkillName, SkillDefinition> ();

		foreach (SkillDefinition def in skillDefinitions) {
			SKILL_DEF[def.skillName] = def;
		}

		
	}

}
