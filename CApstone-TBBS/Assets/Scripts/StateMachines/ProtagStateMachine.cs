using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtagStateMachine : MonoBehaviour
{
    private BattleStateMachine BSM;

    public BaseProtag protag;

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }
    public TurnState currentState;
    //for the ProgressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;
    public GameObject Selector;
    //IeNumerator
    public GameObject EnemyToAttack;
    private bool ActionStarted = false;
    private Vector3 startposition;
    private float animSpeed = 10f;


    void Start()
    {
        startposition = transform.position;
        cur_cooldown = Random.Range(0, 2.5f);
        Selector.SetActive(false);
        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
    }

    void Update()
    {
        //Debug.Log(currentState);
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                    UpgradeProgressBar ();
                break;
            case (TurnState.ADDTOLIST):
                BSM.ProtagsToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;
                break;
            case (TurnState.WAITING):
                //idle
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
        float calc_cooldown = cur_cooldown / max_cooldown;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if(cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.ADDTOLIST;
        }
    }


    private IEnumerator TimeForAction()
    {
        if (ActionStarted)
        {
            yield break;
        }

        ActionStarted = true;

        //animate the enemy near the hero to attack
        Vector3 EnemyPosition = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
        while (MoveTowardsEnemy(EnemyPosition)) { yield return null; }
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
