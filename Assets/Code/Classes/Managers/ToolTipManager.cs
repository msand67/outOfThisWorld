using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ToolTipManager : MonoBehaviour, IPointerEnterHandler
{
    public static ToolTipManager _instance;
    public TMPro.TMP_Text textBox;

    private void Awake()
    {
        if(this!=_instance && _instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition+new Vector3(3,3);
    }

    public void DisplayTooltip(string message)
    {
        textBox.text = message;
        this.gameObject.SetActive(true);
        this.transform.position = Input.mousePosition + new Vector3(3, 3);
    }
    public void HideTooltip()
    {
        textBox.text = "";
        this.gameObject.SetActive(false);
        this.transform.position = Input.mousePosition + new Vector3(3, 3);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)

    {
        this.gameObject.SetActive(true);
        this.transform.position = Input.mousePosition + new Vector3(3, 3);

    }



}
