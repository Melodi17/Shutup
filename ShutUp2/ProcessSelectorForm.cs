using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ShutUp2
{
    public partial class ProcessSelectorForm : Form
    {
        public Form1 f1;
        public ProcessSelectorForm(Form1 f1)
        {
            InitializeComponent();
            this.f1 = f1;
        }

        private void ProcessSelectorForm_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh(char e = ' ')
        {
            listBox1.Items.Clear();
            foreach (Process item in Process.GetProcesses())
            {
                if (item.ProcessName.ToLower().Contains(textBox1.Text.ToLower()) || textBox1.Text == "")
                {
                    listBox1.Items.Add(item.ProcessName);
                }
            }
            if (listBox1.Items.Count == 0)
            {
                listBox1.Items.Add(textBox1.Text.ToLower() + e.ToString().ToLower());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (listBox1.Items.Count != 0)
                {
                    textBox1.Text = listBox1.Items[0].ToString();
                }
            }
            else
            {
                Refresh(e.KeyChar);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                f1.listBox1.Items.Add(textBox1.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("Select Process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
