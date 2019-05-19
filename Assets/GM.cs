using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public CharacterStats[] characters;
    public CharacterStats[] preSets;
    //some public 
    public VIDE_Assign VA;
    public VIDEUIManager1 diag;
    //temp test
    List<int> nodes;
    int index = 0;
    bool menu = true;

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
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            nodes = new List<int> { 20, 1, 36, 2, 3, 65 };
            menu = true;
            characters = new CharacterStats[preSets.Length];
            for (int i = 0; i < preSets.Length; ++i)
            {
                characters[i] = Instantiate(preSets[i]);
            }
        }
    }
    public void setupChar(int chara) {

    }
    public void startGame() {
        SceneManager.LoadScene("Main");
        menu = false;
    }
    public void loadDialogue()
    {
        SceneManager.LoadScene("Dialogue");
        Debug.Log(index);
        VA.overrideStartNode = nodes[index];
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
