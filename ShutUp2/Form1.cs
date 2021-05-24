using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniParser.Model;
using IniParser;
using System.IO;

namespace ShutUp2
{
    public partial class Form1 : Form
    {
        public bool paused = false;
        bool running = File.Exists("config.d") ? File.ReadAllLines("config.d")[0].ToLower() == "true" : false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Save();
            Process.GetCurrentProcess().Kill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Color red = Color.FromArgb(255, 105, 97);
            Color green = Color.FromArgb(62, 180, 137);

            running = !running;
            button1.BackColor = running ? green : red;
            button1.Text = running ? "Turn Shut Up off" : "Turn Shut Up on";

            if (running) KillLoop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("config.d"))
            {
                foreach (string item in File.ReadAllLines("config.d").Skip(1))
                {
                    listBox1.Items.Add(item);
                }
            }

            tabControl1.ItemSize = new Size((this.Width - 30) / 2, 25);
            Color red = Color.FromArgb(255, 105, 97);
            Color green = Color.FromArgb(62, 180, 137);

            button1.BackColor = running ? green : red;
            button1.Text = running ? "Turn Shut Up off" : "Turn Shut Up on";

            if (running) KillLoop();
        }

        private void KillLoop()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                while (running)
                {
                    if (!paused)
                    {
                        Thread.Sleep(500);
                        foreach (string item in listBox1.Items)
                        {
                            try
                            {
                                Process[] tabs = Process.GetProcessesByName(item);
                                foreach (Process tab in tabs)
                                {
                                    tab.Kill();
                                }
                            }
                            catch (Exception e)
                            {
                                new Thread(() =>
                                {
                                    Thread.CurrentThread.IsBackground = true;
                                    /* run your code here */
                                    MessageBox.Show("Unable to close " + item, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }).Start();
                            }
                        }
                    }
                }
            }).Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            paused = true;
            ProcessSelectorForm processSelectorForm = new ProcessSelectorForm(this);
            processSelectorForm.Show();
            paused = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                MessageBox.Show("Deleted Process", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Select a process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Save()
        {
            List<string> fl = new List<string>();
            fl.Add(running.ToString().ToLower());

            foreach (var item in listBox1.Items)
            {
                fl.Add(item.ToString());
            }

            File.WriteAllLines("config.d", fl.ToArray());
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size((this.Width - 30) / 2, 25);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
            e.Cancel = true;
            this.Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
