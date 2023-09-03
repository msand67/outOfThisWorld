using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsEventManager
{
    //FontSize
    public delegate void UpdateFontSize(int size);
    public static event UpdateFontSize FontSizeChanged;

    public static void ChangeFontSize()
    {
        FontSizeChanged(PlayerPrefs.GetInt("fontSize"));
    }


    //Font
    public delegate void UpdateFont(string fontName);
    public static event UpdateFont FontChanged;

    public static void ChangeFont()
    {
        FontChanged(PlayerPrefs.GetString("fontName"));
    }
    //Font Color
    public delegate void UpdateFontColor(float r, float g, float b);
    public static event UpdateFontColor DarkTextColorChanged;
    public static event UpdateFontColor LightTextColorChanged;

    public static void ChangeLightFont()
    {
        LightTextColorChanged(PlayerPrefs.GetFloat("fontColorLightR"), PlayerPrefs.GetFloat("fontColorLightG"), PlayerPrefs.GetFloat("fontColorLightB"));
    }
    public static void ChangeDarkFont()
    {
        DarkTextColorChanged(PlayerPrefs.GetFloat("fontColorDarkR"), PlayerPrefs.GetFloat("fontColorDarkG"), PlayerPrefs.GetFloat("fontColorDarkB"));
    }
}
