using System;
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

		public static void HandleException( Exception ex )
		{
			Logger.WriteErrorLog( ex );

			if( AppEnvironment.GetInstance().IsConsoleMode ) {
				WriteToConsole( ex.ToString() );
			}
			else {
				MessageBox.Show( ex.ToString(), "Error" );
			}

			// for executing destructor of hook
			GC.Collect();
		}

		public static void WriteToConsole( string str )
		{
			NativeMethods.AttachConsole( NativeMethods.ATTACH_PARENT_PROCESS );
			Console.WriteLine( str );
			NativeMethods.FreeConsole();
		}
	}
}
