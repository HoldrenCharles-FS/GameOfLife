using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region Seed Box

        // Sets the font style of the seed box
        private void SeedBox_SetStyle(bool defaultStyle = false)
        {
            // Remove default styling
            if (defaultStyle == false)
            {
                toolStripTextBoxSeed.ForeColor = Color.Black;
                toolStripTextBoxSeed.Font = new Font(toolStripTextBoxSeed.Font, FontStyle.Regular);
            }
            // Reset to default styling, update text to the seed prompt
            else
            {
                toolStripTextBoxSeed.Font = new Font(toolStripTextBoxSeed.Font, FontStyle.Italic);
                toolStripTextBoxSeed.ForeColor = Color.Gray;
                toolStripTextBoxSeed.Text = Properties.Resources.seedPrompt;

                // Disable Generate
                generateToolStripMenuItem.Enabled = false;
                toolStripButtonGenerate.Enabled = false;
            }
        }

        // Parses seed value inside seed box
        private void SeedBox_ParseSeed(object sender = null, EventArgs e = null)
        {
            // Executes when the user has clicked away from text box

            // Used for parsing from text
            int stringSum = 0;


            // If the text box isn't blank, equal to 0, and is not the seed prompt
            if (toolStripTextBoxSeed.Text.Length > 0 && toolStripTextBoxSeed.Text != "0"
                && toolStripTextBoxSeed.Text != Properties.Resources.seedPrompt)
            {
                // If seed can be parsed, it will be in _seed
                if (!long.TryParse(toolStripTextBoxSeed.Text, out _seed))
                {
                    // Else generate seed from text

                    // Multipliers for stringSum
                    int multiplier1 = 1;
                    int multiplier2 = 2;

                    // Add each char as a number to stringSum
                    foreach (char letter in toolStripTextBoxSeed.Text)
                    {
                        // Multiply the int value of each char
                        // by 13 and by two incrementing multipliers

                        // This is to hit higher number ranges and
                        // prevent the letter values from adding up to the same number
                        stringSum += letter * 13 * multiplier1 * multiplier2;

                        // Increment multipliers
                        multiplier1++;
                        multiplier2++;
                    }

                    // Update seed
                    _seed = stringSum;

                    if (_hideParse == false)
                    {
                        toolStripTextBoxSeed.Text = Convert.ToString(_seed);
                    }

                }

                // Used to chop off a seed thats too long
                string maxSeed = null;

                // Used to update the seed if to low or high
                int tempSeed = 0;

                // If user entered 0, seed is blank
                if (_seed == 0)
                {
                    // Empty the universe
                    File_New(sender, e);
                }
                // Else if the seed exceeds the max or min value for the random class
                else if ((_seed > Int32.MaxValue) || (_seed < Int32.MinValue))
                {
                    // Parameter for substring method
                    int substringLength = 10;

                    // Increment for negative numbers
                    if ((_seed < Int32.MinValue))
                    {
                        substringLength++;
                    }

                    // Covert seed to string, limit to 10 characters
                    maxSeed = _seed.ToString().Substring(0, substringLength);

                    // Try to parse the seed within an int range
                    if (!int.TryParse(maxSeed, out tempSeed))
                    {
                        // If that didn't work, cut off another character
                        substringLength--;
                        maxSeed = _seed.ToString().Substring(0, substringLength);

                        // Parsing is safe now
                        tempSeed = Int32.Parse(maxSeed);
                    }

                    // Update seed
                    _seed = tempSeed;

                    if (_hideParse == false)
                    {
                        toolStripTextBoxSeed.Text = Convert.ToString(_seed);
                    }

                }
            }
            // Else the user entered nothing to parse
            else
            {
                // Reset the seed box style
                SeedBox_SetStyle(true);
            }

        }

        // When the seed box is clicked
        private void SeedBox_Click(object sender, EventArgs e)
        {
            // Empty the seed box on click

            // Enable Generate
            generateToolStripMenuItem.Enabled = true;
            toolStripButtonGenerate.Enabled = true;

            // Change font to regular / black
            SeedBox_SetStyle();

            // When focused
            if (toolStripTextBoxSeed.Focused == true)
            {
                // Empty text box
                toolStripTextBoxSeed.Text = "";
            }

        }

        private void toolStripButtonShrink_MouseDown(object sender, MouseEventArgs e)
        {
            if (growTimer.Enabled == true)
            {
                growTimer.Enabled = false;
            }
            shrinkTimer.Enabled = true;

        }

        private void toolStripButtonGrow_MouseDown(object sender, MouseEventArgs e)
        {
            if (shrinkTimer.Enabled == true)
            {
                shrinkTimer.Enabled = false;
            }
            growTimer.Enabled = true;

        }

        private void Process_ShrinkTimer_Tick(object sender, EventArgs e)
        {
            UniverseShrink();
        }

        private void Process_GrowTimer_Tick(object sender, EventArgs e)
        {
            UniverseGrow();
        }

        private void toolStripButtonShrink_MouseUp(object sender, MouseEventArgs e)
        {
            shrinkTimer.Enabled = false;
        }

        private void toolStripButtonGrow_MouseUp(object sender, MouseEventArgs e)
        {
            growTimer.Enabled = false;
        }

        private void toolStripButtonShrink_MouseLeave(object sender, EventArgs e)
        {
            shrinkTimer.Enabled = false;
        }

        private void toolStripButtonGrow_MouseLeave(object sender, EventArgs e)
        {
            growTimer.Enabled = false;
        }
        #endregion
    }
}
