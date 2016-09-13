using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public Vector2		rotMinMax = new Vector2(15,90);
	public Vector2		driftMinMax = new Vector2(.25f, 2);
	public float		lifeTime = 6f;
	public float		fadeTime = 4f;
	public bool			___________________________________;
	public	WeaponType	type;
	public GameObject	cube;
	public TextMesh		letter;
	public Vector3		rotPerSecond;
	public float		birthTime;



	// Use this for initialization
	void Awake () {
		// find PowCube reference
		cube = transform.Find ("PowCube").gameObject;

		// Find the Text Mesh
		letter = GetComponent<TextMesh> ();

		// Set a random velocity
		Vector3 vel = Random.onUnitSphere;
		vel.z = 0;		// Flatten velocity on the XY axis
		vel.Normalize ();	// normalize to 1 unit length
		vel *= Random.Range (driftMinMax.x, driftMinMax.y);
		this.GetComponent<Rigidbody>().velocity = vel;


		// Reset rotation
		this.transform.rotation = Quaternion.identity;
		this.rotPerSecond = new Vector3(Random.Range(this.rotMinMax.x, this.rotMinMax.y),
		                                Random.Range(this.rotMinMax.x, this.rotMinMax.y),
		                                Random.Range(this.rotMinMax.x, this.rotMinMax.y));

		// Schedule CheckOffscreen every 2 sec
		Invoke ("CheckOffscreen", 2.0f);
		birthTime = Time.time;

	}
	
	// Update is called once per frame
	void Update () {
		// Rotate cube
		cube.transform.rotation = Quaternion.Euler (rotPerSecond * Time.time);

		// Fade out cube over time. u = time until death / fade time
		// u act as a countdown to death, u < 0 means that object if still in it's lideTime
		float u = (Time.time - (this.birthTime + this.fadeTime)) / fadeTime;


		// u >= 1, means that object has exceeded it's lifeTime + fadeTime
		if (u >= 1) {
			Destroy(this.gameObject);
			return;
		}

		// u >= 1, means that object has exceeded it's lifeTime + fadeTime
		if (u > 0) {
			// get the cube color and fade the alpha transparancy
			Color c = cube.GetComponent<Renderer>().material.color;
			c.a = 1f - u;
			this.cube.GetComponent<Renderer>().material.color = c;

			// get the letter color and fade the alpha transparancy
			c = letter.color;
			c.a = 1f - (u*0.5f);	// letter fades slower than cube
			this.letter.color = c;
		}

	}

	// Set PowerUp property based on Weapon Definition
	public void SetType( WeaponType wt) {
		WeaponDefinition def = Main.getWeaponDefinition (wt);
		this.cube.GetComponent<Renderer>().material.color = def.color;
		letter.text = def.letter;
		type = wt;
	}

	// this function his called by the hero class when PowerUp is collected
	public void AbsordbedBy(GameObject target) {
		Destroy (this.gameObject);
	}

	// This method is Invoked repeating
	void CheckOffscreen () {
		// Delete PowerUp if it goes completely off screen
		// TODO: change behaviour to bounce within the screen
		if (Utils.ScreenBoundsCheck (cube.GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero) {
			Destroy(this.gameObject);		
		}
	}
}
