*****************************************
******* Game of Life - ReadMe.txt *******
*****************************************

Student: Charles Holdren
ID: #0004966664
Course: Project and Portfolio I: Computer Science - Online
Date: 07/06/2021


*** Extra Features ***
**********************
* Seed Box *
- The seed box is a text box that can be accessed through the randomize menu as a dialog box or can be directly accessed on the tool strip. When used it parses string or int values into a seed. If the user selects Generate after typing in a seed, the seed will generate. Or if the user types in a word and clicks outside, the seed will parse and remain in the Seed Box until the user selects Generate.

* Cursor Modes *
- User can change between three cursor modes:
  - Paint  - MouseDown event, only paints cells
  - Eraser - MouseDown event, only erases cells
  - Single click - Was included in the Game of Life template.

* KeyPreview / Shortcuts *
- Buttons that are commonly used can be accessed by a single key press.
	
  1 = Paint cursor
  2 = Eraser cursor
  3 = Single-Click cursor

  H = Toggle HUD
  N = Toggle Neighbor Count
  G = Toggle Grid
  B = Toggle Boundary

  R = Random Seed
  E = Enter seed (Dialog box)
  S = Speed (Dialog box)
	
  Spacebar    = Start / Pause
  Right Arrow = Next
  Enter       = Generate
  Esc         = Exit
		  
  Up Arrow / MouseWheel Up = Zoom In (Grow universe x and y by 1)
        or 

  Down Arrow / MouseWheel Down = Zoom Out (Shrink universe x and y by 1)


*** Advanced Features ***
*************************
* Import *
- PlainText files can be imported without overwriting the current universe.

* Game Colors *
- The user can change up to 4 color settings (Back Color, Cell Color, Grid Color, and Grid x10 Color).

* Universe Boudary Behavior *
- Toggle between Torodial and Finite universe boundaries through the Settings menu or by pressing the 'B' key.

* Context Sensitive Menu *
- Includes Start, Pause, Next, Color Options, View Options, Boundary Options, and the Speed (interval) option.

* Heads Up Display *
- Displays Generations, Cell Count, Boundary Type, and Universe Size. Toggle through the View menu or by pressing the 'H' key

* Settings *
- The application creates two settings files, one being a copy of the last settings upon closing the application. The user can choose to Reload settings from the old copy at any time before exiting. The settings can also be reset back to default values.