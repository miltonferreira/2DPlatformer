using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;      // util para seta valor de z em -10

    [Range(2, 10)]
    public float smoothFactor;
    public Vector3 minValues, maxValue;

    private void FixedUpdate() 
    {
         Follow(); 
    }

    void Follow(){

        Vector3 targetPosition = target.position + offset;  // pega posição do player

        // definir minimo x,y,z e maximo x,y,z do movimento da cam
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(target.position.x, minValues.x, maxValue.x),
            Mathf.Clamp(target.position.y, minValues.y, maxValue.y),
            Mathf.Clamp(target.position.z, minValues.z, maxValue.z)
        );

        // cria uma interpolação entre a posição da cam e do player/boundPosition
        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;

    }

}
