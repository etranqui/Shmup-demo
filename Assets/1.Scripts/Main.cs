using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	static public Main S;		// Singleton
	static public Dictionary<WeaponType, WeaponDefinition>	W_DEF;	// Allows any class to access the weapon available in the game
    static public Dictionary<CoolDownType, CoolDownDefinition> CD_DEF;   // Allows any class to access the weapon available in the game

    public GameObject	prefabPlayer;
	public GameObject	playerPos;
	public int	life = 3;
	public GameObject[] prefabsEnemy;
	public float spawnPadding = 2.5f;
	public WeaponDefinition[] weaponDefinitions;	// Allow to configure Weapons available in the game
    public CoolDownDefinition[] coolDownDefinitions;    // Allow to configure Weapons available in the game

    public GameObject	prefabPowerUp;
	public WeaponType[] powerUpRate;

	public bool ___________________;

	public float spawnRate = 1.0f;
	public WeaponType[] activeWeaponType;
	public int	score = 0;


	// Use this for initialization
	void Awake () {
		S = this;

		SpawnPlayer ();

		Utils.SetCameraBounds (Camera.main);
		Invoke ("SpawnEnemy", spawnRate);

		// Define a dictionary to access the WeaponDefinition from the WeaponType
		W_DEF = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions) {
			W_DEF[def.type] = def;
		}

	}

	void Start () {
		activeWeaponType = new WeaponType[weaponDefinitions.Length];
		for (int i = 0; i < weaponDefinitions.Length; i++) {
			this.activeWeaponType[i] = weaponDefinitions[i].type;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void SpawnEnemy () {
		int index = Random.Range (0, this.prefabsEnemy.Length);

		GameObject go = Instantiate (prefabsEnemy [index]) as GameObject;
		go.transform.parent = this.transform;

		Vector3 pos = new Vector3 ();
		pos = Vector3.zero;

		float xMin = Utils.camBounds.min.x + spawnPadding;
		float xMax = Utils.camBounds.max.x - spawnPadding;

		pos.x = Random.Range (xMin, xMax);
		pos.y = Utils.camBounds.max.y + spawnPadding;

		go.transform.position = pos;

		Invoke ("SpawnEnemy", spawnRate);
	}


	static public WeaponDefinition getWeaponDefinition (WeaponType type) {
		if (W_DEF.ContainsKey(type)){
			return (W_DEF[type]);
		}
		return new WeaponDefinition();
	}

    static public CoolDownDefinition getCoolDownDefinition(CoolDownType type)
    {
        if (CD_DEF.ContainsKey(type))
        {
            return (CD_DEF[type]);
        }
        return new CoolDownDefinition();
    }

    public void SpawnPowerUp(Enemy e) {

		if (Random.value <= e.dropRate) {
			// get random PowerUp
			int index = Random.Range(0, powerUpRate.Length);

			// spawn PowerUp at enemy position
			GameObject	go = Instantiate(prefabPowerUp) as GameObject;
			go.GetComponent<PowerUp>().SetType(powerUpRate[index]);
			go.transform.position = e.transform.position;
	
		}
	
	}


	public void SpawnPlayer() {
		GameObject go = Instantiate (prefabPlayer) as GameObject;
		go.transform.position = playerPos.transform.position;
	}


	// Restart game in "delay" seconds
	public void DelayedRestart (float delay) {

		if (life >= 0) { 
			Invoke ("SpawnPlayer", delay);
		} else {
			Invoke ("Restart", delay);
		}
	}
	
	// Restart game by reloading scene
	public void Restart () {
		Debug.Log("Restarting Game");
        Application.LoadLevel(0);
	}

}
