using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
                Debug.Log(command.ToString() + " EXECUTED");
                command.Execute();

            }
        }

        public void Undo()
        {
            if (commands.Count > 0)
            {
                ICommand command = commands[commands.Count - 1];
                Debug.Log(commands.Count + " SAVED COMMANDS");
                Debug.Log(commands.Count + " COMMANDS SAVED");

                command.Undo();
                commands.RemoveAt(commands.Count-1);
            }
        }
    }
}


