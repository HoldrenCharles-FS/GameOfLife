using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region Settings
        
        #region Color
        // Initialize all colors upon load
        private void InitializeColors()
        {
            ColorPreview(ref _backColor);
            backColorToolStripMenuItem.Image = ColorPreview(ref _backColor);
            backColorToolStripMenuItem1.Image = ColorPreview(ref _backColor);

            ColorPreview(ref _cellColor);
            cellColorToolStripMenuItem.Image = ColorPreview(ref _cellColor);
            cellColorToolStripMenuItem1.Image = ColorPreview(ref _cellColor);

            if (_displayGrid == true)
            {
                ColorPreview(ref _gridColor);
                gridColorToolStripMenuItem.Image = ColorPreview(ref _gridColor);
                gridColorToolStripMenuItem1.Image = ColorPreview(ref _gridColor);

                ColorPreview(ref _gridX10Color);
                gridX10ColorToolStripMenuItem.Image = ColorPreview(ref _gridX10Color);
                gridX10ColorToolStripMenuItem1.Image = ColorPreview(ref _gridX10Color);
            }
        }

        // Process that opens the Color Dialog Box
        private void ColorDialogBox(ref Color color)
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

                // Enable reset button
                resetToolStripMenuItem.Enabled = true;

                // Tell Windows you need to repaint
                GraphicsPanel.Invalidate();
            }
        }


        private Bitmap ColorPreview(ref Color color)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(color.A, color.R, color.G, color.B)))
                {
                    gfx.FillRectangle(brush, 0, 0, 16, 16);
                }
            }

            return bmp;
        }

        // Back Color
        private void Settings_BackColor(object sender = null, EventArgs e = null)
        {
            ColorDialogBox(ref _backColor);
            GraphicsPanel.BackColor = _backColor;

            ColorPreview(ref _backColor);

            backColorToolStripMenuItem.Image = ColorPreview(ref _backColor);
            backColorToolStripMenuItem1.Image = ColorPreview(ref _backColor);

            // Enable reset button
            resetToolStripMenuItem.Enabled = true;

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Cell Color
        private void Settings_CellColor(object sender = null, EventArgs e = null)
        {
            ColorDialogBox(ref _cellColor);

            ColorPreview(ref _cellColor);

            cellColorToolStripMenuItem.Image = ColorPreview(ref _cellColor);
            cellColorToolStripMenuItem1.Image = ColorPreview(ref _cellColor);

            // Enable reset button
            resetToolStripMenuItem.Enabled = true;

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Grid Color
        private void Settings_GridColor(object sender = null, EventArgs e = null)
        {
            if (_displayGrid == true)
            {
                gridColorToolStripMenuItem.Enabled = true;
                gridColorToolStripMenuItem1.Enabled = true;
                gridX10ColorToolStripMenuItem.Enabled = true;
                gridX10ColorToolStripMenuItem1.Enabled = true;

                ColorDialogBox(ref _gridColor);

                ColorPreview(ref _gridColor);

                gridColorToolStripMenuItem.Image = ColorPreview(ref _gridColor);
                gridColorToolStripMenuItem1.Image = ColorPreview(ref _gridColor);

                // Enable reset button
                resetToolStripMenuItem.Enabled = true;
            }
            else
            {
                gridColorToolStripMenuItem.Enabled = false;
                gridColorToolStripMenuItem1.Enabled = false;
                gridX10ColorToolStripMenuItem.Enabled = false;
                gridX10ColorToolStripMenuItem1.Enabled = false;

                gridColorToolStripMenuItem.Image = null;
                gridColorToolStripMenuItem1.Image = null;
                gridX10ColorToolStripMenuItem.Image = null;
                gridX10ColorToolStripMenuItem1.Image = null;

            }

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();

        }

        // Grid x10 color
        private void Settings_GridX10Color(object sender = null, EventArgs e = null)
        {
            if (_displayGrid == true)
            {
                gridColorToolStripMenuItem.Enabled = true;
                gridColorToolStripMenuItem1.Enabled = true;
                gridX10ColorToolStripMenuItem.Enabled = true;
                gridX10ColorToolStripMenuItem1.Enabled = true;

                ColorDialogBox(ref _gridX10Color);

                ColorPreview(ref _gridX10Color);

                gridX10ColorToolStripMenuItem.Image = ColorPreview(ref _gridX10Color);
                gridX10ColorToolStripMenuItem1.Image = ColorPreview(ref _gridX10Color);

                // Enable reset button
                resetToolStripMenuItem.Enabled = true;
            }
            else
            {
                gridColorToolStripMenuItem.Enabled = false;
                gridColorToolStripMenuItem1.Enabled = false;
                gridX10ColorToolStripMenuItem.Enabled = false;
                gridX10ColorToolStripMenuItem1.Enabled = false;
            }

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
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
                UpdateStatusStrip();

                // Enable reset button
                resetToolStripMenuItem.Enabled = true;
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
                UpdateUniverseSize();

                // Autosave
                Settings_Process_AutoSave();

                // Update status strip
                UpdateStatusStrip();

                // Enable reset button
                resetToolStripMenuItem.Enabled = true;

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

        // Reset / Create new settings file
        private void Settings_Reset(object sender = null, EventArgs e = null)
        {
            // Disable until settings have changed
            resetToolStripMenuItem.Enabled = false;

            // Recreate Settings
            Settings_Process_CreateSettings();

            // Reload Settins
            Settings_Process_LoadSettings();

            // Recount cells
            CountCells();

            // Update Status Strip
            UpdateStatusStrip();

            // Reinitialize graphics
            Init_Graphics();
        }

        // Load settings from file
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
            // Check if settings are equal to fields, if returned false
            // Reset should be enabled upon reload
            _backColor = Color.FromName(data[i]); i++;
            _cellColor = Color.FromName(data[i]); i++;
            _gridColor = Color.FromName(data[i]); i++;
            _gridX10Color = Color.FromName(data[i]); i++;
            _rows = Int32.Parse(data[i]); i++;
            _columns = Int32.Parse(data[i]); i++;
            _boundary = bool.Parse(data[i]); i++;
            _displayHUD = bool.Parse(data[i]); i++;
            _displayNeighbors = bool.Parse(data[i]); i++;
            _displayGrid = bool.Parse(data[i]); i++;
            _interval = decimal.Parse(data[i]);

            // Setup the timer
            timer.Interval = Int32.Parse(data[i]); // milliseconds

            // Initialoze back color
            GraphicsPanel.BackColor = _backColor;

            if (_universe != null)
            {
                _universeCopy = _universe;
            }
            else
            {
                _universeCopy = new bool[_rows, _columns];
            }

            // Allocate the universe
            _universe = new bool[_rows, _columns];

            // Iterate through the universe one row at a time.
            for (int height = 0; height < _universe.GetLength(1); height++)
            {
                // Iterate through the current row one cell at a time.
                for (int length = 0; length < _universe.GetLength(0); length++)
                {
                    if (length < _universeCopy.GetLength(0) && height < _universeCopy.GetLength(1))
                    {
                        _universe[length, height] = _universeCopy[length, height];
                    }

                }
            }



            // Option to reset Settings
            if (_backColor.Name != Color.White.Name || _cellColor.Name != Color.LightGray.Name
                || _gridColor.Name != Color.Gray.Name || _gridX10Color.Name != Color.DarkSlateGray.Name
                || _rows != 30 || _columns != 30
                || _boundary != true || _displayHUD != true
                || _displayNeighbors != true || _displayGrid != true
                || _interval != 20)
            {
                resetToolStripMenuItem.Enabled = true;
            }
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

                // Cell Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelCellColor);
                sw.WriteLine(Color.LightGray.Name);

                // Grid Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelGridColor);
                sw.WriteLine(Color.Gray.Name);

                // Grid 10x Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelGridx10Color);
                sw.WriteLine(Color.DarkSlateGray.Name);

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

                // Cell Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelCellColor);
                sw.WriteLine(_cellColor.Name);

                // Grid Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelGridColor);
                sw.WriteLine(_gridColor.Name);

                // Grid 10x Color
                sw.WriteLine(Properties.Resources.commentPrefix + Properties.Resources.labelGridx10Color);
                sw.WriteLine(_gridX10Color.Name);

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

        // Resize / Alternate version of Process_Crop, but used for Settings_Size
        private void UpdateUniverseSize()
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
            Crop(ref tempUniverse);
        }

        #endregion
    }
}
