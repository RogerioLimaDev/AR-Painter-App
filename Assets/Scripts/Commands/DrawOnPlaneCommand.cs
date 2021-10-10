using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

namespace Com.RogerioLima.ARPaint
{
    public class DrawOnPlaneCommand : MonoBehaviour, ICommand
    {
    #region variables
        //[SerializeField] Text notifications,infos, features;
        [SerializeField] ARPointCloudManager mPointCloudManager;
        [SerializeField] GameObject lineRendererObject;
        [SerializeField] CommandManager commandManager;
        [SerializeField] LineSettingsCommand lineSettingsCommand;
        [SerializeField] SaveToJsonCommand saveToJson;


        // private Vector3 lastCursorPosition;
        private int mCurrentTrackedFeatureCount = 1000;
        private float timeOfLastPtCloudUpdate;
        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
        public List<Vector3> currentLinePositions;
        public ARRaycastManager m_RaycastManager;
        bool reticleStarted = false;
        [HideInInspector]
        public LineRenderer currentLine;
        public bool canDraw;
        int currentLinePointNumber;
        bool isPainting;
        bool touchBegan;
        Vector3 op1;

    #endregion

    #region iCommand implementation
        public bool CanExecute()
        {
            if(canDraw == true)
            {
               return true;
            }
            
            return false;
        }
        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        public void Execute()
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    touchBegan = true;
                    isPainting = true;
                }
                else if(Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    //Save each line and add to a draw collection for recovery
                    saveToJson.SaveLine();
                    isPainting = false;
                }

                if(isPainting == true)
                {
                    GrabPositionOnPlane();  
                }
        }

    #endregion
        //Detect positions on planes hit by raycast and pass to draw functions
        void GrabPositionOnPlane()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            // bool b_raycast = m_RaycastManager.Raycast(rayOrigin, s_Hits, TrackableType.FeaturePoint);
            bool b_raycast = m_RaycastManager.Raycast(rayOrigin, s_Hits, TrackableType.PlaneEstimated);

            //If a position on a plane was detected start drawing
            if (b_raycast)
            {
                if(s_Hits.Count > 0)
                {
                    var targetPose = s_Hits[0].pose;
                    op1 = new Vector3();
                    op1 = targetPose.position;

                    if(touchBegan == true)
                    {
                        StartLineOnPlane(op1);
                    }
                }
            }
            else
            {
                Debug.Log("No target pose yet".ToUpper());
            }

            //Touch already began so keep drawing
            if(touchBegan == false)
            {
                UpdateLineOnPlane(op1);
            }
        }

        void UpdateLineOnPlane(Vector3 position)
        {
            currentLine.positionCount = currentLinePointNumber + 1;
            //Set the vector3 to the position index
            currentLine.SetPosition(currentLinePointNumber, position);
            //Save position to a collection
            currentLinePositions.Add(position);
            //Increase the point
            currentLinePointNumber++;
        }

        void StartLineOnPlane(Vector3 l_position)
        {
            //Clear last line
            currentLinePositions.Clear();
            //reset initial point
            currentLinePointNumber = 0;
            //Instantiate a line object and get the line component
            GameObject emptyGameObject = Instantiate(lineRendererObject, l_position, Quaternion.identity);
            currentLine = emptyGameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
            //Set the received position to the starting point of the line
            currentLine.SetPosition(currentLinePointNumber, l_position);
            currentLinePositions.Add(l_position);
            //Set the line properties
            currentLine.numCapVertices = 3;
            currentLine.numCornerVertices = 5;
            currentLine.material = lineSettingsCommand.currentMaterial;
            currentLine.startWidth = lineSettingsCommand.startWidth;
            currentLine.endWidth = lineSettingsCommand.endWidth;
            currentLine.startColor = lineSettingsCommand.startColor;
            currentLine.endColor = lineSettingsCommand.endColor;
            touchBegan = false;
            //Reset the point number to the next drawing
            currentLinePointNumber = 0;
        }

    #region Monobehaviors
        void Update()
        {
            if(Input.touchCount>0)
            {
                commandManager.Invoke(this);
            }
            // notifications.text = "IS PAINTING: "+isPainting;
        }

        void Start()
        {
            mPointCloudManager.pointCloudsChanged += OnPointCloudChanged;
            isPainting = false;
            // lastCursorPosition = new Vector3(-200,-200,-200);
            currentLinePointNumber = 0;

        }
    
    #endregion
//Not used on the script
        private void OnPointCloudChanged(ARPointCloudChangedEventArgs eventargs)
        {
            if (eventargs.updated.Count == 1)
            {
                foreach (var ptcloud in eventargs.updated)
                {
                    NativeSlice<Vector3> ptCollection = (NativeSlice<Vector3>)ptcloud.positions;
                    mCurrentTrackedFeatureCount = ptCollection.Length;
                    // notifications.text = "Feature count = " + mCurrentTrackedFeatureCount;
                }

                timeOfLastPtCloudUpdate = Time.time;

                if(!reticleStarted)
                {
                    reticleStarted = true;
                    // StartLineOnPlane();
                }
            }
        }
    
    }

}

