using System;
using System.Windows;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace UserInputMacro
{
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application
	{
		[DllImport( "Kernel32.dll" )]
		private	 static extern bool AttachConsole( int processId );

		const int ATTACH_PARENT_PROCESS = -1;

		private async void Application_StartupAsync( object sender, StartupEventArgs e )
		{
			try {
				// when no arguments are specified, execute main window 
				if( e.Args.Length == 0 ) {
					var window = new MainWindow();
					window.Show();
					return;
				}
				else if( e.Args.Length >= 2 ) {
					Usage();
				}

				Regex scriptArgPattern = new Regex( "-script=(?<scriptPath>.+)" );
				Match argsChacker = scriptArgPattern.Match( e.Args[ 0 ] );

				if( argsChacker.Success ) {
					await ScriptExecuter.ExecuteAsync( argsChacker.Groups[ "scriptPath" ].Value );
				}
				else {
					Usage();
				}
			}
			catch( Exception ex ) {
				Console.WriteLine( ex );
			}

			Current.Shutdown();
		}

		private void Usage()
		{
			AttachConsole( ATTACH_PARENT_PROCESS );
			Console.WriteLine( "Usage: UserInputMacro <option>" );
			Console.WriteLine( "[option list]" );
			Console.WriteLine( "script=<scirpt path>: Command line mode and only execute script" );
		}
	}
}
