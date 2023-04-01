using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//class for controling battle scenario
public class BattleScript : MonoBehaviour
{    
    const string Attack = "attack";
    const string Defense = "defense";

    [Header("Health Bar")]
    [SerializeField] Slider[] playerHP;
    [SerializeField] Slider[] enemyHP;
    List<TextMeshProUGUI> PlayerHPText = new List<TextMeshProUGUI>();
    List<TextMeshProUGUI> EnemyHPText = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> PlayerDamageTakenText = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> EnemyDamageTakenText = new List<TextMeshProUGUI>();

    [Header("Action")]
    [SerializeField] GameObject[] actionButtons;

    [Header("Player")]
    [SerializeField] List<CreatureSO> player = new List<CreatureSO>();

    [Header("Enemy")]
    [SerializeField] List<CreatureSO> enemy = new List<CreatureSO>();

    //struct containing param when attacking
    struct AttackParam {
        public CreatureSO attacker;
        public CreatureSO defender;

        public AttackParam(CreatureSO attacker, CreatureSO defender){
            this.attacker = attacker;
            this.defender = defender;
        }
    }

    //enemy turn indicator
    bool enemyTurn = false;

    //data to determine AI action based on player, temp and will be changed to struct
    string playerAction;

    //setter for enemy and player object
    public void enemySetter(List<CreatureSO> enemy){
        this.enemy = enemy;
    }

    public void playerSetter(List<CreatureSO> player){
        this.player = player;
    }

    void Awake() {
        setBattleScene();
    }

    // void Update() {}

    //func to control player and enemy turn
    void TurnManager(){
        //TO DO : add turn and damage indicator
        playerHP[0].value = player[0].getHealth();
        enemyHP[0].value = enemy[0].getHealth();
        PlayerHPText[0].text = player[0].getHealth().ToString("R");
        EnemyHPText[0].text = enemy[0].getHealth().ToString("R");

        if (enemyTurn){
            EnemyAction();
        }
    }

    //func to set battle scene UI
    void setBattleScene(){
        //set hp bar max value
        playerHP[0].maxValue = player[0].getMaxHealth();
        enemyHP[0].maxValue = enemy[0].getMaxHealth();

        //set hp count text
        PlayerHPText.Add(playerHP[0].GetComponentInChildren<TextMeshProUGUI>());
        EnemyHPText.Add(enemyHP[0].GetComponentInChildren<TextMeshProUGUI>());
        PlayerHPText[0].text = player[0].getMaxHealth().ToString("R");
        EnemyHPText[0].text = enemy[0].getMaxHealth().ToString("R");

        //return health to full
        player[0].setHealth(player[0].getMaxHealth());
        enemy[0].setHealth(enemy[0].getMaxHealth());
    }

    //func called when either side are attacking, need to split player and enemy?
    void AttackAction(AttackParam action){
        //set on defense false when attacking
        if (action.attacker.getOnDefense()){
            ToogleDefense(action.attacker);
        }

        float damage;

        if (action.defender.getOnDefense()) {
            damage = action.attacker.getAttack() - 1.5f*(action.defender.getDefense());
            damage = Mathf.Floor(damage);
            ToogleDefense(action.defender);
        }else{
            damage = action.attacker.getAttack() - (action.defender.getDefense());
        }

        if (damage <= 0){
            damage = 1f;
        }

        action.defender.damageHealth(damage);
        if(enemyTurn){
            Debug.Log("player " + damage.ToString("R"));
            PlayerDamageTakenText[0].text = damage.ToString("R") + " dmg";
        }else{
            Debug.Log("enemy "+damage.ToString("R"));
            EnemyDamageTakenText[0].text = damage.ToString("R")  + " dmg";
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
        TurnManager();
    }

    public void PlayerDefense(){
        if (!player[0].getOnDefense()){
            ToogleDefense(player[0]);
            enemyTurn = true;
            playerAction = Defense;
            TurnManager();     
        }
    }

    //func to determine enemy action
    //will be moved to their SO so each archetype have unique AI
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
        enemyTurn = false;
        TurnManager(); 
    }
}
