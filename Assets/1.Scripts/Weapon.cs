using UnityEngine;
using System.Collections;

public enum WeaponType {
	none,			// Default no weapon
	blaster,		// Simple 1 shot blaster
	spread,			// 2 shots
	phaser,			// moves in wave [NA]
	missile,		// Homing missiles [NA]
	laser,			// Damage over time
	shield			// Raise shield level
}


[System.Serializable]
public class WeaponDefinition {
	public WeaponType	type = WeaponType.blaster;
	public string		letter;
	public Color		color;
	public GameObject	projectilePrefab;
	public Color		projectileColor;
	public float		damageOnHit = 0;
	public float		continuousDamage = 0;
	public float		delayBetweenShots = 0;
	public float		velocity = 20;
}

public class Weapon : MonoBehaviour {

	static public Transform	PROJECTILE_ANCHOR;		// Acts as the parent of all projectile to keep herarchy clean

	public bool _________________;

	[SerializeField]
	private WeaponType		_type = WeaponType.blaster;
	public WeaponDefinition	def;
	public GameObject		collar;
	public float			lastShot; // Time since last shot


	// Use this for initialization
	void Awake () {
		collar = this.transform.Find ("Collar").gameObject;
	}


	void Start () {
		SetType (_type);
		if (PROJECTILE_ANCHOR == null) {
			GameObject go = new GameObject("ProjectileAnchor");
			PROJECTILE_ANCHOR = go.transform;
		}

		// Find the FireDelegate of the parent
		GameObject parentGO = this.transform.parent.gameObject;
		if (parentGO.tag == "Hero") {
			Debug.Log("Assign Fire Delegate");
			Hero.S.fireDelegate += Fire;
		}
	}
	
	public WeaponType type {
			get { return _type;}
		set { SetType (value);}
	}

	public void SetType (WeaponType wt) {
		_type = wt;

		if (type == WeaponType.none) {
			this.gameObject.SetActive (false);
			return;
		} else {
			this.gameObject.SetActive(true);		
		}

		def = Main.getWeaponDefinition (type);
		collar.GetComponent<Renderer>().material.color = def.color;				// Race condition example
		lastShot = 0;	// allows to shot immediately
	}
	// Update is called once per frame
	void Update () {
		
	}


	public void Fire () {

		Debug.Log ("Fire");
		if (!this.gameObject.activeInHierarchy) {
			return;
		}

		if (Time.time - lastShot < def.delayBetweenShots)
			return;
	
		Projectile p;
				
		switch (type) {
		case WeaponType.blaster:
				p = MakeProjectile ();
				p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
				break;

		case WeaponType.spread:
				p = MakeProjectile ();
				p.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;

				p = MakeProjectile ();
				p.GetComponent<Rigidbody>().velocity = new Vector3 (-0.4f, 1.0f, 0f) * def.velocity;
				p.transform.rotation = Quaternion.Euler(new Vector3 (0f, 0f, Mathf.Atan(0.4f) * Mathf.Rad2Deg));

				p = MakeProjectile ();
				p.GetComponent<Rigidbody>().velocity = new Vector3 (0.3f, 1.0f, 0f) * def.velocity;
				p.transform.rotation = Quaternion.Euler(new Vector3 (0f, 0f, Mathf.Atan(0.4f) * Mathf.Rad2Deg * -1));

				break;
		// TODO: Implement code for Laser
		case WeaponType.laser:
			break;
		
		// TODO: Implement code for Homing Missile
		case WeaponType.missile:
			break;

		}
	}


	public Projectile MakeProjectile (){

		GameObject go = Instantiate (def.projectilePrefab) as GameObject;
		go.transform.parent = this.gameObject.transform;
		if (transform.parent.gameObject.tag == "Hero") {
						go.tag = "HeroProjectile";
						go.layer = LayerMask.NameToLayer ("HeroProjectile");
				} else {
						go.tag = "EnemyProjectile";
						go.layer = LayerMask.NameToLayer ("EnemyProjectile");
				}

		// create projectile at collar posiiton and assign it to projectile anchor
		go.transform.position = collar.transform.position;
		go.transform.parent = PROJECTILE_ANCHOR;

		Projectile p = go.GetComponent<Projectile> ();
		p.type = type;
		lastShot = Time.time;
		return (p);
	}
	
}
