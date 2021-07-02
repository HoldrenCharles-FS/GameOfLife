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
        private bool _boundary;          // Boundary type : True = Torodial, False = Finite

        Timer timer = new Timer();      // The Timer class
        private int _cellCount = 0;     // Cell count

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
            string[] data = new string[9];

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
            _boundary = bool.Parse(data[i]); i++;

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

                // Boundary
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelBoundary);
                sw.WriteLine(true);

                // Timer
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelTimer);
                sw.WriteLine(20);
            }
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
            // Update status strip universe size
            toolStripStatusLabelUniverseSize.Text = $"Universe Size = {_rows} x {_columns}";

            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
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

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + _generations;

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

            if (_cellCount > 0)
            {

                nextToolStripMenuItem.Enabled = true;
                toolStripButtonNext.Enabled = true;
            }
            else
            {
                nextToolStripMenuItem.Enabled = false;
                toolStripButtonNext.Enabled = false;
            }

            // Update status strip cell count
            toolStripStatusLabelCellCount.Text = "Cell Count = " + _cellCount;

            string boundary = (_boundary == true) ? "Torodial" : "Finite";

            // Update status strip boundary
            toolStripStatusLabelBoundary.Text = "Boundary = " + boundary;

            // Update status strip universe size
            toolStripStatusLabelUniverseSize.Text = $"Universe Size = {_rows} x {_columns}";

            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
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

                    // Paint the 10x grid
                    if ((x % 10 == 0) || (y % 10 == 0))
                    {
                        e.Graphics.DrawRectangle(grid10xPen, cellRect.X * 10, cellRect.Y * 10, clientWidth, clientHeight);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                    



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

                if (_cellCount > 0)
                {

                    nextToolStripMenuItem.Enabled = true;
                    toolStripButtonNext.Enabled = true;
                }
                else
                {
                    nextToolStripMenuItem.Enabled = false;
                    toolStripButtonNext.Enabled = false;
                }

                toolStripStatusLabelCellCount.Text = "Cell Count = " + _cellCount;

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        // New
        private void New(object sender, EventArgs e)
        {
            // Reset universe
            _universe = new bool[_rows, _columns];

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

        // Pause
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

        // Next
        private void Next(object sender, EventArgs e)
        {
            if (_cellCount > 0)
            {

                nextToolStripMenuItem.Enabled = true;
                toolStripButtonNext.Enabled = true;

                // Pause
                Pause(sender, e);

                // Step forward one generation
                NextGeneration();
            }
            else
            {
                nextToolStripMenuItem.Enabled = false;
                toolStripButtonNext.Enabled = false;
            }
        }

        // Exit
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Duplicate Methods

        // Tool strip version of the Start Button
        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            Start(sender, e);
        }

        // Tool strip version of the New Button
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            New(sender, e);
        }
        #endregion

    }
}