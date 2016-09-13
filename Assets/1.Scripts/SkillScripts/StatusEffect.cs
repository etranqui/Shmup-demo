using UnityEngine;
using System.Collections;

public class StatusEffect : MonoBehaviour {

    [SerializeField]
    public CoolDownType _type;
    public float activeTimer;

	// Use this for initialization
	void Start () {
        // Set Active Timer
        // TODO: Retrieve value from CD definition
        activeTimer = Main.getCoolDownDefinition(_type).activeTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (activeTimer > 0)
            activeTimer -= Time.deltaTime;

        if (activeTimer < 0)
        {
            activeTimer = 0;
            Destroy(this.gameObject);
        }
	}
}
