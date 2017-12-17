using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	static public Hero		S;

	public float gameRestartDelay = 2f;

	public Weapon[] weapons;

	public float	speed = 30;
	public float	rollMult = -45;
	public float  	pitchMult=30;

	[SerializeField]
	private float	_shieldLevel=1;

	public bool	_____________________;
	public Bounds bounds;

	public delegate void WeaponFireDelegate ();
	public WeaponFireDelegate fireDelegate;

	public GameObject lastTriggerGo = null;

	void Awake(){
		S = this;
		bounds = Utils.CombineBoundsOfChildren (this.gameObject);

		ClearWeapons ();
		weapons [0].SetType (WeaponType.blaster);
	}
	
	// Update is called once per frame
	void Update () {
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;
		
		bounds.center = transform.position;
		
		// constrain to screen
		Vector3 off = Utils.ScreenBoundsCheck(bounds,BoundsTest.onScreen);
		if (off != Vector3.zero) {  // we need to move ship back on screen
			pos -= off;
			transform.position = pos;
		}
		
		// rotate the ship to make it feel more dynamic
		transform.rotation = Quaternion.Euler(yAxis*pitchMult, xAxis*rollMult,0);

		if(Input.GetAxis("Jump") == 1 && fireDelegate != null){
			fireDelegate ();
		}
	}

	void OnTriggerEnter(Collider other){
		GameObject go = Utils.FindTaggedParent(other.gameObject);
		if (go != null) {
			if (go == lastTriggerGo) {
				return;
			}
			lastTriggerGo = go;
			if (go.tag == "Enemy") {
				shieldLevel--;
				Destroy (go);
			} else if (go.tag == "PowerUp") {
				AbsorbPowerUp (go);
			} else {
				print ("3. Triggered: " + go.name);
			}
		} else {
			print ("4. Triggered: " + other.gameObject.name);
		}
	}

	public void AbsorbPowerUp(GameObject go){
		PowerUp pu = go.GetComponent<PowerUp> ();
		switch(pu.type){
		case WeaponType.shield:
			print ("Shielded");
			shieldLevel++;
			break;

		default:
			if(pu.type == weapons[0].type) {
				print ("Weapons");
				Weapon w = GetEmptyWeaponSlot();
				if(w!=null){
					w.SetType(pu.type);
				}
			}else{
				ClearWeapons ();
				weapons [0].SetType (pu.type);
			}
			break;
		}
		pu.AbsorbedBy (this.gameObject);
	}

	Weapon GetEmptyWeaponSlot(){
		for(int i=0; i<weapons.Length; i++){
			if(weapons[i].type == WeaponType.none){
				return(weapons [i]);
			}
		}
		return(null);
	}
			
	void ClearWeapons(){
		foreach (Weapon w in weapons) {
			w.SetType(WeaponType.none);
		}
	}

	public float shieldLevel {
		get {
			return(_shieldLevel);
		}
		set {
			_shieldLevel = Mathf.Min (value, 4);
			if (value < 0) {
				Destroy (this.gameObject);
				Main.S.DelayedRestart (gameRestartDelay);
			}
		}
	}

}