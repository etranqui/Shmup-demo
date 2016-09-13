using UnityEngine;
using System.Collections;

public class Enemy_1 : Enemy {

	public float waveFrequency = 3f;	// time to perform a wave motion
	public float waveWidth = 6f;		// width of wave
	public float waveRotY = 30;

	private float x0 = -12345;		// Initial value of x value of pos?
	private float birthTime;


	// Parent class does not have a Start method
	void Start() {
		x0 = pos.x;

		birthTime = Time.time;
	}

	public override void Move ()
	{
		Vector3 tempPos = pos;

		// Theta adjust based on time
		float age = Time.time - birthTime;
		float theta = Mathf.PI * 2 * age / waveFrequency;
		float sin = Mathf.Sin (theta);
		tempPos.x = x0 + sin * waveWidth;
		this.pos = tempPos;

		// rotate on y axis
		Vector3 rot = new Vector3 (0, sin * waveRotY, 0);
		this.transform.rotation = Quaternion.Euler(rot);

		// base.Move still handles movement on Y
		base.Move ();
	}

}
