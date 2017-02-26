/* 
 * Esse Script movimenta o GameObject quando você clica ou
 * mantém o botão esquerdo do mouse apertado.
 * 
 * Para usá-lo, adicione esse script ao gameObject que você quer mover
 * seja o Player ou outro
 * 
 * Autor: Vinicius Rezendrix - Brasil
 * Data: 11/08/2012
 * 
 * This script moves the GameObeject when you
 * click or click and hold the LeftMouseButton
 * 
 * Simply attach it to the gameObject you wanna move (player or not)
 * 
 * Autor: Vinicius Rezendrix - Brazil
 * Data: 11/08/2012 
 *
 */

using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour
{
    private Transform myTransform;              // this transform
    private Vector3 destinationPosition;        // The destination Point
    private float destinationDistance;          // The distance between myTransform and destinationPosition
    private bool isGround;
    private bool isTeam;
    private GameObject Selected;
    private bool AA;
    private float AAtimer;

    private float moveSpeed;                     // The Speed the character will move
    public int buttonID;
    public char First_Spell;
    public char Second_Spell;
    public char Third_Spell;
    public char Fourth_Spell;

    private bool aPressed;
    private bool zPressed;
    private bool ePressed;
    private bool rPressed;


    private float AArange;


    void Start()
    {
        myTransform = transform;                            // sets myTransform to this GameObject.transform
        destinationPosition = myTransform.position;         // prevents myTransform reset
        aPressed = false;
        zPressed = false;
        ePressed = false;
        rPressed = false;
    }

    void Update()
    {
        moveSpeed = GetComponent<Carac>().SuchMSpeed();
        AArange = GetComponent<Carac>().ManyRange();

        booleanUpdate(); //spells
        AAUpdate(); //AATimer

        if (this.GetComponent<Carac>().CanMove()){
            Move(); //movementScript
        }
 
       
    }

    private bool AllFalse()
    {
        return (aPressed == false && zPressed == false && ePressed == false && rPressed == false);
    }

    private void booleanUpdate()
    {
        if (Input.GetKeyDown(First_Spell.ToString()))
        {
            aPressed = true;
        }
        else
        {
            aPressed = false;
        }
        if (Input.GetKeyDown(Second_Spell.ToString()))
        {
            zPressed = true;
        }
        else
        {
            zPressed = false;
        }
        if (Input.GetKeyDown(Third_Spell.ToString()))
        {
            ePressed = true;
        }
        else
        {
            ePressed = false;
        }
        if (Input.GetKeyDown(Fourth_Spell.ToString()))
        {
            rPressed = true;
        }
        else
        {
            rPressed = false;
        }
    }

    void AAUpdate()
    {
        AAtimer += Time.deltaTime;
        if (!AA && AAtimer >= this.gameObject.GetComponent<Carac>().SuchASpeed())
        {
            AA = true;
            AAtimer = 0f;
        }
    }

    void Move()
    {
        if (isGround || isTeam) destinationDistance = Vector2.Distance(new Vector2(destinationPosition.x, destinationPosition.z), new Vector2(myTransform.position.x, myTransform.position.z));
        else destinationDistance = Vector2.Distance(new Vector2(destinationPosition.x, destinationPosition.z), new Vector2(myTransform.position.x, myTransform.position.z)) - AArange;

        if (destinationDistance < .5f && !isGround && !isTeam && Selected != null)
        {
            if (AA)
            {
                this.GetComponent<Carac>().Attaque(Selected);
                AA = false;
                AAtimer = 0;
            }
        }
        else if (destinationDistance < .5f || (destinationDistance < 3 && isTeam))
        {       // To prevent shakin behavior when near destination
            moveSpeed = 0;
        }
        else if (destinationDistance > .5f)
        {           // To Reset Speed to default
            moveSpeed = 5;
        }


        // Moves the Player if the Left Mouse Button was clicked
        if (Input.GetMouseButtonDown(buttonID) && GUIUtility.hotControl == 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100.0f))
            {
                if (hit.collider.tag.Equals("Ground"))
                {
                    // move
                    isGround = true;
                    isTeam = false;
                    Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                    float hitdist = 0.0f;

                    if (playerPlane.Raycast(ray, out hitdist))
                    {
                        Vector3 targetPoint = ray.GetPoint(hitdist);
                        destinationPosition = ray.GetPoint(hitdist);
                        //       destinationPosition.y += 1;
                        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                        myTransform.rotation = targetRotation;
                    }
                }

                else if (hit.collider.tag.Equals("Enemi") && hit.collider.GetComponent<Carac>().team != this.gameObject.GetComponent<Carac>().team)
                {
                    //Enemy : AA
                    Selected = hit.collider.gameObject;
                    isGround = false;
                    isTeam = false;
                    Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                    float hitdist = 0.0f;

                    if (playerPlane.Raycast(ray, out hitdist))
                    {
                        Vector3 targetPoint = ray.GetPoint(hitdist);
                        destinationPosition = ray.GetPoint(hitdist);
                        //    destinationPosition.y += 1;
                        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                        myTransform.rotation = targetRotation;
                    }

                }
                else if (hit.collider.tag.Equals("Enemi") && hit.collider.GetComponent<Carac>().team == this.gameObject.GetComponent<Carac>().team)
                {
                    //Same team
                    Selected = hit.collider.gameObject;
                    isGround = false;
                    isTeam = true;
                    Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                    float hitdist = 0.0f;

                    if (playerPlane.Raycast(ray, out hitdist))
                    {
                        Vector3 targetPoint = ray.GetPoint(hitdist);
                        destinationPosition = ray.GetPoint(hitdist);
                        // destinationPosition.y += 1;
                        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                        myTransform.rotation = targetRotation;
                    }

                }

            }
        }

        // To prevent code from running if not needed
        if (destinationDistance > 0.5f)
        {

            Vector3 vect;
            vect.x = destinationPosition.x - transform.position.x;
            vect.z = destinationPosition.z - transform.position.z;
            vect.y = 0;

            vect.Normalize();
            vect.x = vect.x * GetComponent<Carac>().SuchMSpeed();
            vect.z = vect.z * GetComponent<Carac>().SuchMSpeed();
            GetComponent<Rigidbody>().velocity = new Vector3(vect.x, GetComponent<Rigidbody>().velocity.y, vect.z);
            //myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
        }
    }
}