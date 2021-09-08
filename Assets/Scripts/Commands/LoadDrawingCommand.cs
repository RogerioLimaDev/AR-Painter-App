using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Com.RogerioLima.ARPaint
{
    public class LoadDrawingCommand : MonoBehaviour, ICommand
    {
        [SerializeField] GameObject lineObject;
        DrawLineCommand drawLineCommand;
        CommandManager commandManager;

        void Start()
        {
            commandManager = gameObject.GetComponent<CommandManager>();
            drawLineCommand = gameObject.GetComponent<DrawLineCommand>();
        }
        public bool CanExecute()
        {
            if(drawLineCommand.lineRenderers.Count>0)
            {
                Debug.Log("THERE IS A SAVED DRAWING");
                return true;
            }
            else
            {
                Debug.Log("NO SAVED DRAWING");
                return false;
            }
        }

        public void Execute()
        {
            Debug.Log(drawLineCommand.lineDrawing.Count+" DRAWINGS SAVED");

            //Resgata as propriedades do ultimo desenho salvo
            //Numero de linhas do desenho
            var m_index = drawLineCommand.lineDrawing.Count-1;
            LineProperties lp = drawLineCommand.lineDrawing[m_index];
            Debug.Log("O DESENHO TEM "+lp.lineStartColor.Count+" LINHAS");

            //Loop pelas propriedades de cada linha
            for(int i = 0 ; i < (lp.lineStartColor.Count); i++)
            {
                DrawOneLine(i,lp);
            }
        }

        private void DrawOneLine(int i, LineProperties lp)
        {
            LineRenderer cur_line = Instantiate(lineObject,Camera.main.transform.position, Quaternion.identity).GetComponent<LineRenderer>();
            int m_index = i+1;
            Debug.Log("LINHA "+m_index+" INSTANCIADA");

            //Atribui um valor para a espessura da linha
            cur_line.startWidth = lp.lineStartSize[i];
            cur_line.endWidth = lp.lineEndSize[i];
            cur_line.numCapVertices = 3;
            cur_line.numCornerVertices = 5;

            cur_line.startColor = lp.lineStartColor[i];
            cur_line.endColor = lp.lineEndColor[i];
            cur_line.material = drawLineCommand.brushMaterials[lp.lineMaterial[i]];
            cur_line.loop = false;
            //Atribui o numero de ponttos na linha de acordo com o numero de pontos salvos na colecão
            cur_line.positionCount = lp.drawLinePositions[i].Length;
            Debug.Log("LINE "+m_index+" HAS " +cur_line.positionCount+ " POINTS");

            //Transforma a lista de pontos salvos em um Array
            Vector3[] l_array = lp.drawLinePositions[i].ToArray();
            l_array.Skip(2);
            //Atribui o array de pontos a linha
            cur_line.SetPositions(l_array);
        }
        

        public void Undo()
        {
            Destroy(lineObject);
        }

        public void LoadDrawing()
        {
            commandManager.Invoke(this);

        }

    }
}


