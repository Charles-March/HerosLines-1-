using UnityEngine;
using System.Collections;

public class GUIHealth : MonoBehaviour
{

    // textures
    public Texture red; // back segment
    public Texture green; // front segment
    public int decalX;
    public int decalY;

    //values   
    private float healthBarWidth; //a value for creating the health bar size
    public static float maxHP; // maximum HP

    void Start()
    {
        maxHP = this.GetComponent<Carac>().SuchHP();
        healthBarWidth = 100f; // create the health bar value
    }

    void Update()
    {
        adjustCurrentHealth();
    }

    public void adjustCurrentHealth()
    {
        //OnGUI();
    }

    void OnGUI()
    {
        float posX = Camera.main.WorldToScreenPoint(this.GetComponent<Transform>().position).x;
        float posY = (Camera.main.pixelHeight) - Camera.main.WorldToScreenPoint(this.GetComponent<Transform>().position).y;
        float height = 4;

        float percentage = healthBarWidth * (this.GetComponent<Carac>().SuchHP() / maxHP);
        float HPMax = healthBarWidth;

        GUI.DrawTexture(new Rect(posX - decalX, posY + decalY, HPMax, height), red);
        GUI.DrawTexture(new Rect(posX - decalX, posY + decalY, (percentage), height), green);
        
    }
}