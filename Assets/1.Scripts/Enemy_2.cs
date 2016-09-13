using UnityEngine;
using System.Collections;

public class Enemy_2 : Enemy {

	public Vector3[]		points;
	public float			birthTime;
	public float			lifeTime = 10f;
	public float			sinEffectMult =0.6f;


	// Use this for initialization
	void Start () {
		points = new Vector3[2];

		// Fin Utils.CamBounds
		Vector3 cbMin = Utils.camBounds.min;
		Vector3 cbMax = Utils.camBounds.max;

		Vector3 v = Vector3.zero;

		// Pick random point on  the left side of the screen
		v.x = cbMin.x - Main.S.spawnPadding;
		v.y = Random.Range (cbMin.y, cbMax.y);
		points [0] = v;


		// Pick random point on  the right side of the screen
		v = Vector3.zero;
		v.x = cbMax.x + Main.S.spawnPadding;
		v.y = Random.Range(cbMin.y, cbMax.y	);
		points [1] = v;

		// Swapp point if necessary so movement can start from left or right
		if (Random.value < 5.0f) {
		
			points[0] *= -1;
			points[1] *= -1;
		}

		birthTime = Time.time;

	}
	
	// Update is called once per frame
	public override void Move () {
		// Bezier Curve multiplier
		float u  = (Time.time - birthTime) / lifeTime;

		// if u > 1, then it's been alive longer than lifeTime
		if (u > 1) {
			// Destroy enemy
			Destroy(this.gameObject);
			return;
		}

		u = u + sinEffectMult *  (Mathf.Sin(u * Mathf.PI * 2));

		pos = (1 - u) * points [0] + u * points[1];
	
	}
}
