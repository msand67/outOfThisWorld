using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontChanger : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public float textSizeScale=1;
    public bool isDarkText = true;
    // Start is called before the first frame update
    void Start()
    {
        SettingsEventManager.FontChanged += SetTextFont;
        SettingsEventManager.FontSizeChanged += SetTextSize;
        if (isDarkText)
        {
            SettingsEventManager.DarkTextColorChanged += SetTextColor;
                }
        else
        {
            SettingsEventManager.LightTextColorChanged += SetTextColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        SettingsEventManager.FontChanged -= SetTextFont;
        SettingsEventManager.FontSizeChanged -= SetTextSize;
        if (isDarkText)
        {
            SettingsEventManager.DarkTextColorChanged -= SetTextColor;
        }
        else
        {
            SettingsEventManager.LightTextColorChanged -= SetTextColor;
        }
    }

    public void SetTextColor(float red, float green, float blue)
    {
        text.color = new Color(red, green, blue);
    }

    public void SetTextSize(int textSize)
    {
        text.fontSize = textSize * textSizeScale;
    }
    public void SetTextFont(string fontName)
    {
        TMPro.TMP_FontAsset font = Resources.Load(fontName, typeof(TMPro.TMP_FontAsset)) as TMPro.TMP_FontAsset;
        text.font = font;
    }
}
