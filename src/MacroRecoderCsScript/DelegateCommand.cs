using System;
using System.Windows.Input;

namespace MacroRecoderCsScript
{
	class DelegateCommand : ICommand
	{
		private readonly Action command;
		private readonly Func<bool> executable;

		public DelegateCommand( Action execute, Func<bool> canExecute )
		{
			command = execute;
			executable = canExecute;
		}

		public DelegateCommand( Action execute )
		{
			command = execute;
			executable = () => true;        // default
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
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}

			return canExecute;
		}

		public void Execute( object parameter )
		{
			try {
				command();
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}
	}
}
