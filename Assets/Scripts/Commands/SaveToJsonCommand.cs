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
        [SerializeField] DrawOnPlaneCommand drawOnPlaneCommand;
        [SerializeField] LineSettingsCommand lineSettingsCommand;
        [SerializeField] SavedDrawingsManager savedDrawings;
        // [SerializeField] PanelsController panelsController;
        [SerializeField] newUIController newUiController;
        List<LineProperties> newDraw;
        LineProperties newLine;
        [SerializeField] InputField drawNameInput;
        [SerializeField] Text drawNameInputText;
        [SerializeField] Transform drawNameBtnParent;
        private string drawName;
        public int totalLines;

        void Start()
        {
            newDraw = new List<LineProperties>();
        }

        //Save the latest drawing and clear collection
        public void SaveNewDraw()
        {
            commandManager.Invoke(this);
            
        }

        //Save all lines on the screen and clear collection
        public void SaveDrawAs()
        {
            SetDrawingName();
            GetLinesToSave();
            commandManager.Invoke(this);
        }

        public bool CanExecute()
        {
            return true;
        }

        //Save drawings to a JSON file
        public void Execute()
        {
            NewDrawing drawingToSave = new NewDrawing();
            drawingToSave.newDrawing = newDraw;

            string json = JsonUtility.ToJson(drawingToSave);
            string path =Application.persistentDataPath + "/"+drawName+".json";
            File.WriteAllText(path, json);

            savedDrawings.CheckSavedFiles(drawName);

            // panelsController.EnableLoadDrawingPanel();
            newUiController.EnableLoadPanel();
            newDraw.Clear();
        }

        //Called from draw commands
        public void SaveLine()
        {
            InstantiateAndPopulateLineProperties();
        }
        
        //Called from SaveLine - add lines to the latest drawing
        private void InstantiateAndPopulateLineProperties()
        {
            newLine = new LineProperties();
            string activeCommand = "";
            int activeCount;

            if(drawLineCommand.enabled==true)
            {
                newLine.drawLinePositions = CopyList(drawLineCommand.currentLinePositions);
                // activeCount = newLine.drawLinePositions.Count;
                activeCommand = "DRAW LINE";
            }

            if(drawOnPlaneCommand.enabled==true)
            {
                newLine.drawLinePositions = CopyList(drawOnPlaneCommand.currentLinePositions);
                // activeCount = newLine.drawLinePositions.Count;
                activeCommand = "DRAW LINE ON PLANE";
            }

            activeCount = newLine.drawLinePositions.Count;

            newLine.lineMaterial = lineSettingsCommand.materialIndex;
            newLine.lineStartSize = lineSettingsCommand.startWidth;
            newLine.lineEndSize = lineSettingsCommand.endWidth;
            newLine.lineStartColor = lineSettingsCommand.startColor;
            newLine.lineEndColor = lineSettingsCommand.endColor;
            newDraw.Add(newLine);

            Debug.Log("PTS POSITIONS COUNTFROM "+activeCommand+"is: "+activeCount);
            Debug.Log("LINE SAVED FROM "+activeCommand);

            // Debug.Log("NEW LINE POPULATED");
            // NewDraw(newLine);
        }

        private List<Vector3> CopyList(List<Vector3> source)
        {
            List<Vector3> destination = new List<Vector3>();
            for(int i = 0; i< source.Count; i++)
            {
                destination.Add(source[i]);
            }
            return destination;
        }

        //grab all lines on the screen
        private void GetLinesToSave()
        {
            GameObject[] screenLines = GameObject.FindGameObjectsWithTag("original");
            GameObject[] drawLines = GameObject.FindGameObjectsWithTag("loaded");
            totalLines = screenLines.Length  + drawLines.Length;

            if(screenLines.Length>0)
            {
                foreach(GameObject go in screenLines)
                {
                    SaveLineAs(go);
                    Debug.Log("SALVEI UMA SCREEN LINE");
                }
            }
                // noDrawing = true;
            
            if (drawLines.Length>0)
            {
                foreach(GameObject go in drawLines)
                {
                    SaveLineAs(go);
                    Debug.Log("SALVEI UMA DRAWLINE");
                }
            }
        }
        //extract properties from the lines on the screen
        private void SaveLineAs(GameObject go)
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

            for(int i = 0; i<lineSettingsCommand.brushMaterials.Length; i++)
            {
                Material currentMaterial = line.material;
                if(currentMaterial == lineSettingsCommand.brushMaterials[i])
                {
                    newLine.lineMaterial = i;
                }
            }

            newLine.lineStartSize = line.startWidth;
            newLine.lineEndSize = line.endWidth;
            newLine.lineStartColor = line.startColor;
            newLine.lineEndColor = line.endColor;
            newDraw.Add(newLine);
        }
        //Set the drawname string as typed on the UI
         public void SetDrawingNameAsTyped()
        {
            drawName = drawNameInput.text;
        }
        //Set the drawname string when button pressed
        public void SetDrawingName()
        {
            drawName = drawNameInput.text;
        }
        
        //Not implemented yet
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


