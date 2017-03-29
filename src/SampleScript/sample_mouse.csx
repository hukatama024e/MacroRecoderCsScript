using System;
using System.Collections.Generic;

[Flags]
enum DebugMode : byte
{
	None = 0x00,
	MouseOnly = 0x01,
	KeyOnly = 0x02,
	CreateLog = 0x04
};

SetMode( ( byte )DebugMode.MouseOnly | ( byte )DebugMode.CreateLog );

SetCoordinate(100, 100);
SetCoordinate(200, 100);
SetCoordinate(300, 100);
SetCoordinate(400, 100);
SetCoordinate(500, 100);
SetCoordinate(600, 100);
SetCoordinate(700, 100);
SetCoordinate(800, 100);
SetCoordinate(900, 100);
SetCoordinate(1000, 100);

WriteCustomLog( "test custom log" );

SetCoordinate(900, 100);
SetCoordinate(800, 100);
SetCoordinate(700, 100);
SetCoordinate(600, 100);
SetCoordinate(500, 100);
SetCoordinate(400, 100);
SetCoordinate(300, 100);
SetCoordinate(200, 100);
SetCoordinate(100, 100);

void SetCoordinate( int x, int y )
{
	SetMousePos( x, y );
	Delay( 100 );
}

void WriteCustomLog( string message )
{
	var userCustomDic = new Dictionary<string, string>
	{
		{ "ScriptName", "sample_mouse" },
		{ "Message",    message        }
	};
	
	WrileUserCustomLog( userCustomDic );

}