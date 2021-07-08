using System;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Game : Form
    {
        #region Help
        // About
        private void Help_About(object sender, EventArgs e)
        {
            // Show dialog
            AboutBox dlg = new AboutBox();
            dlg.ShowDialog();
        }

        #endregion
    }
}
