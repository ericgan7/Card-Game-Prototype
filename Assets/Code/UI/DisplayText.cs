using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayText : MonoBehaviour
{
    GameController game;
    public Text viewTextPrefab;
    public TextMeshProUGUI worldTextPrefab;
    public float duration;

    public Canvas world;
    public Canvas view;
    public List<TextMeshProUGUI> worldTexts;

    public void Start()
    {
        game = FindObjectOfType<GameController>();
    }

    public void CreateWorldText(Vector3 position, Vector3 direction, string text, Color color, bool move = true)
    {
        var t = Instantiate(worldTextPrefab);
        t.transform.SetParent(world.transform);
        t.transform.position = position;
        t.text = text;
        t.color = color;
        if (move)
        {
            IEnumerator corountine = MoveText(t, position, direction);
            StartCoroutine(corountine);
        }
        else
        {
            worldTexts.Add(t);
        }
    }

    IEnumerator MoveText(TextMeshProUGUI t, Vector3 position, Vector3 direction)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            t.transform.position = Vector3.Lerp(position, direction, elapsed / duration);
            yield return new WaitForFixedUpdate();
        }
        Destroy(t.gameObject);
    }
    
    public void DisplayTargets(Card toPlay, Character origin)
    {
        foreach(var t in worldTexts)
        {
            Destroy(t.gameObject);
        }
        worldTexts.Clear();
        List<Character> targets = game.map.targets.GetTargets();
        
        foreach (Character c in targets)
        {
            List<Card.EffectResult> results = toPlay.EffectAmount(origin, c);
            foreach(Card.EffectResult r in results)
            {
                CreateWorldText(r.position, r.position, r.effect, r.color, false);
            }
           
        }
    }

    public void ResetTargets()
    {
        foreach (var t in worldTexts)
        {
            Destroy(t.gameObject);
        }
        worldTexts.Clear();
    }

}
