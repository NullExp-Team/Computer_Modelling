using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR6
{
    public partial class Form1 : Form
    {

        bool isWorking = false;

        ComputingSystem system;

        public Form1()
        {
            InitializeComponent();

            system = new ComputingSystem(
                   new ComputingSystemSettings(
                         Convert.ToInt32(textBox1.Text), // время между заданий
                         Convert.ToInt32(textBox2.Text), // погрешность заданий
                         Convert.ToDouble(textBox4.Text), // вероятность в 1-ю
                         Convert.ToDouble(textBox3.Text), // вероятность во 2-ю
                         Convert.ToInt32(textBox7.Text), // время работы 1-й
                         Convert.ToInt32(textBox5.Text), // погрешность 1-й
                         Convert.ToInt32(textBox9.Text), // время работы 2-й
                         Convert.ToInt32(textBox8.Text), // погрешность 2-й
                         Convert.ToInt32(textBox11.Text), // время работы 3-й
                         Convert.ToInt32(textBox10.Text), // погрешность 3-й
                         Convert.ToInt32(textBox13.Text), // кол-во заданий
                         Convert.ToDouble(textBox6.Text) // вероятность перехода во 2-ю
                   )
              );

            label16.Text = system.ToString();



        }




        private void button1_Click(object sender, EventArgs e)
        {
            if (!isWorking)
            {
                timer1.Start();
                button1.Text = "Остановить";
            }
            else
            {
                timer1.Stop();
                button1.Text = "Запустить";
            }

            isWorking = !isWorking;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            system.Restart();
            label16.Text = system.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //loadNewParameters()
            //привяжу, когда будет модель готова
            try
            {
                var settings =
                      new ComputingSystemSettings(
                            Convert.ToInt32(textBox1.Text), // время между заданий
                         Convert.ToInt32(textBox2.Text), // погрешность заданий
                         Convert.ToDouble(textBox4.Text), // вероятность в 1-ю
                         Convert.ToDouble(textBox3.Text), // вероятность во 2-ю
                         Convert.ToInt32(textBox7.Text), // время работы 1-й
                         Convert.ToInt32(textBox5.Text), // погрешность 1-й
                         Convert.ToInt32(textBox9.Text), // время работы 2-й
                         Convert.ToInt32(textBox8.Text), // погрешность 2-й
                         Convert.ToInt32(textBox11.Text), // время работы 3-й
                         Convert.ToInt32(textBox10.Text), // погрешность 3-й
                         Convert.ToInt32(textBox13.Text), // кол-во заданий
                         Convert.ToDouble(textBox6.Text) // вероятность перехода во 2-ю
                      );

                system.loadNewParameters(settings);

                //


                timer1.Interval = 1000 / Convert.ToInt32(textBox12.Text);

            }
            catch
            {
                MessageBox.Show("Неправильный формат данных!");
            }
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            system.Process(0.1);

            if(system.completedTaskCount == system.settings.maxTasks)
            {
                timer1.Stop();
                button1.Text = "Запустить";
            }

            label16.Text = system.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            system.InstantlyFinish();

            label16.Text = system.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
    }
}
