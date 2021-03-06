using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody2D rb;
    private Animator animator;
    float horizontalValue;
    float runSpeedModifier = 2f;
    bool isRunning; // faz player correr
    bool facingRight = true;

    // verifica se colide com o chao ----------------------
    [Header("GroundCheck")]
    [SerializeField]bool isGrounded;
    [SerializeField]Transform groundCheckCollider;
    const float groundCheckRadius = 0.2f;                   // tamanho do circulo no pé do player
    [SerializeField]LayerMask groundLayer;

    // pulo -------------------------------------------------
    [Header("Jump")]
    public float jumpPower;                                  // força do pulo do player
    public int totalJumps;
    public int availableJumps;                                // checa quantos pulos o player deu
    bool multipleJump;
    bool coyoteJump;    // pulo da fé quando tiver fora do chao

    // crouch -------------------------------------------------
    [Header("Crouch")]
    [SerializeField]bool crouchPressed;
    [SerializeField]Collider2D standingCollider, crouchingCollider;            // pega colisor padrão do player
    float crouchSpeedModifier = 0.5f;                       // velocidade quando tiver gachado
    [SerializeField]Transform overHeadCheckCollider;        // posição para checar se tem chão na cabeça do player
    const float overHeadCheckRadius = 0.2f;                   // tamanho do circulo na cabeça do player

    public bool isDead = false;

    // wall slide & jump --------------------------------------
    [Header("Slide&Jump")]
    [SerializeField]Transform wallCheckCollider;
    [SerializeField]LayerMask wallLayer;
    const float wallCheckRadius = 0.2f;
    public float slideFactor = 0.2f;
    bool isSliding;

    float varHor;       //????

    // HasLanded event -----------------------------------------
    public static event Action HasLanded;

    // var quando cam troca target
    public bool isNotMovePlayer;

    // Dano -----------------------------------------
    public bool isHurt;     // indica que player tomou dano

    // joystick mobile ---------------------------------
    [Header("joystick mobile")]
    public FixedJoystick fixedJoystick;
    public bool isMobile;

    // ramp ----------------------------------------------------
    // [Header("Ramp")]
    // public PhysicsMaterial2D ph;
    // [SerializeField]LayerMask RampLayer;
    
    void Awake() 
    {
        availableJumps = totalJumps;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update(){

        if(!CanMoveOrInteract())
            return;
        
        if(!isMobile){
            horizontalValue = Input.GetAxisRaw("Horizontal");
        }else{
            fixedJoystick.SnapX = true;
            horizontalValue = fixedJoystick.Horizontal;

            // faz raposa gacha no mobile
            fixedJoystick.SnapY = true;
            if(fixedJoystick.Vertical == -1){
                crouchPressed = true;
            }else{
                crouchPressed = false;
            }
        }
        
        // faz player correr ----------------------------------------
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            isRunning = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            isRunning = false;
        }

        // faz player pular ----------------------------------------
        if(Input.GetButtonDown("Jump") && !isHurt){
            Jump(); 
        } 

        // faz player gacha ----------------------------------------
        if(!isMobile){
            if(Input.GetButtonDown("Crouch")){
                crouchPressed = true;
            } else if(Input.GetButtonUp("Crouch")){
                crouchPressed = false;
            }
        }

        // set yVelocity do animator do player
        // pega velocidade y do rigidbody
        animator.SetFloat("yVelocity", rb.velocity.y);

        WallCheck();    // slide or jump wall

    }

    void FixedUpdate(){
        GroundCheck();      // checa se player está no chão
        Move(horizontalValue, crouchPressed);
    }

    // verifica se pode se mover ou nao
    bool CanMoveOrInteract(){
        bool can = true;

        if(FindObjectOfType<InteractionSystem>().isExamining){
            can = playerMove(false);
        }
        if(FindObjectOfType<InventorySystem>().isOpen){
            can = playerMove(false);
        }
        if(isNotMovePlayer){    // quando cam troca target, não deixa player se mover
            can = playerMove(false);
        }
        if(isDead){
            can = playerMove(false);
        }

        return can;
    }

    bool playerMove(bool can){
        horizontalValue = 0;    // player para de andar
        return can;
    }

    void GroundCheck(){
        bool wasGrounded = isGrounded;  // pega valor do var antes de ficar false
        isGrounded = false;     // o player não está no chão
        //checa se o GroundCheckObj está tocando o chao
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if(colliders.Length>0){
            isGrounded = true;  // o player está no chão
            if(!wasGrounded){
                availableJumps = totalJumps;    // se tiver no chão pode pulo extra
                multipleJump = false;           // se não tiver no chão, não dá pulo duplo

                // trigger the HasLanded event
                HasLanded?.Invoke();    // ? indica que pode invocar ou não
            }

            // checa se tem plataforma que move embaixo do player
            // se colidor com plataforma movel, player vira parente dele
            foreach(var c in colliders){
                if(c.tag == "MovingPlatform")
                transform.parent = c.transform;
            }
        } else {

            // se player não tiver tocando na plataforma movel, não é mais parente dele
            transform.parent = null;

            if(wasGrounded)
                StartCoroutine(CoyoteJumpDelay());
        }
        
        // chama animação de pulo do player
        // no chão termina animação de pulo, no ar animação de pulo
        animator.SetBool("Jump", !isGrounded);
        
    }

    IEnumerator CoyoteJumpDelay(){  // tempo para fazer pulo da fé
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }

    public void Jump(){

        if(isGrounded){ // faz player pular
            //rb.AddForce(new Vector2(0f, jumpPower)); //adiciona força ao rb
            multipleJump = true;                    // permite pular + que 1 vez
            availableJumps--;                       // subtrai 1 pulo
            rb.velocity = Vector2.up * jumpPower;   // adiciona velocidade ao rb
            AudioManager.instance.PlaySFX("jump");

        } else {

            if(coyoteJump && availableJumps>0){         // pulo da fé
                multipleJump = true;                    // permite pular + que 1 vez
                availableJumps--;                       // subtrai 1 pulo
                rb.velocity = Vector2.up * jumpPower;   // adiciona velocidade ao rb
                //Debug.Log("Pulo Fé");

            } else if(multipleJump && availableJumps>0){    // + pulos enquanto tiver availableJumps
                availableJumps--;                       // subtrai 1 pulo
                rb.velocity = Vector2.up * jumpPower;   // adiciona velocidade ao rb
            }
        }

    }

    void WallCheck(){
        // if(Physics2D.OverlapCircle(wallCheckCollider.position, wallCheckRadius, wallLayer)
        // && Mathf.Abs(horizontalValue)>0 && rb.velocity.y<0 && !isGrounded){

        if(Physics2D.OverlapCircle(wallCheckCollider.position, wallCheckRadius, wallLayer)
        && rb.velocity.y<0 && !isGrounded){

            // if(!isSliding){
            //     varHor = horizontalValue;
            // }

            if(!isSliding){
                availableJumps = totalJumps;
                multipleJump = false;
            }
                
            Vector2 v = rb.velocity;
            v.y = -slideFactor;
            rb.velocity = v;
            isSliding = true;

            if(Input.GetButtonDown("Jump") && !isHurt){
                availableJumps--;                       // subtrai 1 pulo
                rb.velocity = Vector2.up * jumpPower;   // adiciona velocidade ao rb
            }

            // Debug.Log(rb.velocity.y);

        } else {
            isSliding = false;
        }

        animator.SetBool("Slide", isSliding);
    }

    void Move(float dir, bool crouchFlag){
        
        #region Crouch

        if(!crouchFlag){    // faz player gacha

            if(Physics2D.OverlapCircle(overHeadCheckCollider.position, overHeadCheckRadius, groundLayer)){
                crouchFlag = true;
            }

        }

        // se true o crouchFlag o player gacha
        animator.SetBool("Crouch", crouchFlag);

        // se gacha desativa colisor padrão
        standingCollider.enabled = !crouchFlag;     // quando gacha desativa collider grande
        if(!isDead){
            crouchingCollider.enabled = crouchFlag;     // quando gacha desativa collider pequeno
        }
        #endregion

        #region Move & Run
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;

        if(isRunning){
            xVal *= runSpeedModifier;   // acelera o player
        }

        if(crouchFlag){
            xVal *= crouchSpeedModifier;   // acelera o player
        }

        if(!isHurt){    // se não tomou dano pode mover player
            Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
            rb.velocity = targetVelocity;
        }

        // se o player estiver olhando para a esquerda
        if(facingRight && dir<0){
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
            
        } else if(!facingRight && dir>0){
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }

        // (0 idle , 4 walk, 8 run)
        // muda animação do player baseado na velocidade do rb
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        #endregion
    }

    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(overHeadCheckCollider.position, overHeadCheckRadius);
    }


    public void HurtOver(){
        isHurt = false; // indica que saiu do dano
    }

    public void Die(){
        isDead = true;
    }

    public void ResetPlayer(){
        //horizontalValue = 0;    // zera movimento pros lados
        isDead = false;
    }

    /*
    void CollisionRamp(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, RampLayer);
        if(colliders.Length>0){
            ph.friction = 1;
            Debug.Log("Entro Rampa");
        } else {
            ph.friction = 0;
            Debug.Log("Saiu Rampa");
        }
    }
    */

}
