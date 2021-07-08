using System;
using System.IO;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region File
        // New
        private void File_New(object sender = null, EventArgs e = null)
        {
            // Reset universe
            _seed = 0;
            _generations = 0;
            _universe = new bool[_rows, _columns];

            // Update Game.Text to New World
            Text = Properties.Resources.fileNewWorld + Properties.Resources.appendTitle;

            // Display N/A instead of _seed
            _seedFlag = false;

            // Display default message in seed box
            SeedBox_SetStyle(true);

            // Update Status Strip
            UpdateStatusStrip();

            // Count alive cells
            CountCells();

            // Update controls (will enable Start and Next if seed is blank)
            UpdateControls();

            // Pause in the case that it is running
            Pause(sender, e);

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

                if (_importFlag == true)
                {
                    _universeCopy = _universe;
                }

                // Resize the universe
                if (_importFlag == true)
                {
                    _rows = _universeCopy.GetLength(0);
                    _columns = _universeCopy.GetLength(1);
                }
                else
                {
                    _rows = rows;
                    _columns = columns;

                }
                _universe = new bool[_rows, _columns];

                if (_importFlag == true)
                {
                    // Iterate through the universe one row at a time.
                    for (int height = 0; height < _universeCopy.GetLength(1); height++)
                    {

                        // Iterate through the current row one cell at a time.
                        for (int length = 0; length < _universeCopy.GetLength(0); length++)
                        {
                            _universe[length, height] = _universeCopy[length, height];
                        }
                    }
                }

                // Reset the file pointer back to the beginning of the file.
                sr.BaseStream.Seek(0, SeekOrigin.Begin);

                // Column indexer
                int y = 0;

                // Iterate through the file again, this time reading in the cells.
                while (!sr.EndOfStream)
                {
                    // Read one row at a time.
                    string row = sr.ReadLine();

                    // Used only for Import
                    bool[,] tempUniverse = new bool[row.Length, columns];

                    // If the row begins with '!' it is a comment
                    if (row[0] != '!')
                    {
                        // Update the universe
                        for (int x = 0; x < row.Length; x++)
                        {
                            // On open
                            if (_importFlag == false)
                            {
                                // If row[xPos] is a 'O' (capital O) then it is alive
                                _universe[x, y] = (row[x] == 'O') ? true : false;
                            }
                            // On import
                            else
                            {
                                // If row[xPos] is a 'O' (capital O) then it is alive
                                tempUniverse[x, y] = (row[x] == 'O') ? true : false;

                                if (x < _universe.GetLength(0) && y < _universe.GetLength(1))
                                {
                                    // Use the OR operator to keep alive cells, well, alive.
                                    _universe[x, y] = _universe[x, y] | tempUniverse[x, y];
                                }

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

                if (_importFlag == false || Text != (Properties.Resources.fileNewWorld + Properties.Resources.appendTitle))
                {
                    // Update Game.Text to include filename
                    Text = _fileName + Properties.Resources.appendTitle;
                }

                // Display N/A instead of _seed
                _seedFlag = false;

                // Recount cells
                CountCells();

                // Update status strip
                UpdateStatusStrip();

                // Update controls
                UpdateControls();

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
    }
}
