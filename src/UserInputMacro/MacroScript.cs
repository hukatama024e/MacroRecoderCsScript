using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace UserInputMacro
{
	public class MacroScript
	{
		private static readonly int COORDINATE_MAX = 65535;

		public async Task Delay( int millsecond )
		{
			try {
				await Task.Delay( millsecond );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task PressKey( ushort virtualKey )
		{
			try {
				var input = new List<KeyInput>
				{
					CreateKeyInput( virtualKey, KeyEvent.None )
				};

				await SendKeyInput( input.ToArray() );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task ReleaseKey( ushort virtualKey )
		{
			try {
				var input = new List<KeyInput>
				{
					CreateKeyInput( virtualKey, KeyEvent.KeyUp )
				};

				await SendKeyInput( input.ToArray() );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task SetMousePos( int x, int y )
		{
			try {
				await GetSingleMouseEventTask( x, y, MouseEvent.Move );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		} 

		public async Task PushLeftButton( int x, int y )
		{
			try {
				await GetSingleMouseEventTask( x, y, MouseEvent.LeftDown );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task PullLeftButton( int x, int y )
		{
			try {
				await GetSingleMouseEventTask( x, y, MouseEvent.LeftUp );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task PushMiddleButton( int x, int y )
		{
			try {
				await GetSingleMouseEventTask( x, y, MouseEvent.MiddleDown );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task PullMiddleButton( int x, int y )
		{
			try {
				await GetSingleMouseEventTask( x, y, MouseEvent.MiddleUp );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task PushRightButton( int x, int y )
		{
			try {
				await GetSingleMouseEventTask( x, y, MouseEvent.RightDown );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task PullRightButton( int x, int y )
		{
			try {
				await GetSingleMouseEventTask( x, y, MouseEvent.RightUp );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task WheelMouse( int x, int y, int wheelRotate )
		{
			try {
				var input = new List<MouseInput>
				{
					CreateMouseWheel( x, y, MouseEvent.Wheel, wheelRotate ),
				};

				await SendMouseInput( input.ToArray() );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task HWheelMouse( int x, int y, int wheelRotate )
		{
			try {
				var input = new List<MouseInput>
				{
					CreateMouseWheel( x, y, MouseEvent.Hwheel, wheelRotate ),
				};

				await SendMouseInput( input.ToArray() );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task SetMode( byte mode )
		{
			try {
				await Task.Run( () => 
				{
					AppEnvironment.GetInstance().Mode = ( ModeKind ) mode;
				} );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		public async Task WrileUserCustomLog( Dictionary<string, string> userCustomDic )
		{
			try {
				await Logger.WriteUserCustomAsync( userCustomDic );
			}
			catch( Exception ex ) {
				await CommonUtil.HandleExceptionAsync( ex );
			}
		}

		private static int GetAbsoluteCoodinateX( int coordX )
		{
			return ( int ) Math.Round( coordX * COORDINATE_MAX /
				( SystemParameters.PrimaryScreenWidth / AppEnvironment.GetInstance().DpiWidth ), MidpointRounding.AwayFromZero );
		}

		private static int GetAbsoluteCoodinateY( int coordY )
		{
			return ( int ) Math.Round( coordY * COORDINATE_MAX /
				( SystemParameters.PrimaryScreenHeight / AppEnvironment.GetInstance().DpiHeight ), MidpointRounding.AwayFromZero );
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

		private async Task GetSingleMouseEventTask( int x, int y, MouseEvent ev )
		{
			var input = new List<MouseInput>
			{
				CreateMouseInput( x, y, ev )
			};

			await SendMouseInput( input.ToArray() );
		}

		private async Task SendMouseInput( MouseInput[] mouseInput )
		{
			if( CommonUtil.CheckMode( ModeKind.CreateLog )) {
				await Logger.WriteMouseInputAsync( mouseInput );
			}

			if( !CommonUtil.CheckMode( ModeKind.KeyOnly ) ) {
				await Task.Run( () => SendInputWrapper.SendMouseInput( mouseInput ) );
			}
		}

		private async Task SendKeyInput( KeyInput[] keyInput )
		{
			if( CommonUtil.CheckMode( ModeKind.CreateLog ) ) {
				await Logger.WriteKeyInputAsync( keyInput );
			}

			if( !CommonUtil.CheckMode( ModeKind.MouseOnly ) ) {
				await Task.Run( () => SendInputWrapper.SendKeyInput( keyInput ) );
			}
		}
	}
}
