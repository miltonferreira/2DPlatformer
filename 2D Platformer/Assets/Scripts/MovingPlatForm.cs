using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatForm : MonoBehaviour
{
    public List<Transform> points;
    public Transform platform;
    int goalPoint=0;                    // contador de pontos que a plataforma vai passar
    public float moveSpeed = 2;

    private void Update() {
        MoveToNextPoint();
    }

    void MoveToNextPoint(){

        // movimenta a plataforma entre os pontos
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position, 1*Time.deltaTime*moveSpeed);

        // se a distancia da plataforma for menor de 0.1 do ponto
        if(Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f){
            //
            if(goalPoint == points.Count-1){
                goalPoint = 0;  // se chegou ultimo ponto reseta
            } else {
                goalPoint++;    // se não chegou ultimo ponto reseta, ++
            }
        }

    }
}
