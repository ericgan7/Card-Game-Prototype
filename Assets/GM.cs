using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{

    //some public 
    public VIDE_Assign VA;
    public VIDEUIManager1 diag;
    public void startDialogue(int nodeID) {
        //set the node id here

        //
        VA.overrideStartNode = nodeID;
        diag.Interact(VA);


    }
    public static GM instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
    }
    public void setupChar(int chara) {

    }
    public void startGame(int lv) {
        SceneManager.LoadScene("Main");
    }
    
    public void quit() {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        if (VD.isActive && Input.GetKeyUp(KeyCode.Space)){
                diag.Interact(VA);
            
        }

    }
}
