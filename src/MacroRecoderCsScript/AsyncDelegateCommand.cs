using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MacroRecoderCsScript
{
	class AsyncDelegateCommand : ICommand
	{
		private readonly Func<Task> command;
		private readonly Func<bool> executable;

		public AsyncDelegateCommand( Func<Task> execute, Func<bool> canExecute )
		{
			command = execute;
			executable = canExecute;
		}

		public AsyncDelegateCommand( Func<Task> execute )
		{
			command = execute;
			executable = () => true;		// default
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		protected void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		public bool CanExecute( object parameter )
		{
			bool canExecute = true;

			try {
				canExecute = executable();
			}
			catch(Exception ex) {
				CommonUtil.HandleException( ex );
			}

			return canExecute;
		}

		public async void Execute( object parameter )
		{
			try {
				await ExcecuteAsync();
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		private Task ExcecuteAsync()
		{
			return command();
		}
	}
}
