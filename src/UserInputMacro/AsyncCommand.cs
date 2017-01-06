using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace UserInputMacro
{
	class AsyncCommand : ICommand
	{
		private Func<Task> execute;
		private Func<bool> canExecute;
		private CancellationTokenSource cancelSrc; 

		public AsyncCommand( Func<Task> executeTask, Func<bool> canExecuteFunc )
		{
			execute = executeTask;
			canExecute = canExecuteFunc;
		}

		event EventHandler ICommand.CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute( object parameter )
		{
			return canExecute();
		}

		public async void Execute( object parameter )
		{
			cancelSrc = new CancellationTokenSource();
			await execute();
		}

		public void Cancel()
		{
			cancelSrc?.Cancel();
		}
	}
}
