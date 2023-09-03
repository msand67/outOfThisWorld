using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinInterface : MonoBehaviour
{
    public SpriteRenderer pinHead;
    public SpriteRenderer pinBody;
    public TMPro.TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(float red, float green, float blue)
    {
        pinHead.color = new Color(red, green, blue);
    }
    public void SetColor(Color iColor)
    {
        pinHead.color = iColor;
    }

    public void SetNumber(int number)
    {
        text.text = number.ToString();
    }
}
