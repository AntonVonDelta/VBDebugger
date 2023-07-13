using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VBDebugger.Debugger;
using System.Net;

namespace VBDebugger
{
    public partial class Form1 : Form
    {
        DebuggerClient debugger = new DebuggerClient(
            new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5050),
            (string message) => Console.WriteLine(message));

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await debugger.Attach();

            Console.WriteLine("Attached");
        }
    }
}
