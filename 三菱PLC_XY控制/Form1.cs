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
using System.Threading;

namespace 三菱PLC_XY控制
{
    public partial class Form1 : Form
    {
        private static SerialPort PLC_SerialPort = new SerialPort();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text=MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.OpenY("Y0"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenY("Y0"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.OpenY("Y1"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenY("Y1"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.CloseY("Y0"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseY("Y0"));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.ReadX("X0"));
            textBox2.Text = MitsubishiPLC_XYControl.ReadXStatus(PLC_SerialPort,"X0");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.ReadX("X1"));
            textBox2.Text = MitsubishiPLC_XYControl.ReadXStatus(PLC_SerialPort, "X1");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MitsubishiPLC_XYControl.OpenMitsubishiPLC(PLC_SerialPort, "COM4");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.CloseY("Y1"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseY("Y1"));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            char[] c = "123".ToCharArray();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.OpenY("Y2"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenY("Y2"));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.CloseY("Y2"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseY("Y2"));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.OpenS("S0"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenS("S0"));
            //Thread.Sleep(1100);
            //MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseS("S0"));
            //MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenY("Y2"));
            //MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenS("S1"));
            //Thread.Sleep(2200);
            //MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseY("Y2"));
            //MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseS("S1"));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.ReadS("S0"));
            textBox2.Text = MitsubishiPLC_XYControl.ReadSStatus(PLC_SerialPort, "S0");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.CloseS("S0"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseS("S0"));
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.OpenS("S1"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenS("S1"));
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.CloseS("S1"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseS("S1"));
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.ReadS("S1"));
            textBox2.Text = MitsubishiPLC_XYControl.ReadSStatus(PLC_SerialPort, "S1");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.OpenS("S15"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenS("S15"));
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.CloseS("S15"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseS("S15"));
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.ReadS("S15"));
            textBox2.Text = MitsubishiPLC_XYControl.ReadSStatus(PLC_SerialPort, "S15");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.OpenS("S3"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenS("S3"));
        }

        private void button19_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.CloseS("S3"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseS("S3"));
        }

        private void button20_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.ReadS("S3"));
            textBox2.Text = MitsubishiPLC_XYControl.ReadSStatus(PLC_SerialPort, "S3");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.OpenX("X0"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenX("X0"));
        }

        private void button25_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.OpenS("S4"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenS("S4"));
        }

        private void button23_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.CloseS("S4"));
            MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseS("S4"));
        }

        private void button24_Click(object sender, EventArgs e)
        {
            textBox1.Text = MitsubishiPLC_XYControl.byteToHexStr(MitsubishiPLC_XYControl.ReadS("S4"));
            textBox2.Text = MitsubishiPLC_XYControl.ReadSStatus(PLC_SerialPort, "S4");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            string address = textBox3.Text.Substring(0, 1);
            if (address == "Y")
            {
                MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenY(textBox3.Text));
            }
            else if (address=="X")
            {
                MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenX(textBox3.Text));
            }
            else if (address == "S")
            {
                MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.OpenS(textBox3.Text));
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            string address = textBox3.Text.Substring(0, 1);
            if (address == "Y")
            {
                MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseY(textBox3.Text));
            }
            else if (address == "S")
            {
                MitsubishiPLC_XYControl.SendControlCmd(PLC_SerialPort, MitsubishiPLC_XYControl.CloseS(textBox3.Text));
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string address = textBox3.Text.Substring(0, 1);
            if (address == "X")
            {
                textBox4.Text= MitsubishiPLC_XYControl.ReadXStatus(PLC_SerialPort, textBox3.Text);
            }
            else if (address == "S")
            {
                textBox4.Text = MitsubishiPLC_XYControl.ReadSStatus(PLC_SerialPort, textBox3.Text);
            }
        }
    }
}
