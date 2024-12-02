using API_Monitor.Properties;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Windows.Forms;
using WebSocketSharp;

namespace API_Monitor
{
    public partial class Form1 : Form
    {
        WebSocket ws;
        public int nTotal, nChecked, nError;
        public Form1()
        {
            InitializeComponent();

            // Subscribe to the Form_Load event
            this.Load += Form1_Load;
        }

        // Initialize WebSocket in Form_Load event
        private void Form1_Load(object sender, EventArgs e)
        {

            getAPI_Status();

            // Initialize the WebSocket connection
            ws = new WebSocket("ws://3.34.251.227:3000");

            // Set up event handlers for the WebSocket
            ws.OnMessage += (s, eArgs) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        API_Response aPI_Response = JsonConvert.DeserializeObject<API_Response>(eArgs.Data);
                        string control_name = aPI_Response.shop_type.ToLower() + "_" + aPI_Response.api_type[0].ToString().ToLower() + aPI_Response.api_type.Substring(1);
                        PictureBox control = (PictureBox)this.Controls.Find(control_name, true)[0];

                        if (aPI_Response.status)
                        {
                            control.Image = Resources.green;
                            control.Tag = "green";
                        }
                        else
                        {
                            control.Image = Resources.red;
                            control.Tag = "red";
                        }

                        Console.WriteLine("Received from server: " + eArgs.Data);
                        // Example: You can update a TextBox or ListBox with the received data
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Data type error!");
                    }

                });
            };

            ws.OnOpen += (s, eArgs) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Console.WriteLine("Connected to server");
                    ws.Send("Hello from C# client!");
                });
            };

            ws.OnClose += (s, eArgs) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Console.WriteLine("Disconnected from server");
                    baemin_getRiderDistance.Image = Resources.gray;
                });
            };

            ws.OnError += (s, eArgs) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Console.WriteLine("Error: " + eArgs.Message);
                });
            };

            // Connect to the WebSocket server
            ws.Connect();
        }

        // Ensure to close the WebSocket when the form is closing
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (ws != null && ws.IsAlive)
            {
                ws.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            getAPI_Status();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ws.Close();
            ws.Connect();
        }

        public void getAPI_Status()
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control is PictureBox)
                    {
                        if (((PictureBox)control).Tag?.ToString() == "green" || ((PictureBox)control).Tag?.ToString() == "red") nChecked++;
                        if(((PictureBox)control).Tag?.ToString() == "red") nError++;
                        nTotal++;
                    }
                }
            }
            nTotal_API.Text = nTotal.ToString();
            nChecked_API.Text = nChecked.ToString();
            nError_API.Text = nError.ToString();
            nTotal = nChecked = nError = 0;
        }

    }
}
