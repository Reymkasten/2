using System;
using System.Windows.Forms;

namespace Arenda
{
    public partial class Reg : Form
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public Reg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                Username = textBox1.Text;
                Password = textBox2.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
