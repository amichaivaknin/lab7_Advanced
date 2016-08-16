using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundPrimeCalc
{
    public partial class Form1 : Form
    {
        private static AutoResetEvent _cancelEvent = new AutoResetEvent(false);

        public Form1()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            calculatebutton.Enabled = false;
            var argument = e.Argument as int[];
            if (argument != null)
            {
                CalcPrimes(argument[0], argument[1]);
            }
            else
            {
                calculatebutton.Invoke(new Action(() => calculatebutton.Enabled = true));
            }
        }

        private void calculatebutton_Click(object sender, EventArgs e)
        {
            int n1, n2;
            if (!int.TryParse(textBox1.Text, out n1) || !int.TryParse(textBox2.Text, out n2)
                || (n1 < 0) || (n2 < n1))
            {
                return;
            }
            progressBar1.Maximum = n2 - n1;
            calculatebutton.Enabled = false;
            listBox1.Items.Clear();
            backgroundWorker1.RunWorkerAsync(new[] {n1, n2});
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            _cancelEvent.Set();
            progressBar1.Value = 0;
        }

        private void CalcPrimes(int n1, int n2)
        { 
            var list = new List<int>();
            
            for (var i = n1; i <= n2; i++)
            {
                if (_cancelEvent.WaitOne(0))
                {
                    calculatebutton.Invoke(new Action(() => calculatebutton.Enabled = true));
                    return;
                }
                Thread.Sleep(1000);
                if (IsPrime(i))
                {
                    list.Add(i);
                }
                progressBar1.Invoke(new Action(delegate { progressBar1.PerformStep(); }));
            }
            foreach (var number in list)
            {
                listBox1.Invoke(new Action(delegate { listBox1.Items.Add(number); }));
            }

            calculatebutton.Invoke(new Action(() => calculatebutton.Enabled = true));
            progressBar1.Invoke(new Action(delegate { progressBar1.Value = 0; }));
        }

        private static bool IsPrime(int i)
        {
            if (i == 0 || i == 1)
            {
                return false;
            }

            if (i == 2 || i == 3)
            {
                return true;
            }

            for (var j = 2; j <= Math.Sqrt(i); j++)
            {
                if (i%j == 0)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
