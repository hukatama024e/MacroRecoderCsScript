using System;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Scripting;

namespace UserInputMacro
{
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application
	{
		private async void Application_StartupAsync( object sender, StartupEventArgs e )
		{
			try {
				// when no arguments are specified, execute main window 
				if( e.Args.Length == 0 ) {
					AppEnvironment.GetInstance().IsConsoleMode = false;
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
					string filePath = argsChacker.Groups[ "scriptPath" ].Value;
					if( File.Exists( filePath ) ){
						await ScriptExecuter.ExecuteAsync( filePath );
					}
					else {
						CommonUtil.WriteToConsole( "[File Error]" + Environment.NewLine + "'" + filePath + "' is not found." );
					}
				}
				else {
					Usage();
				}
			}
			catch( CompilationErrorException ex ) {
				CommonUtil.WriteToConsole( "[Compile Error]" + Environment.NewLine + ex.Message );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}

			Current.Shutdown();
		}

		private void Usage()
		{
			CommonUtil.WriteToConsole( "Usage: UserInputMacro <option>" + Environment.NewLine +
										"[option]" + Environment.NewLine +
										"-script=<scirpt path>: Command line mode and only execute script" );
		}
	}
}
