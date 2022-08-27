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

namespace DataDisplay
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            scanCOMPorts();
        }

        private void scanCOMPorts()
        {
            cmbCOMPort.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string p in ports)
            {
                cmbCOMPort.Items.Add(p);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            scanCOMPorts();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cmbCOMPort.Text; // COM名設定
                serialPort1.Open();                     // ポート接続
                btnOpen.Enabled = false;                // 接続　Off
                btnClose.Enabled = true;                // 切断　On
                btnScan.Enabled = false;                // 更新　Off
                cmbCOMPort.Enabled = false;             // COMリスト　Off
                //btnSend.Enabled = true;                 // 送信　On
                //tbxRxData.Clear();                      // 画面クリア
                //tbxRxData.AppendText("Connected\r\n");  // 接続と表示
            }
            catch
            {
                btnClose_Click(this, null);     // 切断ボタンを押す
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            btnOpen.Enabled = true;             // 接続　On
            btnClose.Enabled = false;           // 切断　Off
            btnScan.Enabled = true;             // 更新　On
            cmbCOMPort.Enabled = true;          // COMリスト　On
            //btnSend.Enabled = false;            // 送信　Off
            try
            {
                serialPort1.DiscardInBuffer();  // 入力バッファを破棄
                serialPort1.DiscardOutBuffer(); // 出力バッファを破棄
                serialPort1.Close();             // COMポートを閉じる
            }
            catch { };
        }

        delegate void SetTextCallback(string text);

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SetText(serialPort1.ReadExisting());
            }
            catch
            {
                btnClose_Click(this, null);
            }
        }

        private void SetText(string text)
        {
            if (tbxRxData.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text });
            }
            else
            {
                tbxRxData.AppendText(text);
            }
        }
    }
}
