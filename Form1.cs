using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace proc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void butaction()
        {
            if ((textBox1.Text != "") && (!textBox1.Text.Contains("Enter Server Name to Get Process")))
            {
                string comp = textBox1.Text.ToString();


                try
                {
                    using (ManagementObjectSearcher sc = new ManagementObjectSearcher("\\\\" + comp + "\\root\\cimv2", "select * from win32_process"))
                    {
                        ManagementObjectCollection coll = sc.Get();
                        foreach (ManagementObject obj in coll)
                        {
                            dataGridView1.Rows.Add(obj["ProcessId"], obj["Name"], obj["CSName"]);
                        }
                    }         
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                MessageBox.Show("Enter Server Name please", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
            
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 1)
            {
                dataGridView1.Rows.Clear();
                System.Threading.Thread.Sleep(1000);
                butaction();
            }
            else
            {
                butaction();
            }
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("RightClick the PID and Select Close",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

  
                foreach (DataGridViewCell item in this.dataGridView1.SelectedCells)
            {
                // MessageBox.Show(item.Value.ToString());
                if (item.OwningColumn.Name.ToString().Contains("PID"))
                {
                    MessageBox.Show("Selected PID " + item.Value.ToString() + " will be closed", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    try
                    {
                        using (ManagementObjectSearcher sc1 = new ManagementObjectSearcher("\\\\" + textBox1.Text.ToString() + "\\root\\cimv2", "select * from win32_process where ProcessId=" + item.Value))
                        {

                            foreach (ManagementObject obj1 in sc1.Get())
                            {
                                object result = obj1.InvokeMethod("Terminate", null);
                                switch (Convert.ToInt16(result))
                                {
                                    case 0:
                                        MessageBox.Show("Selected PID " + item.Value.ToString() + "  closed and statuscode " + result, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        break;
                                    default:
                                        MessageBox.Show("Error and statuscode " + result, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        break;
                                }

                            }

                        }
                    }
                    catch(Exception ee)
                    {
                        MessageBox.Show(ee.Message.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Only PID should be selected", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

                    



            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 3;

            dataGridView1.Columns[0].Name = "PID";
            dataGridView1.Columns[1].Name = "ProcessName";
            dataGridView1.Columns[2].Name = "ServerName";
        }
    }
}
