using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public List<CharacterSelect> characters;
    public List<Vector3> defaultPosition;
    GM gm;

    public void Start()
    {
        gm = FindObjectOfType<GM>();
        defaultPosition = new List<Vector3>();
        foreach(CharacterSelect cs in characters)
        {
            cs.index = defaultPosition.Count;
            cs.SetCharacter(gm.characters[cs.index]);
            defaultPosition.Add(cs.transform.position);
        }
    }

    public void CheckSwap(int index)
    {
        //check swap left
        if (index - 1 >= 0)
        {
            if (characters[index].transform.position.x < characters[index - 1].transform.position.x)
            {
                CharacterSelect cs = characters[index];
                characters[index] = characters[index - 1];
                characters[index].index = index;
                characters[index].destination = defaultPosition[index];
                characters[index - 1] = cs;
                cs.index = index - 1;
            }
        }
        //check swap right
        if (index + 1 < characters.Count)
        {
            if (characters[index].transform.position.x > characters[index + 1].transform.position.x)
            {
                CharacterSelect cs = characters[index];
                characters[index] = characters[index + 1];
                characters[index].index = index;
                characters[index].destination = defaultPosition[index];
                characters[index + 1] = cs;
                cs.index = index + 1;
            }
        }
    }

    public Vector3 GetPosition(int index)
    {
        return defaultPosition[index];
    }

    public void StartGame()
    {
        gm.characters.Clear();
        foreach(CharacterSelect cs in characters)
        {
            gm.characters.Add(Instantiate(cs.character));
        }
        gm.loadDialogue();
    }
}
