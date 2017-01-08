using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace UserInputMacro
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private ButtonState buttonState;
		private ScriptRecorder recorder;

		public static RoutedCommand RecordCommand = new RoutedCommand();
		public static RoutedCommand PlayCommand = new RoutedCommand();
		public static RoutedCommand StopCommand = new RoutedCommand();

		public MainWindow()
		{
			buttonState = new ButtonState();
			InitializeComponent();

			recorder = new ScriptRecorder();
		}

		private void RecordCmdBinding_CanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = !buttonState.IsRecording && !buttonState.IsPlaying;
		}

		private void PlayCmdBinding_CanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = !buttonState.IsRecording && !buttonState.IsPlaying;
		}

		private void StopCmdBinding_CanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = buttonState.IsRecording || buttonState.IsPlaying;
		}

		private void RecordCmdBinding_Executed( object sender, ExecutedRoutedEventArgs e )
		{
			buttonState.IsRecording = true;
			recorder.StartRecording();
		}

		private async void PlayCmdBinding_ExecutedAsync( object sender, ExecutedRoutedEventArgs e )
		{
			try {
				await ScriptExecuter.ExecuteAsync( scriptPath.Text );
			}
			catch( Exception ex ) {
				MessageBox.Show( ex.ToString(), "Error" );
			}
		}

		private void StopCmdBinding_Executed( object sender, ExecutedRoutedEventArgs e )
		{
			recorder.EndRecording();
			buttonState.IsRecording = false;

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

		private void BrowseButton_Click( object sender, RoutedEventArgs e )
		{
			var dialog = new OpenFileDialog()
			{
				Title = "Open macro script",
				Filter = "Script File(*.csx)|*.csx|All files (*.*) |*.*",
				FilterIndex = 1
			};

			if( dialog.ShowDialog() == true ) {
				scriptPath.Text = dialog.FileName;
			}
		}
	}
}
