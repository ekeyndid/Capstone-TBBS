using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Obsolete]
public class EnemySelectButton : MonoBehaviour
{


    public GameObject EnemyPrefab;
    private GameObject Selector;
   
    public void SelectEnemy()
    {
        HideSelector();
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input2(EnemyPrefab); //save input of enemy prefab
        


    }

   
    public void HideSelector()
    {

            EnemyPrefab.transform.Find("Selector").gameObject.SetActive(false);
    }
    public void ShowSelector()
    {

        EnemyPrefab.transform.Find("Selector").gameObject.SetActive(true);
    }

}
