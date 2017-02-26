using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Carac : MonoBehaviour {
    
    public int MLife;
    public int team;
    public int MArmor;
    public int MDamage;
    public float MAspeed;
    public float MMs;
    public float MArange;
    public float AggroRange;
    public int MMana;
    public int ManaRegen;
    public int HPRegen;
    public bool isRanged;
    public bool spells;

    private int damage;
    private float Aspeed;
    private float ms;
    private float Arange;
    private int clife;
    private int armor;
    private GameObject Weapon;
    private GameObject SpellFolder;
    public GameObject HPSlider;
    public GameObject ManaSlider;
    private bool HPslid;
    private bool Manaslid;
    private float Silenced;
    private float Stunt;
    private float SlowPercent;
    private float Slow;
    private float Incant_Time;
    private int mana;
    private ArrayList DotList;

    public class Dot
    {
        public float time;
        public float damageParSec;

        public Dot(int damage, float t)
        {
            time = t;
            damageParSec = damage;
        }
    }

    public void Attaque(GameObject cible)
    {
        if (isRanged && Weapon!=null)
        {
            GameObject arrow;
            arrow = Instantiate(Weapon);
            arrow.GetComponent<Transform>().position = this.GetComponent<Transform>().position;
            arrow.GetComponent<WeaponScript>().cible = cible;
            arrow.GetComponent<WeaponScript>().damage = damage;
            arrow.SetActive(true);
        }
        else
        {
            cible.GetComponent<Carac>().damaged(ManyDamage());
        }
    }

    public bool CanBeLaunched(int num,float distance,GameObject target)
    {
       
        string s = "spell" + num;
        GameObject spell_launched = SpellFolder.transform.FindChild(s).gameObject;

        bool opponent_Focus = spell_launched.GetComponent<SpellScript>().Opponent_Focus;
        bool Launch_On_Ground = spell_launched.GetComponent<SpellScript>().Launch_On_Ground;
        int mana_Cost = spell_launched.GetComponent<SpellScript>().mana_Cost;
        float range = spell_launched.GetComponent<SpellScript>().range;

        if (distance > range) return false;
        if (target.tag != "Ground")
        {
            if ((opponent_Focus != (target.GetComponent<Carac>().team != team))) return false;
        }
        else
        {
            if (!Launch_On_Ground) return false;
        }
        if (mana < mana_Cost) return false;
        if (Silenced>0) return false;

        return true;
    }

    public void launchSpell(int num, Vector3 position, GameObject target)
    {
        string s = "spell" + num;
        GameObject spell_launched=SpellFolder.transform.FindChild(s).gameObject;

        bool opponent_Focus = spell_launched.GetComponent<SpellScript>().Opponent_Focus; //done
        bool Launch_On_Ground = spell_launched.GetComponent<SpellScript>().Launch_On_Ground;//done
        int Skill_Shot_Speed = spell_launched.GetComponent<SpellScript>().Skill_Shot_Speed; 
        int Area = spell_launched.GetComponent<SpellScript>().Area;  //done
        int Slow_Value = spell_launched.GetComponent<SpellScript>().Slow_Value; //done
        int Slow_Time = spell_launched.GetComponent<SpellScript>().Slow_Time; //done
        int DOT_damage = spell_launched.GetComponent<SpellScript>().DOT_damage;
        float DOT_Time = spell_launched.GetComponent<SpellScript>().DOT_Time;
        int Direct_Damage = spell_launched.GetComponent<SpellScript>().Direct_Damage; //done
        int Damage_On_Mana = spell_launched.GetComponent<SpellScript>().Damage_On_Mana;
        int mana_Cost = spell_launched.GetComponent<SpellScript>().mana_Cost; //done
        float Stunt_Time = spell_launched.GetComponent<SpellScript>().Stunt_Time; //done

        float cast_time = spell_launched.GetComponent<SpellScript>().cast_time;//done
        bool isCanalised = spell_launched.GetComponent<SpellScript>().isCanalised;
        float range = spell_launched.GetComponent<SpellScript>().range; //done

        Collider[] hitColliders;
        mana -= mana_Cost;
        if (Area > 0)
        {
           hitColliders = Physics.OverlapSphere(position, Area);
        }
        else
        {
            hitColliders = new Collider[1];
            hitColliders[0] = target.GetComponent<Collider>();
        }
        if (opponent_Focus) // Effet sur adversaire
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag != "Ground" && hitColliders[i].GetComponent<Carac>().team != team)
                {
                    Carac c = hitColliders[i].GetComponent<Carac>();
                    c.Slow += Slow_Time;
                    c.SlowPercent += Slow_Value;
                    c.Stunt += Stunt_Time;
                    c.magic_damage(Direct_Damage);
                    c.Incant_Time += cast_time;
                    if (DOT_damage > 0)
                    {
                        Dot d = new Dot(DOT_damage,DOT_Time);
                        DotList.Add(d);
                    }


                }
            }
        }
        else // Effet sur les alliés
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag != "Ground" && hitColliders[i].GetComponent<Carac>().team == team)
                {
                    //Effet
                }
            }
        }

    }

    public float SuchHP()
    {
        return clife;
    }
    public int ManyDamage()
    {
        return damage;
    }

    public float ManyRange()
    {
        return Arange;
    }

    public float SuchMSpeed()
    {
        if (Slow > 0)
        {
            return (ms * SlowPercent / 100);
        }
        return ms;
    }

    public float SuchASpeed()
    {
        return Aspeed;
    }

    public void healed(int heal)
    {
        clife += heal;
        if (HPslid)
        {
            HPSlider.GetComponent<Slider>().value = clife;
        }
    }

    public bool damaged(int dmg)
    {
        if (dmg > armor) clife -= (dmg - armor);
        if (HPslid)
        {
            HPSlider.GetComponent<Slider>().value = clife;
        }
        if (clife <= 0)
        {

            Destroy(this.gameObject);
            return true;
        }

        return false;
    }

    public bool magic_damage(int dmg)
    {
        clife -= dmg;
        if (HPslid)
        {
            HPSlider.GetComponent<Slider>().value = clife;
        }
        if (clife <= 0)
        {

            Destroy(this.gameObject);
            return true;
        }

        return false;
    }

	// Use this for initialization
	void Start () {
        damage = MDamage;
        Aspeed = MAspeed;
        ms = MMs;
        Arange = MArange;
        clife = MLife;
        armor = MArmor;
        Silenced = 0;
        Slow = 0;
        SlowPercent = 0;
        Stunt = 0;
        mana = MMana;
        DotList = new ArrayList();
        if (HPSlider != null)
        {
            HPslid = true;
            HPSlider.GetComponent<Slider>().maxValue = MLife;
            HPSlider.GetComponent<Slider>().value = clife;
        }
        if (ManaSlider != null)
        {
            Manaslid = true;
            ManaSlider.GetComponent<Slider>().maxValue = MMana;
            ManaSlider.GetComponent<Slider>().value = mana;
        }

        if (spells){
            SpellFolder = transform.FindChild("spells").gameObject;
        }
        if (isRanged)
        {
            Weapon= transform.FindChild("weapon").gameObject;
        }
        if(HPRegen>0 || ManaRegen>0) InvokeRepeating("Regen", 0, 1.0f);
    }
	
    void Regen()
    {
        clife += HPRegen;
        mana += ManaRegen;
        if (clife > MLife) clife = MLife;
        if (mana > MMana) mana = MMana;
        if (HPslid)
        {
            HPSlider.GetComponent<Slider>().value = clife;
        }
        if (Manaslid)
        {
            ManaSlider.GetComponent<Slider>().value = mana;
        }
        if (DotList.Capacity >0)
        {

        }
    }
    public bool CanMove()
    {
        if (Stunt > 0) return false;
        if (Incant_Time > 0) return false;
        return true;
    }

    public void Slowed(float percent,float time)
    {
        Slow = time;
        SlowPercent = percent;
    }

    void StateUpdate()
    {
        if(Silenced>0) Silenced -= Time.deltaTime;
        if(Stunt>0) Stunt -= Time.deltaTime;
        if(Slow > 0) Slow -= Time.deltaTime;
        if (Incant_Time > 0) Incant_Time -= Time.deltaTime;
    }
	// Update is called once per frame
	void Update () {
        if (Stunt > 0 || Silenced > 0 || Slow>0 || Incant_Time>0) StateUpdate();
	}
}
