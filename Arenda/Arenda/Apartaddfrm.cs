using System;
using System.Windows.Forms;
using static System.Windows.Forms.MonthCalendar;

namespace Arenda
{
    public partial class Apartaddfrm : Form
    {
        public Apartment Apartment { get; private set; }

        public Apartaddfrm()
        {
            InitializeComponent();
            Apartment = new Apartment();
            button1.Click += BtnAdd_Click;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Apartment.Address = textBox4.Text;
                Apartment.Rooms = int.Parse(textBox1.Text);
                Apartment.Area = double.Parse(textBox2.Text);
                Apartment.RentPrice = decimal.Parse(textBox3.Text);
                Apartment.ImagePath = textBox6.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
