using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace MotorTestPC
{
    public partial class MotorCtrl : Form
    {
        List<string>  uartName =new List<string>();
        string[] sUartName; //= new string[10];
        DataTable dt = new DataTable("Current_Temperture");
        DataTable tmpDT = new DataTable("SaveTable");
        bool listeningFlag = false;
        bool dtCopyOk = false;
        bool displayTempertureLineFlag = false;
        long gTimeCnt = 0;
        long gSaveTimeCnt = 0;
        string fileName;

        public static void SaveCSV(DataTable dt, string fullPath)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(fullPath);
            if(!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            System.IO.FileStream fs = new System.IO.FileStream(fullPath,System.IO.FileMode.OpenOrCreate|System.IO.FileMode.Append,System.IO.FileAccess.Write);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if(i < dt.Columns.Count -1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);

            for (int i = 0; i < dt.Rows.Count; i++ )
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count;j++ )
                {
                    string str = dt.Rows[i][j].ToString();
                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
        }

        public MotorCtrl()
        {
            InitializeComponent();

            dt.Columns.Add("CurrentVale",typeof(Int16));
            dt.Columns.Add("TempertureValue",typeof(Int16));
            dt.Columns.Add("Date",System.Type.GetType("System.DateTime"));

            trackBarYmax.Value = Convert.ToInt32(chart1.ChartAreas[0].AxisY.Maximum);
            trackBarYlength.Value = Convert.ToInt32(chart1.ChartAreas[0].AxisY.Maximum - chart1.ChartAreas[0].AxisY.Minimum);
            textBox2.Text = trackBarYlength.Value.ToString();
            buttonStart.Enabled = false;
            trackBarYmax.Enabled = false;
            trackBarYlength.Enabled = false;
            textBox3.Enabled = false;
            sUartName = SerialPort.GetPortNames();
            if (sUartName.Length > 0)
            {
                comboBox1.DataSource = sUartName;
                comboBox1.SelectedIndex = 0;
                buttonStart.Enabled = true;
            }
            else
            {
                MessageBox.Show("没有发现串口设备！");
            }
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if ("开始" == buttonStart.Text)
            {
                if (radioButton1.Checked)
                {
                    if ("" == textBox3.Text)
                    {
                        MessageBox.Show("请输入数据保存名！！！");
                        return;
                    }
                    else
                    {
                        fileName = textBox3.Text;
                    }

                }

                serialPortOfSenser.PortName = comboBox1.SelectedItem.ToString();
                try
                {
                    timer1.Start();
                    timer2.Start();
                    serialPortOfSenser.Open();
                    serialPortOfSenser.Write("S");
                    buttonStart.Text = "停止";
                    button1.BackColor = Color.Red;
                    trackBarYmax.Enabled = true;
                    trackBarYlength.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (true == serialPortOfSenser.IsOpen)
                {

                }
                else
                {
                    MessageBox.Show(" 设备打开失败！");
                }            
            }
            else
            {
                try
                {
                    timer1.Stop();
                    timer2.Stop();
                    serialPortOfSenser.Write("P");
                    serialPortOfSenser.Close();
                    chart1.Series[0].Points.Clear();
                    chart1.Series[1].Points.Clear();
                    gTimeCnt = 0;
                    textBox3.Text = "";
                    buttonStart.Text = "开始";
                    button1.BackColor = Color.Green;
                    trackBarYmax.Enabled = false;
                    trackBarYlength.Enabled = false;
                    textBox3.Text = "";
                    textBox3.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
        }

        private void serialPortOfSenser_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (false == serialPortOfSenser.IsOpen && listeningFlag)
            {
                MessageBox.Show("请打开某个串口","错误提示");
                return;
            }
            byte[] ReDatas = new byte[6];
            serialPortOfSenser.Read(ReDatas,0,6);
            serialPortOfSenser.DiscardInBuffer();
            if (ReDatas[0] != 0xFF || ReDatas[1] != 0xFF)
            {
                return;
            }
            int iCurrentValue = 0;
            int tTempertureValue = 0;

            iCurrentValue = ReDatas[2] << 8;
            iCurrentValue += ReDatas[3];

            tTempertureValue = ReDatas[4] << 8;
            tTempertureValue += ReDatas[5];
            this.BeginInvoke(new EventHandler(delegate
            {
                chart1.Series[0].Points.AddXY(gTimeCnt, iCurrentValue);
                if(displayTempertureLineFlag)
                {
                    chart1.Series[1].Points.AddXY(gTimeCnt, tTempertureValue);
                }
                gTimeCnt++;

                if (gTimeCnt > 500)
                {
                    gTimeCnt = 0;
                    chart1.Series[0].Points.Clear();
                    if (displayTempertureLineFlag)
                    {
                        chart1.Series[1].Points.Clear();
                    }
                    
                }
            }));

            dt.Rows.Add(iCurrentValue, tTempertureValue, DateTime.Now);
            if (dt.Rows.Count > 10000)
            {
                tmpDT = dt.Copy();
                dtCopyOk = true;
                dt.Rows.Clear();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listeningFlag = true;
            long tempNumber = 0;
            if (dtCopyOk)
            {
                fileName = textBox3.Text;
                gSaveTimeCnt++;
                tempNumber = gSaveTimeCnt;
                fileName += "_" + tempNumber.ToString() + ".csv";
                this.BeginInvoke(new EventHandler(delegate
                {
                    SaveCSV(tmpDT,fileName);
                    tmpDT.Rows.Clear();
                    dtCopyOk = false;
                }));
            }

            listeningFlag = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                displayTempertureLineFlag = true;
            }
            else
            {
                displayTempertureLineFlag = false;
                chart1.Series[1].Points.Clear();
            }
        }

        private void trackBarYmax_ValueChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Maximum = trackBarYmax.Value;
            chart1.ChartAreas[0].AxisY.Minimum = trackBarYmax.Value - trackBarYlength.Value;
            textBox1.Text = trackBarYmax.Value.ToString();
        }

        private void trackBarYlength_ValueChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Maximum = trackBarYmax.Value;
            chart1.ChartAreas[0].AxisY.Minimum = trackBarYmax.Value - trackBarYlength.Value;
            textBox2.Text = trackBarYlength.Value.ToString();
        }


        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void trackBarYmax_Scroll(object sender, EventArgs e)
        {
            this.BeginInvoke(new EventHandler(delegate
            {
                textBox1.Text = trackBarYmax.Value.ToString();
            }));
        }

        private void trackBarYlength_Scroll(object sender, EventArgs e)
        {
            this.BeginInvoke(new EventHandler(delegate
            {
                textBox2.Text = trackBarYlength.Value.ToString();
            }));
        }
        private void groupBox2_MouseCaptureChanged(object sender, EventArgs e)
        {
            //this.BeginInvoke(new EventHandler(delegate
            //{
            //    textBox4.Text = trackBar1.Value.ToString();
            //}));
        }

        private void groupBox1_MouseCaptureChanged(object sender, EventArgs e)
        {
            //this.BeginInvoke(new EventHandler(delegate
            //{
            //    textBox1.Text = trackBarYmax.Value.ToString();
            //    textBox2.Text = trackBarYlength.Value.ToString();
            //})); 
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.BeginInvoke(new EventHandler(delegate
            {
                chart1.ChartAreas[0].AxisY.Maximum = trackBarYmax.Value;
                chart1.ChartAreas[0].AxisY.Minimum = trackBarYmax.Value - trackBarYlength.Value;

                chart1.ChartAreas[0].AxisY.Maximum = trackBarYmax.Value;
                chart1.ChartAreas[0].AxisY.Minimum = trackBarYmax.Value - trackBarYlength.Value;

                textBox1.Text = trackBarYmax.Value.ToString();
                textBox2.Text = trackBarYlength.Value.ToString();
            }));
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            radioButton1.AutoCheck = false;
            if (true == radioButton1.Checked)
            {
                radioButton1.Checked = false;
            }
            else
            {
                radioButton1.Checked = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox3.Enabled = true;
            }
            else
            {
                textBox3.Enabled = false;
            }
        }
    }
}
