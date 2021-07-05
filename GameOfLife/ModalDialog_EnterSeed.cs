using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class ModalDialog_EnterSeed : Form
    {
        public long Seed { get; set; }

        public ModalDialog_EnterSeed()
        {
            InitializeComponent();
            
        }

        // Sets the font style of the seed box
        public void SeedBox_SetStyle()
        {
            // Remove default styling
            if (Seed != 0)
            {
                modalTextBoxSeed.ForeColor = Color.Black;
                modalTextBoxSeed.Font = new Font(modalTextBoxSeed.Font, FontStyle.Regular);
                modalTextBoxSeed.Text = Seed.ToString();
            }
            // Reset to default styling, update text to the seed prompt
            else
            {
                modalTextBoxSeed.Font = new Font(modalTextBoxSeed.Font, FontStyle.Italic);
                modalTextBoxSeed.ForeColor = Color.Gray;
                modalTextBoxSeed.Text = Properties.Resources.seedPrompt;
            }
        }

        // Parses seed value inside seed box
        private void SeedBox_ParseSeed(object sender = null, EventArgs e = null)
        {
            // Executes when the user has clicked away from text box

            // Used for parsing from text
            int stringSum = 0;
            long seed = 0;

            // If the text box isn't blank, equal to 0, and is not the seed prompt
            if (modalTextBoxSeed.Text.Length > 0 && modalTextBoxSeed.Text != "0"
                && modalTextBoxSeed.Text != Properties.Resources.seedPrompt)
            {
                // If seed can be parsed, it will be in _seed
                if (!long.TryParse(modalTextBoxSeed.Text, out seed))
                {
                    // Else generate seed from text

                    // Multipliers for stringSum
                    int multiplier1 = 1;
                    int multiplier2 = 2;

                    // Add each char as a number to stringSum
                    foreach (char letter in modalTextBoxSeed.Text)
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
                    Seed = stringSum;
                    modalTextBoxSeed.Text = Convert.ToString(Seed);
                }

                // Used to chop off a seed thats too long
                string maxSeed = null;

                // Used to update the seed if to low or high
                int tempSeed = 0;

                // If user entered 0, seed is blank
                if (Seed != 0)
                {
                    // If the seed exceeds the max or min value for the random class
                    if ((Seed > Int32.MaxValue) || (Seed < Int32.MinValue))
                    {
                        // Parameter for substring method
                        int substringLength = 10;

                        // Increment for negative numbers
                        if ((Seed < Int32.MinValue))
                        {
                            substringLength++;
                        }

                        // Covert seed to string, limit to 10 characters
                        maxSeed = Seed.ToString().Substring(0, substringLength);

                        // Try to parse the seed within an int range
                        if (!int.TryParse(maxSeed, out tempSeed))
                        {
                            // If that didn't work, cut off another character
                            substringLength--;
                            maxSeed = Seed.ToString().Substring(0, substringLength);

                            // Parsing is safe now
                            tempSeed = Int32.Parse(maxSeed);
                        }

                        // Update seed
                        Seed = tempSeed;

                        

                    }

                }
                // Else the user entered nothing to parse
                else
                {
                    // Reset the seed box style
                    Seed = seed;
                }
            }

        }

        // When the seed box is clicked
        private void SeedBox_Click(object sender, EventArgs e)
        {
            // Empty the seed box on click

            // Change font to regular / black
            SeedBox_SetStyle();

            // When focused
            if (modalTextBoxSeed.Focused == true)
            {
                // Empty text box
                modalTextBoxSeed.Text = "";
            }

        }


        // Random Seed
        private void Randomize_RandomSeed(object sender = null, EventArgs e = null)
        {
            // Instantiate a random object
            Random rnd = new Random(); // <- Random seed from time (no parameters)

            // Generate random seed between all acceptable ranges
            Seed = rnd.Next(Int32.MinValue, Int32.MaxValue);

            // Pase seed into box
            SeedBox_ParseSeed();

            SeedBox_SetStyle();

            modalTextBoxSeed.Text = Seed.ToString();
        }
    }
}
