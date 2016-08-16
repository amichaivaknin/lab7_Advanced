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

namespace PrimesCalculator
{
    public partial class Form1 : Form
    {
        private static AutoResetEvent _cancelEvent = new AutoResetEvent(false);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n1, n2;
            if (!int.TryParse(num1.Text, out n1)|| !int.TryParse(num2.Text, out n2)
                ||(n1<0)||(n2<n1))
            {
                return;
            }

            calcButton.Enabled = false;
            resultsList.Items.Clear();

            var t1=new Thread(delegate() { CalcPrimes(n1, n2); });
            t1.Start();
        }

        private void CalcPrimes(int n1, int n2)
        {
            var list= new List<int>();
            for (var i = n1; i <= n2; i++)
            {
                if (_cancelEvent.WaitOne(0))
                {
                    calcButton.Invoke(new Action(() => calcButton.Enabled = true));
                    return;
                }
                Thread.Sleep(1000);
                if (IsPrime(i))
                {
                  list.Add(i); 
                }
            }

            foreach (var number in list)
            {
                resultsList.Invoke(new Action(delegate { resultsList.Items.Add(number); }));
            }

            calcButton.Invoke(new Action(()=>calcButton.Enabled=true));
        }

        private static bool IsPrime(int i)
        {
            if (i==0 || i==1)
            {
                return false;
            }

            if (i==2||i==3)
            {
                return true;
            }

            for (var j = 2; j <= Math.Sqrt(i); j++)
            {
                if (i%j==0)
                {
                    return false;
                }
            }

            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _cancelEvent.Set();
        }
    }
}
