using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour {
    public GameObject firstStep;
	private GameObject direction;
    private Vector3 vect;
    private float AggroTimer;
    public bool Follow = true;
    private bool Following = false;
    public bool Aggro = true;
    private bool inAggro = false;
    private float AAtimer;
    private bool AA;
    private GameObject Hitting;
    private float VerifyAggro; 

    private float speed;
    // Use this for initialization
	void Start () {
        if(Follow) this.direction = firstStep;
        inAggro = false;
        Following = Follow;
        VerifyAggro = 0;
    }

    // Update is called once per frame
    void Update() {
        speed = GetComponent<Carac>().SuchMSpeed();
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation);  

        if (!AA)
        {
            AAtimer += Time.deltaTime;
            if (AAtimer >= this.gameObject.GetComponent<Carac>().SuchASpeed())
            {
                AA = true;
                AAtimer = 0f;
                inAggro = false;
            }
        }

        if(AA && inAggro)
        {
            VerifyAggro += Time.deltaTime;
            if (VerifyAggro >= 2)
            {
                inAggro = false;
                VerifyAggro = 0;
            }
        }

        //Doit aggro et n'a rien en mire
        if (Aggro && !inAggro)
        {
            //Toute les 0.25s
            AggroTimer += Time.deltaTime;
            if (AggroTimer >= 0.25)
            {
                AggroTimer = 0;
                int i;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<Carac>().AggroRange);
                for (i = 0; i < hitColliders.Length && !inAggro; i++)
                {
                    //Cherche un mec a taper
                    if (hitColliders[i].tag=="Enemi" && hitColliders[i].GetComponent<Carac>().team != GetComponent<Carac>().team)
                    {
                        Following = false;
                        inAggro = true;
                        Hitting = hitColliders[i].gameObject;
                        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    }
                }
                //si personne
                if (!inAggro)
                {
                    inAggro = false;
                    Following = true;
                }
            }
        }
        //Si en train de taper et qu'il y a une cible
        if (inAggro && Hitting.gameObject!=null)
        {
            Following = false;
            float dist = Vector3.Distance(transform.position, Hitting.transform.position)-((Hitting.GetComponent<MeshFilter>().mesh.bounds.size.x * Hitting.transform.localScale.x));
            //Si in range d'aggro
            if (dist <= GetComponent<Carac>().AggroRange)
            {
                vect.x = Hitting.transform.position.x - transform.position.x;
                vect.y = Hitting.transform.position.y - transform.position.y;
                vect.z = Hitting.transform.position.z - transform.position.z;
                //Si in range AA
                if (dist <= GetComponent<Carac>().ManyRange())
                {
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    if (AA) // et AA dispo
                    {
                        AA = false;
                        this.GetComponent<Carac>().Attaque(Hitting);
                   }   
                }
                // Si in range Aggro, mais pas in Range AA
                else
                {
                    GetComponent<Rigidbody>().velocity = vect.normalized * speed;
                    GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x);

                }


            }
            //Si pas la range d'aggro stop aggro
            else 
            {
                inAggro = false;
                Following = true;
            }
        }
        else if (inAggro && Hitting.gameObject!=null)
        {
            Following = true;
        }
        else if (Following && !inAggro && Follow)
        {
            vect.x = direction.transform.position.x - transform.position.x;
            vect.y = direction.transform.position.y - transform.position.y;
            vect.z = direction.transform.position.z - transform.position.z;

            GetComponent<Rigidbody>().velocity = vect.normalized * speed;

            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x);
        }
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject == direction)
            {

                if (direction.GetComponent<PtsPassage>().estFin)
                {
                direction.GetComponent<PtsPassage>().Team.GetComponent<TeamController>().damaged(1);
                Destroy(this.gameObject);
                }

                else if (direction.GetComponent<PtsPassage>()!= null)
                {
                    direction = direction.GetComponent<PtsPassage>().next;
                }
                
            }
        }

}
