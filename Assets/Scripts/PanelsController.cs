using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsController : MonoBehaviour
{
    
    [SerializeField]
    private GameObject colorPanel, brushSizePanel;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if(colorPanel.activeInHierarchy == true)
        {
            colorPanel.SetActive(false);
        }

        if(brushSizePanel.activeInHierarchy == true)
        {
            brushSizePanel.SetActive(false);
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
}
