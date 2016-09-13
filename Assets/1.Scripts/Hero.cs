using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	static public Hero S; 		// Singleton

	public float restartDelay = 2f;

	// Ship movement parameters
	public float	speed = 30;
	public float	rollMult = -45;
	public float	pitchMult = 30;
	public float 	lrMult = 0.60f;


	// Ship status information
	private float	_shieldLevel = 1;
	public	Weapon[]	weapons;
    //public Cooldown[] coolDowns;


	public delegate void WeaponFireDelegate();	// Declare delegate method
	public WeaponFireDelegate fireDelegate;		// Declare function pointer

	[SerializeField]
	public float shieldLevel {
		get { return _shieldLevel;}
		set {
			// limit shield level to 4
			_shieldLevel = Mathf.Min (value, 4);

			// destroy ship is shield level drops below 0 and restart game
			if (_shieldLevel < 0) {
				Main.S.life--;
				Destroy (this.gameObject);
				Main.S.DelayedRestart(restartDelay);
			}
		}
	}

	public bool ___________________;
	public Bounds bounds;
	public GameObject lastTriggeredGo = null;

	// Use this for initialization
	void Awake () {
		S = this;		// Set singleton
		bounds = Utils.CombineChildrenBounds (this.gameObject);

	}

	void Start () {
		//Reset the weapons to start with only 1 blaster
		ClearWeapons ();
		weapons [0].SetType (WeaponType.blaster);
        //coolDowns[0].SetCDType(CoolDownType.speed_up);
        //coolDowns[1].SetCDType(CoolDownType.fire_up);
    }
	
	// Update is called once per frame
	void Update () {
	
		// Get input
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");
		float lrAxis = Input.GetAxis ("LRTriggers");


		// Move ship
		Vector3 pos = this.transform.position;
		pos.x += (xAxis - lrAxis * lrMult) * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		this.transform.position = pos;


		// Constrain ship within screen
		bounds.center = this.transform.position;
		Vector3 off = Utils.ScreenBoundsCheck (bounds, BoundsTest.onScreen);
		if (off != Vector3.zero) {
			pos -= off;
			transform.position = pos;
		}

		// Rotate ship based on movement
		this.transform.rotation = Quaternion.Euler (yAxis * pitchMult, (xAxis - lrAxis * lrMult) * rollMult, 0); 


		// Fire weapon using WeaponFireDelegate
		if ((Input.GetAxis ("A") == -1)) { // && (fireDelegate != null)) {
			Debug.Log("Button Press");
			fireDelegate ();
		}
	}


	void OnTriggerEnter(Collider other) {
		GameObject go = Utils.FindTaggedParent (other.gameObject);

		if (other.gameObject == lastTriggeredGo)
						return;

		// Update last triggered GameObject
		lastTriggeredGo = go;

		// Lower shield level and detroy collided enemy
		if (go.tag == "Enemy") {
			shieldLevel--;
			Destroy(go);
		} else if ( go.tag == "PowerUp") {
			AbsorbPowerUp(go);
		} else {
			Debug.Log("Triggered: " + go.name);
		}
	}

	// Absorb PowerUp
	public void AbsorbPowerUp(GameObject go) {
		PowerUp pow = go.GetComponent<PowerUp> ();

		Debug.Log ("AbsorbPowerUp");
		switch (pow.type) {
		// If it's a Shield
		case WeaponType.shield:
			this.shieldLevel++;
			break;
		
		default: 
			if (pow.type == weapons[0].type) {
				// if it's the same type as the current weapon, increase weapon level
				Weapon w = GetEmptyWeaponSlot(); 	// Find an available weapon slot

				if (w != null) {
					// Set 
					w.SetType(pow.type);
				}
			} else {
					// if it's a different weapon, reset Weapon and change
					ClearWeapons();
					weapons[0].SetType(pow.type);
			}
			break;
		}
		pow.AbsordbedBy (this.gameObject);
	}

	// loop through weapon array and return the first available slot
	Weapon GetEmptyWeaponSlot() {
		for (int i = 0; i < this.weapons.Length; i++) {
			if (weapons[i].type == WeaponType.none)
			{
				return (weapons[i]);
			}
		}
		return (null);
	}


	// Set all weapon slots to WeaponType.non
	void ClearWeapons () {
		foreach (Weapon w in weapons) {
			w.SetType(WeaponType.none);
		}
	}
}
