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
        [SerializeField] LineSettingsCommand lineSettingsCommand;
        [SerializeField] GameObject lineRendererObject;
        [SerializeField] SaveToJsonCommand saveToJson;
        [SerializeField] private CommandManager commandManager;
        
    #endregion
        public bool canDraw;
        public LineRenderer currentLine;
        private int currentLinePointNumber;
        private bool isPainting;
        public bool noDrawing;

        #region Collections
        public List<Vector3> currentLinePositions;
        public List<LineRenderer> lineRenderers;

        #endregion

        #region monobehaviors
        void Start()
        {
            currentLinePositions = new List<Vector3>();
            lineRenderers = new List<LineRenderer>();
            currentLinePointNumber = 0;
            lineSettingsCommand.currentMaterial = lineSettingsCommand.brushMaterials[0];
            isPainting = false;
            noDrawing = false;
            canDraw = true;
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
                isPainting = true;
                currentLinePositions = new List<Vector3>();
                StartNewLine();
                lineSettingsCommand.SetLineWidth();
                lineSettingsCommand.SetLineColor();
            }

            // just lifted finger up
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                //Save Line object for Undo purposes
                lineRenderers.Add(currentLine);
                //Save each line and add to a draw collection for recovery
                saveToJson.SaveLine();
                // stop painting
                isPainting = false;
            }

            // painting
            if(isPainting == true)
            {
                UpdateLine(CalculateFingerPosition());
            }

        
        }

        public void Undo()
        {
            Debug.Log("UNDO CLICKED ON DRAWLINE COMMAND");
        }

        private void StartNewLine()
        {
            currentLinePositions.Clear();
            GameObject emptyGameObject = Instantiate(lineRendererObject, Camera.main.transform.position, Quaternion.identity);
            currentLine = emptyGameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
            currentLine.material = lineSettingsCommand.currentMaterial;
            currentLine.numCapVertices = 3;
            currentLine.numCornerVertices = 5;
            currentLinePointNumber = 0;
            currentLine.startWidth = lineSettingsCommand.startWidth;
            currentLine.endWidth = lineSettingsCommand.endWidth;
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

        // raycast from the finger on the screen to the plane in front of the camera
        // and return the hit position in worldspace 
        private Vector3 CalculateFingerPosition()
        {
            Vector2 fingerPosition= Input.GetTouch(0).position;
            // Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);


            Ray rayOrigin = Camera.main.ScreenPointToRay(fingerPosition);
            RaycastHit hitInfo;

            Vector3 rayTest = rayOrigin.origin;

            if(rayTest != null)
            {
                Debug.Log("Ray origin found".ToUpper());
            }
            else
            {
                Debug.Log("Missing ray origin".ToUpper());
            }

            // layer 8 is reserved for the plane in front of the camera
            if (Physics.Raycast(rayOrigin, out hitInfo, 1 << 8))
            {
                return hitInfo.point;
            }

            // raycast missed plane 
            else
            {
                Debug.Log("Broken");
                return new Vector3(fingerPosition.x, fingerPosition.y, 0);
            }
        }

        
        #endregion
    }
}



