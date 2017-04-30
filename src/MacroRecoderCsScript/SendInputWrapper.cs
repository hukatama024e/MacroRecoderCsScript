using System.Runtime.InteropServices;
using System.ComponentModel;

namespace UserInputMacro
{
	public static class SendInputWrapper
	{
		public static void SendMouseInput( MouseInput[] mouseInput )
		{
			uint result;

			Input[] input = new Input[ mouseInput.Length ];

			for( int i = 0; i < mouseInput.Length; i++ ) {
				input[ i ].type = InputType.Mouse;
				input[ i ].inputInfo.mouseInput = mouseInput[ i ];
			}

			result = NativeMethods.SendInput( ( uint ) input.Length, input, Marshal.SizeOf( input[ 0 ] ) );

			if( result == NativeMethods.SEND_INPUT_FAILED ) {
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

			result = NativeMethods.SendInput( ( uint ) input.Length, input, Marshal.SizeOf( input[ 0 ] ) );

			if( result == NativeMethods.SEND_INPUT_FAILED ) {
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception( errorCode );
			}
		}
	}
}
