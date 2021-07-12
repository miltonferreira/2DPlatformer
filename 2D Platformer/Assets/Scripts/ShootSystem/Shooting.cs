using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject shootingItem;
    public Transform shootingPoint;
    public bool canShot = true;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return)){
            Shoot();
        }
    }

    void Shoot(){

        if(!canShot)
            return;
    
        GameObject si = Instantiate(shootingItem, shootingPoint);
        si.transform.parent = null;

    }
}
