using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CadastralManagementDataSync.Forms
{
    public partial class TextShowDialog : Form
    {
        public TextShowDialog()
        {
            InitializeComponent();
        }
        public void SetText(string text) {
            this.textBox1.Text = text;
        }
    }
}
