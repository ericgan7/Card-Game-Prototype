using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public List<CharacterStats> characters;
    //some public 
    public VIDE_Assign VA;
    public VIDEUIManager1 diag;
    //temp test
    public List<int> nodes;
    int index = 0;

    public void Next()
    {
        ++index; 
    }
    public void startDialogue(int nodeID) {
        //set the node id here

        //
        VA.overrideStartNode = nodeID;
        diag.Interact(VA);
    }
    public static GM instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void setupChar(int chara) {

    }
    public void startGame() {
        SceneManager.LoadScene("Main");
    }
    public void loadDialogue()
    {
        Debug.Log(index);
        VA.overrideStartNode = nodes[index];
        SceneManager.LoadScene("Dialogue");
    }
    public void loadRewards()
    {
        SceneManager.LoadScene("Reward");
    }
    public void loadMenu()
    {
        SceneManager.LoadScene("Menu");
        index = 0;
        
    }
    public int GetIndex()
    {
        return index;
    }

    public void quit() {
        Application.Quit();
    }
    // Update is called once per frame
    /*
    void Update()
    {
        if (VD.isActive && Input.GetKeyUp(KeyCode.Space) && menu){
                diag.Interact(VA);
            
        }

    }
    */
}
