using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;
using TMPro;

public class DialoguePanel : MonoBehaviour
{
    DialogueManager dm;
    public TextMeshProUGUI dialogue;
    public RectTransform dialoguePos;
    public Text characterName;
    public Image portrait;
    public List<Button> buttons;
    public Button buttonPrefab;
    public RectTransform buttonPos;

    public void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        dialoguePos = dialogue.GetComponent<RectTransform>();
        buttonPos = buttonPrefab.GetComponent<RectTransform>();
    }

    public void SetText(string text)
    {
        dialogue.text = text;
    }

    public void SetPortrait(Sprite p)
    {
        portrait.sprite = p;
    }

    public void SetName(string text)
    {
        characterName.text = text;
    }

    public void Reset()
    {
        foreach(Button b in buttons)
        {
            Destroy(b.gameObject);
        }
        buttons.Clear();
    }

    public Button CreateChoice(string text, int choiceIndex)
    {
        Button choice = Instantiate(buttonPrefab) as Button;
        choice.transform.SetParent(this.transform);
        choice.GetComponentInChildren<Text>().text = text;
        choice.onClick.AddListener(delegate
        {
            dm.Choose(choiceIndex);
        });
        RectTransform rt = choice.GetComponent<RectTransform>();
        Debug.Log(rt);
        Debug.Log(dialoguePos);
        Debug.Log(buttonPos);
        rt.anchoredPosition = new Vector2(25f, dialoguePos.anchoredPosition.y - dialoguePos.sizeDelta.y / 2 - 5f - buttons.Count * buttonPos.sizeDelta.y);
        buttons.Add(choice);
        return choice;
    }
}
