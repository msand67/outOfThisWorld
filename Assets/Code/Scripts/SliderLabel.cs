using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderLabel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSliderLabel(System.Single value)
    {
        this.GetComponent<TextMeshProUGUI>().text = $"x{value/2}";
    }
}
