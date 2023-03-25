using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleScript : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] Slider[] playerHP;
    [SerializeField] Slider[] enemyHP;

    [Header("Action")]
    [SerializeField] GameObject[] actionButtons;

    [Header("Player")]
    [SerializeField] List<CreatureSO> player = new List<CreatureSO>();

    [Header("Enemy")]
    [SerializeField] List<CreatureSO> enemy = new List<CreatureSO>();

    struct AttackParam {
        public CreatureSO attacker;
        public CreatureSO defender;

        public AttackParam(CreatureSO attacker, CreatureSO defender){
            this.attacker = attacker;
            this.defender = defender;
        }
    }

    const string Attack = "attack";
    const string Defense = "defense";

    bool enemyTurn = false;
    string playerAction;

    void Awake() {
        playerHP[0].maxValue = player[0].getMaxHealth();
        enemyHP[0].maxValue = enemy[0].getMaxHealth();
        player[0].setHealth(player[0].getMaxHealth());
        enemy[0].setHealth(enemy[0].getMaxHealth());
    }

    void Update() {
        if (enemyTurn){
            EnemyAction();
            enemyTurn = false;
        }
        playerHP[0].value = player[0].getHealth();
        enemyHP[0].value = enemy[0].getHealth();
    }

    void AttackAction(AttackParam action){
        float damage;
        if (action.defender.getOnDefense()) {
            damage = action.attacker.getAttack() - 1.5f*(action.defender.getDefense());
            damage = Mathf.Floor(damage);
            ToogleDefense(action.defender);
        }else{
            damage = action.attacker.getAttack() - (action.defender.getDefense());
        }

        if (damage > 0){
            action.defender.damageHealth(damage);
        }else{
            action.defender.damageHealth(1f);
        }
    }

    void ToogleDefense(CreatureSO action){
        action.setOnDefense();
    }

    public void PlayerAttack(){
        AttackParam param = new AttackParam(player[0],enemy[0]);
        AttackAction(param);
        enemyTurn = true;
        playerAction = Attack;
    }

    public void PlayerDefense(){
        if (!player[0].getOnDefense()){
            ToogleDefense(player[0]);
            enemyTurn = true;
            playerAction = Defense;
        }
    }

    void EnemyAction(){
        switch (playerAction)
        {
            case Attack:
                AttackParam param = new AttackParam(enemy[0],player[0]);
                AttackAction(param);
                break;
            case Defense:
                ToogleDefense(enemy[0]);
                break;
            default:
                break;
        }
    }
}
