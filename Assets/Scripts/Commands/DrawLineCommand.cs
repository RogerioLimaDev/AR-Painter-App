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
    [RequireComponent(typeof(CommandManager))]
    public class DrawLineCommand : MonoBehaviour, ICommand
    {
        #region Fields
        // line settings (can be adjusted within inspector)
        [SerializeField] GameObject lineRendererObject;
        [Space]
        [Header("Color Picker settings")]
        [Space]
        [SerializeField]
        private FlexibleColorPicker fcp;
        public Color externalColor;
        private Color internalColor;
        // current line
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
        [Space]
        [Header("Brush shape settings")]
        public Material[] brushMaterials;
        public Text materialName;
        private int materialIndex;
        protected Material currentMaterial;
    #endregion

        protected CommandManager commandManager;
        public LineProperties lineProperties;
        public bool canDraw;
        public LineRenderer currentLine;
        private int currentLinePointNumber;
        // whether a line is currently being drawn
        private bool isPainting;
        private bool noDrawing;



        #region Collections

        private List<Vector3> currentLinePositions;
        public List<LineProperties> lineDrawing;
        public List<LineRenderer> lineRenderers;

        #endregion

#region monobehaviors
        void Start()
        {
            
            lineProperties = new LineProperties();
            lineDrawing = new List<LineProperties>();
            currentLinePositions = new List<Vector3>();
            lineRenderers = new List<LineRenderer>();
            currentLinePointNumber = 0;
            currentMaterial = brushMaterials[0];
            isPainting = false;
            noDrawing = false;
            canDraw = true;
            materialIndex = 0;
            internalColor = externalColor;
            startImage.color = fcp.startingColor;
            endImage.color = fcp.startingColor;
            startColor = Color.white;
            endColor = Color.white;
            // SetBrushStartSize();
            startWidth = 0.01f;
            endWidth = 0.01f;
            // SetBrushendSize();
            commandManager = GetComponent<CommandManager>();

        }

        void Update()
        {
            commandManager.Invoke(this);
        }

#endregion

#region Drawing functions
        public bool CanExecute()
        {
            if (Input.touchCount > 0 && canDraw == true)
            {
                return true;
            };

            return false;
        }

        public void Execute()
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                StartNewLine();
                SetLineWidth();
                SetLineColor();
                isPainting = true;
            }

            // just lifted finger up
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // stop painting
                isPainting = false;
                //Save Line object for Undo purposes
                lineRenderers.Add(currentLine);
                //Save Line Properties for later recovery
                SaveLineProperties();
            }

            // painting
            if(isPainting == true)
            {
                UpdateLine(CalculateFingerPosition());
            }

            // else if (isPainting)
            // {
            //     // update line segment given the position of the phone and finger
            //     UpdateLine(CalculateFingerPosition()); 
            // }

            if(internalColor != externalColor) 
            {
                fcp.color = externalColor;
                internalColor = externalColor;
            }         
        }

        public void Undo()
        {
            // isPainting = false;
            // Debug.Log("UNDO CLICKED");

            // int commandIndex = (lineRenderers.Count)-1;
            // Debug.Log("LINE RENDERERS COUNT: "+ lineRenderers.Count);
            // var lineRenderer = lineRenderers[commandIndex];
            // Destroy(lineRenderer);
            // ClearLineProperties();
            ClearDrawings();
        }
    
        private void StartNewLine()
        {
            GameObject emptyGameObject = Instantiate(lineRendererObject, Camera.main.transform.position, Quaternion.identity);
            currentLine = emptyGameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
            currentLine.material = currentMaterial;
            materialName.text = brushMaterials[0].name;
            currentLine.numCapVertices = 3;
            currentLine.numCornerVertices = 5;
            currentLinePointNumber = 0;
            startWidth = 0.01f;
            endWidth = 0.01f;
        }

        private void UpdateLine(Vector3 position)
        {
                //Add the Vector3 to the position collection
                currentLinePositions.Add(position);
                //Set next point index
                currentLine.positionCount = currentLinePointNumber + 1;
                //Set the vector3 to the position index
                currentLine.SetPosition(currentLinePointNumber, position);
                //Increase the point
                currentLinePointNumber++;
        }
  
        private void SaveLineProperties()
        {
            //Collections of points on the drawing
            Vector3[] curLinePos = (Vector3[])currentLinePositions.ToArray();
            if(lineProperties != null)
            {
                lineProperties.drawLinePositions.Add(curLinePos);
                Debug.Log("LAST DRAW HAS "+ curLinePos.Length +" POSITIONS TO LOAD");
                lineProperties.lineMaterial.Add(materialIndex);
                lineProperties.lineStartSize.Add(currentLine.startWidth);
                lineProperties.lineEndSize.Add(currentLine.endWidth);
                lineProperties.lineStartColor.Add(currentLine.startColor);
                lineProperties.lineEndColor.Add(currentLine.endColor);
                //
                lineDrawing.Add(lineProperties);
            }
            else
            {
                Debug.Log("No lineProperties instanciated");
            }
        }

        private void ClearLineProperties()
        {
            //called from undo

            if(lineProperties.drawLinePositions.Count>0)
            {
                Debug.Log("PROPERTIES TO CLEAR: "+lineProperties.drawLinePositions.Count);
                lineProperties.drawLinePositions.Clear();
                lineProperties.lineMaterial.Clear();
                lineProperties.lineStartSize.Clear();
                lineProperties.lineEndSize.Clear();
                lineProperties.lineStartColor.Clear();
                lineProperties.lineEndColor.Clear();
                // Debug.Log("REMAINING PROPERTIES: "+lineProperties.drawLinePositions.Count);
            }
            else
            {
                Debug.Log("NO LINE PROPERTIES TO CLEAR");
            }

        }

        private void ClearCollections()
        {            
            lineRenderers.Clear();
            //Debug.Log(lineRenderers.Count + " LINES REMAINED UNDESTROYED");
            lineDrawing.Clear();
            currentLinePositions.Clear();
            Debug.Log("DRAWING DELETED");
            ClearLineProperties();
        }

        public void ClearDrawings()
        {
            if(noDrawing ==false)
            {
                ClearScreen();
            }
            else
            {
                DeleteDrawing();
            }
        }

        private void ClearScreen()
        {
            GameObject[] screenLines = GameObject.FindGameObjectsWithTag("original");
            if(screenLines.Length>0)
            {
                foreach(GameObject go in screenLines)
                {
                    Destroy(go);
                }
                noDrawing = true;
            }
        }

        private void DeleteDrawing()
        {
            GameObject[] screenLines = GameObject.FindGameObjectsWithTag("loaded");
            if(screenLines.Length>0)
            {
                foreach(GameObject go in screenLines)
                {
                    Destroy(go);
                    Debug.Log(screenLines.Length+" LINES DELETED");
                }  
                noDrawing = false;

                ClearCollections();
            }
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
#endregion

#region lineProperties Settings

        public void SetStartColor()
        {
            Debug.Log("Current Color is: "+fcp.color);
            startColor = fcp.color;

            if(currentLine != null)
            {
                // currentLine.startColor = startColor;
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                //new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                //new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(startColor.a, 0.0f), new GradientAlphaKey(endColor.a, 1.0f) }
                );

                currentLine.colorGradient = gradient;

            }
            startImage.color = startColor;
            startBrushsize.color = startColor;

        }

        public void SetEndColor()
        {
            endColor = fcp.color;

            if(currentLine != null)
            {
                // currentLine.endColor = endColor;
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                //new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                //new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(startColor.a, 0.0f), new GradientAlphaKey(endColor.a, 1.0f) }
                );

                currentLine.colorGradient = gradient;
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

        public void SetBrushMaterial()
        {
            materialIndex++;
            currentMaterial = brushMaterials[materialIndex];
            materialName.text = brushMaterials[materialIndex].name;

            if(materialIndex >= brushMaterials.Length -1)
            {
                materialIndex = -1;
            }
        }

        private void SetLineWidth()
        {
            currentLine.startWidth = startWidth;
            currentLine.endWidth = endWidth;
        }

        private void SetLineColor()
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(startColor.a, 0.0f), new GradientAlphaKey(endColor.a, 1.0f) }
            );

            currentLine.colorGradient = gradient;
        }

#endregion
    }

    [Serializable]
    public class LineProperties
    {
        // public List<Vector3> linePositions;
        public List<Vector3[]> drawLinePositions = new List<Vector3[]>();
        public List<int> lineMaterial = new List<int>();
        public List<float> lineStartSize = new List<float>();
        public List<float> lineEndSize = new List<float>();
        public List<Color> lineStartColor= new List<Color>();
        public List<Color> lineEndColor = new List<Color>();
    }
}



