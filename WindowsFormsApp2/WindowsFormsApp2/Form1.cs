using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private List<(string Question, string Answer)> questions = new List<(string, string)>
        {
            ("What is the capital of Ukraine?", "kyiv"),
            ("Is it true that earth orbits around the sun?", "yes"),
            ("Who was the first person to land on the moon?", "neil armstrong")
        };
        private List<(string Question, string UserAnswer, string CorrectAnswer)> answerHistory = new List<(string, string, string)>();
        private int Index = 0;
        private int Answers = 0;
        private int Attempt = 0;
        private bool IsDragging = false;
        private Point location;
        private Control item;

        public Form1()
        {
            InitializeComponent();
            Form1_Load(null, null);
            Dragev();
        }

        private void Dragev()
        {
            foreach (Control control in this.Controls)
            {
                control.MouseDown += Control_MouseDown;
                control.MouseMove += Control_MouseMove;
                control.MouseUp += Control_MouseUp;
            }
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                IsDragging = true;
                item = sender as Control;
                location = e.Location;
                this.Cursor = Cursors.Hand;
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging && item != null)
            {
                item.Left += e.X - location.X;
                item.Top += e.Y - location.Y;
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            IsDragging = false;
            item = null;
            this.Cursor = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userAnswer = textBox1.Text.ToLower();
            if (userAnswer == questions[Index].Answer)
            {
                Answers++;
                label4.BackColor = Color.Green;
                answerHistory.Add((questions[Index].Question, userAnswer, questions[Index].Answer));
                Index++;
                Attempt = 0;
            }
            else
            {
                label4.BackColor = Color.Red;
                answerHistory.Add((questions[Index].Question, userAnswer, questions[Index].Answer));
                Attempt++;
            }
            if (Index < questions.Count)
            {
                Form1_Load(null, null);
            }
            else
            {
                MessageBox.Show("Quiz completed!");
                AnswerHistory();
                var result = MessageBox.Show("Do you want to start over?", "Restart", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Index = 0;
                    Answers = 0;
                    Attempt = 0;
                    label4.BackColor = Color.White;
                    answerHistory.Clear();
                    Form1_Load(null, null);
                }
                else
                {
                    Close();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Index = 0;
            Answers = 0;
            Attempt = 0;
            answerHistory.Clear();
            Form1_Load(null, null);
            label4.BackColor = Color.White;
        }
        private void AnswerHistory()
        {
            string history = "Answer History:\n";
            foreach (var entry in answerHistory)
            {
                history += $"Question: {entry.Question}\nYour Answer: {entry.UserAnswer}\nCorrect Answer: {entry.CorrectAnswer}\n\n";
            }
            MessageBox.Show(history, "Answer History");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Text = questions[Index].Question;
            label2.Text = $"Correct Answers: {Answers}";
            label3.Text = $"Attempts: {Attempt}";
            textBox1.Clear();
        }
    }
}
