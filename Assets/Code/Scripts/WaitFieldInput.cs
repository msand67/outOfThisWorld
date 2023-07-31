using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitFieldInput : MonoBehaviour
{

    public UnityEngine.UI.Slider mySlider;
    public TMPro.TMP_InputField myBox;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTextValue()
    {
        myBox.text = mySlider.value.ToString();
    }

    public void UpdateSliderValue()
    {

        if (int.Parse(myBox.text) < 0)
        {
            myBox.text = 0.ToString();
        }
        if (int.Parse(myBox.text) > 20)
        {
            myBox.text = 20.ToString();
        }
        mySlider.value = int.Parse(myBox.text);
    }
}
