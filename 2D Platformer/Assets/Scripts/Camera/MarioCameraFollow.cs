#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MarioCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;                  // util para seta valor de z em -10

    [Range(2, 10)]
    public float smoothFactor;              // velocidade de movimento da cam
    [HideInInspector]
    public Vector3 minValues, maxValue;
    [Range(0, 1)]
    public float moveCameraTriggerValue;    //???

    // editor ----------------------------------------------
    [HideInInspector]
    public bool setupComplete = false;
    public enum SetupState{None, Step1, Step2}
    [HideInInspector]
    public SetupState ss = SetupState.None;

    private void Start() {
        Follow();
    }

    private void FixedUpdate() 
    {
        var playerViewPortPos = Camera.main.WorldToViewportPoint(target.transform.position);
        if(playerViewPortPos.x>moveCameraTriggerValue){
            Follow();
        }
    }

    void Follow(){

        Vector3 targetPosition = target.position + offset;  // pega posição do player

        // definir minimo x,y,z e maximo x,y,z do movimento da cam
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValue.z)
        );

        // cria uma interpolação entre a posição da cam e do player/boundPosition
        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;

        minValues.x = smoothPosition.x; //??

    }

    public void ResetValues(){
        setupComplete = false;
        minValues = Vector3.zero;
        maxValue = Vector3.zero;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MarioCameraFollow))]
public class MarioCameraFollowEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var script = (MarioCameraFollow) target;     // indica qual script vai pegar os valores

        GUILayout.Space(20);                // espaço de cima/baixo que separa o texto

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 15;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("-=- Camera Boundaries Settings -=-", titleStyle);

        GUIStyle defaultStyle = new GUIStyle();
        defaultStyle.fontSize = 12;
        defaultStyle.alignment = TextAnchor.MiddleCenter;

        if(script.setupComplete){

            GUILayout.BeginHorizontal();
            GUILayout.Label("Minimum Values:", defaultStyle);
            GUILayout.Label("Maximum Values:", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"X = {script.minValues.x}", defaultStyle);
            GUILayout.Label($"X = {script.maxValue.x}", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Y = {script.minValues.y}", defaultStyle);
            GUILayout.Label($"Y = {script.maxValue.y}", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("View Minimum")){
                Camera.main.transform.position = script.minValues;
            }
            if(GUILayout.Button("View Maximum")){
                Camera.main.transform.position = script.maxValue;
            }
            GUILayout.EndHorizontal();

            // faz a cam foca no target
            if(GUILayout.Button("Focus On Target")){
                Vector3 targetPos = script.target.position + script.offset;
                targetPos.z = script.minValues.z;
                Camera.main.transform.position = targetPos;
            }

            // zera valores da cam
            if(GUILayout.Button("Reset Camera Values")){
                script.ResetValues();
            }

        }else{

            if(script.ss == MarioCameraFollow.SetupState.None){
                if(GUILayout.Button("Start Settings Camera Values")){

                    script.ss = MarioCameraFollow.SetupState.Step1;
                }
            } else if(script.ss == MarioCameraFollow.SetupState.Step1){
                // pega valor minimo da camera
                GUILayout.Label($"1- Select your main Camera", defaultStyle);
                GUILayout.Label($"2- Move it to the bottom left bound limit of your level", defaultStyle);
                GUILayout.Label($"3- Click the 'Set Minimum Values' Buttom", defaultStyle);

                if(GUILayout.Button("Set Minimum Values")){
                    script.minValues = Camera.main.transform.position;
                    script.ss = MarioCameraFollow.SetupState.Step2;  // indica proximo passo
                }

            } else if(script.ss == MarioCameraFollow.SetupState.Step2){
                // pega valor maximo da camera
                GUILayout.Label($"1- Select your main Camera", defaultStyle);
                GUILayout.Label($"2- Move it to the top right bound limit of your level", defaultStyle);
                GUILayout.Label($"3- Click the 'Set Maximum Values' Buttom", defaultStyle);

                if(GUILayout.Button("Set Maximum Values")){
                    script.maxValue = Camera.main.transform.position;
                    script.ss = MarioCameraFollow.SetupState.None;   // indica proximo passo
                    script.setupComplete = true;

                    Vector3 targetPos = script.target.position  + script.offset;
                    targetPos.z = script.minValues.z;
                    Camera.main.transform.position = targetPos;
                }

            } 

        }


    }
}
#endif
