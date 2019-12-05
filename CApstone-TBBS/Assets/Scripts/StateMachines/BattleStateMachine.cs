using System.Collections;
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
    public Transform EnemySelectSpacer;
    public Transform AttackSpacer;
    public Transform MagicSpacer;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
    public GameObject MagicPanel;
    public GameObject actionbutton;
    public GameObject magicbutton;
    private List<GameObject> Buttons = new List<GameObject>();




    void Start()
    {
        currentAction = PreformAction.WAIT;
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        ProtagsInBattle.AddRange(GameObject.FindGameObjectsWithTag("Protag"));

        ProtagInput = ProtagGUI.ACTIVATE;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);
        MagicPanel.SetActive(false);

        
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
                    for(int i = 0; i < ProtagsInBattle.Count; i++)
                    {
                        if(PreformList[0].AttackersTarget == ProtagsInBattle[i])
                        {
                            ESM.ProtagToAttack = PreformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                            break;
                        }
                        else
                        {
                            PreformList[0].AttackersTarget = ProtagsInBattle[Random.Range(0, ProtagsInBattle.Count)];

                            ESM.ProtagToAttack = PreformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                        }
                    }

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
                    CreateAttackButtons();
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

            buttonText.text = cur_enemy.enemy.thename;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(EnemySelectSpacer,false);
                
        }
    }

    public void Input1() // attack button
    {
        ProtagChoice.Attacker = ProtagsToManage[0].name;
        ProtagChoice.AttackersGameObject = ProtagsToManage[0];
        ProtagChoice.Type = "Protag";
        ProtagChoice.ChooseAttack = ProtagsToManage[0].GetComponent<ProtagStateMachine>().protag.MeleeAttacks[0];
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
        foreach(GameObject actionbutton in Buttons)
        {
            Destroy(actionbutton);
        }
        Buttons.Clear();
        ProtagsToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        ProtagsToManage.RemoveAt(0);
        ProtagInput = ProtagGUI.ACTIVATE;

    }
    
    
    void CreateAttackButtons()
    {
        GameObject AttackButton = Instantiate(actionbutton) as GameObject;
        Text AttackButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        AttackButtonText.text = "Attack";
        AttackButton.GetComponent<Button>().onClick.AddListener(()=>Input1());
        AttackButton.transform.SetParent(AttackSpacer, false);
        Buttons.Add(AttackButton);


        GameObject MagicAttackButton = Instantiate(actionbutton) as GameObject;
        Text MagicAttackButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        MagicAttackButtonText.text = "Magic";
        MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
        //magic panel shit here
        MagicAttackButton.transform.SetParent(AttackSpacer, false);
        Buttons.Add(MagicAttackButton);

        if(ProtagsToManage[0].GetComponent<ProtagStateMachine>().protag.MagicAttacks.Count > 0)
        {
            foreach(BaseAttack magicAtk in ProtagsToManage[0].GetComponent<ProtagStateMachine>().protag.MagicAttacks)
            {
                GameObject MagicButton = Instantiate(magicbutton) as GameObject;
                Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
                MagicButtonText.text = magicAtk.AttackName;
                AttackButton ATB = MagicButton.GetComponent<AttackButton>();
                ATB.magicAttackToPreform = magicAtk;
                MagicButton.transform.SetParent(MagicSpacer, false);
                Buttons.Add(MagicButton);
            }
        }
        else
        {
            MagicAttackButton.GetComponent<Button>().interactable = false;
        }
    }
    public void Input4(BaseAttack choosenMagic) // chosen magic attack
    {
        ProtagChoice.Attacker = ProtagsToManage[0].name;
        ProtagChoice.AttackersGameObject = ProtagsToManage[0];
        ProtagChoice.Type = "Protag";

        ProtagChoice.ChooseAttack = choosenMagic;
        MagicPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }

    public void Input3() //switching to magic attacks
    {
        AttackPanel.SetActive(false);
        MagicPanel.SetActive(true);
    }
}
