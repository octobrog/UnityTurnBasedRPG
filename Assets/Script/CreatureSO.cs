using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creature Creator", fileName = "New Creature")]
public class CreatureSO : ScriptableObject
{
    
    [SerializeField]float maxHealth;
    [SerializeField]float attack;
    [SerializeField]float defense;
    bool onDefense = false;
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
        return health;
    }

    public void setHealth(float hp){
        this.health = hp;
    }

    public void setOnDefense(){
        onDefense = !onDefense;
    }
}
