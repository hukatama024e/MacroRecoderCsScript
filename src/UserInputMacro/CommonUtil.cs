using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace UserInputMacro
{
	class CommonUtil
	{
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
			MessageBox.Show( ex.ToString(), "Error" );

			// for executing destructor of hook
			GC.Collect();
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
