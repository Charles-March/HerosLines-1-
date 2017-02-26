using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TeamController : MonoBehaviour {
    public int MaxHP;
    public int team;
    public Slider slider;
    private int HP;
    public bool slid;
	// Use this for initialization
    public void playend()
    {
        Application.Quit();
    }

    public void damaged(int damage)
    {
        HP -= damage;
        slider.value = HP;
        if (HP <= 0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1000000000);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if( hitColliders[i].tag == "Enemi" && hitColliders[i].gameObject.GetComponent<Carac>().team == team)
                {
                    Destroy(hitColliders[i]);
                } 
            }
            playend();
        }
    }
    
	void Start () {
        HP = MaxHP;
        slid = (slider != null);
        if (slid)
        {
            slider.value = HP;
            slider.maxValue = HP;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
