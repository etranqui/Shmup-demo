using UnityEngine;
using System.Collections;



public enum CoolDownType
{
    none,           // Default no weapon
    speed_up,        // increase speed by 20%
    fire_up,         // increate firing rate by 20%
}


[System.Serializable]
public class CoolDownDefinition
{
    public CoolDownType type;
    public string name;
    public GameObject coolDownPrefab;
    public float activeTime;
    public float recastTime = 15;     // Default Global CD
}



public class Cooldown : MonoBehaviour {

    public CoolDownType _type;

    public CoolDownDefinition def;
    public float coolDownRecastTimer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // Update Recast Timer
        if (coolDownRecastTimer > 0)
        {
            coolDownRecastTimer -= Time.deltaTime;
        }

        if (coolDownRecastTimer < 0)
        {
            coolDownRecastTimer = 0;
        }
    }

    public void SetCDType(CoolDownType cdt)
    {
        _type = cdt;

        if (_type == CoolDownType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else {
            this.gameObject.SetActive(true);
        }

        def = Main.getCoolDownDefinition(_type);
    }

    public void Activate()
    {
        
        if (coolDownRecastTimer > 0)
            return ;

        switch (_type)
        {
            case CoolDownType.fire_up:

                break;
            case CoolDownType.speed_up:

                break;
        }
    }
}
