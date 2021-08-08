using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalLevel : MonoBehaviour
{

    public GameObject cCherrys;     // container das cherrys
    public Crank openGate;          // pega var que ativa portão
    public GameObject uiCherrys;    // UI das Cherrys

    [Header("Boss Refs")]
    public GameObject boss;
    public GameObject headBoss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collison) {
        if(collison.tag == "Player"){
            
            if(FindObjectOfType<LevelManager>().isChoice){
                cCherrys.SetActive(true);
                BossStopped();                  // boss fica neutro
                Invoke("fLevel", 3);            // deixa o player ir embora
            }else{
                Invoke("Fight", 4);             // luta com o player
            }
        }
    }

    private void fLevel(){
        openGate.isActive = true;           // ativa portão
        disableObjs();
    }

    private void Fight(){
        FindObjectOfType<Boss1>().xVal = 5; // faz atacar fraco
        FindObjectOfType<Boss1>().Up();     // faz boss atacar
        disableObjs();
    }

    public void BossStopped(){
        boss.GetComponent<Rigidbody2D>().isKinematic = true;
        boss.GetComponent<CapsuleCollider2D>().enabled = false;
        headBoss.SetActive(false);
    }

    private void disableObjs(){
        uiCherrys.SetActive(false);                                 // desativa UI das cerejas
        gameObject.GetComponent<BoxCollider2D>().enabled = false;   // desativa o colisor de dialogo
        FindObjectOfType<Player>().isNotMovePlayer = false;         // deixa player se mover
    }

}
