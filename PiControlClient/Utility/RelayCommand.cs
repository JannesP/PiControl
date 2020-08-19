using System;
using System.Windows.Input;

namespace PiControlClient.Utility
{
    public class SimpleCommand<TParam> : ICommand
    {
        private readonly Action<TParam> _action;
        public SimpleCommand(Action<TParam> action)
        {
            _action = action;
        }
        
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (parameter == null) _action.Invoke(default!);
            if (parameter is TParam castedParam)
            {
                _action.Invoke(castedParam);
            }
            else
            {
                throw new ArgumentException($"Expected parameter of type '{typeof(TParam).FullName}', got '{parameter?.GetType().FullName}'", nameof(parameter));
            }  
        }

#pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067
    }
    
    public class SimpleCommand : ICommand
    {
        private readonly Action _action;
        public SimpleCommand(Action action)
        {
            _action = action;
        }
        
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (parameter == null)
            {
                _action.Invoke();
            }
            else
            {
                throw new ArgumentException($"Expected parameter to be null, got '{parameter?.GetType().FullName}'", nameof(parameter));
            }  
        }

#pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067
    }
}