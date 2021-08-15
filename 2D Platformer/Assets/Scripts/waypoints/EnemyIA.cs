using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyIA : MonoBehaviour
{
    //waypoints
    public List<Transform> points;
    // next point index
    public int nextID = 0;
    // ????
    int idChangeValue = 1;
    // Speed of movement or flying
    public float speed;

    private void Reset() {
        Init();
    }

    void Init(){
        // boxCollider trigger
        GetComponent<BoxCollider2D>().isTrigger = true;
        // create root object
        GameObject root = new GameObject(name + "_Root");
        // reseta posição do obj
        root.transform.position = transform.position;
        transform.SetParent(root.transform);
        // cria obj Waypoints
        GameObject waypoints = new GameObject("WayPoints");
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;
        // P1
        GameObject p1 = new GameObject("Point1");
        p1.transform.SetParent(waypoints.transform);
        p1.transform.position = root.transform.position;
        // P2
        GameObject p2 = new GameObject("Point2");
        p2.transform.SetParent(waypoints.transform);
        p2.transform.position = root.transform.position;

        // list de points
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);

    }

    private void Update() {
        MoveToNextPoint();
    }

    void MoveToNextPoint(){
        Transform goalPoint = points[nextID];
        if(goalPoint.transform.position.x > transform.position.x){
            // faz enemy olhar para direita
            transform.localScale = new Vector3(-1,1,1);
        } else {
            // faz enemy olhar para esquerda
            transform.localScale = new Vector3(1,1,1);
        }
        // Move the enemy towards the goal point
        float t = speed * Time.deltaTime;
        //Debug.Log(t);
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, t);
        // check the distance between enemy and goal point to trigger next point
        if(Vector2.Distance(transform.position, goalPoint.position) < 0.2f){
            if(nextID==points.Count-1){
                idChangeValue= -1;
            }
            if(nextID == 0){
                idChangeValue= 1;
            }
            nextID += idChangeValue;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            //Debug.Log($"{name} collision with the player");
            FindObjectOfType<LifeCount>().LoseLife(this.gameObject.transform.position.x);
        }
    }
}
