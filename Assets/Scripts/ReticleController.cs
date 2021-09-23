using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Com.RogerioLima.ARPaint
{

    public class ReticleController : MonoBehaviour
    {
        #region variables
        [SerializeField] GameObject mReticle;
        [SerializeField] Text notifications, cursorInfo;

        [SerializeField] ARPointCloudManager mPointCloudManager;
        private Vector3 lastCursorPosition;
        private int mCurrentTrackedFeatureCount = 1000;
        private float timeOfLastPtCloudUpdate;

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
        public ARRaycastManager m_RaycastManager;

        private IEnumerator mGoToTarget;
        private IEnumerator mContinuousHittest;
        bool reticleStarted = false;
        public LineRenderer currentLine;
        [SerializeField] GameObject lineRendererObject;
        int currentLinePointNumber;



        #endregion
        void Start()
        {
            // mContinuousHittest = ContinuousHittest();

            //Initialize the cursor position to this Vector
            lastCursorPosition = new Vector3(-200f, -200f, -200f);
            mPointCloudManager.pointCloudsChanged += OnPointCloudChanged;

            //StartReticle();

        }

        // starts the cursor
        // public void StartReticle()
        // {
        //     // mReticle.SetActive(false);
        //     StartCoroutine(mContinuousHittest);
        // }

        // public void StopReticle()
        // {
        //     StopCoroutine(mContinuousHittest);
        //     // mReticle.SetActive(false);
        // }

        void LateUpdate()
        {
            if(reticleStarted)
            {
                HitTestAlternative();
            }
        }

        void StartLineOnPlane()
        {
            GameObject emptyGameObject = Instantiate(lineRendererObject, Camera.main.transform.position, Quaternion.identity);
            currentLine = emptyGameObject.GetComponent(typeof(LineRenderer)) as LineRenderer;
            currentLine.numCapVertices = 3;
            currentLine.numCornerVertices = 5;
            currentLinePointNumber = 0;
            currentLine.startWidth = 0.01f;
            currentLine.endWidth = 0.01f;
            Debug.Log("ONE LINE INSTANTIATED");
        }

        void UpdateLineOnPlane(Vector3 position)
        {
            int posCount = currentLine.positionCount;
            currentLine.positionCount = currentLinePointNumber + 1;
            //Set the vector3 to the position index
            currentLine.SetPosition(currentLinePointNumber, position);
            //Increase the point
            currentLinePointNumber++;
        }

        void HitTestAlternative()
        {
            bool withinDistance = false;
            int badTrackingCounter = 0;

                // getting screen point
            var screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

            // World Hit Test - Type PlaneWithinBounds
            // if (m_RaycastManager.Raycast(screenPosition, s_Hits, TrackableType.FeaturePoint))
            if (m_RaycastManager.Raycast(screenPosition, s_Hits, TrackableType.Planes))
            {
                // Raycast hits are sorted by distance, so get the closest hit.
                var targetPose = s_Hits[0].pose;
                // mReticle.transform.position = targetPose.position;
                // mReticle.transform.rotation = targetPose.rotation;

                UpdateLineOnPlane(targetPose.position);
                // Grab camera center vector
                Vector3 screenCenter = Camera.main.ScreenToWorldPoint(screenPosition);
                //calculate distance from camera to the closest hit
                float distanceToReticle = Vector3.Magnitude(targetPose.position - screenCenter);
                float reticleDistanceChange = (targetPose.position - lastCursorPosition).magnitude;
                cursorInfo.text = "Hit position = " + targetPose.position;
                
                // mReticle.transform.LookAt(Camera.main.transform);

                if (reticleDistanceChange < 0.03f)
                {
                    Debug.Log("Camera too close to target");
                    // do nothing
                }
                else
                {
                    // too few features
                    if (mCurrentTrackedFeatureCount < 30) //  && trustworthyHitTestDistanceChange > 0.15f
                    {
                        // Debug.Log("Cursor: Too few features");
                        notifications.text = "Cursor: Feature count below 30";
                    }
                    //Too far
                    else if (withinDistance == false && distanceToReticle > 1.7f)
                    {
                        // Debug.Log("Camera too far to draw cursor");
                        notifications.text = "Move closer than 1.7 mts to activate the focus ring";
                        badTrackingCounter = 0;
                    }
                    // too far
                    else if (withinDistance == true && distanceToReticle > 1.73f) // hysteresis for distance
                    {
                        
                        withinDistance = false;
                        // Debug.Log("Camera too far to draw cursor");
                        notifications.text = "Move closer to activate the focus ring";
                        badTrackingCounter = 0;
                    }
                    // Passed all checks
                    else
                    {
                        notifications.text = "Cursor: Passed all checks";                            
                        withinDistance = true;
                        badTrackingCounter++;
                        // Debug.Log("Bad tracking count: "+badTrackingCounter);

                        if (badTrackingCounter > 0)
                        {
                            Debug.Log("Cursor: Showing cursor");

                            // mReticle.SetActive(true);

                            mReticle.transform.position = targetPose.position;
                            mReticle.transform.LookAt(Camera.main.transform);
                            notifications.text = "";
                            // update the last cursor position
                            lastCursorPosition = targetPose.position;

                            // stop the previous animation
                            if (mGoToTarget != null)
                            {
                                StopCoroutine(mGoToTarget);
                            }

                            // start new animation to go to this destination
                            mGoToTarget = GoToTarget(targetPose.position);
                            StartCoroutine(mGoToTarget);

                        }
                    }
                }
            }
        }

        IEnumerator GoToTarget(Vector3 destination)
        {
            float distance = (destination - mReticle.transform.position).magnitude;
            while (distance > 0)
            {
                float step = distance * Time.deltaTime / 0.2f;
                // Move our position a step closer to the target.
                mReticle.transform.position = Vector3.MoveTowards(mReticle.transform.position, destination, step);
                // update distance
                distance = (destination - mReticle.transform.position).magnitude;

                yield return null;
            }

        }

        // private IEnumerator ContinuousHittest()
        // {

        //     bool withinDistance = false;
        //     int badTrackingCounter = 0;

        //         // getting screen point
        //     var screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        //     // World Hit Test - Type PlaneWithinBounds
        //     if (m_RaycastManager.Raycast(screenPosition, s_Hits, TrackableType.FeaturePoint))
        //     {
        //         // Raycast hits are sorted by distance, so get the closest hit.
        //         var targetPose = s_Hits[0].pose;
        //         // Grab camera center vector
        //         Vector3 screenCenter = Camera.main.ScreenToWorldPoint(screenPosition);
        //         //calculate distance from camera to the closest hit
        //         float distanceToReticle = Vector3.Magnitude(targetPose.position - screenCenter);
        //         float reticleDistanceChange = (targetPose.position - lastCursorPosition).magnitude;
        //         cursorInfo.text = "Cursor: Distance = " + distanceToReticle;

        //         if (reticleDistanceChange < 0.03f)
        //         {
        //             Debug.Log("Camera too close to target");
        //             // do nothing
        //         }

        //         else
        //         {
        //             // too few features
        //             if (mCurrentTrackedFeatureCount < 30) //  && trustworthyHitTestDistanceChange > 0.15f
        //             {
        //                 // Debug.Log("Cursor: Too few features");
        //                 notifications.text = "Cursor: Too few features";
        //             }
        //             //Too far
        //             else if (withinDistance == false && distanceToReticle > 1.7f)
        //             {
        //                 // Debug.Log("Camera too far to draw cursor");
        //                 notifications.text = "Move closer to activate the focus ring";
        //                 badTrackingCounter = 0;
        //             }
        //             // too far
        //             else if (withinDistance == true && distanceToReticle > 1.73f) // hysteresis for distance
        //             {
                        
        //                 withinDistance = false;
        //                 // Debug.Log("Camera too far to draw cursor");
        //                 notifications.text = "Move closer to activate the focus ring";
        //                 badTrackingCounter = 0;
        //             }
        //             // Passed all checks
        //             else
        //             {
        //                 notifications.text = "Cursor: Passed all checks";                            
        //                 withinDistance = true;
        //                 badTrackingCounter++;

        //                 if (badTrackingCounter > 10)
        //                 {
        //                     Debug.Log("Cursor: Showing cursor");

        //                     mReticle.SetActive(true);

        //                     mReticle.transform.LookAt(Camera.main.transform);
        //                     notifications.text = "";
        //                     // update the last cursor position
        //                     lastCursorPosition = targetPose.position;

        //                     // stop the previous animation
        //                     if (mGoToTarget != null)
        //                     {
        //                         StopCoroutine(mGoToTarget);
        //                     }

        //                     // start new animation to go to this destination
        //                     mGoToTarget = GoToTarget(targetPose.position);
        //                     StartCoroutine(mGoToTarget);

        //                 }
        //             }
        //         }

        //         // go to next frame
        //         yield return null;
        //     }
        // }

        // Add this
        void OnPointCloudChanged(ARPointCloudChangedEventArgs eventargs)
        {
            if (eventargs.updated.Count == 1)
            {
                foreach (var ptcloud in eventargs.updated)
                {
                    NativeSlice<Vector3> ptCollection = (NativeSlice<Vector3>)ptcloud.positions;
                    mCurrentTrackedFeatureCount = ptCollection.Length;
                    // Debug.Log("Cursor: Current tracked feature count = " + mCurrentTrackedFeatureCount);
                    notifications.text = "Feature count = " + mCurrentTrackedFeatureCount;
                }

                timeOfLastPtCloudUpdate = Time.time;
                if(!reticleStarted)
                {
                    // StartReticle();
                    reticleStarted = true;
                    StartLineOnPlane();
                    Debug.Log("RETICLE STARTED");
                }
            }
        }
    }
}


