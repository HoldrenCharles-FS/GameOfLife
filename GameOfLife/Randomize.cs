using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace GameOfLife
{
    public partial class Game : Form
    {
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
                Control_Process_Pause();
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
            // Reset generations
            _generations = 0;

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
    }
}
