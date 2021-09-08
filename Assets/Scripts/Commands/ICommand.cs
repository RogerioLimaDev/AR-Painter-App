using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RogerioLima.ARPaint
{
    public interface ICommand
    {
        void Execute();
        bool CanExecute();
        void Undo();
    }
}


