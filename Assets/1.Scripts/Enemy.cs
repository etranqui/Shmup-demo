using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public float		speed = 15f;
	public float 		fireRate = 0.3f;
	public float 		HP = 10;
	public int 			score = 100;
	public int			dmgFrames = 6;
	public float		dropRate = 0.3f; 		// PowerUp drop Rate

	public bool			__________________;

	public Bounds		bounds;
	public Vector3		boundsCenter;
	public Vector3		boundsCenterOffset;
	public Color[]		originalColors;
	public Material[]	materials;
	public int			remainingDmgFrames;


	// Use this for initialization
	void Awake() {
		InvokeRepeating("CheckOffScreen", 0f, 2f);
		this.speed = Mathf.Lerp (speed, 2 * speed, Random.value);

		// Save original material color for animating damage frames
		this.materials = Utils.GetAllMaterials (this.gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++) {
			originalColors[i] = materials[i].color;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Move ();

		if (remainingDmgFrames > 0) {
			remainingDmgFrames--;
			if (remainingDmgFrames == 0) 
			{
				UnShowDamage();
			}
		}
	}

	public virtual void Move() {
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;
		pos = tempPos;
	}

	public Vector3 pos {
				get {
						return (this.transform.position);
				}
				set {
						this.transform.position = value;
				}
		}


	void CheckOffScreen () {
		if (bounds.size == Vector3.zero) {
						bounds = Utils.CombineChildrenBounds (this.gameObject);
						boundsCenterOffset = bounds.center - this.transform.position;
				}

		bounds.center = this.transform.position + boundsCenterOffset;

		Vector3 off = Utils.ScreenBoundsCheck (bounds, BoundsTest.offScreen);
		if (off != Vector3.zero) {
					if (off.y < 0) {
				Destroy(this.gameObject);
			}
		}
	}


	void OnCollisionEnter(Collision coll) {

		GameObject other = coll.gameObject;

		switch (other.tag) {
		case "HeroProjectile":
			Projectile p = other.GetComponent<Projectile>();

			// Enemies don't take damage unless onScreen so they can't be shot until visible
			bounds.center = this.transform.position + boundsCenterOffset;
			// if enemy is hit outside of the screen, destroy projectile
			if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(this.bounds, BoundsTest.offScreen) != Vector3.zero) {
				Destroy(other);
				break;
			}

			// Debug.Log (this.name + " is hit by " + other.tag );
			// reduce HP, check enemy death and destroy projectile
			ShowDamage();
			this.HP -= Main.W_DEF[p.type].damageOnHit;
			if (this.HP <= 0) {
				Main.S.SpawnPowerUp(this);
				Main.S.score += this.score;
				Destroy(this.gameObject);
			}
			Destroy(other);
			break;
		}
	}


	public void ShowDamage() {
		foreach (Material m in materials) {
			m.color = Color.red;
		}
		remainingDmgFrames = dmgFrames;
	}

	public void UnShowDamage() {
		for (int i = 0; i < materials.Length; i++) {
			materials[i].color = originalColors[i];
		}
	}

}
