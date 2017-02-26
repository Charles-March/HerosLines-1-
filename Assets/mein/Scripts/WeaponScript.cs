using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    public GameObject cible;
    public int moveSpeed;
    public int damage;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (cible == null)
        {
            Destroy(this.gameObject);
        }
        if (cible != null)
        {
            Vector3 direction = cible.GetComponent<Transform>().position - this.GetComponent<Transform>().position;
            this.GetComponent<Transform>().position += (direction.normalized / 30) * moveSpeed;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == cible)
        {
            cible.GetComponent<Carac>().damaged(damage);
            Destroy(this.gameObject);
        }   
    }
}
