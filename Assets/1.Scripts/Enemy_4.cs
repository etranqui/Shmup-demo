using UnityEngine;
using System.Collections;


[System.Serializable]
public class Part {

	// Configurable field in the Inspector
	public string		name;			// name of parts
	public float		HP;				// amount of health
	public string[]		protectedBy;	// othr parts protecting this

	// Reference variable for faster access. Will be initialized in Eneemy.Start()
	public	GameObject	go;				// reference to the GameObject of this part
	public	Material	mat;			// reference to the Material of this part(for displaying damage)

}


public class Enemy_4 : Enemy {

	public Vector3[]		points;		// store positions for interpolation
	public float		timeStart;
	public float		duration = 4;
	public Part[]			parts;


	// Use this for initialization
	void Start () {
	
		points = new Vector3[2];

		// assign original spawn position
		points [0] = pos;
		points [1] = pos;

		InitMovement ();


		// Cache GameObject & Material for each Parts
		Transform t;
		foreach (Part prt in parts) {
			t = transform.Find(prt.name);
			if (t != null) {
				prt.go = t.gameObject;
				prt.mat = t.GetComponent<Renderer>().material;
			}
		}
	}


	void InitMovement()
	{
		// Pick a random destination point within camera bounds
		Vector3 dest = Vector3.zero;
		float esp = Main.S.spawnPadding;		// esp = Enemy Spawn Padding
		Bounds cbounds = Utils.camBounds;

		dest.x = Random.Range (cbounds.min.x + esp, cbounds.max.x - esp);
		dest.y = Random.Range (cbounds.min.y + esp, cbounds.max.y - esp);

		// reset the start point and assign the new destination
		points [0] = points [1];
		points [1] = dest;

		// Reset time
		timeStart = Time.time;
	}

	// Update is called once per frame
	public override void Move () {
		// Calculation move duration %
		float u = (Time.time - timeStart) / duration;

		// U > 1, Enemy arrived at destination. Pick a new destination
		if (u > 1) {
			InitMovement();
			u = 0;
		}

		// TODO: Try out different EaseOut functions
		//u = 1 - Mathf.Pow (1 - u, 2);		// apply EaseOut function
		//u = Mathf.Pow (u,2);					// apply EaseIn function
		u = u + 0.1f * Mathf.Sin (2 * Mathf.PI * u);


		pos = (1 - u) * points [0] + u * points [1];
	}


	void OnCollisionEnter (Collision coll) {
	
		GameObject other = coll.gameObject;

		switch (other.tag) { 
		case "HeroProjectile": 
			Projectile p = other.GetComponent<Projectile>();
			bounds.center = transform.position + boundsCenterOffset;
			if ( bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(bounds, 	BoundsTest.offScreen) != Vector3.zero) {
			
				Destroy(other);
				break;
			}


			GameObject goHit = coll.contacts[0].thisCollider.gameObject;
			Part prHit = FindPart(goHit);
			if ( prHit == null) {
				goHit = coll.contacts[0].otherCollider.gameObject;
				prHit = FindPart(goHit);
			}


			if (prHit.protectedBy != null ) {
				foreach ( string s in prHit.protectedBy) {
					// if there is still a part protecting it, don't damage it
					if (!Destroyed (s)) {
						// Destroy projectile
						Destroy(other);
						return;
					}
				}
			}

			prHit.HP -= Main.W_DEF[p.type].damageOnHit;

			// Show damage on the part
			ShowLocalizedDamage(prHit.mat);
			if ( prHit.HP <= 0) {
				// Instead of Destrying the enemy, disable the damage part
				prHit.go.SetActive(false);
			}

			// Check to se if the whole ship is destroyed
			bool allDestroyed = true; 	// Assume it is destroyed
			foreach (Part prt in parts) {
				if (!Destroyed(prt)) {
					allDestroyed = false;	// ... change allDestroyed to false
					break;
				}
			}

			if (allDestroyed) {			// if it is completely destroyed
				// Tell the main singleton that the ship has been destroyed
				Main.S.SpawnPowerUp(this);
				// Destroy this Enemy
				Destroy(this);
			}
			Destroy(other);		// Destroy projectile
			break;
		}
	}	// End OnCollisionEnter



	// Find part, either from a GameObject or from a String
	Part FindPart(string n) {
		foreach (Part prt in parts) {
			if (prt.name == n) {
				return (prt);
			}
		}
		// couldn't find anything
		return null;
	}

	Part FindPart(GameObject go) {
		foreach (Part prt in parts) {
			if (prt.go == go) {
				return ( prt);
			}
		}
		// couldn't fint anything
		return (null);
	}

	// check is object is destroyed, either from a GameObject or from a String
	bool Destroyed ( GameObject go) {
		return ( Destroyed ( FindPart(go)));
	}

	bool Destroyed ( string n ) {
		return (Destroyed (FindPart (n)));
	}

	bool Destroyed(Part prt) {
		// if no part was passed in
		if (prt == null) {
			return (true);
		}

		return (prt.HP <= 0);
	}

	void ShowLocalizedDamage ( Material m ) {
		m.color = Color.red;
		remainingDmgFrames = dmgFrames;
	}
}
