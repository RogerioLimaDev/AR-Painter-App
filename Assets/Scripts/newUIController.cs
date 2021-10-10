using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RogerioLima.ARPaint
{
    public class newUIController : MonoBehaviour
    {
        [SerializeField] Toggle onAir, onSurface;
        [SerializeField] DrawLineCommand drawLineCommand;
        [SerializeField] DrawOnPlaneCommand drawOnPlaneCommand;
        [SerializeField] SavedDrawingsManager savedDrawingsManager;
        [SerializeField] List<GameObject> panels = new List<GameObject>();

        bool loadPanelOpen, saveAsPanelOpen;
        GameObject paintSetupPanel, loadPaintingsPanel, savePaintingsAsPanel, colorPickerPanel;


        void Start()
        {
            drawLineCommand.enabled = true;
            drawOnPlaneCommand.enabled = false;
            drawLineCommand.canDraw = false;
            drawOnPlaneCommand.canDraw = false;
            onAir.isOn = true;
            onSurface.isOn = false;
            loadPanelOpen = false;
            saveAsPanelOpen = false;
            paintSetupPanel = panels[0];
            loadPaintingsPanel = panels[1];
            savePaintingsAsPanel = panels[2];
            colorPickerPanel = panels[3];
            EnableSettingsPanel();
        }

        public void EnablePaintOnAirMode()
        {

            bool swithOnOff = onAir.isOn;
            Debug.Log("OnAir is: " + swithOnOff.ToString());
            drawLineCommand.enabled = swithOnOff;

;
        }

        public void EnablePaintOnSurfaceMode()
        {
            bool swithOnOff = onSurface.isOn;
            Debug.Log("onSurface is: " + swithOnOff.ToString());
            drawOnPlaneCommand.enabled = swithOnOff;

        }

        public void EnableLoadPanel()
        {
            savedDrawingsManager.CheckSavedFiles("");

            foreach(GameObject g in panels)
            {
                g.SetActive(false);
            }
            loadPaintingsPanel.SetActive(true);

        }

        public void EnableSaveAsPanel()
        {
            foreach(GameObject g in panels)
            {
                g.SetActive(false);
            }
            savePaintingsAsPanel.SetActive(true);

        }

        public void EnableSettingsPanel()
        {
            foreach(GameObject g in panels)
            {
                g.SetActive(false);
            }
            drawOnPlaneCommand.canDraw = false;
            drawLineCommand.canDraw = false;
            paintSetupPanel.SetActive(true);


        }

        public void DisableAllPanels()
        {
            foreach(GameObject g in panels)
            {
                g.SetActive(false);
            }

            drawOnPlaneCommand.canDraw = true;
            drawLineCommand.canDraw = true;
        }
    
        public void EnableColorPickerPanel()
        {
            foreach(GameObject g in panels)
            {
                g.SetActive(false);
            }
            paintSetupPanel.SetActive(true);
            colorPickerPanel.SetActive(true);
        }
    
    }
}

