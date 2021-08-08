using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    public int cole_cherry = 0;     // quantidade de cherry's coletadas pelo players
    [Header("HUD Cherry's")]
    public Image[] cherryImgs;
    public bool isChoice;           // false=ruim true=bom

    Vector2 playerInitPosition;     //  seta posição inicial do player
    
    private void Awake() {
        playerInitPosition = FindObjectOfType<Player>().transform.position;
    }

    public void Restart(){
        // restart a cena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // reseta posição do player e reseta vidas
        // salvar posição inicial do player
        //FindObjectOfType<Player>().ResetPlayer();
        //FindObjectOfType<Player>().transform.position = playerInitPosition;
        // reseta contador de vidas

    }

    public void getCherry(){

        for(int i = 0; i < cole_cherry; i++)
            cherryImgs[i].color = Color.white;

        if(cole_cherry == 3){
            isChoice = true;    // fim de fase bom
        }

    }

}
