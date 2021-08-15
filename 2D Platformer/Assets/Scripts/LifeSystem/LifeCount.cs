using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;

    private CapsuleCollider2D cap;

    public Image[] lives;
    public int livesRemaining;

    public int ForceThrow = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cap = GetComponent<CapsuleCollider2D>();
    }

    private void Update() {
        if(this.gameObject.transform.position.y < -10){
            FindObjectOfType<LevelManager>().Restart(); // player morreu
        }
    }

    public void LoseLife(float _x){

        // se zerar vidas não deixa executar codigo abaixo
        if(livesRemaining==0)
            return;
        
        // decrementa o valor de livesRemaining
        livesRemaining--;
        // esconde uma imagem da vida
        lives[livesRemaining].enabled = false;
        AudioManager.instance.PlaySFX("hurt");
        FindObjectOfType<Player>().isHurt = true;   // indica que tomou dano e não deixa player mover

        if(livesRemaining > 0){
            animator.SetTrigger("Hurt");                // animação de dano
        }else{
            animator.SetBool("Dead", true);
        }
        

        float speed = 1;
        float jumpPower = 16;
        float xVal = 0;

        if(this.transform.position.x < _x){
                // player está a esquerda e será jogado para esquerda
                xVal = -ForceThrow * speed * 100 * Time.fixedDeltaTime;
            }else{
                // player está a direita e será jogado para direita
                xVal = ForceThrow * speed * 100 * Time.fixedDeltaTime;
            }

        rb.velocity = Vector2.up * jumpPower;   // adiciona velocidade ao rb
                        
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;

        GameOver();     // checa se vidas == 0

    }

    public void GameOver(){
        if(livesRemaining==0){
            FindObjectOfType<Player>().Die();
            cap.isTrigger = true;  //player vira fantasma
        }
    }

}
