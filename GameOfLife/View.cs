using System;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region View
        // Toggle HUD
        private void View_HUD(object sender = null, EventArgs e = null)
        {
            // Update related field
            _displayHUD = !_displayHUD;

            // Toggle checked state
            hUDToolStripMenuItem.Checked = _displayHUD;
            hUDToolStripMenuItem1.Checked = _displayHUD;

            // Autosave
            Settings_Process_AutoSave();

            // Enable reset button
            resetToolStripMenuItem.Enabled = true;

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Initialize HUD
        private void InitializeHUD()
        {
            // Initialize checked state
            hUDToolStripMenuItem.Checked = _displayHUD;
            hUDToolStripMenuItem.Checked = _displayHUD;
        }

        // Toggle Neighbor Count
        private void View_NeighborCount(object sender = null, EventArgs e = null)
        {
            // Update related field
            _displayNeighbors = !_displayNeighbors;

            // Toggle checked state
            neighborCountToolStripMenuItem.Checked = _displayNeighbors;
            neighborCountToolStripMenuItem1.Checked = _displayNeighbors;

            // Autosave
            Settings_Process_AutoSave();

            // Enable reset button
            resetToolStripMenuItem.Enabled = true;

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Initialize Neighbor Count
        private void InitializeNeighborCount()
        {
            // Initialize checked state
            neighborCountToolStripMenuItem.Checked = _displayNeighbors;
            neighborCountToolStripMenuItem1.Checked = _displayNeighbors;
        }

        // Toggle Grid
        private void View_Grid(object sender = null, EventArgs e = null)
        {
            // Update related field
            _displayGrid = !_displayGrid;

            // Toggle checked state
            gridToolStripMenuItem.Checked = _displayGrid;
            gridToolStripMenuItem1.Checked = _displayGrid;

            // Enable / disable color buttons
            gridColorToolStripMenuItem.Enabled = _displayGrid;
            gridColorToolStripMenuItem1.Enabled = _displayGrid;
            gridX10ColorToolStripMenuItem.Enabled = _displayGrid;
            gridX10ColorToolStripMenuItem1.Enabled = _displayGrid;

            if (_displayGrid == false)
            {
                gridColorToolStripMenuItem.Image = null;
                gridColorToolStripMenuItem1.Image = null;
                gridX10ColorToolStripMenuItem.Image = null;
                gridX10ColorToolStripMenuItem1.Image = null;
            }
            else
            {
                InitializeColors();
            }

            // Autosave
            Settings_Process_AutoSave();

            // Enable reset button
            resetToolStripMenuItem.Enabled = true;

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Initialize Grid
        private void InitializeGrid()
        {
            // Initialize checked state
            _universeCopy = new bool[_universe.GetLength(0), _universe.GetLength(1)];
            gridToolStripMenuItem.Checked = _displayGrid;
            gridToolStripMenuItem1.Checked = _displayGrid;

            // Enable / disable color buttons
            gridColorToolStripMenuItem.Enabled = _displayGrid;
            gridColorToolStripMenuItem1.Enabled = _displayGrid;
            gridX10ColorToolStripMenuItem.Enabled = _displayGrid;
            gridX10ColorToolStripMenuItem1.Enabled = _displayGrid;
        }

        // Torodial
        private void View_Torodial(object sender = null, EventArgs e = null)
        {

            // If boundary is finite
            if (_boundary == false)
            {
                // Boudary becomes Torodial
                _boundary = true;

                // Toggle checked state
                torodialToolStripMenuItem.Checked = true;
                torodialToolStripMenuItem1.Checked = true;
                finiteToolStripMenuItem.Checked = false;
                finiteToolStripMenuItem1.Checked = false;

                // Update Status strip
                UpdateStatusStrip();
            }

            // Autosave
            Settings_Process_AutoSave();

            // Enable reset button
            resetToolStripMenuItem.Enabled = true;

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }

        // Initialize Boundary
        private void InitializeBoundary()
        {
            // Initialize checked state
            if (_boundary == true)
            {
                torodialToolStripMenuItem.Checked = true;
                torodialToolStripMenuItem1.Checked = true;
                finiteToolStripMenuItem.Checked = false;
                finiteToolStripMenuItem1.Checked = false;
            }
            else
            {
                torodialToolStripMenuItem.Checked = false;
                torodialToolStripMenuItem1.Checked = false;
                finiteToolStripMenuItem.Checked = true;
                finiteToolStripMenuItem1.Checked = true;
            }
        }

        // Finite
        private void View_Finite(object sender = null, EventArgs e = null)
        {

            // If boundary is torodial
            if (_boundary == true)
            {
                // Boudary becomes Finite
                _boundary = false;

                // Toggle checked state
                torodialToolStripMenuItem.Checked = false;
                torodialToolStripMenuItem1.Checked = false;
                finiteToolStripMenuItem.Checked = true;
                finiteToolStripMenuItem1.Checked = true;

                // Update Status strip
                UpdateStatusStrip();
            }

            // Autosave
            Settings_Process_AutoSave();

            // Enable reset button
            resetToolStripMenuItem.Enabled = true;

            // Tell Windows you need to repaint
            GraphicsPanel.Invalidate();
        }
        #endregion
    }
}
