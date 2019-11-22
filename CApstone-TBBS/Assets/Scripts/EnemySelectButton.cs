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

            EnemyPrefab.transform.FindChild("Selector").gameObject.SetActive(false);
    }
    public void ShowSelector()
    {

        EnemyPrefab.transform.FindChild("Selector").gameObject.SetActive(true);
    }

}
