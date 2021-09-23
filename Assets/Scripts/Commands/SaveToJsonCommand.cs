using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RogerioLima.ARPaint
{

    [RequireComponent(typeof(CommandManager))]
    public class SaveToJsonCommand : MonoBehaviour, ICommand
    {
        [SerializeField] CommandManager commandManager;
        [SerializeField] DrawLineCommand drawLineCommand;
        [SerializeField] LoadFromJsonCommand LoadFromJson;
        List<LineProperties> newDraw;
        LineProperties newLine;

        [SerializeField] InputField drawNameInput;
        [SerializeField] GameObject drawNameBtn, nameButtonsPanel, drawNamePanel,saveDrawingBtn,saveAsBtn;
        [SerializeField] Text drawNameInputText;
        [SerializeField] Transform drawNameBtnParent;
        private string drawName;
        private int linesToSaveAs;

        void Start()
        {
            nameButtonsPanel.SetActive(false);
            drawNamePanel.SetActive(false);
            newDraw = new List<LineProperties>();
        }

        public void SaveDrawingToJson()
        {
            drawNamePanel.SetActive(false);
            commandManager.Invoke(this);   
        }

        public void ActivateSaveDrawingPanel()
        {
            drawNamePanel.SetActive(true);
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
        }

        public void ActivateSaveAsDrawingPanel()
        {
            drawNamePanel.SetActive(true);
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

        public bool CanExecute()
        {
            if(drawLineCommand.currentLinePositions.Count > 0)
            {
                return true;
            }
            else
            {
                Debug.Log("NO DRAW TO SERIALIZE TO JSON");
                return false;
            }
        }

         public void SetDrawingNameAsTyped()
        {
            drawName = drawNameInput.text;
        }
        
        public void SetDrawingName()
        {
            drawName = drawNameInput.text;
            drawNameInputText.text = "";
        }

        public void Execute()
        {
            NewDrawing drawingToSave = new NewDrawing();
            drawingToSave.newDrawing = newDraw;
            string json = JsonUtility.ToJson(drawingToSave);
            // Debug.Log(json.ToUpper());
            string path =Application.persistentDataPath + "/"+drawName+".json";
            File.WriteAllText(path, json);
            InstantiateDrawNameBtn();
            PlayerPrefs.SetString(drawName,path);
            nameButtonsPanel.SetActive(true);
            newDraw.Clear();
        }

        public void SaveLine()
        {
            InstantiateAndPopulateLineProperties();
        }



        private void InstantiateAndPopulateLineProperties()
        {
            newLine = new LineProperties();
            newLine.drawLinePositions = drawLineCommand.currentLinePositions;
            newLine.lineMaterial = drawLineCommand.materialIndex;
            newLine.lineStartSize = drawLineCommand.startWidth;
            newLine.lineEndSize = drawLineCommand.endWidth;
            newLine.lineStartColor = drawLineCommand.startColor;
            newLine.lineEndColor = drawLineCommand.endColor;
            Debug.Log("NEW LINE POPULATED");
            NewDraw(newLine);
        }



        public void SaveLineAs(GameObject go)
        {
            LineRenderer line = go.GetComponent<LineRenderer>();
            newLine = new LineProperties();
            List<Vector3> l_positions = new List<Vector3>();
            for(int i = 0; i<line.positionCount; i++)
            {
                Vector3 v = line.GetPosition(i);
                l_positions.Add(v);
            }
            newLine.drawLinePositions = l_positions;

            for(int i = 0; i<drawLineCommand.brushMaterials.Length; i++)
            {
                Material currentMaterial = line.material;
                if(currentMaterial == drawLineCommand.brushMaterials[i])
                {
                    newLine.lineMaterial = i;
                }
            }

            newLine.lineStartSize = line.startWidth;
            newLine.lineEndSize = line.endWidth;
            newLine.lineStartColor = line.startColor;
            newLine.lineEndColor = line.endColor;
            NewDraw(newLine);
        }


        private void NewDraw(LineProperties newLine)
        {
            newDraw.Add(newLine);
            Debug.Log("ATÉ AGORA O DESENHO TEM "+newDraw.Count+" LINHAS");
            if(newDraw.Count == drawLineCommand.totalLines)
            {
                commandManager.Invoke(this);
            }
        }

        private void InstantiateDrawNameBtn()
        {
            GameObject currentDrawNameBtn = Instantiate(drawNameBtn, gameObject.transform.position,Quaternion.identity);
            Text drawNametext = currentDrawNameBtn.GetComponentInChildren<Text>();
            drawNametext.text = drawName;
            currentDrawNameBtn.transform.SetParent(drawNameBtnParent);
        }


        public void Undo()
        {
            throw new System.NotImplementedException();
        }

    }

    [Serializable]
    public class LineProperties
    {
        // public List<Vector3> linePositions;
        public List<Vector3> drawLinePositions = new List<Vector3>();
        public int lineMaterial;
        public float lineStartSize;
        public float lineEndSize;
        public Color lineStartColor;
        public Color lineEndColor;
    }

    [Serializable]
    public class  NewDrawing
    {
        public List<LineProperties> newDrawing = new List<LineProperties>();
    }

}


