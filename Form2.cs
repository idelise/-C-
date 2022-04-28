using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Daoxian
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Add("±8√n");
            comboBox2.Items.Add("±5√n");
            comboBox2.Items.Add("±10√n");
            comboBox2.Items.Add("±16√n");
            comboBox2.Items.Add("±24√n");
            comboBox2.Items.Add("±40√n");
            comboBox1.Items.Add("1/60000");
            comboBox1.Items.Add("1/40000");
            comboBox1.Items.Add("1/14000");
            comboBox1.Items.Add("1/10000");
            comboBox1.Items.Add("1/6000");
            comboBox1.Items.Add("1/4000");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Program.K = 1;
            }
            else
            {
                Program.K = -1;
            }
            switch (comboBox1 .Text)
            {
                case "1/60000":
                    {
                        Program.Q = 1.0/ 60000;//不加1.0要被当成整型计算掉
                        break;

                    }
                case "1/40000":
                    {
                        Program.Q = 1.0 / 40000;
                        break;

                    }
                case "1/14000":
                    {
                        Program.Q = 1.0 / 14000;
                        break;

                    }
                case "1/10000":
                    {
                        Program.Q = 1.0 / 10000;
                        break;
                    }
               case "1/6000":
                    {
                        Program.Q = 1.0 / 6000;
                        break;

                    }
                case "1/4000":
                    {
                        Program.Q = 1.0 / 4000;
                        break;

                    }
            }
            switch (comboBox2.Text)
            {
                case "±8√n":
                    {
                        Program.P = 8;
                        break;

                    }
                case "±5√n":
                    {
                        Program.P  = 5;
                        break;

                    }
                case "±10√n":
                    {
                        Program.P = 10;
                        break;

                    }
                case "±16√n":
                    {
                        Program.P  = 16;
                        break;
                    }
                case "±24√n":
                    {
                        Program.P = 24;
                        break;

                    }
                case "±40√n":
                    {
                        Program.P = 40;
                        break;

                    }
            }

            Close();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
           
        }
    }
}
