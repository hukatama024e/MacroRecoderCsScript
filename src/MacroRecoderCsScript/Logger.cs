using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;

namespace UserInputMacro
{
	static class Logger
	{
		private static SemaphoreSlim semaphore = new SemaphoreSlim( 1, 1 );

		private static readonly string DATE_FORMAT = "yyyy/MM/dd HH:mm:ss.fff";
		private static readonly string INPUT_LOG_NAME = "input_log.txt";
		private static readonly string ERROR_LOG_NAME = "error_log.txt";

		private static readonly int COORDINATE_MAX = 65535;

		public async static Task WriteMouseInputAsync( MouseInput[] mouseInput )
		{
			foreach( var singleInput in mouseInput ) {
				int x = GetRelativeCoodinateX( singleInput.coordinateX );
				int y = GetRelativeCoodinateY( singleInput.coordinateY );

				var labeledData = new Dictionary<string, string>
				{
					{ "Date",       GetDateLog()                       },
					{ "LogKind",    "MouseInput"                       },
					{ "X",          x.ToString()                       },
					{ "Y",          y.ToString()                       },
					{ "MouseData",  singleInput.mouseData.ToString()   },
					{ "Flags",      singleInput.flags.ToString()       }
				};

				await AppendLogAsync( INPUT_LOG_NAME, labeledData );
			}
		}

		public async static Task WriteKeyInputAsync( KeyInput[] keyInput )
		{
			foreach( var singleInput in keyInput ) {
				var labeledData = new Dictionary<string, string>
				{
					{ "Date",       GetDateLog()			                                          },
					{ "LogKind",    "KeyInput"						                                  },
					{ "Key",        KeyInterop.KeyFromVirtualKey( singleInput.virtualKey ).ToString() },
					{ "Flags",      singleInput.flags.ToString()			                          }
				};

				await AppendLogAsync( INPUT_LOG_NAME, labeledData );
			}
		}

		public static void WriteMouseEvent( MouseHookStruct mouseHookStr, MouseHookEvent mouseEvent )
		{
			AppendLog( INPUT_LOG_NAME, CreateMouseEventLog( mouseHookStr, mouseEvent ) );
		}

		public static void WriteKeyEvent( KeyHookStruct keyHookStr, KeyHookEvent keyEvent )
		{
			AppendLog( INPUT_LOG_NAME, CreateKeyEventLog( keyHookStr, keyEvent ) );
		}

		public static void WriteErrorLog( Exception ex )
		{
			AppendLog( ERROR_LOG_NAME, CreateErrorLog( ex ) );
		}

		public async static Task WriteMouseEventAsync( MouseHookStruct mouseHookStr, MouseHookEvent mouseEvent )
		{
			await AppendLogAsync( INPUT_LOG_NAME, CreateMouseEventLog( mouseHookStr, mouseEvent ) );
		}

		public async static Task WriteKeyEventAsync( KeyHookStruct keyHookStr, KeyHookEvent keyEvent )
		{
			await AppendLogAsync( INPUT_LOG_NAME, CreateKeyEventLog( keyHookStr, keyEvent ) );
		}

		public async static Task WriteUserCustomAsync( Dictionary<string, string> userCustomDic )
		{
			var labeledData = new Dictionary<string, string>
			{
				{ "Date",       GetDateLog() },
				{ "LogKind",    "UserCustom" },
			};

			labeledData = labeledData.Concat( userCustomDic ).ToDictionary( dic => dic.Key, dic => dic.Value );
			await AppendLogAsync( INPUT_LOG_NAME, labeledData );
		}

		public async static Task WriteErrorLogAsync( Exception ex )
		{
			await AppendLogAsync( ERROR_LOG_NAME, CreateErrorLog( ex ) );
		}

		private static string GetDateLog()
		{
			return DateTime.Now.ToString( DATE_FORMAT );
		}

		private static Dictionary<string, string> CreateMouseEventLog( MouseHookStruct mouseHookStr, MouseHookEvent mouseEvent )
		{
			var labeledData = new Dictionary<string, string>
			{
				{ "Date",       GetDateLog()                              },
				{ "LogKind",    "MouseEvent"                              },
				{ "X",          mouseHookStr.coordinatePoint.x.ToString() },
				{ "Y",          mouseHookStr.coordinatePoint.y.ToString() },
				{ "MouseData",  mouseHookStr.mouseData.ToString()         },
				{ "Flags",      mouseHookStr.flags.ToString()             },
				{ "Event",      mouseEvent.ToString()                     }
			};

			return labeledData;
		}

		private static Dictionary<string, string> CreateKeyEventLog( KeyHookStruct keyHookStr, KeyHookEvent keyEvent )
		{
			var labeledData = new Dictionary<string, string>
			{
				{ "Date",       GetDateLog()                                                      },
				{ "LogKind",    "KeyEvent"                                                        },
				{ "Key",        KeyInterop.KeyFromVirtualKey( keyHookStr.virtualKey ).ToString()  },
				{ "Flags",      keyHookStr.flags.ToString()                                       },
				{ "Event",      keyEvent.ToString()                                               }
			};

			return labeledData;
		}

		private static Dictionary<string, string> CreateErrorLog( Exception ex )
		{
			var labeledData = new Dictionary<string, string>
			{
				{ "Date",       GetDateLog()  },
				{ "Message",    ex.Message    },
				{ "Source",     ex.Source     },
				{ "StackTrace", ex.StackTrace },
			};

			return labeledData;
		}

		private async static Task AppendLogAsync( string pass, Dictionary<string, string> labeledData )
		{
			await semaphore.WaitAsync().ConfigureAwait( false );

			try {
				using( var fs = new FileStream( pass, FileMode.Append, FileAccess.Write, FileShare.ReadWrite ) ) {
					using( var sw = new StreamWriter( fs ) ) {
						await sw.WriteLineAsync( CreateLtsvLog( labeledData ) );
					}
				}
			}
			catch( Exception ) {
				throw;
			}
			finally {
				semaphore.Release();
			}
		}

		private static void AppendLog( string pass, Dictionary<string, string> labeledData )
		{
			using( var fs = new FileStream( pass, FileMode.Append, FileAccess.Write, FileShare.ReadWrite ) ) {
				using( var sw = new StreamWriter( fs ) ) {
					sw.WriteLine( CreateLtsvLog( labeledData ) );
				}
			}
		}

		private static string CreateLtsvLog( Dictionary<string, string> labeledData )
		{
			var ltsvLog = new StringBuilder();

			foreach( var singleData in labeledData ) {
				ltsvLog.Append( singleData.Key + ":" + singleData.Value + "\t" );
			}

			// delete last tub character
			ltsvLog.Remove( ltsvLog.Length - 1, 1 );

			return ltsvLog.ToString();
		}

		private static int GetRelativeCoodinateX( int coordX )
		{
			return ( int ) ( coordX * ( SystemParameters.PrimaryScreenWidth / AppEnvironment.GetInstance().DpiWidth ) / COORDINATE_MAX );
		}

		private static int GetRelativeCoodinateY( int coordY )
		{
			return ( int ) ( coordY * ( SystemParameters.PrimaryScreenHeight / AppEnvironment.GetInstance().DpiHeight ) / COORDINATE_MAX );
		}
	}
}
