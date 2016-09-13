using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	public float 	rotSpeed = 0.1f;
	public bool		_______________________;
	public int		levelShown = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Read the current shield level from Hero Singleton
		int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);


		// Update displayed shield level
		if (levelShown != currLevel) {
			levelShown = currLevel;
			Material mat = this.GetComponent<Renderer>().material;
			// Adjust texture shown
			mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
		}

		// Rotate Shield
		float rZ = (rotSpeed * Time.time * 360) % 360f;
		this.transform.rotation = Quaternion.Euler (0, 0, rZ);

	}
}
