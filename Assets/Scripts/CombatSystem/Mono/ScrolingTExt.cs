using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrolingTExt : MonoBehaviour
{
    private float Duration = 2f;
    private float Speed = 5f;

    private TextMesh textMesh;
    private float startTime;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        startTime = Time.time;
    
    }

    private void Update()
    {
        if (Time.time - startTime < Duration)
        {
            //scroll up
            transform.LookAt(Camera.main.transform);
            transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }
        else
        { //destroy
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }
    public void SetColor(Color color)
    {
        textMesh.color = color;

    }
}
