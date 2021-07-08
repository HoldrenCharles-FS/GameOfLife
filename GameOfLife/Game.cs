using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        // Fields
        private bool[,] _universe;          // The universe array
        Timer timer = new Timer();          // The Timer class
        Timer shrinkTimer = new Timer();    // For mouse down event
        Timer growTimer = new Timer();      // For mouse down event
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
        private Color _gridX10Color;    // Grid 10x color
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
            shrinkTimer.Interval = 30;
            shrinkTimer.Tick += Process_ShrinkTimer_Tick;
            growTimer.Interval = 30;
            growTimer.Tick += Process_GrowTimer_Tick;
            timer.Tick += Timer_Tick;
        }

        // Count alive cells
        private void CountCells()
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

        // Count Neighbors
        private int CountNeighbors(int x, int y)
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

        // Calculate the next generation of cells
        private void NextGeneration()
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
                    int count = CountNeighbors(x, y);

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
            UpdateStatusStrip();

            // Recount cells
            CountCells();

            // Update Controls
            UpdateControls();

            // Update Status strip
            UpdateStatusStrip();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Pause timer if there are no living cells
            if (_cellCount == 0)
            {
                Pause(sender, e);
            }
            // Else keep going
            else
            {
                NextGeneration();
            }

        }

        // Shrink the universe's x and y by 1
        private void UniverseShrink(object sender = null, EventArgs e = null)
        {
            // Universe min size is 3 rows or columns
            // Useful for saving a basic glider with no padding
            if (_rows > 3 && _columns > 3)
            {
                // Decrement until at 3
                _rows--;
                _columns--;
            }
            if (_rows != 30 || _columns != 30)
            {
                resetToolStripMenuItem.Enabled = true;
            }

            // Process zoom
            Zoom();
        }

        // Grow the universe's x and y by 1
        private void UniverseGrow(object sender = null, EventArgs e = null)
        {

            // Universe max size is 200 rows or columns
            if (_rows < 200 && _columns < 200)
            {
                // Increment until at 200
                _rows++;
                _columns++;
            }
            if (_rows != 30 || _columns != 30)
            {
                resetToolStripMenuItem.Enabled = true;
            }

            // Process zoom
            Zoom();
        }

        // Update controls for Control_Next
        private void UpdateControls()
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
        private void UpdateStatusStrip()
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

        // Used by MouseWheel and Up/Down Arrow Keys
        private void Zoom()
        {
            // Copy current universe to a temporary one
            bool[,] tempUniverse = _universe;

            // Reallocate the universe
            _universe = new bool[_rows, _columns];

            Crop(ref tempUniverse);
        }

        // Crop universe
        private void Crop(ref bool[,] tempUniverse)
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
            UpdateStatusStrip();

            // Update cell count
            CountCells();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Psuedo Destructor
        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Save settings.cfg and old_settings.cfg upon exit
            Settings_Process_AutoSave();
            Settings_Process_AutoSave(true);
        }

        
    }
}