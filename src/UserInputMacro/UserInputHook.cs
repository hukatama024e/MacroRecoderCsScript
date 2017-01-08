using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.ComponentModel;

namespace UserInputMacro
{
	class UserInputHook : IDisposable
	{
		private IntPtr KeyHookHandle { get; set; }
		private IntPtr MouseHookHandle { get; set; }

		private delegate int HookProc( int hookCode, IntPtr wParam, IntPtr lParam );
		private HookProc KeyHookCallback { get; set; }
		private HookProc MouseHookCallback { get; set; }

		public Action<KeyHookStruct, int> KeyHook { get; set; }
		public Action<MouseHookStruct, int> MouseHook { get; set; }
		public Action<Exception> HookErrorProc { get; set; }

		[DllImport( "user32.dll", SetLastError = true )]
		private static extern IntPtr SetWindowsHookEx( int hookEventId, [MarshalAs( UnmanagedType.FunctionPtr )] HookProc hook, IntPtr module, uint threadId );

		[DllImport( "user32.dll", SetLastError = true )]
		private static extern bool UnhookWindowsHookEx( IntPtr hookHandle );

		[DllImport( "user32.dll", SetLastError = true )]
		private static extern int CallNextHookEx( IntPtr hookHandle, int hookCode, IntPtr wParam, IntPtr lParam );

		public UserInputHook()
		{
			KeyHookCallback = KeyHookProc;
			MouseHookCallback = MouseHookProc;

			KeyHook = ( s, e ) => { };
			MouseHook = ( s, e ) => { };
			HookErrorProc = ( e ) => { };
		}

		~UserInputHook()
		{
			Dispose( false );
		}

		protected virtual void Dispose( bool disposing )
		{
			if( disposing ) {
			}

			UnregisterKeyHook();
			UnregisterMouseHook();
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		public void RegisterKeyHook()
		{
			IntPtr module = Marshal.GetHINSTANCE( Assembly.GetExecutingAssembly().GetModules()[ 0 ] );
			KeyHookHandle = SetWindowsHookEx( HookEventID.WH_KEYBOARD_LL, KeyHookCallback, module, 0 );

			if( KeyHookHandle == IntPtr.Zero ) {
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception( errorCode );
			}
		}

		public void RegisterMouseHook()
		{
			IntPtr module = Marshal.GetHINSTANCE( Assembly.GetExecutingAssembly().GetModules()[ 0 ] );
			MouseHookHandle = SetWindowsHookEx( HookEventID.WH_MOUSE_LL, MouseHookCallback, module, 0 );

			if( MouseHookHandle == IntPtr.Zero ) {
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception( errorCode );
			}
		}

		public void UnregisterMouseHook()
		{
			if( MouseHookHandle != IntPtr.Zero ) {
				UnhookWindowsHookEx( MouseHookHandle );
			}
		}

		public void UnregisterKeyHook()
		{
			if( KeyHookHandle != IntPtr.Zero ) {
				UnhookWindowsHookEx( KeyHookHandle );
			}
		}

		private int KeyHookProc( int hookCode, IntPtr wParam, IntPtr lParam )
		{
			try {
				var keyStruct = ( KeyHookStruct ) Marshal.PtrToStructure( lParam, typeof( KeyHookStruct ) );
				KeyHook( keyStruct, wParam.ToInt32() );
			}
			catch( Exception ex ) {
				HookErrorProc( ex );
			}

			return CallNextHookEx( KeyHookHandle, hookCode, wParam, lParam );
		}

		private int MouseHookProc( int hookCode, IntPtr wParam, IntPtr lParam )
		{
			try {
				var mouseStruct = ( MouseHookStruct ) Marshal.PtrToStructure( lParam, typeof( MouseHookStruct ) );
				MouseHook( mouseStruct, wParam.ToInt32() );
			}
			catch( Exception ex ) {
				HookErrorProc( ex );
			}

			return CallNextHookEx( KeyHookHandle, hookCode, wParam, lParam );
		}
	}
}
