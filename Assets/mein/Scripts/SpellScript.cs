using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour {
    public bool Opponent_Focus;
    public bool Launch_On_Ground;

    public int Skill_Shot_Speed;
    public int Area;
    public int Slow_Value;
    public int Slow_Time;
    public int DOT_damage;
    public float DOT_Time;
    public int Direct_Damage;
    public int Damage_On_Mana;
    public float Stunt_Time;

    public float cast_time;
    public bool isCanalised;
    public float range;
    public int mana_Cost;
    public Animation anim;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
