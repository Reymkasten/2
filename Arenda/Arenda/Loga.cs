using System;
using System.Windows.Forms;

namespace Arenda
{
    public partial class Loga : Form
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public Loga()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                Username = textBox1.Text;
                Password = textBox2.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter both username and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

