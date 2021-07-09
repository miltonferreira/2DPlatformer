using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrapObject : MonoBehaviour
{
    //public int damage = 25;
    private void Reset() {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            Debug.Log($"{name} collision with the player");
            //FindObjectOfType<HealthBar>().LoseHealth(damage);
            FindObjectOfType<LifeCount>().LoseLife();
        }
    }
}
