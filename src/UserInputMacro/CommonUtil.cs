using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace UserInputMacro
{
	class CommonUtil
	{
		const int ATTACH_PARENT_PROCESS = -1;

		[DllImport( "Kernel32.dll" )]
		private static extern bool AttachConsole( int processId );

		[DllImport( "Kernel32.dll" )]
		private static extern bool FreeConsole();

		public static bool CheckMode( ModeKind mode )
		{
			var currentMode = AppEnvironment.GetInstance().Mode;
			return ( currentMode & mode ) == mode;
		}

		public static double GetDpiWidth()
		{
			return GetTransform().M22;
		}

		public static double GetDpiHeight()
		{
			return GetTransform().M11;
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
			AttachConsole( ATTACH_PARENT_PROCESS );
			Console.WriteLine( str );
			FreeConsole();
		}

		private static Matrix GetTransform()
		{
			Matrix transform;

			var window = Application.Current.MainWindow ?? new MainWindow();
			var srcFromWindow = PresentationSource.FromVisual( window );

			if( srcFromWindow != null ) {
				transform = srcFromWindow.CompositionTarget.TransformFromDevice;
			}
			else {
				using( var hwndSrc = new HwndSource(new HwndSourceParameters()) ) {
					transform = hwndSrc.CompositionTarget.TransformFromDevice;
				}
			}

			return transform;
		}
	}
}
