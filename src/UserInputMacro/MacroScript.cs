using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace UserInputMacro
{
	public class MacroScript
	{
		private static readonly int COORDINATE_MAX = 65535;

		public void Delay( int millsecond )
		{
			try {
				Thread.Sleep( millsecond );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void PressKey( ushort virtualKey )
		{
			try {
				var input = new List<KeyInput>
				{
					CreateKeyInput( virtualKey, KeyEvent.None )
				};

				SendKeyInput( input.ToArray() );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void ReleaseKey( ushort virtualKey )
		{
			try {
				var input = new List<KeyInput>
				{
					CreateKeyInput( virtualKey, KeyEvent.KeyUp )
				};

				SendKeyInput( input.ToArray() );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void SetMousePos( int x, int y )
		{
			try {
				SendSingleMouseEvent( x, y, MouseEvent.Move );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		} 

		public void PushLeftButton( int x, int y )
		{
			try {
				SendSingleMouseEvent( x, y, MouseEvent.LeftDown );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void PullLeftButton( int x, int y )
		{
			try {
				SendSingleMouseEvent( x, y, MouseEvent.LeftUp );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void PushMiddleButton( int x, int y )
		{
			try {
				SendSingleMouseEvent( x, y, MouseEvent.MiddleDown );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void PullMiddleButton( int x, int y )
		{
			try {
				SendSingleMouseEvent( x, y, MouseEvent.MiddleUp );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void PushRightButton( int x, int y )
		{
			try {
				SendSingleMouseEvent( x, y, MouseEvent.RightDown );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void PullRightButton( int x, int y )
		{
			try {
				SendSingleMouseEvent( x, y, MouseEvent.RightUp );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void WheelMouse( int x, int y, int wheelRotate )
		{
			try {
				var input = new List<MouseInput>
				{
					CreateMouseWheel( x, y, MouseEvent.Wheel, wheelRotate ),
				};

				SendMouseInput( input.ToArray() );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void HWheelMouse( int x, int y, int wheelRotate )
		{
			try {
				var input = new List<MouseInput>
				{
					CreateMouseWheel( x, y, MouseEvent.Hwheel, wheelRotate ),
				};

				SendMouseInput( input.ToArray() );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void SetMode( byte mode )
		{
			try {
				AppEnvironment.GetInstance().Mode = ( ModeKind ) mode;
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		public void WrileUserCustomLog( Dictionary<string, string> userCustomDic )
		{
			try {
				Logger.WriteUserCustom( userCustomDic );
			}
			catch( Exception ex ) {
				CommonUtil.HandleException( ex );
			}
		}

		private static int GetAbsoluteCoodinateX( int coordX )
		{
			return ( int ) Math.Round( coordX * COORDINATE_MAX /
				( SystemParameters.PrimaryScreenWidth / CommonUtil.GetDpiWidth() ), MidpointRounding.AwayFromZero );
		}

		private static int GetAbsoluteCoodinateY( int coordY )
		{
			return ( int ) Math.Round( coordY * COORDINATE_MAX /
				( SystemParameters.PrimaryScreenHeight / CommonUtil.GetDpiHeight() ), MidpointRounding.AwayFromZero );
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
			if( CommonUtil.CheckMode( ModeKind.CreateLog )) {
				Logger.WriteMouseInputInfo( mouseInput );
			}

			if( !CommonUtil.CheckMode( ModeKind.KeyOnly ) ) {
				SendInputWrapper.SendMouseInput( mouseInput );
			}
		}

		private void SendKeyInput( KeyInput[] keyInput )
		{
			if( CommonUtil.CheckMode( ModeKind.CreateLog ) ) {
				Logger.WriteKeyInputInfo( keyInput );
			}

			if( !CommonUtil.CheckMode( ModeKind.MouseOnly ) ) {
				SendInputWrapper.SendKeyInput( keyInput );
			}
		}
	}
}
