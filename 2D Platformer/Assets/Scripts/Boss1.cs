using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{

    public GameObject punch;
    Rigidbody2D rb;

    public bool isRight = false;
    public float xVal = 100;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
