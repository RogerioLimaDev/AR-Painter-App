using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Com.RogerioLima.ARPaint
{
    public class SelectDrawingMode : MonoBehaviour
    {
        [SerializeField] Toggle drawModeToggle;
        [SerializeField] DrawLineCommand drawLineCommand;
        [SerializeField] DrawOnPlaneCommand drawOnPlaneCommand;

        void Start()
        {
            drawOnPlaneCommand.enabled = true;
            drawLineCommand.enabled = false;
        }

        public void SelectDrawMode()
        {
            if(drawModeToggle.isOn)
            {
                drawOnPlaneCommand.enabled = true;
                drawLineCommand.enabled = false;
            }
            else
            {
                drawOnPlaneCommand.enabled = false;
                drawLineCommand.enabled = true;
            }
        }
    }
}

