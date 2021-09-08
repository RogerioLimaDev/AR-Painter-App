using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RogerioLima.ARPaint
{
    public class PanelsController : MonoBehaviour
    {
        
        [SerializeField]
        private GameObject colorPanel, brushSizePanel, settingsPanel;
        [SerializeField] Image menuBtnImage;
        [SerializeField] List<Sprite> menuSprites;
        [SerializeField] DrawLineCommand drawLineCommand;

        private bool panelOpen;
        
        
        // Start is called before the first frame update
        void Start()
        {
            panelOpen = true;
            drawLineCommand.canDraw = false;

            if(colorPanel.activeInHierarchy == true)
            {
                colorPanel.SetActive(false);
            }

            if(brushSizePanel.activeInHierarchy == true)
            {
                brushSizePanel.SetActive(false);
            }

            if(settingsPanel.activeInHierarchy == false)
            {
                settingsPanel.SetActive(true);
                menuBtnImage.sprite = menuSprites[0];
            }
        }

        public void ActivateSetColorPanel()
        {
            if(brushSizePanel.activeInHierarchy == false && colorPanel.activeInHierarchy == false)
            {
                colorPanel.SetActive(true);
            }
            else if(brushSizePanel.activeInHierarchy == false && colorPanel.activeInHierarchy == true)
            {
                colorPanel.SetActive(false);
            }
            else if(brushSizePanel.activeInHierarchy == true && colorPanel.activeInHierarchy == false)
            {
                brushSizePanel.SetActive(false);
                colorPanel.SetActive(true);
            }
        }

        public void ActivateSetBrushSizePanel()
        {
            if(brushSizePanel.activeInHierarchy == false && colorPanel.activeInHierarchy == false)
            {
                brushSizePanel.SetActive(true);
            }
            else if(brushSizePanel.activeInHierarchy == false && colorPanel.activeInHierarchy == true)
            {
                colorPanel.SetActive(false);
                brushSizePanel.SetActive(true);
            }
            else if(brushSizePanel.activeInHierarchy == true && colorPanel.activeInHierarchy == false)
            {
                brushSizePanel.SetActive(false);
            }

        }

        public void ActivateSettingsPanel()
        {
            if(panelOpen == false)
            {
                settingsPanel.SetActive(true);
                menuBtnImage.sprite = menuSprites[0];
                drawLineCommand.canDraw = false;
            }
            else
            {
                settingsPanel.SetActive(false);
                colorPanel.SetActive(false);
                brushSizePanel.SetActive(false);
                menuBtnImage.sprite = menuSprites[1];
                drawLineCommand.canDraw = true;
            }

            panelOpen = !panelOpen;

        }
    }
    
}


