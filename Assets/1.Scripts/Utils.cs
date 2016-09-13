using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//======================== Bounds functions ===========================\\

public enum BoundsTest {
	center,			// Is center of the GameObject on screen?
	onScreen,		// Are bounds entirely on screen ?
	offScreen		// Are bounds entirely off screen ?
}

public class Utils : MonoBehaviour {

	static private Bounds _camBounds;

	public static Bounds BoundsUnion (Bounds b0, Bounds b1) {

		if (b0.size == Vector3.zero && b1.size != Vector3.zero) {
			return b1;
		} else if (b1.size == Vector3.zero && b0.size != Vector3.zero) {
			return b0;
		} else if (b0.size == Vector3.zero && b1.size == Vector3.zero) {
			return b0;
		}

		// Stretch b0 to encapsulate b1 min/max
		b0.Encapsulate (b1.min);
		b0.Encapsulate (b1.max);

		return b0;
	}


	public static Bounds CombineChildrenBounds (GameObject go) {

		// Create emtpy bound
		Bounds b = new Bounds (Vector3.zero, Vector3.zero);

		// Expand b to include the renderer's bounds
		if (go.GetComponent<Renderer>() != null) {
			b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
		}

		// Expand b to include the collider's bounds
		if (go.GetComponent<Collider>() != null) {
			b = BoundsUnion (b, go.GetComponent<Collider>().bounds);
		}

		// Recursively expand b to each children's bounds
		foreach (Transform t in go.transform) {
			b = BoundsUnion (b, CombineChildrenBounds(t.gameObject));
		}
		return (b);
		
	}


	static public Bounds camBounds {
				get {
						//
						if (_camBounds.size == Vector3.zero) {
								SetCameraBounds ();
						}
						return _camBounds;
				}
		}


	public static void SetCameraBounds (Camera cam = null) {
		if (cam == null)
						cam = Camera.main;

		Vector3 topLeft = new Vector3 (0, 0, 0);
		Vector3 bottomRight = new Vector3 (Screen.width, Screen.height, 0);

		Vector3 boundTLN = cam.ScreenToWorldPoint (topLeft);
		Vector3 boundBRF = cam.ScreenToWorldPoint (bottomRight);

		boundTLN.z += cam.nearClipPlane;
		boundBRF.z += cam.farClipPlane;

		Vector3 center = (boundTLN + boundBRF) / 2f;

		_camBounds = new Bounds (center, Vector3.zero);
		_camBounds.Encapsulate (boundTLN);
		_camBounds.Encapsulate (boundBRF);

	}


	// checks to see whether the Bounds bnd are within the camBounds
	public static Vector3  ScreenBoundsCheck( Bounds bnd, BoundsTest test = BoundsTest.onScreen) {
		return (BoundsInBoundsCheck (camBounds, bnd, test));
	}

	// Checks to see whether Bounds lilB are within Bounds bigB
	public static Vector3 BoundsInBoundsCheck ( Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen) {
	
		Vector3 pos = lilB.center;

		// Initialize the offset at [0,0,0]
		Vector3 off = Vector3.zero;

		switch (test) {

			// Determine the required offset to move lilB's center inside bigB
		case BoundsTest.center:
			if (bigB.Contains(pos)) {
				return (Vector3.zero);
			}

			if (pos.x > bigB.max.x) {
				off.x = pos.x - bigB.max.x;
			} else if (pos.x < bigB.min.x) {
				off.x = pos.x - bigB.min.x;
			}

			if ( pos.y > bigB.max.y) {
				off.y = pos.y - bigB.max.y;
			} else if (pos.y < bigB.min.y) {
				off.y = pos.y - bigB.min.y;
			}

			if ( pos.z > bigB.max.z) {
				off.z = pos.z - bigB.max.z;
			} else if (pos.y < bigB.min.y) {
				off.z = pos.z - bigB.min.z;
			}

			return (off);
		

		// Determine the required offset to keep all of lilB inside bigB
		case BoundsTest.onScreen:
			if (bigB.Contains(lilB.min) && bigB.Contains(lilB.max)) {
				return (Vector3.zero);
			}

			if (lilB.max.x > bigB.max.x) {
				off.x = lilB.max.x - bigB.max.x;
			} else if (lilB.min.x < bigB.min.x) {
				off.x = lilB.min.x - bigB.min.x;
			}

			if (lilB.max.y > bigB.max.y) {
				off.y = lilB.max.y - bigB.max.y;
			} else if (lilB.min.y < bigB.min.y) {
				off.y = lilB.min.y - bigB.min.y;
			}

			if (lilB.max.z > bigB.max.z) {
				off.z = lilB.max.z - bigB.max.z;
			} else if (lilB.min.z < bigB.min.z) {
				off.z = lilB.min.z - bigB.min.z;
			}

			return (off);


		// Determine the required offset to have any part of lilB inside bigB
		case BoundsTest.offScreen:
			bool cMin = bigB.Contains(lilB.min);
			bool cMax = bigB.Contains(lilB.max);

			if (cMin || cMax) {
				return (Vector3.zero);
			}


			if (lilB.min.x > bigB.max.x) {
				off.x = lilB.min.x - bigB.max.x;
			} else if (lilB.max.x < bigB.min.x) {
				off.x = lilB.max.x - bigB.min.x;
			}

			if (lilB.min.y > bigB.max.y) {
				off.y = lilB.min.y - bigB.max.y;
			} else if (lilB.max.y < bigB.min.y) {
				off.y = lilB.max.y - bigB.min.y;
			}

			if (lilB.min.z > bigB.max.z) {
				off.z = lilB.min.z - bigB.max.z;
			} else if (lilB.max.z < bigB.min.z) {
				off.z = lilB.max.z - bigB.min.z;
			}

			return (off);
		}

		return (Vector3.zero);

	}


// ================== Transform functions ===================\\
	
	// This function recursively climb in the GameObject hierarchy tree to find the first tagged parent or the last parent
	public static GameObject FindTaggedParent (GameObject go) {

		if (go.tag != "Untagged") {
			return (go);
		}

		if (go.transform.parent == null) {
			return (go);
		}

		return (FindTaggedParent( go.transform.parent));
	}

	public static GameObject FindTaggedParent (Transform t) {
		return FindTaggedParent ( t.gameObject);
	}


// ======================== Materials =======================\\

// Returns the list of all material on this GameObject and it's children
	static public Material[] GetAllMaterials( GameObject go ) {
		List<Material> mats = new List<Material> ();

		if (go.GetComponent<Renderer>() != null) {
			mats.Add (go.GetComponent<Renderer>().material);
		}

		foreach (Transform t in go.transform) {
			mats.AddRange(GetAllMaterials (t.gameObject));
		}

		return mats.ToArray ();
	}


	}
