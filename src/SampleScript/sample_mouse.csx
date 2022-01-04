using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Flags]
enum DebugMode : byte
{
	None = 0x00,
	MouseOnly = 0x01,
	KeyOnly = 0x02,
	CreateLog = 0x04
};

await SetMode( ( byte )DebugMode.MouseOnly | ( byte )DebugMode.CreateLog );

await SetCoordinate(100, 100);
await SetCoordinate(200, 100);
await SetCoordinate(300, 100);
await SetCoordinate(400, 100);
await SetCoordinate(500, 100);
await SetCoordinate(600, 100);
await SetCoordinate(700, 100);
await SetCoordinate(800, 100);
await SetCoordinate(900, 100);
await SetCoordinate(1000, 100);

await WriteCustomLog( "test custom log" );

await SetCoordinate(900, 100);
await SetCoordinate(800, 100);
await SetCoordinate(700, 100);
await SetCoordinate(600, 100);
await SetCoordinate(500, 100);
await SetCoordinate(400, 100);
await SetCoordinate(300, 100);
await SetCoordinate(200, 100);
await SetCoordinate(100, 100);

async Task SetCoordinate( int x, int y )
{
	await SetMousePos( x, y );
	await Delay( 100 );
}

async Task WriteCustomLog( string message )
{
	await Task.Run( () => {
		var userCustomDic = new Dictionary<string, string>
		{
			{ "ScriptName", "sample_mouse" },
			{ "Message",    message        }
		};
	
		WriteUserCustomLog( userCustomDic );
	} );
}