using System.IO;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace UserInputMacro
{
	class MainWindowViewModel : INotifyPropertyChanged
	{
		private ButtonState buttonState;
		private ScriptRecorder recorder;
		private string scriptPath;

		private static readonly PropertyChangedEventArgs scriptPathChangedEventArgs = new PropertyChangedEventArgs( nameof( ScriptPath ) );
		public event PropertyChangedEventHandler PropertyChanged;

		public DelegateCommand RecordCommand { get; set; }
		public DelegateCommand StopCommand { get; set; }
		public DelegateCommand BrowseCommand { get; set; }
		public AsyncDelegateCommand PlayCommand { get; set; }

		public Dispatcher WinDispacher { get; set; }

		public string ScriptPath
		{
			get { return scriptPath; }
			set {
				if( scriptPath == value ) {
					return;
				}

				scriptPath = value;
				PropertyChanged?.Invoke( this, scriptPathChangedEventArgs );
			}
		}

		public MainWindowViewModel()
		{
			buttonState = new ButtonState();
			recorder = new ScriptRecorder();

			RecordCommand = new DelegateCommand( RecordCmd_Execute, RecordCmd_CanExecute );
			StopCommand = new DelegateCommand( StopCmd_Execute, StopCmd_CanExecute );
			BrowseCommand = new DelegateCommand( BrowseCmd_Execute );
			PlayCommand = new AsyncDelegateCommand( PlayCmd_ExecuteAsync, PlayCmd_CanExecute );
		}

		private bool RecordCmd_CanExecute()
		{
			return !buttonState.IsRecording && !buttonState.IsPlaying;
		}

		private bool PlayCmd_CanExecute()
		{
			return !buttonState.IsRecording && !buttonState.IsPlaying;
		}

		private bool StopCmd_CanExecute()
		{
			return buttonState.IsRecording || buttonState.IsPlaying;
		}

		private void RecordCmd_Execute()
		{
			buttonState.IsRecording = true;
			recorder.StartRecording();
		}

		private async Task PlayCmd_ExecuteAsync()
		{
			buttonState.IsPlaying = true;
			await Task.Delay( 50 );
			WinDispacher?.Invoke( new Action( CommandManager.InvalidateRequerySuggested ) );

			await ScriptExecuter.ExecuteAsync( ScriptPath );
			buttonState.IsPlaying = false;
		}

		private void StopCmd_Execute()
		{
			if( buttonState.IsRecording ) {
				recorder.EndRecording();
				buttonState.IsRecording = false;
				SaveMacroScript();
			}
			else if( buttonState.IsPlaying ) {
				buttonState.IsPlaying = false;
			}
		}

		private void BrowseCmd_Execute()
		{
			var dialog = new OpenFileDialog()
			{
				Title = "Open macro script",
				Filter = "Script File(*.csx)|*.csx|All files (*.*) |*.*",
				FilterIndex = 1
			};

			if( dialog.ShowDialog() == true ) {
				ScriptPath = dialog.FileName;
			}
		}

		private void SaveMacroScript()
		{
			var dialog = new SaveFileDialog()
			{
				Title = "Save macro script",
				Filter = "Script File(*.csx)|*.csx|All files (*.*) |*.*",
				FilterIndex = 1
			};

			if( dialog.ShowDialog() == true ) {
				using( var fs = dialog.OpenFile() ) {
					using( var sw = new StreamWriter( fs ) ) {
						sw.Write( recorder.Record );
					}
				}
			}
		}
	}
}
