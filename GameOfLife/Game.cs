using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region Fields and Contructor
        // Fields
        private bool[,] _universe;          // The universe array
        Timer timer = new Timer();          // The Timer class
        private int _cellCount = 0;         // Cell count
        private bool _seedFlag = false;     // Keeps track if a seed should be displayed
        private bool _importFlag = false;   // Keeps track if whether or not the user is importing
        private bool _saveAsFlag = false;   // Keeps frack if whether or not the user is Saving As...
        private string _fileName;           // Save file
        private string _path;               // Full path to save
        private bool _hideParse = false;    // Used to hide the parse process when text box is focsued
        private bool _cursorMove = false;   // Used for MouseDown/MouseUp
        private int _draw = 0;              // Cursor mode : 0 = Paint, 1 = Erase, 2 = Single Click
        private bool[,] _universeCopy;      // Used to make a copy of the universe for cursor modes

        //  Settings
        private Color _backColor;       // Back color
        private Color _cellColor;       // Cell color
        private Color _gridColor;       // Grid color
        private Color _grid10xColor;    // Grid 10x color
        private int _rows;              // Rows count
        private int _columns;           // Column Count
        private int _generations;       // Generation count
        private long _seed;             // Seed : Declared as long for validation. See SeedBox_ParseSeed()
        private bool _boundary;         // Boundary type : True = Torodial, False = Finite
        private bool _displayHUD;       // Display HUD
        private bool _displayNeighbors; // Display Neighbor Count
        private bool _displayGrid;      // Display Grid
        private decimal _interval;      // Interval

        // Constructor
        public Game()
        {
            // Initialize components for Windows Form (avoid editing)
            InitializeComponent();

            // Load settings from file
            Settings_Process_LoadSettings();

            // Initalize settings graphics
            Init_Graphics();

            // Subscribe a custom method to the Mouse wheel
            // Used to enable scrolling
            MouseWheel += OnMouseWheel_Zoom;
        }
        #endregion

        #region Buttons

        #region File
        // New
        private void File_New(object sender = null, EventArgs e = null)
        {
            // Reset universe
            _seed = 0;
            _universe = new bool[_rows, _columns];

            // Update Game.Text to New World
            Text = Properties.Resources.fileNewWorld + Properties.Resources.appendTitle;

            // Display N/A instead of _seed
            _seedFlag = false;

            // Display default message in seed box
            SeedBox_SetStyle(true);

            // Update Status Strip
            Update_StatusStrip();

            // Count alive cells
            Process_CountCells();

            // Update controls (will enable Start and Next if seed is blank)
            Update_Controls();

            // Pause in the case that it is running
            Control_Process_Pause(sender, e);

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Open
        private void File_Open(object sender, EventArgs e)
        {
            // Instantiate a new OpenFileDialog
            OpenFileDialog dlg = new OpenFileDialog();

            // Change the title to import if importing
            if (_importFlag == true)
            {
                dlg.Title = Properties.Resources.import;
            }

            // Filters for file types
            dlg.Filter = "All Files|*.*|Cells|*.cells";

            // Set default type to filter index 2 (.cells)
            dlg.FilterIndex = 2;

            // Open Dialog Box until the user cancels or confirms
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // Read from file
                StreamReader sr = new StreamReader(dlg.FileName);

                // Calculate the width and height of the data in the file.
                int rows = 0;
                int columns = 0;

                // Iterate through the file once to get its size.
                while (!sr.EndOfStream)
                {
                    // Read one row at a time.
                    string row = sr.ReadLine();

                    // If the row begins with '!' it is a comment
                    if (row[0] != '!')
                    {
                        // Increment the maxHeight variable for each row read.
                        columns++;
                    }
                    // Get the length of the current row string and adjust the maxWidth variable
                    rows = row.Length;
                }

                // If the user is just opening, not importing
                if (_importFlag == false)
                {
                    // Resize the universe
                    _rows = rows;
                    _columns = columns;
                    _universe = new bool[rows, columns];
                }

                // Reset the file pointer back to the beginning of the file.
                sr.BaseStream.Seek(0, SeekOrigin.Begin);

                // Column indexer
                int y = 0;

                // Iterate through the file again, this time reading in the cells.
                while (!sr.EndOfStream)
                {
                    // Used only for Import
                    bool[,] tempUniverse = new bool[_universe.GetLength(0), _universe.GetLength(1)];

                    // Used for for loop
                    int tempRow = 0;

                    // Read one row at a time.
                    string row = sr.ReadLine();

                    // If the row begins with '!' it is a comment
                    if (row[0] != '!')
                    {
                        // If opening a file
                        if (_importFlag == false)
                        {
                            // Loop for as long as the opened file's row
                            tempRow = row.Length;
                        }
                        // Else importing a file
                        else
                        {
                            // Else don't access what isn't resized
                            tempRow = _universe.GetLength(0);
                        }

                        // Update the universe
                        for (int x = 0; x < tempRow; x++)
                        {
                            // On open
                            if (_importFlag == false)
                            {
                                // If row[xPos] is a 'O' (capital O) then it is alive
                                _universe[x, y] = (row[x] == 'O') ? true : false;
                            }
                            // On import
                            else if (y < _universe.GetLength(1))
                            {
                                // If row[xPos] is a 'O' (capital O) then it is alive
                                tempUniverse[x, y] = (row[x] == 'O') ? true : false;

                                // Use the OR operator to keep alive cells, well, alive.
                                _universe[x, y] = _universe[x, y] | tempUniverse[x, y];
                            }

                        }
                    }
                    // Update indexer
                    y++;

                }

                // Close the file.
                sr.Close();

                // Get path
                string path = dlg.FileName;

                // Update _path and _filename
                File_Process_UpdatePath(ref path);

                // Update Game.Text to include filename
                Text = _fileName + Properties.Resources.appendTitle;

                // Display N/A instead of _seed
                _seedFlag = false;

                // Recount cells
                Process_CountCells();

                // Update status strip
                Update_StatusStrip();

                // Update controls
                Update_Controls();

                // Tell windows to repaint
                GraphicsPanel.Invalidate();

            }
        }

        // Import
        private void File_Import(object sender, EventArgs e)
        {
            // Change the flag to true and call File_Open
            _importFlag = true;
            File_Open(sender, e);

            // Done importing, reset to false
            _importFlag = false;
        }

        // Save
        private void File_Save(object sender, EventArgs e)
        {
            // If the user is Saving and has not specified file name
            if (_saveAsFlag == false && (_fileName == null || Text == Properties.Resources.fileNewWorld + Properties.Resources.appendTitle))
            {
                // Open Save As..
                File_SaveAs(sender, e);
            }
            else
            {
                // Check if file exists
                if (!File.Exists(_path))
                {
                    File.Create(_path);
                }

                // Update _path and _filename
                File_Process_UpdatePath(ref _path);

                // Write to file
                StreamWriter sw = new StreamWriter(_path);

                // Iterate through the universe one row at a time.
                for (int y = 0; y < _universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < _universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        if (_universe[x, y] == true)
                        {
                            currentRow += 'O';
                        }
                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        else
                        {
                            currentRow += '.';
                        }


                    }

                    // Write row to file
                    sw.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                sw.Close();


                // Update Game.Text to include filename
                Text = _fileName + Properties.Resources.appendTitle;
            }

        }
        // Save As
        private void File_SaveAs(object sender, EventArgs e)
        {
            // Instantiate new SaveFileDialog
            SaveFileDialog dlg = new SaveFileDialog();

            // Filters for file types
            dlg.Filter = "All Files|*.*|Cells|*.cells";

            // Set default type to filter index 2 (.cells)
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";

            // Open Dialog Box until the user cancels or confirms
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // Mark Save As flag as true
                _saveAsFlag = true;

                // Get path
                string path = dlg.FileName;

                // Update _path and _filename
                File_Process_UpdatePath(ref path);

                // Save file
                File_Save(sender, e);

                // Done saving as, reset flag
                _saveAsFlag = false;
            }

        }

        private void File_Process_UpdatePath(ref string path)
        {
            // Update filename to user specified name
            _path = path;

            // Split into an array
            string[] pathArr = path.Split('\\');

            // Grab the filename (last element)
            for (int i = 0; i < pathArr.Length; i++)
            {
                if (i == pathArr.Length - 1)
                {
                    _fileName = pathArr[i];
                }
            }
        }

        // Exit
        private void File_Exit(object sender, EventArgs e)
        {
            // Exit the application
            Application.Exit();
        }
        #endregion

        #region Control
        // Start
        private void Control_Start(object sender = null, EventArgs e = null)
        {
            // Toggle between Start / Pause states 

            // When the timer is disabled
            if (timer.Enabled == false)
            {
                // Start the timer
                timer.Enabled = true;

                // Update properties to pause properties
                startToolStripMenuItem.Image = Properties.Resources.pauseIcon;
                startToolStripMenuItem.Text = Properties.Resources.pause;
                startToolStripMenuItem1.Image = Properties.Resources.pauseIcon;
                startToolStripMenuItem1.Text = Properties.Resources.pause;
                toolStripButtonStart.Image = Properties.Resources.pauseIcon;
                toolStripButtonStart.ToolTipText = Properties.Resources.toolTipPause;

                // Update controls
                Update_Controls();
            }
            // Else the game is running, feel free to pause!
            else
            {
                // Pause
                Control_Process_Pause(sender, e);
            }
        }

        // Pause
        private void Control_Process_Pause(object sender = null, EventArgs e = null)
        {
            // Stop timer
            timer.Enabled = false;

            // Update properties to start properties
            startToolStripMenuItem.Image = Properties.Resources.startIcon;
            startToolStripMenuItem.Text = Properties.Resources.start;
            startToolStripMenuItem1.Image = Properties.Resources.startIcon;
            startToolStripMenuItem1.Text = Properties.Resources.start;
            toolStripButtonStart.Image = Properties.Resources.startIcon;
            toolStripButtonStart.ToolTipText = Properties.Resources.toolTipStart;

            // Update controls unless cell count is 0
            if (_cellCount != 0)
            {
                Update_Controls();
            }
            else
            {
                // Disable File > Start
                // The world is empty
                startToolStripMenuItem.Enabled = false;
            }
        }

        // Next
        private void Control_Next(object sender = null, EventArgs e = null)
        {
            // Update the controls before checking the cell count
            Update_Controls();

            // Because if there are 0 alive cells, you shouldn't be able to...
            if (_cellCount > 0)
            {
                // Pause or
                Control_Process_Pause(sender, e);

                // Step forward one generation
                Process_NextGeneration();
            }
        }

        // Paint
         private void Control_Paint(object sender = null, EventArgs e = null)
        {
            // Set cursor to paint
            _draw = 0;

            // Update cursor style
            GraphicsPanel.Cursor = Cursors.Cross;

            // Update cursor properties
            toolStripButtonSingleClick.Checked = false;
            toolStripMenuItemSingleClick.Checked = false;
            toolStripButtonPaint.Checked = true;
            paintToolStripMenuItem.Checked = true;
            eraseToolStripMenuItem.Checked = false;
            toolStripButtonErase.Checked = false;

            // Tell windows to repaint panel
            GraphicsPanel.Invalidate();
        }

        // Erase
        private void Control_Erase(object sender = null, EventArgs e = null)
        {
            // Set cursor to erase
            _draw = 1;

            // Load custom eraser cursor
            Cursor eraser = new Cursor(Properties.Resources.eraser.GetHicon());

            // Update cursor
            GraphicsPanel.Cursor = eraser;

            // Update cursor properties
            toolStripButtonSingleClick.Checked = false;
            toolStripMenuItemSingleClick.Checked = false;
            toolStripButtonPaint.Checked = false;
            paintToolStripMenuItem.Checked = false;
            eraseToolStripMenuItem.Checked = true;
            toolStripButtonErase.Checked = true;

            // Tell windows to repaint panel
            GraphicsPanel.Invalidate();
        }

        // Single-Click
        private void Control_SingleClick(object sender = null, EventArgs e = null)
        {
            // Set cursor to single-click
            _draw = 2;

            // Update cursor style
            GraphicsPanel.Cursor = Cursors.Arrow;

            // Update cursor properties
            toolStripButtonSingleClick.Checked = true;
            toolStripMenuItemSingleClick.Checked = true;
            toolStripButtonPaint.Checked = false;
            paintToolStripMenuItem.Checked = false;
            eraseToolStripMenuItem.Checked = false;
            toolStripButtonErase.Checked = false;

            // Tell windows to repaint panel
            GraphicsPanel.Invalidate();
        }
        #endregion

        #region Randomize
        // Generate
        private void Randomize_GenerateSeed(object sender = null, EventArgs e = null)
        {
            // Generates user input inside text box

            // Check that the user didn't click away
            // and that the style is no longer italic
            if ((toolStripTextBoxSeed.Text.Length > 0 || toolStripTextBoxSeed.Text != Properties.Resources.seedPrompt)
                && toolStripTextBoxSeed.Font.Italic == false)
            {
                Randomize_Process_UpdateGraphics();
            }
            // Else nothing was entered
            else
            {
                // Reset seed box style
                SeedBox_SetStyle(true);
            }
        }

        // Random Seed
        private void Randomize_RandomSeed(object sender = null, EventArgs e = null)
        {
            // If timer is enabled, pause
            if (timer.Enabled == true)
            {
                Control_Process_Pause();
            }
            // Seed generated, display it
            _seedFlag = true;

            // Instantiate a random object
            Random rnd = new Random(); // <- Random seed from time (no parameters)

            // Generate random seed between all acceptable ranges
            _seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            // Update the universe array
            Randomize_Process_UpdateArray();

            // Recount cells
            Process_CountCells();

            // Update the status strip
            Update_StatusStrip();

            // Update Control buttons
            Update_Controls();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Opens a modal dialog to enter seed
        private void Randomize_EnterSeed(object sender = null, EventArgs e = null)
        {
            // Instantiate form for Enter a seed...
            ModalDialog_EnterSeed dlg = new ModalDialog_EnterSeed();

            // Grab the current seed
            dlg.Seed = _seed;

            // Grab the seed prompt
            dlg.Text = Properties.Resources.seedPrompt;

            // Set Seedbox Style
            dlg.SeedBox_SetStyle();

            // Open the dialog box
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // Retrieve the seed from the form
                _seed = dlg.Seed;

                // Update graphics
                Randomize_Process_UpdateGraphics();
            }

        }

        // Used to update graphics after a seed has been generated
        private void Randomize_Process_UpdateGraphics()
        {
            // User has input a seed, so display it
            _seedFlag = true;

            // Update the universe array
            Randomize_Process_UpdateArray();

            // Count alive cells
            Process_CountCells();

            // Update status strip
            Update_StatusStrip();

            // Update controls (will enable Start and Next if seed is blank)
            Update_Controls();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Process that updates the universe with random values
        private void Randomize_Process_UpdateArray()
        {
            // 0 is the shortcut for a blank canvas -> File_New()
            if (_seed != 0)
            {
                // Instantiate a random object
                Random rnd = new Random((int)_seed); // <- based on seed

                // Iterate through the universe in the y, top to bottom
                for (int y = 0; y < _universe.GetLength(1); y++)
                {
                    // Iterate through the universe in the x, left to right
                    for (int x = 0; x < _universe.GetLength(0); x++)
                    {
                        // Get a random bool
                        bool result = (rnd.Next(0, 2) == 0) ? false : true;

                        // Update the random value to the universe array
                        _universe[x, y] = result;
                    }
                }
            }

            // Count alive cells
            Process_CountCells();

            // Update status strip
            Update_StatusStrip();

            // Update controls (will enable Start and Next if seed is blank)
            Update_Controls();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }
        #endregion


        #region Settings
        
        #region View
        // Toggle HUD
        private void View_HUD(object sender = null, EventArgs e = null)
        {
            // Toggle checked state
            hUDToolStripMenuItem.Checked = !hUDToolStripMenuItem.Checked;
            hUDToolStripMenuItem1.Checked = !hUDToolStripMenuItem1.Checked;

            // Update related field
            _displayHUD = hUDToolStripMenuItem.Checked;

            // Autosave
            Settings_Process_AutoSave();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Initialize HUD
        private void View_Process_InitHUD()
        {
            // Initialize checked state
            hUDToolStripMenuItem.Checked = _displayHUD;
            hUDToolStripMenuItem.Checked = _displayHUD;
        }

        // Toggle Neighbor Count
        private void View_NeighborCount(object sender = null, EventArgs e = null)
        {

            // Toggle checked state
            neighborCountToolStripMenuItem.Checked = !neighborCountToolStripMenuItem.Checked;
            neighborCountToolStripMenuItem1.Checked = !neighborCountToolStripMenuItem1.Checked;

            // Update related field
            _displayNeighbors = neighborCountToolStripMenuItem.Checked;

            // Autosave
            Settings_Process_AutoSave();


            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Initialize Neighbor Count
        private void View_Process_InitNeighborCount()
        {
            // Initialize checked state
            neighborCountToolStripMenuItem.Checked = _displayNeighbors;
            neighborCountToolStripMenuItem1.Checked = _displayNeighbors;
        }

        // Toggle Grid
        private void View_Grid(object sender = null, EventArgs e = null)
        {

            // Toggle checked state
            gridToolStripMenuItem.Checked = !gridToolStripMenuItem.Checked;
            gridToolStripMenuItem1.Checked = !gridToolStripMenuItem1.Checked;

            // Update related field
            _displayGrid = gridToolStripMenuItem.Checked;

            // Autosave
            Settings_Process_AutoSave();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Initialize Grid
        private void View_Process_InitGrid()
        {
            // Initialize checked state
            _universeCopy = new bool[_universe.GetLength(0), _universe.GetLength(1)];
            gridToolStripMenuItem.Checked = _displayGrid;
            gridToolStripMenuItem1.Checked = _displayGrid;
        }

        // Torodial
        private void View_Torodial(object sender = null, EventArgs e = null)
        {

            // If boundary is finite
            if (_boundary == false)
            {
                // Boudary becomes Torodial
                _boundary = true;

                // Toggle checked state
                torodialToolStripMenuItem.Checked = true;
                finiteToolStripMenuItem.Checked = false;

                // Update Status strip
                Update_StatusStrip();
            }

            // Autosave
            Settings_Process_AutoSave();



            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Initialize Boundary
        private void View_Process_InitTorodial()
        {
            // Initialize checked state
            if (_boundary == true)
            {
                torodialToolStripMenuItem.Checked = true;
                finiteToolStripMenuItem.Checked = false;
            }
            else
            {
                torodialToolStripMenuItem.Checked = false;
                finiteToolStripMenuItem.Checked = true;
            }
        }

        // Finite
        private void View_Finite(object sender = null, EventArgs e = null)
        {

            // If boundary is torodial
            if (_boundary == true)
            {
                // Boudary becomes Finite
                _boundary = false;

                // Toggle checked state
                torodialToolStripMenuItem.Checked = false;
                finiteToolStripMenuItem.Checked = true;

                // Update Status strip
                Update_StatusStrip();
            }

            // Autosave
            Settings_Process_AutoSave();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }
        #endregion

        #region Color
        // Process that opens the Color Dialog Box
        private void Settings_Process_ColorDialogBox(ref Color color)
        {
            // Used for all modifiable colors, Color sent by reference
            ColorDialog dlg = new ColorDialog();

            // Open the dialog box
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // Update the color when the user selects OK
                color = dlg.Color;

                // Autosave
                Settings_Process_AutoSave();

                // Tell Windows you need to repaint
                GraphicsPanel.Invalidate();
            }
        }

        // Back Color
        private void Settings_BackColor(object sender = null, EventArgs e = null)
        {
            Settings_Process_ColorDialogBox(ref _backColor);
            GraphicsPanel.BackColor = _backColor;
        }

        // Cell Color
        private void Settings_CellColor(object sender = null, EventArgs e = null)
        {
            Settings_Process_ColorDialogBox(ref _cellColor);

        }

        // Grid Color
        private void Settings_GridColor(object sender = null, EventArgs e = null)
        {
            Settings_Process_ColorDialogBox(ref _gridColor);
        }

        // Grid x10 color
        private void Settings_GridX10Color(object sender = null, EventArgs e = null)
        {
            Settings_Process_ColorDialogBox(ref _grid10xColor);
        }


        #endregion

        // Speed
        private void Settings_Speed(object sender = null, EventArgs e = null)
        {
            // Instantiate the interval dialog box
            ModalDialog_Interval dlg = new ModalDialog_Interval();

            // Store the previous setting
            dlg.Interval = _interval;

            // Open the dialog box
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // Update to new value if the user clicked OK
                _interval = dlg.Interval;

                // Update the timer's interval
                timer.Interval = (int)dlg.Interval;

                // Autosave
                Settings_Process_AutoSave();

                // Update status strip
                Update_StatusStrip();
            }
        }

        // Size
        private void Settings_Size(object sender, EventArgs e)
        {
            // Instantiate the size dialog box
            ModalDialog_Size dlg = new ModalDialog_Size();

            // Temp variables for comparison
            int tempRow = _rows, tempCol = _columns;

            // Store the previous setting
            dlg.Rows = _rows;
            dlg.Columns = _columns;

            // Open the dialog box
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // Update to new values if the user clicked OK
                _rows = (int)dlg.Rows;
                _columns = (int)dlg.Columns;

                // Resize the universe
                Process_Resize();

                // Autosave
                Settings_Process_AutoSave();

                // Update status strip
                Update_StatusStrip();

                // Tell windows to repaint
                GraphicsPanel.Invalidate();
            }
        }

        // Reload / Loads settings from a file
        private void Settings_Reload(object sender = null, EventArgs e = null)
        {
            // Reload old settings
            Settings_Process_LoadSettings(true);

            // Tell windows to repaint
            GraphicsPanel.Invalidate();
        }

        private void Settings_Process_LoadSettings(bool loadPrevious = false)
        {
            // Get filename
            string fileName = (loadPrevious == false) ? Properties.Resources.settingsFile : Properties.Resources.settingsPrevious;

            // An array to store data from each line
            string[] data = new string[11];

            // Array index #
            int i = 0;

            // Check if the settings file does not exist
            if (!File.Exists(Properties.Resources.settingsFile))
            {
                // If not create new settings file
                Settings_Process_CreateSettings();
            }

            // Check if the old version of settings exists
            if (!File.Exists(Properties.Resources.settingsPrevious))
            {
                // If not create new settings file
                Settings_Process_CreateSettings(true);
            }

            // Read data from file
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // Ignore labels "// " within settings.cfg
                    if (!(line.StartsWith(Properties.Resources.commentPrefix)))
                    {
                        data[i] = line;
                        i++;
                    }
                }
            }

            // Reuse of i to easily access array without hardcoding
            i = 0;

            // Initialize data members here
            _backColor = Color.FromName(data[i]); i++;
            _gridColor = Color.FromName(data[i]); i++;
            _grid10xColor = Color.FromName(data[i]); i++;
            _cellColor = Color.FromName(data[i]); i++;
            _rows = Int32.Parse(data[i]); i++;
            _columns = Int32.Parse(data[i]); i++;
            _boundary = bool.Parse(data[i]); i++;
            _displayHUD = bool.Parse(data[i]); i++;
            _displayNeighbors = bool.Parse(data[i]); i++;
            _displayGrid = bool.Parse(data[i]); i++;
            _interval = decimal.Parse(data[i]);

            // Setup the timer
            timer.Interval = Int32.Parse(data[i]); // milliseconds
            timer.Tick += Process_Timer_Tick;

            // Initialoze back color
            GraphicsPanel.BackColor = _backColor;

            // Allocate the universe
            _universe = new bool[_rows, _columns];
        }

        // Reset / Create new settings file
        private void Settings_Reset(object sender = null, EventArgs e = null)
        {
            // Recreate Settings
            Settings_Process_CreateSettings();

            // Reload Settins
            Settings_Process_LoadSettings();

            // Reinitialize graphics
            Init_Graphics();
        }

        // Creates settings files
        private void Settings_Process_CreateSettings(bool createOld = false)
        {
            // Get filename
            string fileName = (createOld == false) ? Properties.Resources.settingsFile : Properties.Resources.settingsPrevious;

            // Label and write default properties to file
            using (StreamWriter sw = File.CreateText(fileName))
            {
                // All comments are prefixed with "// ", followed by label resource
                // These are default values for the application

                // Back Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelBackColor);
                sw.WriteLine(Color.White.Name);

                // Grid Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelGridColor);
                sw.WriteLine(Color.Gray.Name);

                // Grid 10x Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelGridx10Color);
                sw.WriteLine(Color.DarkSlateGray.Name);

                // Cell Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelCellColor);
                sw.WriteLine(Color.LightGray.Name);

                // Row Count
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelRowCount);
                sw.WriteLine(30);

                // Column Count
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelColumnCount);
                sw.WriteLine(30);

                // Boundary
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelBoundary);
                sw.WriteLine(true);

                // Display HUD
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelDisplayHUD);
                sw.WriteLine(true);

                // Display Neighbors Count
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelDisplayNeighborCount);
                sw.WriteLine(true);

                // Display Grid
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelDisplayGrid);
                sw.WriteLine(true);

                // Interval
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelInterval);
                sw.WriteLine(20);
            }
        }

        // Autosaves settings files
        private void Settings_Process_AutoSave(bool saveOld = false)
        {
            // Get filename
            string fileName = (saveOld == false) ? Properties.Resources.settingsFile : Properties.Resources.settingsPrevious;

            // Label and write default properties to file
            using (StreamWriter sw = File.CreateText(fileName))
            {
                // All comments are prefixed with "// ", followed by label resource
                // These are default values for the application

                // Back Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelBackColor);
                sw.WriteLine(_backColor.Name);

                // Grid Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelGridColor);
                sw.WriteLine(_gridColor.Name);

                // Grid 10x Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelGridx10Color);
                sw.WriteLine(_grid10xColor.Name);

                // Cell Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelCellColor);
                sw.WriteLine(_cellColor.Name);

                // Row Count
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelRowCount);
                sw.WriteLine(_rows);

                // Column Count
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelColumnCount);
                sw.WriteLine(_columns);

                // Boundary
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelBoundary);
                sw.WriteLine(_boundary);

                // Display HUD
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelDisplayHUD);
                sw.WriteLine(_displayHUD);

                // Display Neighbor Count
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelDisplayNeighborCount);
                sw.WriteLine(_displayNeighbors);

                // Display Grid
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelDisplayGrid);
                sw.WriteLine(_displayGrid);

                // Interval
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelInterval);
                sw.WriteLine(_interval);
            }
        }
        #endregion

        #region Help
        // About
        private void Help_About(object sender, EventArgs e)
        {
            // Show dialog
            AboutBox dlg = new AboutBox();
            dlg.ShowDialog();
        }

        #endregion



        #endregion

        #region Seed Box

        // Sets the font style of the seed box
        private void SeedBox_SetStyle(bool defaultStyle = false)
        {
            // Remove default styling
            if (defaultStyle == false)
            {
                toolStripTextBoxSeed.ForeColor = Color.Black;
                toolStripTextBoxSeed.Font = new Font(toolStripTextBoxSeed.Font, FontStyle.Regular);
            }
            // Reset to default styling, update text to the seed prompt
            else
            {
                toolStripTextBoxSeed.Font = new Font(toolStripTextBoxSeed.Font, FontStyle.Italic);
                toolStripTextBoxSeed.ForeColor = Color.Gray;
                toolStripTextBoxSeed.Text = Properties.Resources.seedPrompt;

                // Disable Generate
                generateToolStripMenuItem.Enabled = false;
                toolStripButtonGenerate.Enabled = false;
            }
        }

        // Parses seed value inside seed box
        private void SeedBox_ParseSeed(object sender = null, EventArgs e = null)
        {
            // Executes when the user has clicked away from text box

            // Used for parsing from text
            int stringSum = 0;


            // If the text box isn't blank, equal to 0, and is not the seed prompt
            if (toolStripTextBoxSeed.Text.Length > 0 && toolStripTextBoxSeed.Text != "0"
                && toolStripTextBoxSeed.Text != Properties.Resources.seedPrompt)
            {
                // If seed can be parsed, it will be in _seed
                if (!long.TryParse(toolStripTextBoxSeed.Text, out _seed))
                {
                    // Else generate seed from text

                    // Multipliers for stringSum
                    int multiplier1 = 1;
                    int multiplier2 = 2;

                    // Add each char as a number to stringSum
                    foreach (char letter in toolStripTextBoxSeed.Text)
                    {
                        // Multiply the int value of each char
                        // by 13 and by two incrementing multipliers

                        // This is to hit higher number ranges and
                        // prevent the letter values from adding up to the same number
                        stringSum += letter * 13 * multiplier1 * multiplier2;

                        // Increment multipliers
                        multiplier1++;
                        multiplier2++;
                    }

                    // Update seed
                    _seed = stringSum;

                    if (_hideParse == false)
                    {
                        toolStripTextBoxSeed.Text = Convert.ToString(_seed);
                    }

                }

                // Used to chop off a seed thats too long
                string maxSeed = null;

                // Used to update the seed if to low or high
                int tempSeed = 0;

                // If user entered 0, seed is blank
                if (_seed == 0)
                {
                    // Empty the universe
                    File_New(sender, e);
                }
                // Else if the seed exceeds the max or min value for the random class
                else if ((_seed > Int32.MaxValue) || (_seed < Int32.MinValue))
                {
                    // Parameter for substring method
                    int substringLength = 10;

                    // Increment for negative numbers
                    if ((_seed < Int32.MinValue))
                    {
                        substringLength++;
                    }

                    // Covert seed to string, limit to 10 characters
                    maxSeed = _seed.ToString().Substring(0, substringLength);

                    // Try to parse the seed within an int range
                    if (!int.TryParse(maxSeed, out tempSeed))
                    {
                        // If that didn't work, cut off another character
                        substringLength--;
                        maxSeed = _seed.ToString().Substring(0, substringLength);

                        // Parsing is safe now
                        tempSeed = Int32.Parse(maxSeed);
                    }

                    // Update seed
                    _seed = tempSeed;

                    if (_hideParse == false)
                    {
                        toolStripTextBoxSeed.Text = Convert.ToString(_seed);
                    }

                }
            }
            // Else the user entered nothing to parse
            else
            {
                // Reset the seed box style
                SeedBox_SetStyle(true);
            }

        }

        // When the seed box is clicked
        private void SeedBox_Click(object sender, EventArgs e)
        {
            // Empty the seed box on click

            // Enable Generate
            generateToolStripMenuItem.Enabled = true;
            toolStripButtonGenerate.Enabled = true;

            // Change font to regular / black
            SeedBox_SetStyle();

            // When focused
            if (toolStripTextBoxSeed.Focused == true)
            {
                // Empty text box
                toolStripTextBoxSeed.Text = "";
            }

        }
        #endregion

        #region Keyboard / MouseWheel
        // Enables zoom scaling with mouse wheel
        private void OnMouseWheel_Zoom(object sender, MouseEventArgs e)
        {
            // Scroll down (zoom out)
            if (e.Delta < 0)
            {
                Process_UniverseGrow();
            }
            // Scroll up (zoom in)
            else
            {
                Process_UniverseShrink();
            }

            // Process zoom
            Process_Zoom();
        }

        // Detects MouseDown on Graphics panel
        private void GraphicsPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // If cursor is not single-click
            if (_draw != 2)
            {
                _cursorMove = true;
            }
        }

        // Detects Mouse movement on Graphics panel
        private void GraphicsPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // If cursor movement is valid
            if (_cursorMove == true)
            {
                // Click when moving
                GraphicsPanel_MouseClick(sender, e);
            }
        }
        
        // Detects MouseUp on Graphics Panel
        private void GraphicsPanel_MouseUp(object sender, MouseEventArgs e)
        {
            // Invalidate cursor movement
            _cursorMove = false;
        }

        // For key detection within the application
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Enter = Generate
            if (e.KeyCode == Keys.Enter)
            {
                // So Enter doesn't make an alert noise
                e.SuppressKeyPress = true;

                // If focus is on seed box, parse seed, then generate
                if (toolStripTextBoxSeed.Focused == true)
                {
                    // Hide the seed parsing (seed parses when clicked outside of Seed Box)
                    _hideParse = true;
                    SeedBox_ParseSeed();
                    Randomize_GenerateSeed();
                    _hideParse = false;
                }
                // Else generate seed
                else
                {
                    // But only if the user entered something
                    if (toolStripTextBoxSeed.Text.Length > 0 && toolStripTextBoxSeed.Text != Properties.Resources.seedPrompt)
                    {
                        Randomize_GenerateSeed();
                    }

                }

                // Disable Generate
                generateToolStripMenuItem.Enabled = false;
                toolStripButtonGenerate.Enabled = false;

                // Change focus to Graphics panel
                GraphicsPanel.Focus();

                // Reset seed box style
                SeedBox_SetStyle(true);

            }

            // Don't register shortcuts when typing seed
            if (toolStripTextBoxSeed.Focused == false)
            {
                // Space = Start / Stop
                if (e.KeyCode == Keys.Space)
                {
                    Control_Start();
                }

                // Right Arrow = Next
                if (e.KeyCode == Keys.Right)
                {
                    Control_Next();
                }

                // Up Arrow = Zoom In
                if (e.KeyCode == Keys.Up)
                {
                    Process_UniverseShrink();
                    Process_Zoom();
                }

                // Down Arrow = Zoom Out
                if (e.KeyCode == Keys.Down)
                {
                    Process_UniverseGrow();
                    Process_Zoom();
                }

                // H = HUD
                if (e.KeyCode == Keys.H)
                {
                    View_HUD();
                }

                // N = Neighbor Count
                if (e.KeyCode == Keys.N)
                {
                    View_NeighborCount();
                }

                // G = Grid
                if (e.KeyCode == Keys.G)
                {
                    View_Grid();
                }

                // R = Random Seed
                if (e.KeyCode == Keys.R)
                {
                    Randomize_RandomSeed();
                }

                // E = Enter Seed
                if (e.KeyCode == Keys.E)
                {
                    Randomize_EnterSeed();
                }

                // B = Boundary change
                if (e.KeyCode == Keys.B)
                {
                    if (_boundary == true)
                    {
                        View_Finite();
                    }
                    else
                    {
                        View_Torodial();
                    }
                }

                // S = Speed
                if (e.KeyCode == Keys.S)
                {
                    Settings_Speed();
                }

                // Escape = File_Exit
                if (e.KeyCode == Keys.Escape)
                {
                    Application.Exit();
                }

                // 1 = Paint
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                {
                    Control_Paint();
                }

                // 2 = Erase
                if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                {
                    Control_Erase();
                }

                // 3 = Single-Click
                if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                {
                    Control_SingleClick();
                }

            }

            // Tell windows to repaint
            GraphicsPanel.Invalidate();
        }

        #endregion

        #region Graphics
        // Initialize graphics
        private void Init_Graphics()
        {
            // Update Grid
            View_Process_InitGrid();

            // Update Neighbor Cound
            View_Process_InitNeighborCount();

            // Update HUD
            View_Process_InitHUD();

            // Update Boundary
            View_Process_InitTorodial();

            // Set the cursor to paint
            Control_Paint();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        private void Process_GraphicsPanel_Paint(object sender, PaintEventArgs e)
        {
            // Covert to floats
            float clientWidth = GraphicsPanel.ClientSize.Width, zeroCount = _universe.GetLength(0),
                clientHeight = GraphicsPanel.ClientSize.Height, oneCount = _universe.GetLength(1);

            // Calculate the width and height of each cell in pixels
            float cellWidth = clientWidth / zeroCount;
            float cellHeight = clientHeight / oneCount;

            // Pen for drawing the grid lines
            Pen gridPen = new Pen(_gridColor, 1);

            // Pen for drawing the x10 Grid
            Pen gridx10Pen = new Pen(_grid10xColor, 2);

            // Brush for drawing the HUD
            Brush hudBrush = new SolidBrush(Color.FromArgb(0x78FF0000));

            Color neighborColor = Color.FromArgb(255, 255, 0, 0);

            Brush neighborBrush = new SolidBrush(neighborColor);
            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(_cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < _universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < _universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = RectangleF.Empty;

                    // Convert to floats for calculation
                    float fX = x, fY = y;

                    // Calculate for window scaling / cell placement
                    cellRect.X = fX * cellWidth;
                    cellRect.Y = fY * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (_universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // If the grid is enabled
                    if (_displayGrid == true)
                    {
                        // Outline the cell with a pen
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                        // Paint the 10x grid
                        if ((x % 10 == 0) || (y % 10 == 0))
                        {
                            e.Graphics.DrawRectangle(gridx10Pen, cellRect.X * 10, cellRect.Y * 10, clientWidth, clientHeight);
                        }
                    }

                    // If the neighbor count is enabled
                    if (_displayNeighbors == true)
                    {
                        // Font for neighbors
                        Font neighborsFont = new Font("Courier New", cellHeight * 0.4f, FontStyle.Regular);

                        // Create an empty string to append to
                        StringFormat stringFormat = new StringFormat();

                        // Center text
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;

                        // Get neighbor count for current x, y
                        int count = Process_CountNeighbors(x, y);

                        // Alpha value
                        int alpha = 255;

                        // Display neighbors if there are more than 0
                        if (count > 0)
                        {
                            // If the cell isn't alive, change alpha
                            if (_universe[x, y] != true)
                            {
                                alpha = 190;
                            }

                            // Green / Cells that will live next generation
                            if (count == 3 || (_universe[x, y] == true && count == 4))
                            {
                                neighborColor = Color.FromArgb(alpha, 0, 150, 0);
                                neighborBrush = new SolidBrush(neighborColor);
                            }
                            // Red / Cells that will die next generation
                            else
                            {
                                neighborColor = Color.FromArgb(alpha, 255, 0, 0);
                                neighborBrush = new SolidBrush(neighborColor);
                            }

                            // Decrement the count for active cells
                            if (_universe[x, y] == true)
                            {
                                count--;
                            }

                            // Check count after decrement
                            if (count > 0)
                            {
                                // Draw neighbor count
                                e.Graphics.DrawString(count.ToString(), neighborsFont, neighborBrush, cellRect, stringFormat);
                            }
                        }

                    }

                }
            }


            // If the HUD is enabled
            if (_displayHUD == true)
            {
                // Font for heads up display
                Font hudFont = new Font("Consolas", 15, FontStyle.Bold);

                // Draw the HUD
                e.Graphics.DrawString($"Generations: {_generations}\nCell Count: {_cellCount}" +
                    $"\nBoundary Type: {GetBoundaryString()}\nUniverse Size: {_rows} x {_columns}",
                    hudFont, hudBrush, 3, clientHeight - 95);
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            gridx10Pen.Dispose();
            cellBrush.Dispose();
            hudBrush.Dispose();

        }

        // Mouse click on graphics panel
        private void GraphicsPanel_MouseClick(object sender, MouseEventArgs e)
        {

            // When the Seed Box loses focus
            if (toolStripTextBoxSeed.Focused == false)
            {
                // Check the value within text box
                // If invalid
                if (toolStripTextBoxSeed.Text.Length == 0 || toolStripTextBoxSeed.Text == Properties.Resources.seedPrompt)
                {
                    // Reset Seed Box style
                    SeedBox_SetStyle(true);

                }
                // Else parse seed in seed box
                // Activates upon clicking GraphicsPanel
                else
                {
                    SeedBox_ParseSeed();
                }

            }

            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Covert to floats
                float clientWidth = GraphicsPanel.ClientSize.Width, zeroCount = _universe.GetLength(0),
                clientHeight = GraphicsPanel.ClientSize.Height, oneCount = _universe.GetLength(1),
                eX = e.X, eY = e.Y;

                // Calculate the width and height of each cell in pixels
                float cellWidth = clientWidth / zeroCount;
                float cellHeight = clientHeight / oneCount;

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = eX / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = eY / cellHeight;

                // If MouseDown is enabled, execute within panel bounds
                if ((_draw == 0 || _draw == 1) && (x < _universe.GetLength(0)) && (y < _universe.GetLength(0))
                    && x >= 0 && y >= 0)
                {
                    // Paint
                    if (_draw == 0)
                    {
                        _universe[(int)x, (int)y] = true;
                    }
                    // Erase
                    else
                    {
                        _universe[(int)x, (int)y] = false;
                    }
                }
                // Single-Click
                else if (_draw == 2)
                {
                    // Toggle the cell's state
                    _universe[(int)x, (int)y] = !_universe[(int)x, (int)y];
                }

                // Count cells

                // Only count what is in range
                if (x < _universe.GetLength(0) && y < _universe.GetLength(1)
                    && x >= 0 && y >= 0)
                {
                    // Paint
                    if (_draw == 0)
                    {
                        // If the universe copy hasn't been updated with painted
                        // cell, add it to array and increment count.
                        if (_universe[(int)x, (int)y] == true && _universeCopy[(int)x, (int)y] == false)
                        {
                            _universeCopy[(int)x, (int)y] = _universe[(int)x, (int)y];
                            _cellCount++;
                        }
                    }
                    // Erase
                    else if (_draw == 1)
                    {
                        // If the universe copy hasn't been updated with deleted
                        // cell, remove it from array and decrement count.
                        if (_universe[(int)x, (int)y] == false && _universeCopy[(int)x, (int)y] == true)
                        {
                            _universeCopy[(int)x, (int)y] = _universe[(int)x, (int)y];
                            _cellCount--;
                        }
                    }
                    // Single-Click
                    else if (_draw == 2)
                    {
                        // If toggled on, increment cell count
                        if (_universe[(int)x, (int)y] == true)
                        {
                            _cellCount++;
                        }
                        // Else if toggled off, decrement cell count
                        else
                        {
                            _cellCount--;
                        }
                    }
                }

                // Update controls
                Update_Controls();

                // Update Status strip
                Update_StatusStrip();

                // Count cells
                Process_CountCells();

                // Tell Windows you need to repaint
                GraphicsPanel.Invalidate();
            }
        }
        #endregion

        #region Background Processes
        // Count alive cells
        private void Process_CountCells()
        {
            // Reset cell count
            _cellCount = 0;

            // Get the current cell count
            foreach (bool cell in _universe)
            {
                if (cell == true)
                {
                    _cellCount++;
                }
            }
        }

        // Calculate the next generation of cells
        private void Process_NextGeneration()
        {
            // 2-D bool array identical in size to _universe array
            bool[,] nextUniverse = new bool[_universe.GetLength(0), _universe.GetLength(1)];

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < _universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < _universe.GetLength(0); x++)
                {
                    // Get neighbor count
                    int count = Process_CountNeighbors(x, y);

                    // Decrement the count for the current cell
                    if (_universe[x, y] == true)
                    {
                        count--;
                    }


                    // Game of Life Rules
                    // Cell is alive but has less than 2 neighbors or more than 3 neighbors
                    if ((_universe[x, y] == true) && (count < 2 || (count > 3)))
                    {
                        nextUniverse[x, y] = false;
                    }
                    // Cell is dead but has 3 alive neighbors will live next generation
                    else if ((_universe[x, y] == false) && (count == 3))
                    {
                        nextUniverse[x, y] = true;
                    }
                    // Else nothing has changed
                    else
                    {
                        nextUniverse[x, y] = _universe[x, y];
                    }
                }
            }

            // Update the universe to the next generation
            _universe = nextUniverse;

            // Increment generation count
            _generations++;

            // Update Status strip
            Update_StatusStrip();

            // Recount cells
            Process_CountCells();

            // Update Controls
            Update_Controls();

            // Update Status strip
            Update_StatusStrip();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Count Neighbors
        private int Process_CountNeighbors(int x, int y)
        {
            // Count will be returned
            // Horizontal / Vertical are calculated dependent on boundary
            int count = 0, horizontal = 0, vertical = 0;

            // Iterate each adjacent space next to the current cell
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // Torodial boundary
                    if (_boundary == true)
                    {
                        // Use modulous to wrap around the universe
                        horizontal = (x + i + _rows) % _rows;
                        vertical = (y + j + _columns) % _columns;

                        // Increment neighbors if the cell is alive
                        if (_universe[horizontal, vertical] == true)
                        {
                            count++;
                        }
                    }
                    // Finite boundary
                    else
                    {
                        // Checking for out of bounds
                        if ((x + i >= 0 && y + j >= 0) && (x + i < _universe.GetLength(0) && y + j < _universe.GetLength(1)))
                        {
                            // Check each adjacent space through addition and iteration
                            horizontal = x + i;
                            vertical = y + j;
                            // Increment neighbors if the cell is alive
                            if (_universe[horizontal, vertical] == true)
                            {
                                count++;
                            }
                        }
                    }
                    
                }
            }

            // Return count
            return count;
        }

        // The event called by the timer every Interval milliseconds.
        private void Process_Timer_Tick(object sender, EventArgs e)
        {
            // Pause timer if there are no living cells
            if (_cellCount == 0)
            {
                Control_Process_Pause(sender, e);
            }
            // Else keep going
            else
            {
                Process_NextGeneration();
            }

        }

        // Update controls for Control_Next
        private void Update_Controls()
        {
            // If there is at least 1 alive cell
            if (_cellCount > 0)
            {
                // Enable controls
                toolStripButtonStart.Enabled = true;    // Enable Start
                startToolStripMenuItem.Enabled = true;  // Enable Start
                startToolStripMenuItem1.Enabled = true; // Enable Start

                nextToolStripMenuItem.Enabled = true;   // Enable Next
                nextToolStripMenuItem1.Enabled = true;  // Enable Next
                toolStripButtonNext.Enabled = true;     // Enable Next
            }
            // Else _cellcount = 0
            else
            {
                // Disable controls
                toolStripButtonStart.Enabled = false;       // Disable Start
                startToolStripMenuItem.Enabled = false;     // Disable Start
                startToolStripMenuItem1.Enabled = false;    // Disable Start    

                nextToolStripMenuItem.Enabled = false;      // Disable Next
                nextToolStripMenuItem1.Enabled = false;     // Disable Next
                toolStripButtonNext.Enabled = false;        // Disable Next
            }
        }

        // Updates the status strip
        private void Update_StatusStrip()
        {
            // Seed flag indicates whether or not the user has generated a seed
            if (_seedFlag == true)
            {
                // Update the current seed
                toolStripStatusLabelSeed.Text = Properties.Resources.labelSeed + Properties.Resources.equalSign + _seed;
            }
            // Else display "Seed = N/A"
            else
            {
                toolStripStatusLabelSeed.Text = Properties.Resources.labelSeed + Properties.Resources.equalSign + Properties.Resources.labelTextNA;
            }

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = Properties.Resources.labelGenerations + Properties.Resources.equalSign + _generations;

            // Update status strip cell count
            toolStripStatusLabelAlive.Text = Properties.Resources.labelCellCount + Properties.Resources.equalSign + _cellCount;

            // Update status strip interval
            toolStripStatusLabelInterval.Text = Properties.Resources.labelInterval + Properties.Resources.equalSign + _interval;
        }

        // Shrink the universe's x and y by 1
        private void Process_UniverseShrink()
        {
            // Universe min size is 3 rows or columns
            // Useful for saving a basic glider with no padding
            if (_rows > 3 && _columns > 3)
            {
                // Decrement until at 3
                _rows--;
                _columns--;
            }
        }

        // Grow the universe's x and y by 1
        private void Process_UniverseGrow()
        {
            // Universe max size is 100 rows or columns
            if (_rows < 100 && _columns < 100)
            {
                // Increment until at 300
                _rows++;
                _columns++;
            }
        }

        // Used by MouseWheel and Up/Down Arrow Keys
        private void Process_Zoom()
        {
            // Copy current universe to a temporary one
            bool[,] tempUniverse = _universe;

            // Reallocate the universe
            _universe = new bool[_rows, _columns];

            Process_Crop(ref tempUniverse);
        }


        // Resize / Alternate version of Process_Crop, but used for Settings_Size
        private void Process_Resize()
        {
            // Allocate a temp universe to the current number of rows and columns
            bool[,] tempUniverse = new bool[_rows, _columns];

            // Get the smaller row and column count
            int tempRow = (_rows < _universe.GetLength(0)) ? _rows : _universe.GetLength(0);
            int tempCol = (_columns < _universe.GetLength(1)) ? _columns : _universe.GetLength(1);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < tempCol; y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < tempRow; x++)
                {
                    tempUniverse[x, y] = _universe[x, y];
                }
            }

            // Reallocate the universe
            _universe = new bool[_rows, _columns];

            // Crop universe
            Process_Crop(ref tempUniverse);
        }

        // Crop universe
        private void Process_Crop(ref bool[,] tempUniverse)
        {
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < _columns; y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < _rows; x++)
                {
                    // Boudary limits, delete cells at boundry
                    if ((x == _rows - 1 || y == _columns - 1))
                    {
                        _universe[x, y] = false;
                    }
                    // Else copy cells that fit within universe
                    else
                    {
                        _universe[x, y] = tempUniverse[x, y];
                    }
                }
            }

            // Reallocate the universe copy upon resize
            _universeCopy = new bool[_universe.GetLength(0), _universe.GetLength(1)];

            // Update statuses
            Update_StatusStrip();

            // Update cell count
            Process_CountCells();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Get a string value for the boundary
        private string GetBoundaryString()
        {
            return (_boundary == true) ? Properties.Resources.torodial : Properties.Resources.finite;
        }

        // Psuedo Destructor
        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Save settings.cfg and old_settings.cfg upon exit
            Settings_Process_AutoSave();
            Settings_Process_AutoSave(true);
        }
        #endregion
    }
}