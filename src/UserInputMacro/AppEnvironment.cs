using System;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace UserInputMacro
{
	[Flags]
	public enum ModeKind : byte
	{
		None = 0x00,
		MouseOnly = 0x01,
		KeyOnly = 0x02,
		CreateLog = 0x04
	};

	class AppEnvironment
	{
		private static AppEnvironment instance = new AppEnvironment();
		public ModeKind Mode { get; set; } = ModeKind.None;
		public bool IsConsoleMode { get; set; } = true;
		public double DpiWidth { get; internal set; } = 1.0;
		public double DpiHeight { get; internal set; } = 1.0;
		public CancellationToken CancelToken { get; set; }

		private AppEnvironment()
		{
		}

		public static AppEnvironment GetInstance()
		{
			return instance;
		}

		public void DpiSetting()
		{
			var transform = GetTransform();
			DpiWidth = transform.M22;
			DpiHeight = transform.M11;
		}

		private Matrix GetTransform()
		{
			Matrix transform;

			var window = Application.Current.MainWindow ?? new MainWindow();
			var srcFromWindow = PresentationSource.FromVisual( window );

			if( srcFromWindow != null ) {
				transform = srcFromWindow.CompositionTarget.TransformFromDevice;
			}
			else {
				using( var hwndSrc = new HwndSource( new HwndSourceParameters() ) ) {
					transform = hwndSrc.CompositionTarget.TransformFromDevice;
				}
			}

			return transform;
		}
	}
}
