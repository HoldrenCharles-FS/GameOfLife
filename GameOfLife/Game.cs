using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        // Fields
        private bool[,] _universe;      // The universe array
        private Color _backColor;       // Back color
        private Color _gridColor;       // Grid color
        private Color _grid10xColor;    // Grid 10x color
        private Color _cellColor;       // Cell color
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
        private bool _enterPressed;

        // Constructor
        public Game()
        {
            // Load settings from file
            LoadSettings();

            // Initialize components for Windows Form (avoid editing)
            InitializeComponent();

            MouseWheel += Zoom_MouseWheel;
        }

        // Load settings from file
        private void LoadSettings()
        {
            // An array to store data from each line
            string[] data = new string[12];

            // Array index #
            int i = 0;

            // Check if the file does not exist
            if (!File.Exists(Properties.Resources.settingsFile))
            {
                // If not create new settings file
                CreateSettings();
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
            _displayGrid = bool.Parse(data[i]); i++;

            // Setup the timer
            timer.Interval = Int32.Parse(data[i]); // milliseconds
            timer.Tick += Timer_Tick;

            _universe = new bool[_rows, _columns];
        }

        // Create new settings file
        private void CreateSettings()
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
                sw.WriteLine(Color.Black.Name);

                // Cell Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelCellColor);
                sw.WriteLine(Color.Gray.Name);

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

                // Display Grid
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelDisplayGrid);
                sw.WriteLine(true);

                // Timer
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelTimer);
                sw.WriteLine(20);
            }
        }

        private void UpdateControls()
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

        private void UpdateStatusStrip()
        {
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = Properties.Resources.labelGenerations + Properties.Resources.equalSign + _generations;

            // Update status strip cell count
            toolStripStatusLabelCellCount.Text = Properties.Resources.labelCellCount + Properties.Resources.equalSign + _cellCount;

            // Update status strip boundary
            string boundary = (_boundary == true) ? Properties.Resources.torodial : Properties.Resources.finite;
            toolStripStatusLabelBoundary.Text = Properties.Resources.labelBoundary + Properties.Resources.equalSign + boundary;

            // Update status strip universe size
            toolStripStatusLabelUniverseSize.Text = Properties.Resources.labelUniverseSize + Properties.Resources.equalSign + $"{ _rows} x {_columns}";
        }

        // Enables zoom scaling with mouse wheel
        private void Zoom_MouseWheel(object sender, MouseEventArgs e)
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
            UpdateStatusStrip();

            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (_enterPressed == true && toolStripTextBoxSeed.Focused == true)
                {
                    toolStripButtonGo_Click();
                }
                if (toolStripTextBoxSeed.Focused == true)
                {
                    _enterPressed = true;
                    SeedParse();
                }
                else
                {
                    toolStripButtonGo_Click();
                }

            }
            if (e.KeyCode == Keys.Space)
            {
                Next();
            }
        }

        private void Randomize()
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
        // Calculate the next generation of cells
        private void NextGeneration()
        {
            bool[,] nextUniverse = new bool[_universe.GetLength(0), _universe.GetLength(1)];

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < _universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < _universe.GetLength(0); x++)
                {
                    // Count neighbors
                    int neighbors = 0;

                    // Iterate each adjacent space next to the current cell
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            // Torodial boundary check
                            if (_boundary == true)
                            {
                                int horizontal = (x + i + _rows) % _rows;
                                int vertical = (y + j + _columns) % _columns;
                                if (_universe[horizontal, vertical] == true)
                                {
                                    // Increment neighbors if the cell is alive
                                    neighbors++;
                                }

                            }
                            else
                            {
                                // Checking for out of bounds
                                if ((x + i >= 0 && y + j >= 0) && (x + i < _universe.GetLength(0) && y + j < _universe.GetLength(1)))
                                {
                                    if (_universe[x + i, y + j] == true)
                                    {
                                        // Increment neighbors if the cell is alive
                                        neighbors++;
                                    }
                                }
                            }

                        }
                    }

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

            UpdateStatusStrip();

            CountCells();

            UpdateControls();

            UpdateStatusStrip();

            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

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

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Pause if there are no living cells
            if (_cellCount == 0)
            {
                Pause(sender, e);
            }
            else
            {
                // Else keep going
                NextGeneration();
            }

        }

        // Paint graphics panel
        private void GraphicsPanel_Paint(object sender, PaintEventArgs e)
        {
            // Covert to floats
            float clientWidth = graphicsPanel1.ClientSize.Width, zeroCount = _universe.GetLength(0),
                clientHeight = graphicsPanel1.ClientSize.Height, oneCount = _universe.GetLength(1);

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = clientWidth / zeroCount;

            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = clientHeight / oneCount;

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(_gridColor, 1);
            Pen grid10xPen = new Pen(_grid10xColor, 2);

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
                        // Paint the 10x grid
                        if ((x % 10 == 0) || (y % 10 == 0))
                        {
                            e.Graphics.DrawRectangle(grid10xPen, cellRect.X * 10, cellRect.Y * 10, clientWidth, clientHeight);
                        }

                        // Outline the cell with a pen
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            grid10xPen.Dispose();
            cellBrush.Dispose();

        }

        // Mouse click on graphics panel
        private void GraphicsPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (toolStripTextBoxSeed.Focused == false)
            {
                if (toolStripTextBoxSeed.Text.Length == 0)
                {
                    SeedBoxStyles(true);
                    toolStripTextBoxSeed.Text = Properties.Resources.seedPrompt;

                }
                else
                {
                    SeedParse();
                }

            }

            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Covert to floats
                float clientWidth = graphicsPanel1.ClientSize.Width, zeroCount = _universe.GetLength(0),
                clientHeight = graphicsPanel1.ClientSize.Height, oneCount = _universe.GetLength(1),
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

                UpdateControls();

                toolStripStatusLabelCellCount.Text = "Cell Count = " + _cellCount;

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        // New
        private void New(object sender, EventArgs e)
        {
            // Reset universe
            _seed = 0;
            _universe = new bool[_rows, _columns];

            SeedBoxStyles(true);

            // Update status strip generations
            _generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = " + _generations;

            // Update status strip Cell count
            _cellCount = 0;
            toolStripStatusLabelCellCount.Text = "Cell Count = " + _cellCount;

            // Update status strip universe size
            toolStripStatusLabelUniverseSize.Text = $"Universe Size = {_rows} x {_columns}";

            // Pause in the case that it is running
            Pause(sender, e);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < _universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < _universe.GetLength(0); x++)
                {
                    // Set each element to false
                    _universe[x, y] = false;
                }
            }
            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

        // Start


        // Exit
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void Start(object sender, EventArgs e)
        {
            // Toggle between Start / Pause states 
            if (timer.Enabled == false)
            {
                // Start timer
                timer.Enabled = true;

                // Change the display name
                toolStripButtonStart.Text = Properties.Resources.pause;

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
                Pause(sender, e);
            }
        }

        private void Pause(object sender, EventArgs e)
        {
            // Stop timer
            timer.Enabled = false;

            // Change the display name
            toolStripButtonStart.Text = Properties.Resources.start;

            // Toggle tool strip Start icon to the Pause icon
            toolStripButtonStart.Image = Properties.Resources.startIcon;

            // Enable File > Start
            startToolStripMenuItem.Enabled = true;

            // Disable File > Pause
            pauseToolStripMenuItem.Enabled = false;
        }

        private void Next(object sender = null, EventArgs e = null)
        {
            UpdateControls();

            if (_cellCount > 0)
            {
                // Pause
                Pause(sender, e);

                // Step forward one generation
                NextGeneration();
            }
        }

        private void TorodialMode(object sender, EventArgs e)
        {
            if (_boundary == false)
            {
                _boundary = true;
                torodialToolStripMenuItem.Checked = true;
                finiteToolStripMenuItem.Checked = false;

                UpdateStatusStrip();
            }
        }

        private void FiniteMode(object sender, EventArgs e)
        {
            if (_boundary == true)
            {
                _boundary = false;
                torodialToolStripMenuItem.Checked = false;
                finiteToolStripMenuItem.Checked = true;

                UpdateStatusStrip();
            }
        }

        private void ToggleHUD(object sender, EventArgs e)
        {
            HUDToolStripMenuItem.Checked = !HUDToolStripMenuItem.Checked;
            hUDToolStripMenuItem1.Checked = !hUDToolStripMenuItem1.Checked;
            _hud = HUDToolStripMenuItem.Checked;

        }

        private void ToggleNeighborCount(object sender, EventArgs e)
        {
            neighborCountToolStripMenuItem.Checked = !neighborCountToolStripMenuItem.Checked;
            neighborCountToolStripMenuItem1.Checked = !neighborCountToolStripMenuItem1.Checked;
            _neighborCount = neighborCountToolStripMenuItem.Checked;
        }

        private void ToggleGrid(object sender, EventArgs e)
        {
            GridToolStripMenuItem.Checked = !GridToolStripMenuItem.Checked;
            gridToolStripMenuItem1.Checked = !gridToolStripMenuItem1.Checked;
            _displayGrid = GridToolStripMenuItem.Checked;

            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

        private void ColorDialogBox(ref Color color)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = color;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                color = dlg.Color;

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void toolStripTextBoxSeed_Click(object sender, EventArgs e)
        {
            SeedBoxStyles();
            if (toolStripTextBoxSeed.Focused == true)
            {
                toolStripTextBoxSeed.Text = "";
            }

        }

        private void SeedBoxStyles(bool defaultStyle = false)
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

        private void toolStripButtonGo_Click(object sender = null, EventArgs e = null)
        {
            if (toolStripTextBoxSeed.Text.Length > 0 && toolStripTextBoxSeed.Font.Italic == false)
            {
                // If user entered 0, seed is blank
                if (_seed == 0)
                {
                    New(sender, e);
                }
                else if (_seed > Int32.MaxValue)
                {
                    _seed = Int32.MaxValue;
                }
                else if (_seed < Int32.MinValue)
                {
                    _seed = Int32.MinValue;
                }

                Randomize();

                CountCells();

                UpdateStatusStrip();

                UpdateControls();

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
            else
            {
                SeedBoxStyles(true);
            }
        }

        private void SeedParse(object sender = null, EventArgs e = null)
        {
            int stringSum = 0;
            if (toolStripTextBoxSeed.Text.Length > 0 && toolStripTextBoxSeed.Font.Italic == false)
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
                SeedBoxStyles(true);
            }
        }

        private void SeedRandom(object sender, EventArgs e)
        {
            SeedBoxStyles();

            Random rnd = new Random();

            _seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            Randomize();

            SeedParse();

            CountCells();

            UpdateStatusStrip();

            UpdateControls();

            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();


        }

        private void SeedBox_Leave(object sender, EventArgs e)
        {
            SeedParse(sender, e);
            SeedBoxStyles();
        }

        #region Duplicate Methods

        // Tool strip version of the Start Button
        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            Start(sender, e);
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Start(sender, e);
        }

        private void pauseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Pause(sender, e);
        }

        private void nextToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Next(sender, e);
        }

        // Tool strip version of the New Button
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            New(sender, e);
        }

        private void BackColorDialog(object sender, EventArgs e)
        {
            ColorDialogBox(ref _backColor);
            graphicsPanel1.BackColor = _backColor;
        }

        private void CellColorDialog(object sender, EventArgs e)
        {
            ColorDialogBox(ref _cellColor);

        }

        private void GridColorDialog(object sender, EventArgs e)
        {
            ColorDialogBox(ref _gridColor);
        }

        private void GridX10ColorDialog(object sender, EventArgs e)
        {
            ColorDialogBox(ref _grid10xColor);
        }

        private void backColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BackColorDialog(sender, e);
        }

        private void cellColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CellColorDialog(sender, e);
        }

        private void gridColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GridColorDialog(sender, e);
        }

        private void gridX10ColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GridX10ColorDialog(sender, e);
        }

        private void hUDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToggleHUD(sender, e);
        }

        private void gridToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToggleGrid(sender, e);
        }

        private void neighborCountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToggleNeighborCount(sender, e);
        }

        private void torodialToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TorodialMode(sender, e);
        }

        private void finiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FiniteMode(sender, e);
        }
        #endregion


    }
}