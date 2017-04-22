using System;
using System.Threading.Tasks;
using System.Windows;

namespace UserInputMacro
{
	class CommonUtil
	{
		public static bool CheckMode( ModeKind mode )
		{
			var currentMode = AppEnvironment.GetInstance().Mode;
			return ( currentMode & mode ) == mode;
		}

		public async static Task HandleExceptionAsync( Exception ex )
		{
			await Logger.WriteErrorLogAsync( ex );
			await Task.Run( () => ProcessException( ex ) );
		}

		public static void HandleException( Exception ex )
		{
			Logger.WriteErrorLog( ex );
			ProcessException( ex );
		}

		public static void WriteToConsole( string str )
		{
			NativeMethods.AttachConsole( NativeMethods.ATTACH_PARENT_PROCESS );
			Console.WriteLine( str );
			NativeMethods.FreeConsole();
		}

		private static void ProcessException( Exception ex )
		{
			if( AppEnvironment.GetInstance().IsConsoleMode ) {
				WriteToConsole( ex.ToString() );
			}
			else {
				MessageBox.Show( ex.ToString(), "Error" );
			}

			// for executing destructor of hook
			GC.Collect();
		}
	}
}
