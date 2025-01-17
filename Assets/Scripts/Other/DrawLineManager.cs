using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARSubsystems;
using System;

namespace Com.RogerioLima.ARPaint
{
    public class DrawLineManager : MonoBehaviour
    {
        #region Fields
        // line settings (can be adjusted within inspector)
        [Header("Color Picker settings")]
        [Space]
        [SerializeField]
        private FlexibleColorPicker fcp;
        public Color externalColor;
        private Color internalColor;
        // current line
        private LineRenderer currentLine;
        private int currentLinePointNumber;
        // whether a line is currently being drawn
        private bool isPainting;
        [Space]
        [Header("Brush color settings")]
        [Space] 
        [SerializeField]
        private Color startColor;
        [SerializeField]
        private Color endColor;
        [SerializeField]
        private RawImage startImage;
        [SerializeField]
        private RawImage endImage;
        [Space]
        [Header("Brush size settings")]
        [Space] 
        [SerializeField]
        private RawImage startBrushsize;
        [SerializeField]
        private RawImage endBrushsize;
        [SerializeField]
        private float startWidth; // 1f = 1 meter in real life
        [SerializeField]
        private float endWidth;
        [SerializeField]
        float sliderMultiplier;
        [SerializeField]
        float brushImageScaleMultiplier;
        [SerializeField]
        Slider startSizeSlider;
        [SerializeField]
        Slider endSizeSlider;
        #endregion
        

        void Start()
        {
            currentLinePointNumber = 0;
            isPainting = false;
            internalColor = externalColor;
            startImage.color = fcp.startingColor;
            endImage.color = fcp.startingColor;
            startColor = Color.white;
            endColor = Color.white;
            SetBrushStartSize();
            SetBrushendSize();

        }

        void Update()
        {
            if (Input.touchCount > 0)
            {
                // just pressed down finger
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    StartNewLine();
                    SetLineWidth();
                    SetLineColor();

                    // start painting
                    isPainting = true;
                }

                // just lifted finger up
                else if(Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    // stop painting
                    isPainting = false;
                }

                // painting
                else if (isPainting)
                {
                    // update line segment given the position of the phone and finger
                    UpdateLine(CalculateFingerPosition()); 
                }
            }

            if(internalColor != externalColor) 
            {
                fcp.color = externalColor;
                internalColor = externalColor;
            }
        }

        private void SetLineWidth()
        {
            currentLine.startWidth = startWidth;
            currentLine.endWidth = endWidth;
        }

        private void SetLineColor()
        {
            currentLine.material = new Material(Shader.Find("Sprites/Default"));
            currentLine.startColor = startColor;
            currentLine.endColor = endColor;
        }
        
        private void StartNewLine()
        {
            GameObject emptyGameObject = new GameObject();
            currentLine = emptyGameObject.AddComponent<LineRenderer>();
            currentLinePointNumber = 0;
        }

        // raycast from the finger on the screen to the plane in front of the camera
        // and return the hit position in worldspace 
        private Vector3 CalculateFingerPosition()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            
            // layer 8 is reserved for the plane in front of the camera
            if (Physics.Raycast(rayOrigin, out hitInfo, 1 << 8))
            {
                return hitInfo.point;
            }
            
            // raycast missed plane 
            else
            {
                Debug.Log("Broken");
                return new Vector3(0, 0, 0);
            }
        }

        private void UpdateLine(Vector3 position)
        {
            currentLine.positionCount = currentLinePointNumber + 1;
            currentLine.SetPosition(currentLinePointNumber, position);
            currentLinePointNumber++;
        }

        public void SetStartColor()
        {
            
            Debug.Log("Current Color is: "+fcp.color);
            startColor = fcp.color;

            if(currentLine != null)
            {
                currentLine.startColor = startColor;

            }
            startImage.color = startColor;
            startBrushsize.color = startColor;

        }

        public void SetEndColor()
        {

            endColor = fcp.color;

            if(currentLine != null)
            {
                currentLine.endColor = endColor;
            }

            endImage.color = endColor;
            endBrushsize.color = endColor;
        }

        public void SetBrushStartSize()
        {
            startWidth = startSizeSlider.value * sliderMultiplier;
            float W = (startSizeSlider.value)*brushImageScaleMultiplier;
            float H = W;
            startBrushsize.GetComponent<RectTransform>().sizeDelta = new Vector2( W, H);
        }

        public void SetBrushendSize()
        {
            endWidth = endSizeSlider.value * sliderMultiplier;
            float W = (endSizeSlider.value)*brushImageScaleMultiplier;
            float H = W;
            endBrushsize.GetComponent<RectTransform>().sizeDelta = new Vector2( W, H);

        }

        public void ClearDrawings()
        {
            LineRenderer[]lineRenderers = FindObjectsOfType<LineRenderer>();
            foreach(LineRenderer l in lineRenderers)
            {
                Destroy(l);
            }
        }
    }
}


