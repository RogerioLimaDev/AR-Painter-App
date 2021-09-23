using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RogerioLima.ARPaint
{
    public class PanelsController : MonoBehaviour
    {
        
        [SerializeField]
        private GameObject colorPanel, brushSizePanel, settingsPanel, loadDrawingPanel,saveDrawingPanel;
        [SerializeField] Image menuBtnImage;
        [SerializeField] List<Sprite> menuSprites;
        [SerializeField] DrawLineCommand drawLineCommand;

        private List<GameObject> allPanels;

        private bool panelOpen;
        

        
        // Start is called before the first frame update
        void Start()
        {
            panelOpen = true;
            drawLineCommand.canDraw = false;
            allPanels = new List<GameObject>();
            allPanels.Add(loadDrawingPanel);
            allPanels.Add(colorPanel);
            allPanels.Add(brushSizePanel);
            allPanels.Add(settingsPanel);
            allPanels.Add(saveDrawingPanel);

            if(loadDrawingPanel.activeInHierarchy == true)
            {
                loadDrawingPanel.SetActive(false);
            }

            if(saveDrawingPanel.activeInHierarchy == true)
            {
                saveDrawingPanel.SetActive(false);
            }


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

        public void DisableLoadDrawingPanel()
        {
            loadDrawingPanel.SetActive(false);
            drawLineCommand.canDraw = true;
  
        }

        public void EnableLoadDrawingPanel()
        {
            if(loadDrawingPanel.activeInHierarchy == false)
            {
                loadDrawingPanel.SetActive(true);
                drawLineCommand.canDraw = false;
   
            }
            else
            {
                loadDrawingPanel.SetActive(false);
                drawLineCommand.canDraw = true;
  
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
                foreach(GameObject g in allPanels)
                {
                    g.SetActive(false);
                }
                menuBtnImage.sprite = menuSprites[1];
                drawLineCommand.canDraw = true;
            }
            panelOpen = !panelOpen;

        }
    }
    
}


