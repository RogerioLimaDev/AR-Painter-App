using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RogerioLima.ARPaint
{
    public class CommandManager : MonoBehaviour
    {
        private List<ICommand> commands = new List<ICommand>();

        public void Invoke(ICommand command)
        {
            if (command.CanExecute())
            {
                commands.Add(command);
                command.Execute();
            }
        }

        public void Undo()
        {
            if (commands.Count > 0)
            {
                ICommand command = commands[commands.Count - 1];
                command.Undo();
                commands.RemoveAt(commands.Count-1);
            }
        }
    }
}


