using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public DialoguePanel left;
    public DialoguePanel right;
    public GameObject bg;
    VIDE_Assign story;

    void Start()
    {
        //VD.LoadDialogues();
        GM gm = FindObjectOfType<GM>();
        if (gm != null)
        {
            story = gm.GetComponent<VIDE_Assign>();
        }
        else {
            story = GetComponent<VIDE_Assign>();
        }
        //VD.LoadDialogues();
        Begin();
    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!VD.isActive)
            {
                Begin();
            }
            else
            {
                VD.Next();
            }
        }
    }

    void Begin()
    {
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += End;
        VD.BeginDialogue(story);
    }

    void UpdateUI(VD.NodeData data)
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        if (data.isPlayer)
        {
            Image img = left.gameObject.GetComponentInChildren<Image>();
            if (data.sprite)
                img.sprite = data.sprite;
            left.gameObject.SetActive(true);
            left.SetText(data.comments[0]);
            left.SetName(data.tag);
            for (int i = 1; i < data.comments.Length; ++i)
            { 
                left.CreateChoice(data.comments[i], i);
            }
        }
        else
        {
            Image img = right.gameObject.GetComponentInChildren<Image>();
            if (data.sprite){
                img.color = new Color (1, 1, 1, 1);
                img.sprite = data.sprite;
            }
            else
                img.color = new Color (0, 0, 0, 0);
            right.gameObject.SetActive(true);
            right.SetText(data.comments[0]);
            right.SetName(data.tag);
        }

        if (data.sprites[0] != null){
            Image background = bg.gameObject.GetComponentInChildren<Image>();
            background.sprite = data.sprites[0];
        }
    }

    void End(VD.NodeData data)
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= End;
        VD.EndDialogue();
    }

    public void Choose(int index)
    {
        VD.nodeData.commentIndex = index;
        if (Input.GetMouseButtonUp(0))
        {
            VD.Next();
        }
    }

}
