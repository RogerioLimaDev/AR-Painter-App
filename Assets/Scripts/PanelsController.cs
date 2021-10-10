using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RogerioLima.ARPaint
{
    public class PanelsController : MonoBehaviour
    {
        
        [SerializeField]
        private GameObject colorPanel, brushSizePanel,
                             settingsPanel, loadDrawingPanel,
                             panelDrawingsToLoad,saveDrawingPanel,
                             saveDrawingBtn,saveAsBtn;
        [SerializeField] Image menuBtnImage;
        [SerializeField] List<Sprite> menuSprites;
        [SerializeField] DrawLineCommand drawLineCommand;
        [SerializeField] DrawOnPlaneCommand drawOnPlaneCommand;
        [SerializeField] SavedDrawingsManager savedDrawingsManager;

        [SerializeField] private GameObject[] uiPanels;
    
        
        // Start is called before the first frame update
        void Start()
        {
            if(drawOnPlaneCommand!=null)
            {
                drawOnPlaneCommand.canDraw = false;
            }
            if(drawLineCommand!=null)
            {
                drawLineCommand.canDraw = false;
            }
            
            Debug.Log(uiPanels[1].name);

            foreach(GameObject go in uiPanels)
            {
                if(go.activeInHierarchy == true)
                {
                    go.SetActive(false);
                }
            }

            if(settingsPanel.activeInHierarchy == false)
            {
                settingsPanel.SetActive(true);
                menuBtnImage.sprite = menuSprites[0];
            }

            panelDrawingsToLoad.SetActive(true);
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
            panelDrawingsToLoad.SetActive(false);
  
        }

        public void EnableLoadDrawingPanel()
        {
            if(loadDrawingPanel.activeInHierarchy == false)
            {
                loadDrawingPanel.SetActive(true);

                if(saveDrawingPanel.activeInHierarchy == true)
                {
                    saveDrawingPanel.SetActive(false);
                }
            }
            else
            {
                DisableLoadDrawingPanel();
            }

            savedDrawingsManager.CheckSavedFiles("");

        }

        public void ActivateSettingsPanel()
        {
            if(settingsPanel.activeInHierarchy == false)
            {
                settingsPanel.SetActive(true);
                menuBtnImage.sprite = menuSprites[0];
                if(drawOnPlaneCommand!=null)
                {
                    drawOnPlaneCommand.canDraw = false;
                }
                if(drawLineCommand!=null)
                {
                    drawLineCommand.canDraw = false;
                }
            }
            else
            {
                foreach(GameObject g in uiPanels)
                {
                    // Debug.Log(uiPanels.Length);
                    g.SetActive(false);
                }
                menuBtnImage.sprite = menuSprites[1];

                if(drawOnPlaneCommand !=null)
                {
                    drawOnPlaneCommand.canDraw = true;
                }
                if(drawLineCommand!=null)
                {
                    drawLineCommand.canDraw = true;
                }
                settingsPanel.SetActive(false);
            }
        }
    
        public void ActivateSaveDrawingPanel()
        {
            saveDrawingPanel.SetActive(true);
            drawLineCommand.canDraw = false;

            if(saveDrawingBtn.activeInHierarchy == false)
            {
                saveDrawingBtn.SetActive(true);
                saveAsBtn.SetActive(false);
            }
            else if(saveAsBtn.activeInHierarchy == true)
            {
                saveAsBtn.SetActive(false);
                saveDrawingBtn.SetActive(true);
            }

            if(loadDrawingPanel.activeInHierarchy == true)
            {
                DisableLoadDrawingPanel();
            }
        }

        public void ActivateSaveAsDrawingPanel()
        {
            saveDrawingPanel.SetActive(true);
            drawLineCommand.canDraw = false;

            if(saveDrawingBtn.activeInHierarchy == true)
            {
                saveDrawingBtn.SetActive(false);
                saveAsBtn.SetActive(true);
            }
            else if(saveAsBtn.activeInHierarchy == false)
            {
                saveAsBtn.SetActive(true);
                saveDrawingBtn.SetActive(false);
            }
        }
    }
    
}


