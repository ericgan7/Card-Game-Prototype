﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public DialoguePanel left;
    public DialoguePanel right;
    VIDE_Assign story;

    void Start()
    {
        //VD.LoadDialogues();
        story = GetComponent<VIDE_Assign>();
        Begin();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
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
            left.gameObject.SetActive(true);
            left.SetText(data.comments[0]);
            for (int i = 1; i < data.comments.Length; ++i)
            {
                Debug.Log(data.comments[i]);
                left.CreateChoice(data.comments[i], i);
            }
        }
        else
        {
            right.gameObject.SetActive(true);
            right.SetText(data.comments[0]);
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
