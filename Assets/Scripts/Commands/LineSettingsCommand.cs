using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RogerioLima.ARPaint
{
    public class LineSettingsCommand : MonoBehaviour, ICommand
    {
        [SerializeField] DrawLineCommand drawLineCommand;
        [SerializeField] DrawOnPlaneCommand drawOnPlaneCommand;
        [SerializeField] private FlexibleColorPicker fcp;
        public Color externalColor;
        private Color internalColor;
        // current line
        [Space]
        [Header("Brush color settings")]
        [Space] 
        public Color startColor;
        public Color endColor;
        [SerializeField] private RawImage startImage;
        [SerializeField] private RawImage endImage;
        [SerializeField] Image startColorButton, endColorButton;
        [Space]
        [Header("Brush size settings")]
        [Space] 
        [SerializeField]
        private RawImage startBrushsize;
        [SerializeField]
        private RawImage endBrushsize;
        public float startWidth; // 1f = 1 meter in real life
        public float endWidth;
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
        public Image [] brushButtons;
        public Color selectedMaterial;
        public Color notSelectedMaterial;
        public Text materialName;
        public int materialIndex;
        public Material currentMaterial;
        LineRenderer currentLine;


        #region lineProperties Settings

        void Start()
        {
            startImage.color = fcp.startingColor;
            endImage.color = fcp.startingColor;
            startColor = Color.white;
            endColor = Color.white;
            startWidth = 0.01f;
            endWidth = 0.01f;
            currentMaterial = brushMaterials[0];
            materialName.text = brushMaterials[materialIndex].name;
            SetBrushendSize();
            SetBrushStartSize();
            SetBrushMaterial(0);
        }

        private LineRenderer CheckLineRenderers()
        {
            if(drawOnPlaneCommand.isActiveAndEnabled)
            {
                currentLine = drawOnPlaneCommand.currentLine;
            }

            if(drawLineCommand.isActiveAndEnabled)
            {
                currentLine = drawLineCommand.currentLine;
            }

            if(currentLine != null)
            {
                Debug.Log("I HAVE A LINE TO SETUP");
            }
            
            return currentLine;
        }

        public void SetStartColor()
        {
            currentLine = CheckLineRenderers();
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
            startColorButton.color = startColor;
        }

        public void SetEndColor()
        {
            endColor = fcp.color;
            currentLine = CheckLineRenderers();

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
            endColorButton.color = endColor;
        }

        public void SetBrushStartSize()
        {
            // startWidth = startSizeSlider.value * sliderMultiplier;
            float W = (startSizeSlider.value)*brushImageScaleMultiplier;
            float H = W;
            startBrushsize.GetComponent<RectTransform>().sizeDelta = new Vector2( W, H);
        }

        public void SetBrushendSize()
        {
            // endWidth = endSizeSlider.value * sliderMultiplier;
            float W = (endSizeSlider.value)*brushImageScaleMultiplier;
            float H = W;
            endBrushsize.GetComponent<RectTransform>().sizeDelta = new Vector2( W, H);

        }

        void Update()
        {
            if(internalColor != externalColor) 
            {
                fcp.color = externalColor;
                internalColor = externalColor;
            } 
        }

        public void SetBrushMaterial()
        {
            materialIndex++;
            currentMaterial = brushMaterials[materialIndex];
            materialName.text = brushMaterials[materialIndex].name;
            Debug.Log("CURRENT MATERIAL: "+ brushMaterials[materialIndex].name);

            if(materialIndex >= brushMaterials.Length -1)
            {
                materialIndex = -1;
            }
        }

        public void SetBrushMaterial(int index)
        {
            currentMaterial = brushMaterials[index];
            Debug.Log("CURRENT MATERIAL: "+ brushMaterials[index].name);
            foreach (Image i in brushButtons)
            {
                i.color = notSelectedMaterial;
            }
            brushButtons[index].color = selectedMaterial;
        }

        public void SetLineWidth()
        {
            currentLine = CheckLineRenderers();
            currentLine.startWidth = startWidth;
            currentLine.endWidth = endWidth;
        }

        public void SetLineColor()
        {
            currentLine = CheckLineRenderers();
            Gradient gradient = new Gradient();
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(startColor.a, 0.0f), new GradientAlphaKey(endColor.a, 1.0f) }
            );

            currentLine.colorGradient = gradient;
        }

        #endregion
        public bool CanExecute()
        {
            throw new System.NotImplementedException();
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

    }

}

