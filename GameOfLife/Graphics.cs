using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
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

            // Update Colors
            Settings_Process_InitColors();

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
            Pen gridx10Pen = new Pen(_gridX10Color, 2);

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
                if ((_draw == 0 || _draw == 1) && (x < _universe.GetLength(0)) && (y < _universe.GetLength(1))
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

        // Get a string value for the boundary
        private string GetBoundaryString()
        {
            return (_boundary == true) ? Properties.Resources.torodial : Properties.Resources.finite;
        }
        #endregion
    }
}
