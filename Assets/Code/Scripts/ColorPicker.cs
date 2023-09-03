using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public UnityEngine.UI.Image myImage;

    public UnityEngine.UI.Slider rSlider;
    public UnityEngine.UI.Slider gSlider;
    public UnityEngine.UI.Slider bSlider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateColor()
    {
        myImage.color = new Color(rSlider.value/255, gSlider.value/255, bSlider.value/255);
        Debug.Log(myImage.color);
    }

    public Color GetColor()
    {
        return new Color(rSlider.value / 255, gSlider.value / 255, bSlider.value / 255);
    }
}
