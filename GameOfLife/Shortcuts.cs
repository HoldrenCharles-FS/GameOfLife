using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region Keyboard / MouseWheel
        // Enables zoom scaling with mouse wheel
        private void OnMouseWheel_Zoom(object sender, MouseEventArgs e)
        {
            // Scroll down (zoom out)
            if (e.Delta < 0)
            {
                UniverseGrow();
            }
            // Scroll up (zoom in)
            else
            {
                UniverseShrink();
            }
        }

        // Detects MouseDown on Graphics panel
        private void GraphicsPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // If cursor is not single-click
            if (_draw != 2)
            {
                _cursorMove = true;
            }
        }

        // Detects Mouse movement on Graphics panel
        private void GraphicsPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // If cursor movement is valid
            if (_cursorMove == true)
            {
                // Click when moving
                GraphicsPanel_MouseClick(sender, e);
            }
        }

        // Detects MouseUp on Graphics Panel
        private void GraphicsPanel_MouseUp(object sender, MouseEventArgs e)
        {
            // Invalidate cursor movement
            _cursorMove = false;
        }

        // For key detection within the application
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Enter = Generate
            if (e.KeyCode == Keys.Enter)
            {
                // So Enter doesn't make an alert noise
                e.SuppressKeyPress = true;

                // If focus is on seed box, parse seed, then generate
                if (toolStripTextBoxSeed.Focused == true)
                {
                    // Hide the seed parsing (seed parses when clicked outside of Seed Box)
                    _hideParse = true;
                    SeedBox_ParseSeed();
                    Randomize_Generate();
                    _hideParse = false;
                }
                // Else generate seed
                else
                {
                    // But only if the user entered something
                    if (toolStripTextBoxSeed.Text.Length > 0 && toolStripTextBoxSeed.Text != Properties.Resources.seedPrompt)
                    {
                        Randomize_Generate();
                    }

                }

                // Disable Generate
                generateToolStripMenuItem.Enabled = false;
                toolStripButtonGenerate.Enabled = false;

                // Change focus to Graphics panel
                GraphicsPanel.Focus();

                // Reset seed box style
                SeedBox_SetStyle(true);

            }

            // Don't register shortcuts when typing seed
            if (toolStripTextBoxSeed.Focused == false)
            {
                // Space = Start / Stop
                if (e.KeyCode == Keys.Space)
                {
                    Control_Start();
                }

                // Right Arrow = Next
                if (e.KeyCode == Keys.Right)
                {
                    Control_Next();
                }

                // Up Arrow = Zoom Out
                if (e.KeyCode == Keys.Up)
                {

                    UniverseGrow();
                }
                // Down Arrow = Zoom In
                if (e.KeyCode == Keys.Down)
                {
                    UniverseShrink();
                }

                // H = HUD
                if (e.KeyCode == Keys.H)
                {
                    View_HUD();
                }

                // N = Neighbor Count
                if (e.KeyCode == Keys.N)
                {
                    View_NeighborCount();
                }

                // G = Grid
                if (e.KeyCode == Keys.G)
                {
                    View_Grid();
                }

                // R = Random Seed
                if (e.KeyCode == Keys.R)
                {
                    Randomize_RandomSeed();
                }

                // E = Enter Seed
                if (e.KeyCode == Keys.E)
                {
                    Randomize_EnterSeed();
                }

                // B = Boundary change
                if (e.KeyCode == Keys.B)
                {
                    if (_boundary == true)
                    {
                        View_Finite();
                    }
                    else
                    {
                        View_Torodial();
                    }
                }

                // S = Speed
                if (e.KeyCode == Keys.S)
                {
                    Settings_Speed();
                }

                // Escape = File_Exit
                if (e.KeyCode == Keys.Escape)
                {
                    Application.Exit();
                }

                // 1 = Paint
                if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                {
                    Control_Paint();
                }

                // 2 = Erase
                if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                {
                    Control_Erase();
                }

                // 3 = Single-Click
                if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                {
                    Control_SingleClick();
                }

            }

            // Tell windows to repaint
            GraphicsPanel.Invalidate();
        }

        #endregion
    }
}
