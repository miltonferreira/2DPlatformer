using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    public int cole_cherry = 0;

    Vector2 playerInitPosition;

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
}
