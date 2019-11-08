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
            
        break;

            case (PreformAction.TAKEACTION):
                break;

            case (PreformAction.PREFORMACTION):
        break;
    }

    }
}
