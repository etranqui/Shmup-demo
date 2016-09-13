using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	[SerializeField]
	private WeaponType _type;

	public WeaponType type {
				get { return _type;}
				set { SetType(value);}
		}

	// Use this for initialization
	void Awake () {
		InvokeRepeating ("CheckOffscreen", 2f, 2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Set type and color for this projectile base on the Weapon definition
	public void SetType ( WeaponType weapon) {
		_type = weapon;
		WeaponDefinition def = Main.getWeaponDefinition(weapon);
		this.GetComponent<Renderer>().material.color = def.color;
	}


	public void CheckOffscreen() {
		if (Utils.ScreenBoundsCheck (this.GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero)
			Destroy (this.gameObject);
						
	}
}
