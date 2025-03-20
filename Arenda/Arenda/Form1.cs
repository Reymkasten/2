using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arenda
{
    public partial class Form1 : Form
    {
        private FlowLayoutPanel flowLayoutPanel;
        private List<Apartment> apartments = new List<Apartment>();
        private Dictionary<string, string> users = new Dictionary<string, string>();
        private bool isLoggedIn = false;
        private string loggedInUsername;

        public Form1()
        {
            InitializeComponent();
            InitializeFlowLayoutPanel();
        }

        private void InitializeFlowLayoutPanel()
        {
            flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(flowLayoutPanel);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var loginForm = new Loga();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                if (users.TryGetValue(loginForm.Username, out var password) && password == loginForm.Password)
                {
                    isLoggedIn = true;
                    loggedInUsername = loginForm.Username;
                    MessageBox.Show("Вхід успішний.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Неправильне ім'я користувача або пароль.", "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new Reg();
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                if (!users.ContainsKey(registerForm.Username))
                {
                    users.Add(registerForm.Username, registerForm.Password);
                    isLoggedIn = true;
                    loggedInUsername = registerForm.Username;
                    MessageBox.Show("Реєстрація успішна. Ви увійшли в систему.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ім'я користувача вже існує.", "Помилка реєстрації", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddApartment_Click(object sender, EventArgs e)
        {
            if (!isLoggedIn)
            {
                MessageBox.Show("Будь ласка, увійдіть спочатку.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var form = new Apartaddfrm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                apartments.Add(form.Apartment);
                AddApartmentToUI(form.Apartment);
            }
        }

        private async void AddApartmentToUI(Apartment apartment)
        {
            var groupBox = new GroupBox
            {
                Text = apartment.Address,
                Width = 250,
                Height = 300,
                Padding = new Padding(10)
            };

            var pictureBox = new PictureBox
            {
                Width = 200,
                Height = 150,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = await LoadImageAsync(apartment.ImagePath)
            };

            var labal = new Label
            {
                Text = $"Кімнат: {apartment.Rooms}\nПлоща: {apartment.Area} м²\nЦіна: {apartment.RentPrice:C}\nСтатус: {apartment.Status}",
                AutoSize = true
            };

            var rent = new Button
            {
                Text = "Зайняти",
                Width = 200,
                Enabled = isLoggedIn
            };
            rent.Click += (s, e) => RentApartment(apartment, labal);

            groupBox.Controls.Add(pictureBox);
            groupBox.Controls.Add(labal);
            groupBox.Controls.Add(rent);
            pictureBox.Top = 20;
            pictureBox.Left = 25;
            labal.Top = pictureBox.Bottom + 10;
            labal.Left = 10;
            rent.Top = labal.Bottom + 10;
            rent.Left = 25;
            flowLayoutPanel.Controls.Add(groupBox);
        }

        private void RentApartment(Apartment apartment, Label lblInfo)
        {
            if (!isLoggedIn)
            {
                MessageBox.Show("Будь ласка залогіньтесь.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var paymentForm = new Karta();
            if (paymentForm.ShowDialog() == DialogResult.OK && paymentForm.PaymentSuccessful)
            {
                apartment.Status = "Зайнята";
                lblInfo.Text = $"Кімнат: {apartment.Rooms}\nПлоща: {apartment.Area} м²\nЦіна: {apartment.RentPrice:C}\nСтатус: {apartment.Status}";
            }
        }

        private async Task<Image> LoadImageAsync(string imagePath)
        {
            if (Uri.IsWellFormedUriString(imagePath, UriKind.Absolute))
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        var response = await client.GetAsync(imagePath);
                        response.EnsureSuccessStatusCode();
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            return Image.FromStream(stream);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Помилка завантаження.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
            }
            else if (System.IO.File.Exists(imagePath))
            {
                return Image.FromFile(imagePath);
            }
            else
            {
                MessageBox.Show("Неправильний шлях.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }

    public class Apartment
    {
        public string Address { get; set; }
        public int Rooms { get; set; }
        public double Area { get; set; }
        public decimal RentPrice { get; set; }
        public string Status { get; set; } = "Вільна";
        public string ImagePath { get; set; }
    }
}
