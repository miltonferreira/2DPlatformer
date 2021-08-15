using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{

    public GameObject punch;                // soco do Boss
    private bool isChangeDir = true;        // indica que pode trocar direção esquerda ou direita
    
    Rigidbody2D rb;

    public bool isRight = false;        // se está direita ou esquerda
    public float xVal = 100;            // velocidade de movimento

    [Header("Detecta Player")]
    public int hurt;                    // verifica se player tocar no colisor de dano do boss
    private bool isHurt;                    // checa se está no estado de machucado
    public GameObject polyCollisor;     // obj de dano do Boss
    private Animator anim;

    [Header("Posicao do Player")]
    public Transform playerTrans;

    // dialogo quando Player derrota boss
    [Header("Diag derrotado")]
    public Sign sign;

    [Header("Troca foco Cam")]
    public CameraSolo camSolo;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        LookAtDir();

    }

    IEnumerator LoopAttack(){
        yield return new WaitForSeconds(2f);
        if(!isHurt) Up();   // se não tiver em estado de machucado pode atacar
    }

    public void LookAtDir(){

        if(isChangeDir && hurt>0)
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

    // Boss ataca
    public void AttackActive(){

        FreezeRotation();               // freeze rotation

        isChangeDir = false;            // indica que não pode trocar direção

        AudioManager.instance.PlaySFX("boss");

        punch.SetActive(true);          // ativa soco do boss

        if(isRight){
            Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
            rb.velocity = targetVelocity;
        }else{
            Vector2 targetVelocity = new Vector2(-xVal, rb.velocity.y);
            rb.velocity = targetVelocity;
        }
    }

    // finaliza ataque
    public void AttackDisable(){
        Freeze();                           // freeze X e rotation
        punch.SetActive(false);             // desativa soco do boss
        isChangeDir = true;                 // indica que pode trocar direção
        xVal = (xVal==5) ? 15:15;
        polyCollisor.SetActive(true);       // ativa colisor de dano
        StartCoroutine(LoopAttack());       // repete o ciclo de ataque
    }

    // quando player pisa no boss, esse metodo é chamado
    public void Hurt(){

        hurt --;
        Over();                             // se hurt <= 0 Boss derrubado

        Freeze();                           // freeze X e rotation

        isChangeDir = false;                // indica que não pode trocar direção
        isHurt = true;                      // indica que machucou
        punch.SetActive(false);             // desativa soco do boss
        
        anim.SetBool("Hurt", true);         // chama animação de dano

        Invoke("HurtIsOver", 1);            
    }

    // quando boss levantar ele vai pode checar em que direção está o player
    // entra no loop de ataque
    public void Up(){
        isChangeDir = true;                 // indica que pode trocar direção
        anim.SetTrigger("Attack");          // chama animação de ataque
    }

    public void HurtIsOver(){
        anim.SetBool("Hurt", false);        // chama animação de dano
        isHurt = false;                     // indica que terminou machucou
    }

    // Boss derrubado, não vai para outras animações
    private void Over(){
        if(hurt <=0){
            anim.SetTrigger("Over");

            FindObjectOfType<finalLevel>().openGate.isActive = true;        // ativa portão
            AudioManager.instance.PlaySFX("gate");
            
            sign.DialogOverBoss();                                          // chama dialogo do boss derrotado
            FindObjectOfType<Player>().isNotMovePlayer = true;          // nao deixa player se mover
            StartCoroutine(CameraSolo());
        }
    }

    IEnumerator CameraSolo(){
        yield return new WaitForSeconds(3f);
        camSolo.changeFocus();                                          // troca foco da camera     
    }

    // constraints X, e rotation do palyer ----------------------------------------------------------------------
    public void NoneFreeze(){
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void Freeze(){
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }

    public void FreezeRotation(){
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
