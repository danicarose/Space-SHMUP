﻿using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	static public Hero		S;

	public float gameRestartDelay = 2f;

	public float	speed = 30;
	public float	rollMult = -45;
	public float  	pitchMult=30;

	[SerializeField]
	private float _shieldLevel = 1;

	public bool	_____________________;
	public Bounds bounds;

	//
	public delegate void WeaponFireDelegate();
	//
	public WeaponFireDelegate fireDelegate;

	void Awake(){
		S = this;
		bounds = Utils.CombineBoundsOfChildren (this.gameObject);
	}


	// Use this for initialization
	void Start () {
	
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
		transform.rotation =Quaternion.Euler(yAxis*pitchMult, xAxis*rollMult,0);
	}//end Update();

	//This variable holds a reference to the last triggering GameObject
	public GameObject lastTriggerGo = null;

	void OnTriggerEnter(Collider other){
		//Find the tag of other.gameObject or its parent GameObjects
		GameObject go = Utils.FindTaggedParent(other.gameObject);
		//If there is a parent with a tag
		if (go != null) {
			//Make sure it's not the same triggering go as last time
			if (go == lastTriggerGo) {
				return;
			}
			lastTriggerGo = go;

			if (go.tag == "Enemy") {
				//If the shield was triggered by an ememy
				//Decrease the level of the shield by 1 
				shieldLevel--;
				//Destroy the enemy
				Destroy (go);
			} else {
				print ("Triggered: " + go.name);
			}
		} else {
			print ("Triggered: " + other.gameObject.name); //does this still need to be here
		}

	}

	public float shieldLevel{
		get{
			return(_shieldLevel);
		}
		set{
			_shieldLevel = Mathf.Min (value, 4);
			//If the shield is going to be set to less than zero
			if (value < 0) {
				Destroy(this.gameObject);
				//Tell Main.S to restart teh game after a delay
				Main.S.DelayedRestart(gameRestartDelay);
			}
		}
	}
}