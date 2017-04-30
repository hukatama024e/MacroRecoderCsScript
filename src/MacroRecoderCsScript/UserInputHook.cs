using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.ComponentModel;

namespace MacroRecoderCsScript
{
	class UserInputHook : IDisposable
	{
		private IntPtr KeyHookHandle { get; set; }
		private IntPtr MouseHookHandle { get; set; }

		private NativeMethods.HookProc KeyHookCallback { get; set; }
		private NativeMethods.HookProc MouseHookCallback { get; set; }

		public Action<KeyHookStruct, int> KeyHook { get; set; }
		public Action<MouseHookStruct, int> MouseHook { get; set; }
		public Action<Exception> HookErrorProc { get; set; }

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
			KeyHookHandle = NativeMethods.SetWindowsHookEx( ( int )WindowsHookID.KeyBoardLowLevel, KeyHookCallback, module, 0 );

			if( KeyHookHandle == IntPtr.Zero ) {
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception( errorCode );
			}
		}

		public void RegisterMouseHook()
		{
			IntPtr module = Marshal.GetHINSTANCE( Assembly.GetExecutingAssembly().GetModules()[ 0 ] );
			MouseHookHandle = NativeMethods.SetWindowsHookEx( ( int )WindowsHookID.MouseLowLevel, MouseHookCallback, module, 0 );

			if( MouseHookHandle == IntPtr.Zero ) {
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception( errorCode );
			}
		}

		public void UnregisterMouseHook()
		{
			if( MouseHookHandle != IntPtr.Zero ) {
				NativeMethods.UnhookWindowsHookEx( MouseHookHandle );
			}
		}

		public void UnregisterKeyHook()
		{
			if( KeyHookHandle != IntPtr.Zero ) {
				NativeMethods.UnhookWindowsHookEx( KeyHookHandle );
			}
		}

		private int KeyHookProc( int hookCode, IntPtr wParam, IntPtr lParam )
		{
			try {
				var keyHookStr = ( KeyHookStruct ) Marshal.PtrToStructure( lParam, typeof( KeyHookStruct ) );
				KeyHook( keyHookStr, wParam.ToInt32() );
			}
			catch( Exception ex ) {
				HookErrorProc( ex );
			}

			return NativeMethods.CallNextHookEx( KeyHookHandle, hookCode, wParam, lParam );
		}

		private int MouseHookProc( int hookCode, IntPtr wParam, IntPtr lParam )
		{
			try {
				var mouseHookStr = ( MouseHookStruct ) Marshal.PtrToStructure( lParam, typeof( MouseHookStruct ) );
				MouseHook( mouseHookStr, wParam.ToInt32() );
			}
			catch( Exception ex ) {
				HookErrorProc( ex );
			}

			return NativeMethods.CallNextHookEx( KeyHookHandle, hookCode, wParam, lParam );
		}
	}
}
