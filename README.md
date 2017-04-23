# UserInputMacro
Execute macro for user input with Roslyn for Scripting(C#)

## Feature

* Execute macro script for user input
Execute macro script writed by C#(with Roslyn for Scripting).
Application support mouse and key input with macro script.

* Record macro script for user input
Record mouse and key input.
Recorded input is writed to macro script.

* Command line mode
Execute macro script in command line.
In this mode, Application is not shown.

## Usage
![Screen Shot]


(1) Record button. When this button pushed, Application record mouse and key input.  
(2) Play button. When this button pushed, Application play macro script.  
(3) Stop button. this button is enabled when Application record or play.  
When this button pushed in recording, stop recording and open file dialog and save user input to macro script.  
When this button pushed in playing macro script, cancel playing macro script.  
(4) File path of macro script.  
(5) Browse button. When this button pushed, Open file dialog and input file path of selected macro scirpt.  
(6) Error information. View expected error. For example, compile error for macro script.  

### Command line mode

```
UserInputMacro -script=<script path>
```

## Download
Download the [latest release]

## Reference
Under construction...

## License
[MIT License]

[latest release]: https://github.com/hukatama024e/UserInputMacro/releases
[Screen Shot]:    Image/readme_screenshot.png
[MIT License]:    LICENSE

