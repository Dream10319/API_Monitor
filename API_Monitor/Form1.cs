﻿using API_Monitor.Properties;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using WebSocketSharp;

namespace API_Monitor
{
    public partial class Form1 : Form
    {
        WebSocket ws;

        public Form1()
        {
            InitializeComponent();

            // Subscribe to the Form_Load event
            this.Load += Form1_Load;
        }

        // Initialize WebSocket in Form_Load event
        private void Form1_Load(object sender, EventArgs e)
        {
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

                        switch (aPI_Response.shop_type)
                        {
                            case "Baemin":
                                if (aPI_Response.api_type == "Login")
                                {
                                    if (aPI_Response.status) baemin_login_state.Image = Resources.green;
                                    else baemin_login_state.Image = Resources.red;
                                }
                                break;

                        }

                        Console.WriteLine("Received from server: " + eArgs.Data);
                        // Example: You can update a TextBox or ListBox with the received data
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Data type error!");
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
                    baemin_login_state.Image = Resources.gray;
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

        private void button1_Click(object sender, EventArgs e)
        {
            //ws.Send("Hello from C# client!");
            baemin_login_state.Image = Resources.red;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ws.Close();
            ws.Connect();
        }
    }
}
