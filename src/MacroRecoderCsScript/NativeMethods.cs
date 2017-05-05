using System;
using System.Runtime.InteropServices;

namespace MacroRecoderCsScript
{
	internal static class NativeMethods
	{
		internal const int SEND_INPUT_FAILED = 0;
		internal const int ATTACH_PARENT_PROCESS = -1;

		internal delegate int HookProc( int hookCode, IntPtr wParam, IntPtr lParam );

		[DllImport( "user32.dll", SetLastError = true )]
		internal static extern IntPtr SetWindowsHookEx( int hookEventId, [MarshalAs( UnmanagedType.FunctionPtr )] HookProc hook, IntPtr module, uint threadId );

		[DllImport( "user32.dll", SetLastError = true )]
		internal static extern bool UnhookWindowsHookEx( IntPtr hookHandle );

		[DllImport( "user32.dll", SetLastError = true )]
		internal static extern int CallNextHookEx( IntPtr hookHandle, int hookCode, IntPtr wParam, IntPtr lParam );

		[DllImport( "user32.dll", SetLastError = true )]
		internal static extern uint SendInput( uint inputNum, Input[] inputs, int inputStructSize );

		[DllImport( "Kernel32.dll", SetLastError = true )]
		internal static extern bool AttachConsole( int processId );

		[DllImport( "Kernel32.dll", SetLastError = true )]
		internal static extern bool FreeConsole();
	}
}
