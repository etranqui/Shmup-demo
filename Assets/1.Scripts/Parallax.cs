using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {


	public GameObject		poi;		// Point of Interest = player ship
	public GameObject[]		panels;		// scrolling foreground
	public float			scrollSpeed	= -10f;
	public float			motionMult = 0.25f;		// control how much the pannels react to player movment

	private float			panelHeight;		// Panel Height
	private float			depth;				// Panel depth (z)

	// Use this for initialization
	void Start () {
		panelHeight = panels [0].transform.localScale.y;
		depth = panels [0].transform.position.z;

		panels [0].transform.position = new Vector3 (0, 0, depth);
		panels [1].transform.position = new Vector3 (0, panelHeight, depth);

	}
	
	// Update is called once per frame
	void Update () {
		float tY, tX = 0f;
		tY = Time.time * scrollSpeed % panelHeight + (panelHeight * 0.5f);

		if (poi != null) {
			tX = -poi.transform.position.x * motionMult;
		}

		panels [0].transform.position = new Vector3 (tX, tY, depth);
		if (tY >= 0) {
			panels [1].transform.position = new Vector3 (tX, tY - panelHeight, depth);
		} else {
			panels [1].transform.position = new Vector3 (tX, tY + panelHeight, depth);
		}
	}
}
