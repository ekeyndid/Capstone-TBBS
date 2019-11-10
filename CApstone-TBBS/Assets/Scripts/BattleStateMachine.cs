using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleStateMachine : MonoBehaviour
{
    public enum PreformAction
    {
        WAIT,
        TAKEACTION,
        PREFORMACTION
    }
    public PreformAction currentAction;

    public List<HandleTurns> PreformList = new List<HandleTurns>();
    public List<GameObject> ProtagsInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();

    void Start()
    {
        currentAction = PreformAction.WAIT;
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        ProtagsInBattle.AddRange(GameObject.FindGameObjectsWithTag("Protag"));

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentAction)
        {
            case (PreformAction.WAIT):
                if (PreformList.Count > 0)
                {
                    currentAction = PreformAction.TAKEACTION;
                }
        break;

            case (PreformAction.TAKEACTION):
                GameObject preformer = GameObject.Find(PreformList[0].Attacker);
                if(PreformList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = preformer.GetComponent<EnemyStateMachine>();
                    ESM.ProtagToAttack = PreformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }

                if (PreformList[0].Type == "Protag")
                {

                }
                currentAction = PreformAction.PREFORMACTION;
                
                break;
            case (PreformAction.PREFORMACTION):
        break;
    }

    }

   public void CollectAction(HandleTurns input)
    {
        PreformList.Add(input);
    }
}
