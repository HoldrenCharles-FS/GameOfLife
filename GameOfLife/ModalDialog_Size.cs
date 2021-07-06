using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class ModalDialog_Size : Form
    {
        public decimal Rows
        {
            get
            {
                return numericUpDownWidth.Value;
            }
            set
            {
                numericUpDownWidth.Value = value;
            }
        }
        public decimal Columns
        {
            get
            {
                return numericUpDownHeight.Value;
            }
            set
            {
                numericUpDownHeight.Value = value;
            }
        }



        public ModalDialog_Size()
        {
            InitializeComponent();
        }
    }
}
