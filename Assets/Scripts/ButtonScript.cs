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
            loadFromJson = FindObjectOfType<LoadFromJsonCommand>();
        }

        public void LoadDrawing()
        {
            drawName = gameObject.transform.parent.name;
            Debug.Log("O BOTAO LOAD DRAWING FOI PRESSIONADO: "+drawName.ToUpper());
            loadFromJson.LoadDrawingFromJson(drawName);


        }
    }

}


