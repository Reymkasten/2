using System;
using System.Windows.Forms;

namespace Arenda
{
    public partial class Karta : Form
    {
        public bool PaymentSuccessful { get; private set; }

        public Karta()
        {
            InitializeComponent();
            PaymentSuccessful = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) &&
                !string.IsNullOrEmpty(textBox2.Text) &&
                !string.IsNullOrEmpty(textBox3.Text))
            {
                PaymentSuccessful = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ведіть деталі карти повністю.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
