using UnityEngine;
using System.Collections;

public class Enemy_3 : Enemy {

	public Vector3[]	points;
	public float		birthTime;
	public float		lifeTime = 10f;


	// Use this for initialization
	void Start () {

		// Set first point of Bezier curze at spawn point (top of the screen)
		points = new Vector3[3];
		points [0] = pos;

		// Set horizontal range for random X value
		float xMin = Utils.camBounds.min.x + Main.S.spawnPadding;
		float xMax = Utils.camBounds.max.x - Main.S.spawnPadding;

		// set second point of Bezier curze in the lower half of the screen
		Vector3 v;
		v = Vector3.zero;
		v.x = Random.Range (xMin, xMax);
		v.y = Random.Range (Utils.camBounds.min.y, 0);
		points [1] = v;

		// Set third point of Bezier curze at top of the screen
		v = Vector3.zero;
		v.x = Random.Range (xMin, xMax);
		v.y = pos.y;
		points [2] = v;

		// set Birthtime to current time
		birthTime = Time.time;
	}
	
	// Update is called once per frame
	public override void Move () {

		// calculate time factor
		float u = (Time.time - birthTime) / lifeTime;

		if (u > 1) {
			// Object has exceeded its lifetime, kill it
			Destroy(this.gameObject);
			return;
		}

		Vector3 p01, p12;
		u = u - (0.5f * Mathf.Sin (u * 2 * Mathf.PI));
		p01 = (1-u) * points[0] + u * points[1];
		p12 = (1-u) * points[1] + u * points[2];
		pos = (1-u) * p01 + u * p12;

	}
}
