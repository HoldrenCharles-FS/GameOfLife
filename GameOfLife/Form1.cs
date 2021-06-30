using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        // Fields
        private bool[,] _universe;      // The universe array

        private Color _gridColor;       // Grid color
        private Color _cellColor;       // Cell color

        Timer timer = new Timer();       // The Timer class

        private int _generations = 0;    // Generation count

        // Constructor
        public Form1()
        {
            // Load settings from file
            LoadSettings();

            // Initialize components for Windows Form (avoid editing)
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
        }

        // Load settings from file
        private void LoadSettings()
        {
            // An array to store data from each line
            string[] data = new string[4];

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
                    if (!(line.StartsWith("//")))
                    {
                        data[i] = line;
                        i++;
                    }
                }
            }

            // Initialize data members here
            _gridColor = Color.FromName(data[0]);
            _cellColor = Color.FromName(data[1]);
            _universe = new bool[Int32.Parse(data[2]), Int32.Parse(data[3])];
        }

        // Create new settings file
        private void CreateSettings()
        {
            // Label and write each property to file
            using (StreamWriter sw = File.CreateText(Properties.Resources.settingsFile))
            {
                // Grid Color
                sw.WriteLine("// " + Properties.Resources.labelGridColor);
                sw.WriteLine(Color.Black.Name);

                // Cell Color
                sw.WriteLine("// " + Properties.Resources.labelCellColor);
                sw.WriteLine(Color.Gray.Name);

                // Row Count
                sw.WriteLine("// " + Properties.Resources.labelRowCount);
                sw.WriteLine(10);

                // Column Count
                sw.WriteLine("// " + Properties.Resources.labelColumnCount);
                sw.WriteLine(10);
            }
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            bool[,] nextUniverse = new bool[_universe.GetLength(0), _universe.GetLength(1)];

            // Iterate through the universe in the y, top to bottom
            for (int y = 1; y < _universe.GetLength(1) - 1; y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 1; x < _universe.GetLength(0) - 1; x++)
                {
                    // Count neighbors
                    int neighbors = 0;

                    // Iterate each adjacent space next to the current cell
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            // Ignore borders for now
                            if (x == 0 || y == 0 || x == _universe.GetLength(0) - 1 || x == _universe.GetLength(1) - 1)
                            {
                                nextUniverse[x, y] = _universe[x, y];
                            }
                            else if (_universe[x + i, y + j] == true)
                            {
                                // Increment neighbors if the cell is alive
                                neighbors++;
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
            toolStripStatusLabelGenerations.Text = "Generations = " + _generations.ToString();

            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
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

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
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

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        // New button
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Pause in the case that it is running
            pauseToolStripMenuItem_Click(sender, e);

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

        // Tool strip version of the New Button
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        // Start button
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle between Start / Pause states 
            if (timer.Enabled == false)
            {
                // Start timer
                timer.Enabled = true;
                timer.Tick += Timer_Tick;

                // Change the display name
                toolStripStart.Text = Properties.Resources.pause;

                // Toggle tool strip Start icon to the Pause icon
                toolStripStart.Image = Properties.Resources.pauseIcon;

                // Disable File > Start
                startToolStripMenuItem.Enabled = false;

                // Enable File > Pause
                pauseToolStripMenuItem.Enabled = true;
            }
            else
            {
                // Pause
                pauseToolStripMenuItem_Click(sender, e);
            }

        }

        // Tool strip version of the Start Button
        private void toolStripButtonStart(object sender, EventArgs e)
        {
            startToolStripMenuItem_Click(sender, e);
        }

        // Pause button
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Stop timer
            timer.Enabled = false;

            // Change the display name
            toolStripStart.Text = Properties.Resources.start;

            // Toggle tool strip Start icon to the Pause icon
            toolStripStart.Image = Properties.Resources.startIcon;

            // Enable File > Start
            startToolStripMenuItem.Enabled = true;

            // Disable File > Pause
            pauseToolStripMenuItem.Enabled = false;
        }

        private void toolStripButtonNext_Click(object sender, EventArgs e)
        {
            // Pause
            pauseToolStripMenuItem_Click(sender, e);

            NextGeneration();
        }
    }
}