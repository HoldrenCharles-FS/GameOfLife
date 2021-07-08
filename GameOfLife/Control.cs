using System;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region Control
        // Start
        private void Control_Start(object sender = null, EventArgs e = null)
        {
            // Toggle between Start / Pause states 

            // When the timer is disabled
            if (timer.Enabled == false)
            {
                // Start the timer
                timer.Enabled = true;

                // Update properties to pause properties
                startToolStripMenuItem.Image = Properties.Resources.pauseIcon;
                startToolStripMenuItem.Text = Properties.Resources.pause;
                startToolStripMenuItem1.Image = Properties.Resources.pauseIcon;
                startToolStripMenuItem1.Text = Properties.Resources.pause;
                toolStripButtonStart.Image = Properties.Resources.pauseIcon;
                toolStripButtonStart.ToolTipText = Properties.Resources.toolTipPause;

                // Update controls
                UpdateControls();
            }
            // Else the game is running, feel free to pause!
            else
            {
                // Pause
                Pause(sender, e);
            }
        }

        // Pause
        private void Pause(object sender = null, EventArgs e = null)
        {
            // Stop timer
            timer.Enabled = false;

            // Update properties to start properties
            startToolStripMenuItem.Image = Properties.Resources.startIcon;
            startToolStripMenuItem.Text = Properties.Resources.start;
            startToolStripMenuItem1.Image = Properties.Resources.startIcon;
            startToolStripMenuItem1.Text = Properties.Resources.start;
            toolStripButtonStart.Image = Properties.Resources.startIcon;
            toolStripButtonStart.ToolTipText = Properties.Resources.toolTipStart;

            // Update controls unless cell count is 0
            if (_cellCount != 0)
            {
                UpdateControls();
            }
            else
            {
                // Disable File > Start
                // The world is empty
                startToolStripMenuItem.Enabled = false;
            }
        }

        // Next
        private void Control_Next(object sender = null, EventArgs e = null)
        {
            // Update the controls before checking the cell count
            UpdateControls();

            // Because if there are 0 alive cells, you shouldn't be able to...
            if (_cellCount > 0)
            {
                // Pause or
                Pause(sender, e);

                // Step forward one generation
                NextGeneration();
            }
        }

        // Paint
        private void Control_Paint(object sender = null, EventArgs e = null)
        {
            // Set cursor to paint
            _draw = 0;

            // Update cursor style
            GraphicsPanel.Cursor = Cursors.Cross;

            // Update cursor properties
            toolStripButtonSingleClick.Checked = false;
            toolStripMenuItemSingleClick.Checked = false;
            toolStripButtonPaint.Checked = true;
            paintToolStripMenuItem.Checked = true;
            eraseToolStripMenuItem.Checked = false;
            toolStripButtonErase.Checked = false;

            // Tell windows to repaint panel
            GraphicsPanel.Invalidate();
        }

        // Erase
        private void Control_Erase(object sender = null, EventArgs e = null)
        {
            // Set cursor to erase
            _draw = 1;

            // Load custom eraser cursor
            Cursor eraser = new Cursor(Properties.Resources.eraser.GetHicon());

            // Update cursor
            GraphicsPanel.Cursor = eraser;

            // Update cursor properties
            toolStripButtonSingleClick.Checked = false;
            toolStripMenuItemSingleClick.Checked = false;
            toolStripButtonPaint.Checked = false;
            paintToolStripMenuItem.Checked = false;
            eraseToolStripMenuItem.Checked = true;
            toolStripButtonErase.Checked = true;

            // Tell windows to repaint panel
            GraphicsPanel.Invalidate();
        }

        // Single-Click
        private void Control_SingleClick(object sender = null, EventArgs e = null)
        {
            // Set cursor to single-click
            _draw = 2;

            // Update cursor style
            GraphicsPanel.Cursor = Cursors.Arrow;

            // Update cursor properties
            toolStripButtonSingleClick.Checked = true;
            toolStripMenuItemSingleClick.Checked = true;
            toolStripButtonPaint.Checked = false;
            paintToolStripMenuItem.Checked = false;
            eraseToolStripMenuItem.Checked = false;
            toolStripButtonErase.Checked = false;

            // Tell windows to repaint panel
            GraphicsPanel.Invalidate();
        }
        #endregion
    }
}
