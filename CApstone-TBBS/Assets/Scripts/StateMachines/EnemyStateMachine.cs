using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : MonoBehaviour
{
    public BaseEnemy enemy;

    private BattleStateMachine BSM;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }
    public TurnState currentState;
    //for the ProgressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 10f;
    public GameObject Selector;
    private Vector3 startposition;
    // the things for TimeForAction
    private bool ActionStarted = false;
    public GameObject ProtagToAttack;
    private float animSpeed = 10f;
    
    void Start()
    {
        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        Selector.SetActive(false);
        startposition = transform.position;
    }

    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpgradeProgressBar();
                break;
            case (TurnState.CHOOSEACTION):
                ChooseAction();
                currentState = TurnState.WAITING;
                break;
            case (TurnState.WAITING):
                //idle state
                break;
            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;
            case (TurnState.DEAD):
                break;
        }
    }

    void UpgradeProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
     
        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction()
    {
        HandleTurns myAttack = new HandleTurns();
        myAttack.Attacker = enemy.thename;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.ProtagsInBattle[Random.Range(0, BSM.ProtagsInBattle.Count)];
        BSM.CollectAction(myAttack);
    }

    private IEnumerator TimeForAction()
    {
        if (ActionStarted)
        {
            yield break;
        }
        
        ActionStarted = true;

        //animate the enemy near the hero to attack
        Vector3 ProtagPosition = new Vector3(ProtagToAttack.transform.position.x-1.5f, ProtagToAttack.transform.position.y, ProtagToAttack.transform.position.z);
        while (MoveTowardsEnemy(ProtagPosition)) {yield return null;}
        //wait a bit
        yield return new WaitForSeconds(0.5f);
        //do damnage

        //animate back to startpositiom
        Vector3 FirstPosition = startposition;
        while (MoveTowardsStart(FirstPosition)) { yield return null; }


        //remove this preformer from the list in BSM
        BSM.PreformList.RemoveAt(0);
        //restart BSM --> WAIT
        BSM.currentAction = BattleStateMachine.PreformAction.WAIT;
        //end courtine
        ActionStarted = false;
        //reset this enemy's state
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;

       
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
}
