using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region Fields and Contructor
        // Fields
        private bool[,] _universe;      // The universe array
        private Color _backColor;       // Back color
        private Color _cellColor;       // Cell color
        private Color _gridColor;       // Grid color
        private Color _grid10xColor;    // Grid 10x color
        private int _rows;              // Rows count
        private int _columns;           // Column Count
        private int _generations;       // Generation count
        private int _seed;              // Seed
        private bool _boundary;         // Boundary type : True = Torodial, False = Finite
        private bool _hud;              // Display HUD
        private bool _neighborCount;    // Display Neighbor Count
        private bool _displayGrid;      // Display Grid

        Timer timer = new Timer();      // The Timer class
        private int _cellCount = 0;     // Cell count
        private int[,] _neighbors;

        // Constructor
        public Game()
        {
            // Load settings from file
            Settings_Reload();

            // Initialize components for Windows Form (avoid editing)
            InitializeComponent();

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

            // Display default message in seed box
            SeedBox_SetStyle(true);

            // Update Status Strip
            Update_StatusStrip();

            // Pause in the case that it is running
            Control_Pause(sender, e);

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Open
        private void File_Open(object sender, EventArgs e)
        {

        }

        // Save
        private void File_Save(object sender, EventArgs e)
        {

        }

        // Save As
        private void File_SaveAs(object sender, EventArgs e)
        {

        }

        // Exit
        private void File_Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Control
        // Start
        private void Control_Start(object sender, EventArgs e)
        {
            // Toggle between Start / Pause states 
            if (timer.Enabled == false)
            {
                // Start timer
                timer.Enabled = true;

                // Toggle tool strip Start icon to the Pause icon
                toolStripButtonStart.Image = Properties.Resources.pauseIcon;

                // Disable File > Start
                startToolStripMenuItem.Enabled = false;

                // Enable File > Pause
                pauseToolStripMenuItem.Enabled = true;
            }
            else
            {
                // Pause
                Control_Pause(sender, e);
            }
        }

        // Pause
        private void Control_Pause(object sender, EventArgs e)
        {
            // Stop timer
            timer.Enabled = false;

            // Toggle tool strip Start icon to the Pause icon
            toolStripButtonStart.Image = Properties.Resources.startIcon;

            // Enable File > Start
            startToolStripMenuItem.Enabled = true;

            // Disable File > Pause
            pauseToolStripMenuItem.Enabled = false;
        }

        // Next
        private void Control_Next(object sender = null, EventArgs e = null)
        {
            Update_Controls();

            if (_cellCount > 0)
            {
                // Pause
                Control_Pause(sender, e);

                // Step forward one generation
                Process_NextGeneration();
            }
        }


        #endregion

        #region Randomize
        // Regenerate
        private void Randomize_GenerateSeed(object sender = null, EventArgs e = null)
        {
            if (toolStripTextBoxSeed.Text.Length > 0 && toolStripTextBoxSeed.Font.Italic == false)
            {
                // If user entered 0, seed is blank
                if (_seed == 0)
                {
                    File_New(sender, e);
                }
                else if (_seed > Int32.MaxValue)
                {
                    _seed = Int32.MaxValue;
                }
                else if (_seed < Int32.MinValue)
                {
                    _seed = Int32.MinValue;
                }

                Randomize_Process_UpdateArray();

                Process_CountCells();

                Update_StatusStrip();

                Update_Controls();

                // Tell Windows you need to repaint
                GraphicsPanel.Invalidate();
            }
            else
            {
                SeedBox_SetStyle(true);
            }
        }

        // Random Seed
        private void Randomize_RandomSeed(object sender, EventArgs e)
        {
            SeedBox_SetStyle();

            Random rnd = new Random();

            _seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            Randomize_Process_UpdateArray();

            SeedBox_ParseSeed();

            Process_CountCells();

            Update_StatusStrip();

            Update_Controls();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();


        }

        // Process that updates the universe with random values
        private void Randomize_Process_UpdateArray()
        {
            // 0 is the shortcut for a blank canvas
            if (_seed != 0)
            {
                Random rnd = new Random(_seed);

                // Iterate through the universe in the y, top to bottom
                for (int y = 0; y < _universe.GetLength(1); y++)
                {
                    // Iterate through the universe in the x, left to right
                    for (int x = 0; x < _universe.GetLength(0); x++)
                    {
                        bool result = (rnd.Next(0, 2) == 0) ? false : true;
                        _universe[x, y] = result;
                    }
                }
            }
        }
        #endregion

        #region Settings
        #region Color
        // Back Color
        private void Settings_BackColor(object sender, EventArgs e)
        {
            Settings_Process_ColorDialogBox(ref _backColor);
            GraphicsPanel.BackColor = _backColor;
        }

        // Cell Color
        private void Settings_CellColor(object sender, EventArgs e)
        {
            Settings_Process_ColorDialogBox(ref _cellColor);

        }

        // Grid Color
        private void Settings_GridColor(object sender, EventArgs e)
        {
            Settings_Process_ColorDialogBox(ref _gridColor);
        }

        // Grid x10 color
        private void Settings_GridX10Color(object sender, EventArgs e)
        {
            Settings_Process_ColorDialogBox(ref _grid10xColor);
        }


        #endregion

        #region View
        // Toggle HUD
        private void View_HUD(object sender, EventArgs e)
        {
            hUDToolStripMenuItem.Checked = !hUDToolStripMenuItem.Checked;
            hUDToolStripMenuItem1.Checked = !hUDToolStripMenuItem1.Checked;
            _hud = hUDToolStripMenuItem.Checked;

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Toggle Neighbor Count
        private void View_NeighborCount(object sender, EventArgs e)
        {
            neighborCountToolStripMenuItem.Checked = !neighborCountToolStripMenuItem.Checked;
            neighborCountToolStripMenuItem1.Checked = !neighborCountToolStripMenuItem1.Checked;
            _neighborCount = neighborCountToolStripMenuItem.Checked;
        }

        // Toggle Grid
        private void View_Grid(object sender, EventArgs e)
        {
            gridToolStripMenuItem.Checked = !gridToolStripMenuItem.Checked;
            gridToolStripMenuItem1.Checked = !gridToolStripMenuItem1.Checked;
            _displayGrid = gridToolStripMenuItem.Checked;

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Torodial
        private void View_Torodial(object sender, EventArgs e)
        {
            if (_boundary == false)
            {
                _boundary = true;
                torodialToolStripMenuItem.Checked = true;
                finiteToolStripMenuItem.Checked = false;

                Update_StatusStrip();
            }
        }

        // Finite
        private void View_Finite(object sender, EventArgs e)
        {
            if (_boundary == true)
            {
                _boundary = false;
                torodialToolStripMenuItem.Checked = false;
                finiteToolStripMenuItem.Checked = true;

                Update_StatusStrip();
            }
        }
        #endregion

        // Reload / Loads settings from a file
        private void Settings_Reload(object sender = null, EventArgs e = null)
        {
            // An array to store data from each line
            string[] data = new string[13];

            // Array index #
            int i = 0;

            // Check if the file does not exist
            if (!File.Exists(Properties.Resources.settingsFile))
            {
                // If not create new settings file
                Settings_Reset();
            }

            // Read data from file
            using (StreamReader sr = new StreamReader(Properties.Resources.settingsFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // Ignore labels within settings.cfg
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
            _generations = Int32.Parse(data[i]); i++;
            _seed = Int32.Parse(data[i]); i++;
            _boundary = bool.Parse(data[i]); i++;
            _hud = bool.Parse(data[i]); i++;
            _neighborCount = bool.Parse(data[i]); i++;
            _displayGrid = bool.Parse(data[i]); i++;

            // Setup the timer
            timer.Interval = Int32.Parse(data[i]); // milliseconds
            timer.Tick += Process_Timer_Tick;

            _universe = new bool[_rows, _columns];
            _neighbors = new int[_rows, _columns];
        }

        // Reset / Create new settings file
        private void Settings_Reset(object sender = null, EventArgs e = null)
        {
            // Label and write default properties to file
            using (StreamWriter sw = File.CreateText(Properties.Resources.settingsFile))
            {
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

                // Generations
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelGenerations);
                sw.WriteLine(0);

                // Seed
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelSeed);
                sw.WriteLine(0);

                // Boundary
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelBoundary);
                sw.WriteLine(true);

                // HUD
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelHUD);
                sw.WriteLine(true);

                // Neighbors Count
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelHUD);
                sw.WriteLine(true);

                // Grid
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelDisplayGrid);
                sw.WriteLine(true);

                // Timer
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelTimer);
                sw.WriteLine(20);
            }
        }

        // Process that opens the Color Dialog Box
        private void Settings_Process_ColorDialogBox(ref Color color)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = color;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                color = dlg.Color;

                // Tell Windows you need to repaint
                GraphicsPanel.Invalidate();
            }
        }

        #endregion

        #region Seed Box
        // Sets the font style of the seed box
        private void SeedBox_SetStyle(bool defaultStyle = false)
        {
            if (defaultStyle == false)
            {
                toolStripTextBoxSeed.ForeColor = Color.Black;
                toolStripTextBoxSeed.Font = new Font(toolStripTextBoxSeed.Font, FontStyle.Regular);
            }
            else
            {
                toolStripTextBoxSeed.Font = new Font(toolStripTextBoxSeed.Font, FontStyle.Italic);
                toolStripTextBoxSeed.ForeColor = Color.Gray;
                toolStripTextBoxSeed.Text = Properties.Resources.seedPrompt;

            }

        }

        // Parses seed value inside seed box
        private void SeedBox_ParseSeed(object sender = null, EventArgs e = null)
        {
            int stringSum = 0;

            if (toolStripTextBoxSeed.Text.Length > 0 && toolStripTextBoxSeed.Text != "0"
                && toolStripTextBoxSeed.Text != Properties.Resources.seedPrompt)
            {
                // If seed can be parsed, it will be in _seed
                // Add each char as a number to stringSum
                if (!int.TryParse(toolStripTextBoxSeed.Text, out _seed))
                {
                    foreach (char letter in toolStripTextBoxSeed.Text)
                    {
                        stringSum += letter;
                    }
                    _seed = stringSum;
                    toolStripTextBoxSeed.Text = Convert.ToString(_seed);
                }
            }
            else
            {
                SeedBox_SetStyle(true);

            }

        }

        // When the seed box is clicked
        private void SeedBox_Click(object sender, EventArgs e)
        {
            SeedBox_SetStyle();
            if (toolStripTextBoxSeed.Focused == true)
            {

                toolStripTextBoxSeed.Text = "";
            }

        }
        #endregion

        #endregion

        #region Keyboard / MouseWheel
        // Enables zoom scaling with mouse wheel
        private void OnMouseWheel_Zoom(object sender, MouseEventArgs e)
        {
            // Scroll down (zoom out)
            if (e.Delta < 0)
            {
                if (_rows < 300 && _columns < 300)
                {
                    _rows++;
                    _columns++;
                }
            }
            // Scroll up (zoom in)
            else
            {
                if (_rows > 5 && _columns > 5)
                {
                    _rows--;
                    _columns--;
                }
            }

            bool[,] tempUniverse = _universe;
            _universe = new bool[_rows, _columns];

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < _columns; y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < _rows; x++)
                {
                    if (x == _rows - 1 || y == _columns - 1)
                    {
                        _universe[x, y] = false;
                    }
                    else
                    {
                        _universe[x, y] = tempUniverse[x, y];
                    }

                }
            }
            Update_StatusStrip();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // For key detection within the application
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (toolStripTextBoxSeed.Text == null)
                {
                    SeedBox_SetStyle();
                }
                else if (toolStripTextBoxSeed.Focused == true)
                {
                    SeedBox_ParseSeed();
                    Randomize_GenerateSeed();
                }
                else
                {
                    Randomize_GenerateSeed();
                }

            }
            if (e.KeyCode == Keys.Space)
            {
                Control_Next();
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

        // Paint graphics panel
        private void Process_GraphicsPanel_Paint(object sender, PaintEventArgs e)
        {
            // Covert to floats
            float clientWidth = GraphicsPanel.ClientSize.Width, zeroCount = _universe.GetLength(0),
                clientHeight = GraphicsPanel.ClientSize.Height, oneCount = _universe.GetLength(1);

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = clientWidth / zeroCount;

            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = clientHeight / oneCount;

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(_gridColor, 1);
            Pen grid10xPen = new Pen(_grid10xColor, 2);
            Brush hudBrush = new SolidBrush(Color.FromArgb(0x78FF0000));


            int r = (_cellColor.R + 127) % 255;
            int g = (_cellColor.G + 127) % 255;
            int b = (_cellColor.B + 127) % 255;

            Color neighborColor = Color.FromArgb(255, r, g, b);

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
                    //Rectangle cellRect = Rectangle.Empty;
                    float fX = x, fY = y;
                    cellRect.X = fX * cellWidth;
                    cellRect.Y = fY * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (_universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    if (_displayGrid == true)
                    {
                        // Outline the cell with a pen
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                        // Paint the 10x grid
                        if ((x % 10 == 0) || (y % 10 == 0))
                        {
                            e.Graphics.DrawRectangle(grid10xPen, cellRect.X * 10, cellRect.Y * 10, clientWidth, clientHeight);
                        }
                    }

                    if (_neighborCount == true)
                    {
                        Font neighborsFont = new Font("Courier New", cellHeight * 0.7f, FontStyle.Regular);


                        int count = Process_CountNeighbors(x, y);


                        if (count > 0)
                        {
                            if (count == 3)
                            {
                                neighborColor = Color.FromArgb(255, r, 160, b);
                                neighborBrush = new SolidBrush(neighborColor);
                            }
                            else if (count != 3)
                            {
                                neighborColor = Color.FromArgb(255, 255, g, b);
                                neighborBrush = new SolidBrush(neighborColor);
                            }

                            if (_universe[x,y] == true)
                            {
                                count--;
                            }

                            if (count > 0)
                            {
                                e.Graphics.DrawString(Convert.ToString(count), neighborsFont, neighborBrush, cellRect.X + 2, cellRect.Y);
                            }
                        }

                    }
                }
            }

            if (_hud == true)
            {
                Font hudFont = new Font("Consolas", 15, FontStyle.Bold);

                e.Graphics.DrawString($"Generations: {_generations}\nCell Count: {_cellCount}" +
                    $"\nBoundary Type: {GetBoundaryString()}\nUniverse Size: {_rows} x {_columns}",
                    hudFont, hudBrush, 3, clientHeight - 95);
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            grid10xPen.Dispose();
            cellBrush.Dispose();
            hudBrush.Dispose();

        }

        // Mouse click on graphics panel
        private void Process_GraphicsPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (toolStripTextBoxSeed.Focused == false)
            {
                if (toolStripTextBoxSeed.Text.Length == 0)
                {
                    SeedBox_SetStyle(true);
                    toolStripTextBoxSeed.Text = Properties.Resources.seedPrompt;

                }
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

                // Toggle the cell's state
                _universe[(int)x, (int)y] = !_universe[(int)x, (int)y];

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

                Update_Controls();

                Update_StatusStrip();

                // Tell Windows you need to repaint
                GraphicsPanel.Invalidate();
            }
        }

        // Calculate the next generation of cells
        private void Process_NextGeneration()
        {
            bool[,] nextUniverse = new bool[_universe.GetLength(0), _universe.GetLength(1)];

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < _universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < _universe.GetLength(0); x++)
                {
                    int neighbors = Process_CountNeighbors(x, y);

                    // Decrement the count for the current cell
                    if (_universe[x, y] == true)
                    {
                        neighbors--;
                    }


                    // Game of Life Rules
                    // Cell is alive but has less than 2 neighbors or more than 3 neighbors
                    if ((_universe[x, y] == true) && (neighbors < 2 || (neighbors > 3)))
                    {
                        nextUniverse[x, y] = false;
                    }
                    // Cell is dead but has 3 alive neighbors will live next generation
                    else if ((_universe[x, y] == false) && (neighbors == 3))
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

            Update_StatusStrip();

            Process_CountCells();

            Update_Controls();

            Update_StatusStrip();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        private int Process_CountNeighbors(int x, int y)
        {
            // Count neighbors
            int count = 0, horizontal = 0, vertical = 0, cntr = -1;

            // Iterate each adjacent space next to the current cell
            for (int i = -1; i <= 1; i++, cntr++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // Torodial boundary check
                    if (_boundary == true)
                    {
                        horizontal = (x + i + _rows) % _rows;
                        vertical = (y + j + _columns) % _columns;
                        if (_universe[horizontal, vertical] == true)
                        {
                            // Increment neighbors if the cell is alive
                            count++;
                        }

                    }
                    else
                    {
                        // Checking for out of bounds
                        if ((x + i >= 0 && y + j >= 0) && (x + i < _universe.GetLength(0) && y + j < _universe.GetLength(1)))
                        {
                            horizontal = x + i;
                            vertical = y + j;
                            if (_universe[horizontal, vertical] == true)
                            {
                                // Increment neighbors if the cell is alive
                                count++;
                            }
                        }
                    }


                }
            }
            return count;
        }

        // The event called by the timer every Interval milliseconds.
        private void Process_Timer_Tick(object sender, EventArgs e)
        {
            // Pause if there are no living cells
            if (_cellCount == 0)
            {
                Control_Pause(sender, e);
            }
            else
            {
                // Else keep going
                Process_NextGeneration();
            }

        }

        // Update controls for Control_Next
        private void Update_Controls()
        {
            if (_cellCount > 0)
            {
                nextToolStripMenuItem.Enabled = true;
                nextToolStripMenuItem1.Enabled = true;
                toolStripButtonNext.Enabled = true;
            }
            else
            {
                nextToolStripMenuItem.Enabled = false;
                nextToolStripMenuItem1.Enabled = false;
                toolStripButtonNext.Enabled = false;
            }
        }

        // Updates the status strip
        private void Update_StatusStrip()
        {
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = Properties.Resources.labelGenerations + Properties.Resources.equalSign + _generations;

            // Update status strip cell count
            toolStripStatusLabelAlive.Text = Properties.Resources.labelCellCount + Properties.Resources.equalSign + _cellCount;

            // Update status strip boundary
            string boundary = GetBoundaryString();
            toolStripStatusLabelType.Text = Properties.Resources.labelBoundary + Properties.Resources.equalSign + boundary;

            // Update status strip universe size
            toolStripStatusLabelSize.Text = Properties.Resources.labelUniverseSize + Properties.Resources.equalSign + $"{ _rows} x {_columns}";
        }

        private string GetBoundaryString()
        {
            return (_boundary == true) ? Properties.Resources.torodial : Properties.Resources.finite;
        }


        #endregion
    }
}