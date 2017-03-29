#r "PresentationCore"

using System.Diagnostics;
using System.Windows.Input;

enum DebugMode : byte
{
	None = 0x00,
	MouseOnly = 0x01,
	KeyOnly = 0x02,
	CreateLog = 0x04
};

SetMode( ( byte )DebugMode.KeyOnly | ( byte )DebugMode.CreateLog );

Process.Start( "Notepad" );
Delay(500);

KeyInput( Key.A );
PressKey( (ushort)KeyInterop.VirtualKeyFromKey( Key.LeftShift ) );
KeyInput( Key.B );
KeyInput( Key.C );
ReleaseKey( (ushort)KeyInterop.VirtualKeyFromKey( Key.LeftShift ) );
PressKey( (ushort)KeyInterop.VirtualKeyFromKey( Key.LeftCtrl ) );
KeyInput( Key.S );
ReleaseKey( (ushort)KeyInterop.VirtualKeyFromKey( Key.LeftCtrl ) );

void KeyInput( Key key )
{
	ushort virtualKey = (ushort)KeyInterop.VirtualKeyFromKey( key );

	PressKey( virtualKey );
	Delay( 100 );
	ReleaseKey( virtualKey );
	Delay( 100 );
}