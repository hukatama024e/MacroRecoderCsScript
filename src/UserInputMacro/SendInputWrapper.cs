using System.Runtime.InteropServices;
using System.ComponentModel;

namespace UserInputMacro
{
	public static class SendInputWrapper
	{
		private static readonly int FAIL_SENDINPUT = 0;

		[ DllImport( "user32.dll", SetLastError = true ) ]
		private static extern uint SendInput( uint inputNum, Input[] inputs, int inputStructSize );

		public static void SendMouseInput( MouseInput[] mouseInput )
		{
			uint result;

			Input[] input = new Input[ mouseInput.Length ];

			for( int i = 0; i < mouseInput.Length; i++ ) {
				input[ i ].type = InputType.Mouse;
				input[ i ].inputInfo.mouseInput = mouseInput[ i ];
			}

			result = SendInput( ( uint ) input.Length, input, Marshal.SizeOf( input[ 0 ] ) );

			if( result == FAIL_SENDINPUT ) {
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception( errorCode );
			}
		}

		public static void SendKeyInput( KeyInput[] keyInput )
		{
			uint result;

			Input[] input = new Input[ keyInput.Length ];

			for( int i = 0; i < keyInput.Length; i++ ) {
				input[ i ].type = InputType.Keyboard;
				input[ i ].inputInfo.keyInput = keyInput[ i ];
			}

			result = SendInput( ( uint ) input.Length, input, Marshal.SizeOf( input[ 0 ] ) );

			if( result == FAIL_SENDINPUT ) {
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception( errorCode );
			}
		}
	}
}
