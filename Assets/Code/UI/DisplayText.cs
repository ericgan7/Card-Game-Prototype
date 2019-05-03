using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayText : MonoBehaviour
{
    public Text viewTextPrefab;
    public TextMeshProUGUI worldTextPrefab;
    public float duration;

    public bool test;
    public Canvas world;
    public Canvas view;

    private void Update()
    {
        if (test)
        {
            test = false;
            GameController g = FindObjectOfType<GameController>();
            Vector3 d = g.currentCharacter.transform.position;
            d.y += 1;
            CreateWorldText(g.currentCharacter.transform.position, d, "10", Color.black);
        }
    }

    public void CreateWorldText(Vector3 position, Vector3 direction, string text, Color color)
    {
        var t = Instantiate(worldTextPrefab);
        t.transform.SetParent(view.transform);
        t.text = text;
        t.color = color;
        IEnumerator corountine = MoveText(t, position, direction);
        StartCoroutine(corountine);
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
    
}
