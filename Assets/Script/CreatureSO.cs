using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creature Creator", fileName = "New Creature")]
public class CreatureSO : ScriptableObject
{
    //param for max creature health, immutable from gameplay as a constant
    [SerializeField]float maxHealth;

    //attack and defense attribute, will probably be split like health to facilitate de/buff
    [SerializeField]float attack;
    [SerializeField]float defense;

    //flag to show creature is defending or not
    bool onDefense = false;

    //mutable health point for calculating dmg / heal
    float health;

    public float getMaxHealth(){
        return maxHealth;
    }

    public float getHealth(){
        return health;
    }

    public float getAttack(){
        return attack;
    }

    public float getDefense(){
        return defense;
    }
    
    public bool getOnDefense(){
        return onDefense;
    }

    public float damageHealth(float damage){
        health = health - damage;
        if (health > maxHealth){
            health = maxHealth;
        } 
        if (health < 0){
            health = 0;
        }
        return health;
    }

    //setter for health on init combat
    public void setHealth(float hp){
        this.health = hp;
    }

    public void setAttack(float attack){
        this.attack = attack;
    }

    public void setDefense(float defense){
        this.defense = defense;
    }

    public void setOnDefense(){
        onDefense = !onDefense;
    }
}
