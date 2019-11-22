﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    public enum ProtagGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    public ProtagGUI ProtagInput;

    public List<GameObject> ProtagsToManage = new List<GameObject>();
    private HandleTurns ProtagChoice;

    public GameObject enemyButton;
    public Transform Spacer;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;


    void Start()
    {
        currentAction = PreformAction.WAIT;
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        ProtagsInBattle.AddRange(GameObject.FindGameObjectsWithTag("Protag"));

        ProtagInput = ProtagGUI.ACTIVATE;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);
        
        EnemyButtons();
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
                    ProtagStateMachine PSM = preformer.GetComponent<ProtagStateMachine>();
                    PSM.EnemyToAttack = PreformList[0].AttackersTarget;
                    PSM.currentState = ProtagStateMachine.TurnState.ACTION;

                }
                currentAction = PreformAction.PREFORMACTION;
                
                break;
            case (PreformAction.PREFORMACTION):
                //idle
        break;
        }

        switch (ProtagInput)
        {
                case (ProtagGUI.ACTIVATE):
                if(ProtagsToManage.Count > 0)
                {
                    ProtagsToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    ProtagChoice = new HandleTurns();
                    

                    AttackPanel.SetActive(true);
                    ProtagInput = ProtagGUI.WAITING;
                }
                    break;

            case (ProtagGUI.WAITING):
                //idle
                break;

            case (ProtagGUI.DONE):
                ProtagInputDone();
                break;
        }

    }

   public void CollectAction(HandleTurns input)
    {
        PreformList.Add(input);
    }

    void EnemyButtons()
    {
        foreach(GameObject enemy in EnemiesInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();

            buttonText.text = cur_enemy.enemy.name;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer,false);
                
        }
    }

    public void Input1() // attack button
    {
        ProtagChoice.Attacker = ProtagsToManage[0].name;
        ProtagChoice.AttackersGameObject = ProtagsToManage[0];
        ProtagChoice.Type = "Protag";

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }

    public void Input2(GameObject choosenEnemy)
    {
        ProtagChoice.AttackersTarget = choosenEnemy;
        ProtagInput = ProtagGUI.DONE;
    }

    void ProtagInputDone()
    {
        PreformList.Add(ProtagChoice);
        EnemySelectPanel.SetActive(false);
        ProtagsToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        ProtagsToManage.RemoveAt(0);
        ProtagInput = ProtagGUI.ACTIVATE;
    }
}