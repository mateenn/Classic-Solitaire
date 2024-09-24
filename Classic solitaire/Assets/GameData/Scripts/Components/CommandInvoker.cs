using System.Collections.Generic;
using UnityEngine;

namespace TheSyedMateen.ClassicSolitaire
{
    public static class CommandInvoker
    {
        private static readonly Stack<ICommand> UndoStack = new Stack<ICommand>();

        // Executes the command
        // pushes it onto the undo stack
        public static void ExecuteCommand(ICommand command)
        {
            command.Execute();
            UndoStack.Push(command);
        }

        // Pops the last command and undoes it
        public static void UndoCommand()
        {
            
            if (UndoStack.Count > 0)
            {
                var activeCommand = UndoStack.Pop();
                activeCommand.Undo();
            }
        }

        //Clear all commands in the undo stack
        public static void ClearUndoStack()
        {
            UndoStack.Clear();
        }
    }
}
