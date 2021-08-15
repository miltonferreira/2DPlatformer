using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{

    SpriteRenderer spr;
    public Sprite spr_crank_down;
    public Transform gate;  //portão que libera caminho ao player
    public bool isActive;          // true faz gate descer e abrir caminho

    // gate ----------------------------------
    public Transform wayPoint;
    public float speed;


    // Start is called before the first frame update
    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        gateMove();
    }

    void gateMove(){
        if(isActive){
            spr.sprite = spr_crank_down;
            float t = speed * Time.deltaTime;
            gate.position = Vector2.MoveTowards(gate.position, wayPoint.position, t);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player" && !isActive){
            isActive = true;
            AudioManager.instance.PlaySFX("gate");
        }
    }
}
