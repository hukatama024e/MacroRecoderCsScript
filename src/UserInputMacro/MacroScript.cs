using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace UserInputMacro
{
	public class MacroScript
	{
		[Flags]
		enum DebugMode : byte
		{
			None = 0x00,
			MouseOnly = 0x01,
			KeyOnly = 0x02,
			CreateLog = 0x04
		};

		private static readonly int COORDINATE_MAX = 65535;
		private DebugMode debugMode = DebugMode.None;

		public void Delay( int millsecond )
		{
			Thread.Sleep( millsecond );
		}

		public void PressKey( ushort virtualKey )
		{
			var input = new List<KeyInput>
			{
				CreateKeyInput( virtualKey, KeyEvent.None )
			};

			SendKeyInput( input.ToArray() );
		}

		public void ReleaseKey( ushort virtualKey )
		{
			var input = new List<KeyInput>
			{
				CreateKeyInput( virtualKey, KeyEvent.KeyUp )
			};

			SendKeyInput( input.ToArray() );
		}

		public void SetMousePos( int x, int y )
		{
			SendSingleMouseEvent( x, y, MouseEvent.Move );
		} 

		public void PushLeftButton( int x, int y )
		{
			SendSingleMouseEvent( x, y, MouseEvent.LeftDown );
		}

		public void PullLeftButton( int x, int y )
		{
			SendSingleMouseEvent( x, y, MouseEvent.LeftUp );
		}

		public void PushMiddleButton( int x, int y )
		{
			SendSingleMouseEvent( x, y, MouseEvent.MiddleDown );
		}

		public void PullMiddleButton( int x, int y )
		{
			SendSingleMouseEvent( x, y, MouseEvent.MiddleDown );
		}

		public void PushRightButton( int x, int y )
		{
			SendSingleMouseEvent( x, y, MouseEvent.RightDown );
		}

		public void PullRightButton( int x, int y )
		{
			SendSingleMouseEvent( x, y, MouseEvent.RightUp );
		}

		public void WheelMouse( int x, int y, int wheelRotate )
		{
			var input = new List<MouseInput>
			{
				CreateMouseWheel( x, y, MouseEvent.Wheel, wheelRotate ),
			};

			SendMouseInput( input.ToArray() );
		}

		public void HWheelMouse( int x, int y, int wheelRotate )
		{
			var input = new List<MouseInput>
			{
				CreateMouseWheel( x, y, MouseEvent.Hwheel, wheelRotate ),
			};

			SendMouseInput( input.ToArray() );
		}

		public void LeftClick( int x, int y )
		{
			var input = new List<MouseInput>
			{
				CreateMouseInput( x, y, MouseEvent.LeftDown ),
				CreateMouseInput( x, y, MouseEvent.LeftUp )
			};

			SendMouseInput( input.ToArray() );
		}

		public void RightClick( int x, int y )
		{
			var input = new List<MouseInput>
			{
				CreateMouseInput( x, y, MouseEvent.RightDown ),
				CreateMouseInput( x, y, MouseEvent.RightUp )
			};

			SendMouseInput( input.ToArray() );
		}

		public void MiddleClick( int x, int y )
		{
			var input = new List<MouseInput>
			{
				CreateMouseInput( x, y, MouseEvent.MiddleDown ),
				CreateMouseInput( x, y, MouseEvent.MiddleUp )
			};

			SendMouseInput( input.ToArray() );
		}

		public void SetDebugMode( byte mode )
		{
			debugMode = ( DebugMode ) mode;
		}

		private static int GetAbsoluteCoodinateX( int coordX )
		{
			var src = PresentationSource.FromVisual( Application.Current.MainWindow );
			var dpiWidth = src.CompositionTarget.TransformFromDevice.M11;

			return ( int ) ( coordX / ( SystemParameters.PrimaryScreenWidth / dpiWidth ) * COORDINATE_MAX );
		}

		private static int GetAbsoluteCoodinateY( int coordY )
		{
			var src = PresentationSource.FromVisual( Application.Current.MainWindow );
			var dpiHeight = src.CompositionTarget.TransformFromDevice.M22;

			return ( int ) ( coordY / ( SystemParameters.PrimaryScreenHeight / dpiHeight ) * COORDINATE_MAX );
		}

		private KeyInput CreateKeyInput( ushort virtualKeyCode, KeyEvent ev )
		{
			var input = new KeyInput
			{
				virtualKey = virtualKeyCode,
				flags = ev
			};

			return input;
		}

		private MouseInput CreateMouseInput( int x, int y, MouseEvent ev )
		{
			var input = new MouseInput
			{
				//coordinate is always absolute
				coordinateX = GetAbsoluteCoodinateX( x ),
				coordinateY = GetAbsoluteCoodinateY( y ),
				flags = ev | MouseEvent.Absolute,
			};

			return input;
		}

		private MouseInput CreateMouseWheel( int x, int y, MouseEvent ev, int wheelRotate )
		{
			var input = CreateMouseInput( x, y, ev );
			input.mouseData = wheelRotate;

			return input;
		}

		private void SendSingleMouseEvent( int x, int y, MouseEvent ev )
		{
			var input = new List<MouseInput>
			{
				CreateMouseInput( x, y, ev )
			};

			SendMouseInput( input.ToArray() );
		}

		private void SendMouseInput( MouseInput[] mouseInput )
		{
			if( IsDebugMode( DebugMode.CreateLog )) {
				Logger.WriteMouseInputInfo( mouseInput );
			}

			if( !IsDebugMode( DebugMode.KeyOnly ) ) {
				SendInputWrapper.SendMouseInput( mouseInput );
			}
		}

		private void SendKeyInput( KeyInput[] keyInput )
		{
			if( IsDebugMode( DebugMode.CreateLog ) ) {
				Logger.WriteKeyInputInfo( keyInput );
			}

			if( !IsDebugMode( DebugMode.MouseOnly ) ) {
				SendInputWrapper.SendKeyInput( keyInput );
			}
		}

		private bool IsDebugMode( DebugMode mode )
		{
			return ( debugMode & mode ) == mode;
		}
	}
}
