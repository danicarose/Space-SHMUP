using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField]
    private WeaponType _type;

    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    private void Awake()
    {
        InvokeRepeating("CheckOffScreen", 2f, 2f);
    }

	public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = def.projectileColor;
    }

	void CheckOffScreen(){
		Collider col = GetComponent<Collider> ();
		if (Utils.ScreenBoundsCheck (col.bounds, BoundsTest.offScreen) != Vector3.zero) {
			Destroy (this.gameObject);
		}
	}
}
