﻿using System.Collections;
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
    private Image ProgressBar;
    public Color ProgColor;
    public GameObject Selector;
    //IeNumerator
    public GameObject EnemyToAttack;
    private bool ActionStarted = false;
    private Vector3 startposition;
    private float animSpeed = 10f;
    private bool alive = true;
    private Text MagicT;
    private ProtagPanelStats stats;
    public GameObject ProtagPanel;
    private Transform ProtagPanelSpacer;


    void Start()
    {
        // find spacer
        ProtagPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("ProtagPanel").transform.Find("ProtagPanelSpacer");
       
        //create panel, fill in info
        CreateProtagPanel();
       
        ProgColor = ProgressBar.color;
        startposition = transform.position;
        cur_cooldown = Random.Range(0, 2.5f);
        
        Selector.SetActive(false);
        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        MagicT = BSM.MagicName.transform.Find("T").gameObject.GetComponent<Text>();
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
                if (!alive)
                {
                    return;
                }
                else
                {
                    //change tag of protags
                    this.gameObject.tag = "DeadProtag";
                     //not attackable by any enemy
                    BSM.ProtagsInBattle.Remove(this.gameObject);
                    //not managable
                    BSM.ProtagsToManage.Remove(this.gameObject);
                    // deactivate the selector
                    Selector.SetActive(false);
                    //reset GUI
                    BSM.AttackPanel.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
                    //remove item from preform list
                    if(BSM.ProtagsInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PreformList.Count; i++)
                        {
                            if (BSM.PreformList[i].AttackersGameObject == this.gameObject)
                            {
                                BSM.PreformList.Remove(BSM.PreformList[i]);
                            }

                            if (BSM.PreformList[i].AttackersTarget == this.gameObject)
                            {
                                BSM.PreformList[i].AttackersTarget = BSM.ProtagsInBattle[Random.Range(0, BSM.ProtagsInBattle.Count)];
                            }
                        }
                    }
                   
                    //change color / play animation
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                    //reset protaginput
                    BSM.currentAction = BattleStateMachine.PreformAction.CHECKALIVE;

                    alive = false;
                }

                break;
        }
    }

    void UpgradeProgressBar()
    {
        ProgressBar.color = ProgColor;
       // print(Time.deltaTime);
        float Modifier = protag.agility / 15;
        float Timeo = Time.deltaTime;
        cur_cooldown = cur_cooldown + (Timeo * Modifier);
        //print(cur_cooldown + " of "+ protag.thename);
        float calc_cooldown = cur_cooldown / max_cooldown;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if(cur_cooldown >= max_cooldown)
        {
            ProgressBar.color = new Color32(200,200,0,255);
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
        if (BSM.PreformList[0].ChooseAttack.ismagic)
        {
            if(protag.currMP >= BSM.PreformList[0].ChooseAttack.ManaCost)
            {
                protag.currMP = protag.currMP - BSM.PreformList[0].ChooseAttack.ManaCost;
                UpdateProtagPanel();
                MagicT.text = BSM.PreformList[0].ChooseAttack.AttackName;
                BSM.MagicName.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                Vector3 EnemyPosition = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
                while (MoveTowardsEnemy(EnemyPosition)) { yield return null; }
                BSM.EFFECTS.GetComponent<PlayAnims>().Decide(MagicT.text = BSM.PreformList[0].ChooseAttack.AttackName, EnemyToAttack.GetComponent<EnemyStateMachine>().GiveGuiPos());
                yield return new WaitForSeconds(2f);
                DoDamage(BSM.PreformList[0].ChooseAttack.Type);
                BSM.MagicName.gameObject.SetActive(false);
            }
            else
            {
                MagicT.text = "Not Enough Mana!";
                BSM.MagicName.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                BSM.MagicName.gameObject.SetActive(false);
            }
            
        }
        else if (!BSM.PreformList[0].ChooseAttack.ismagic)
        {
            Vector3 EnemyPosition = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
            while (MoveTowardsEnemy(EnemyPosition)) { yield return null; }
            //wait a bit
            yield return new WaitForSeconds(0.5f);
            DoDamage(BSM.PreformList[0].ChooseAttack.Type);
        }
        //animate the enemy near the hero to attack
       

        //animate back to startpositiom
        Vector3 FirstPosition = startposition;
        while (MoveTowardsStart(FirstPosition)) { yield return null; }


        //remove this preformer from the list in BSM
        BSM.PreformList.RemoveAt(0);
        //restart BSM --> WAIT
        if (BSM.currentAction != BattleStateMachine.PreformAction.WIN && BSM.currentAction != BattleStateMachine.PreformAction.LOSE)
        {
            BSM.currentAction = BattleStateMachine.PreformAction.WAIT;
            //end courtine
            ActionStarted = false;
            //reset this enemy's state
            cur_cooldown = 0f;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }

    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    public void TakeDamage(float DamageA)
    {
        float calc_dmg = DamageA - protag.currDEF;
        print("Took "+calc_dmg);
        if (calc_dmg < 0) { calc_dmg = 0; };
        protag.currHP -= calc_dmg;
        if (protag.currHP <= 0) {
            currentState = TurnState.DEAD;
            protag.currHP = 0;   
        };
        UpdateProtagPanel();
        
    }

    void DoDamage(string Type)
    {
         float calc_dmg;
        if (Type == "Magic")
        {
            print("magic");
            calc_dmg = (protag.intellect * 2) + BSM.PreformList[0].ChooseAttack.Damage;
        }
        else if (Type == "STR")
        {
            calc_dmg = (protag.stamina * 1.5f) + BSM.PreformList[0].ChooseAttack.Damage;
        }
        else if(Type == "Normal")
        {
            calc_dmg = protag.currATK + BSM.PreformList[0].ChooseAttack.Damage;
        }
        else { calc_dmg = 0; }

        Debug.Log(this.gameObject + " used " + BSM.PreformList[0].ChooseAttack.AttackName + " for " + (calc_dmg) + " Arrox. dmg!");
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_dmg);



    }
//create a panel for player
    void CreateProtagPanel()
    {
        ProtagPanel = Instantiate(ProtagPanel) as GameObject;
        stats = ProtagPanel.GetComponent<ProtagPanelStats>();
        stats.ProtagName.text = protag.thename;
        stats.ProtagHP.text = "HP: " + protag.currHP;
        stats.ProtagMP.text = "MP: " + protag.currMP;


        ProgressBar = stats.ProgressBar;
        ProtagPanel.transform.SetParent(ProtagPanelSpacer, false);
    }

    void UpdateProtagPanel()
    {
        stats.ProtagHP.text = "HP: " + protag.currHP;
        stats.ProtagMP.text = "MP: " + protag.currMP;
    }

    public Vector2 GiveGuiPos()
    {
        return protag.GUIPosition;
    }
}
