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
    public partial class ModalDialog_Interval : Form
    {
        public decimal Interval
        {
            get
            {
                return numericUpDownInterval.Value;
            }
            set
            {
                numericUpDownInterval.Value = value;
            }
        }
        public ModalDialog_Interval()
        {
            InitializeComponent();
        }
    }
}
