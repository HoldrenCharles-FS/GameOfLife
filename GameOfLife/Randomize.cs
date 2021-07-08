using System;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region Randomize
        // Generate
        private void Randomize_Generate(object sender = null, EventArgs e = null)
        {
            // Generates user input inside text box

            // Check that the user didn't click away
            // and that the style is no longer italic
            if ((toolStripTextBoxSeed.Text.Length > 0 || toolStripTextBoxSeed.Text != Properties.Resources.seedPrompt)
                && toolStripTextBoxSeed.Font.Italic == false)
            {
                Pause();
                UpdateGraphics();
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
                Pause();
            }
            // Seed generated, display it
            _seedFlag = true;

            // Instantiate a random object
            Random rnd = new Random(); // <- Random seed from time (no parameters)

            // Generate random seed between all acceptable ranges
            _seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            // Update the universe array
            Populate();

            // Recount cells
            CountCells();

            // Update the status strip
            UpdateStatusStrip();

            // Update Control buttons
            UpdateControls();

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
                UpdateGraphics();
            }

        }

        // Used to update graphics after a seed has been generated
        private void UpdateGraphics()
        {
            // User has input a seed, so display it
            _seedFlag = true;

            // Update the universe array
            Populate();

            // Count alive cells
            CountCells();

            // Update status strip
            UpdateStatusStrip();

            // Update controls (will enable Start and Next if seed is blank)
            UpdateControls();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Process that updates the universe with random values
        private void Populate()
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
            CountCells();

            // Update status strip
            UpdateStatusStrip();

            // Update controls (will enable Start and Next if seed is blank)
            UpdateControls();

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }
        #endregion
    }
}
