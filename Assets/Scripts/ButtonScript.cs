using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RogerioLima.ARPaint
{
    public class ButtonScript : MonoBehaviour
{
    string drawName;
    LoadFromJsonCommand loadFromJson;
    void OnEnable()
    {
        drawName = GetComponentInChildren<Text>().text;
        loadFromJson = FindObjectOfType<LoadFromJsonCommand>();
    }

    public void LoadDrawing()
    {
        loadFromJson.LoadDrawingFromJson(drawName);
        Debug.Log("O BOTAO PRESSIONADO FOI O "+drawName.ToUpper());

    }
}

}


