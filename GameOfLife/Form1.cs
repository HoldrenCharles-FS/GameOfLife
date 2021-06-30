using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        //Fields
        // The universe array
        private bool[,] _universe = new bool[5, 5];

        // Drawing colors
        private Color _gridColor;
        private Color _cellColor;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            LoadConfig();

            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        private void LoadConfig()
        {
            string[] data = new string[2];

            int i = 0;

            if (!File.Exists(Properties.Resources.settings))
            {
                CreateConfig();
            }
            using (StreamReader sr = new StreamReader(Properties.Resources.settings))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!(line.StartsWith("//")))
                    {
                        data[i] = line;
                        i++;
                    }
                }
            }

            _gridColor = Color.FromName(data[0]);
            _cellColor = Color.FromName(data[1]);
        }

        private void CreateConfig()
        {
            using (StreamWriter sw = File.CreateText(Properties.Resources.settings))
            {
                sw.WriteLine("// " + Properties.Resources.labelGridColor);
                sw.WriteLine(Color.Black.Name);

                sw.WriteLine("// " + Properties.Resources.labelCellColor);
                sw.WriteLine(Color.Gray.Name);
            }
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < _universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < _universe.GetLength(0); x++)
                {
                    _universe[x, y] = false;

                }
            }
            // Tell Windows you need to repaint
            graphicsPanel1.Invalidate();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonStart(object sender, EventArgs e)
        {

        }
    }
}