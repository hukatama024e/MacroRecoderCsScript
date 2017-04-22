#r "PresentationCore"

using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

enum DebugMode : byte
{
	None = 0x00,
	MouseOnly = 0x01,
	KeyOnly = 0x02,
	CreateLog = 0x04
};

await SetMode( ( byte )DebugMode.KeyOnly | ( byte )DebugMode.CreateLog );

await Task.Run( () => { Process.Start( "Notepad" ); } );
await Delay(500);

await KeyInput( Key.A );
await PressKey( (ushort)KeyInterop.VirtualKeyFromKey( Key.LeftShift ) );
await KeyInput( Key.B );
await KeyInput( Key.C );
await ReleaseKey( (ushort)KeyInterop.VirtualKeyFromKey( Key.LeftShift ) );
await PressKey( (ushort)KeyInterop.VirtualKeyFromKey( Key.LeftCtrl ) );
await KeyInput( Key.S );
await ReleaseKey( (ushort)KeyInterop.VirtualKeyFromKey( Key.LeftCtrl ) );

async Task KeyInput( Key key )
{
	ushort virtualKey = (ushort)KeyInterop.VirtualKeyFromKey( key );

	await PressKey( virtualKey );
	await Delay( 100 );
	await ReleaseKey( virtualKey );
	await Delay( 100 );
}