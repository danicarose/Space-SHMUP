using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour {
	[SerializeField]
	private WeaponType _type;

	//
	public WeaponType type{
		get{
			return(_type);
		}
		set{
			SetType (value);
		}
	}

	void Awake(){
		InvokeRepeating ("CheckOffscreen", 2f, 2f);
	}

	public void SetType(WeaponType eType){
		_type = eType;
		WeaponDefinition def = Main.GetWeaponDefinition (_type);
		GetComponent<Renderer>().GetComponent<Material>().color = def.projectileColor; //outdated in textbook
	}

	void CheckOffscreen(){
		if (Utils.ScreenBoundsCheck (GetComponent<Collider>().GetComponent<Bounds>(), BoundsTest.offScreen) != Vector3.zero) { //outdated in textbook
			Destroy (this.gameObject);
		}
	}
		

}
