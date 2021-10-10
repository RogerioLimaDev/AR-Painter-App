using UnityEngine;


namespace Com.RogerioLima.ARPaint
{
    [RequireComponent(typeof(CommandManager))]
    public class DeleteLinesCommand : MonoBehaviour, ICommand
    {
        [SerializeField] DrawLineCommand drawLineCommand;
        [SerializeField] CommandManager commandManager;
        public bool noDrawing;

         //CHECK THE KIND OF LINES ARE ON THE SCREEN AND SELECT THE METHOD TO DESTROY THEM
        public void ClearDrawings()
        {
            commandManager.Invoke(this);
        }
        
        //COLLECT ALL LINES AND DESTROY THEM
        private void ClearScreen()
        {
            GameObject[] screenLines = GameObject.FindGameObjectsWithTag("original");
            GameObject[] drawLines = GameObject.FindGameObjectsWithTag("loaded");

            if(screenLines.Length>0)
            {
                foreach(GameObject go in screenLines)
                {
                    Destroy(go);
                }
                // noDrawing = true;
            } else if (drawLines.Length>0)
            {
                foreach(GameObject go in drawLines)
                {
                    Destroy(go);
                }
            }
            else
            {
                Debug.Log("No Lines to clear");
            }
        }

        
        //COLLECT LOADED LINES AND DESTROY THEM
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

        private void ClearCollections()
        {            
            drawLineCommand.lineRenderers.Clear();
            // drawLineCommand.lineDrawing.Clear();
            drawLineCommand.currentLinePositions.Clear();
            Debug.Log("DRAWING DELETED");
        }


        private void ClearLineProperties()
        {

        }

    #region Command implementation

        public bool CanExecute()
        {
            return true;
        }

        public void Undo()
        {

        }

        public void Execute()
        {
            if(drawLineCommand.noDrawing ==false)
            {
                ClearScreen();
            }
            else
            {
                DeleteDrawing();
            }

        }
        
    #endregion

    }
}
