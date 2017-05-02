# MacroRecoderCsScript
macro recorder written by C# script with Roslyn for Scripting

## Feature

* Record key and mouse macro
Record mouse and key input.
Recorded input is writed to macro with C# script.

* Execute Record key and mouse macro
Execute macro script writed by C# script.

* Command line mode
Execute macro script in command line.
In this mode, Application is not shown.

## Usage
![Screen Shot]


(1) Record button. When this button pushed, Application record mouse and key input.  
(2) Play button. When this button pushed, Application play macro.  
(3) Stop button. this button is enabled when Application record or play.  
When this button pushed in recording, stop recording and open file dialog for saving macro to C# script.  
When this button pushed in playing macro, cancel playing macro.  
(4) File path of C# script.  
(5) Browse button. When this button pushed, Open file dialog and input file path of selected C# script.  
(6) Error information. View expected error. For example, compile error for C# script.  

### Command line mode

```
MacroRecoderCsScript -script=<script path>
```

### Script Example

```cs
await SetMousePos(100, 100);  //set mouse coordinate to (100, 100)
await Delay(100);             //wait 100ms

//Left Click
await PushLeftButton(100, 100);
await PullLeftButton(100, 100);
```

## Download
Download the [latest release].

## Reference
See [Macro Reference]. 

## License
[MIT License]

[latest release]: https://github.com/hukatama024e/MacroRecoderCsScript/releases
[Screen Shot]:    Image/readme_screenshot.png
[MIT License]:    LICENSE
[Macro Reference]: https://github.com/hukatama024e/MacroRecoderCsScript/wiki/Macro-Reference
