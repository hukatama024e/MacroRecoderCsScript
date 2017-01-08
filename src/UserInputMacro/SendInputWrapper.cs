using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Input;

namespace UserInputMacro
{
	public static class SendInputWrapper
	{
		private static readonly int WIN32_NOERROR = 0x00000000;

		[ DllImport( "user32.dll", SetLastError = true ) ]
		private static extern uint SendInput( uint inputNum, Input[] inputs, int inputStructSize );

		public static void PressKey( Key key )
		{
			KeyInput[] keyInput = new KeyInput[ 1 ];

			keyInput[ 0 ] = new KeyInput
			{
				virtualKey = ( ushort ) KeyInterop.VirtualKeyFromKey( key )
			};

			SendKeyInput( keyInput );
		}

		public static void SendMouseInput( MouseInput[] mouseInput )
		{
			Input[] input = new Input[ mouseInput.Length ];

			for( int i = 0; i < mouseInput.Length; i++ ) {
				input[ i ].type = InputType.Mouse;
				input[ i ].inputInfo.mouseInput = mouseInput[ i ];
			}

			SendInput( ( uint ) input.Length, input, Marshal.SizeOf( input[ 0 ] ) );

			int errorCode = Marshal.GetLastWin32Error();
			if( errorCode != WIN32_NOERROR ) {
				throw new Win32Exception( errorCode );
			}
		}

		public static void SendKeyInput( KeyInput[] keyInput )
		{
			Input[] input = new Input[ keyInput.Length ];

			for( int i = 0; i < keyInput.Length; i++ ) {
				input[ i ].type = InputType.Keyboard;
				input[ i ].inputInfo.keyInput = keyInput[ i ];
			}

			SendInput( ( uint ) input.Length, input, Marshal.SizeOf( input[ 0 ] ) );

			int errorCode = Marshal.GetLastWin32Error();
			if( errorCode != WIN32_NOERROR ) {
				throw new Win32Exception( errorCode );
			}
		}
	}
}
