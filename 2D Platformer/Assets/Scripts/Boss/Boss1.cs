using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{

    public GameObject punch;        // soco do Boss
    Rigidbody2D rb;

    public bool isRight = false;    // se está direita ou esquerda
    public float xVal = 100;        // velocidade de movimento

    [Header("Detecta Player")]
    public int hurt;               // verifica se player tocar no colisor de dano do boss
    public GameObject polyCollisor; // obj de dano do Boss
    private Animator anim;

    [Header("Posicao do Player")]
    public Transform playerTrans;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTrans.position.x < transform.position.x){
            //Esquerda
            isRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }else{
            // Direita
            isRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }

    }

    public void AttackActive(){
        punch.SetActive(true);

        if(isRight){
            Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
            rb.velocity = targetVelocity;
        }else{
            Vector2 targetVelocity = new Vector2(-xVal, rb.velocity.y);
            rb.velocity = targetVelocity;
        }
    }

    public void AttackDisable(){
        punch.SetActive(false);
    }

    public void Hurt(){
        hurt --;;
        anim.SetBool("Hurt", true);     // chama animação de dano
        Invoke("Combat", 1);            //
    }

    public void Combat(){
        anim.SetBool("Hurt", false);     // chama animação de dano
        anim.SetTrigger("Attack");      // chama animação de dano
        polyCollisor.SetActive(true);
    }

}
