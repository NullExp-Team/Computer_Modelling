using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LR6
{
    public partial class Form1 : Form
    {

        bool isWorking = false;

        ComputingSystem system;

        ComputingSystemSettings parseSettings()
        {
            try
            {
                return new ComputingSystemSettings(
                         Convert.ToDouble(textBox1.Text), // время между заданий
                         Convert.ToDouble(textBox2.Text), // погрешность заданий
                         Convert.ToDouble(textBox4.Text), // вероятность в 1-ю
                         Convert.ToDouble(textBox3.Text), // вероятность во 2-ю
                         Convert.ToDouble(textBox7.Text), // время работы 1-й
                         Convert.ToDouble(textBox5.Text), // погрешность 1-й
                         Convert.ToDouble(textBox9.Text), // время работы 2-й
                         Convert.ToDouble(textBox8.Text), // погрешность 2-й
                         Convert.ToDouble(textBox11.Text), // время работы 3-й
                         Convert.ToDouble(textBox10.Text), // погрешность 3-й
                         Convert.ToInt32(textBox13.Text), // кол-во заданий
                         Convert.ToDouble(textBox6.Text), // вероятность перехода во 2-ю
                         Convert.ToDouble(textBox14.Text) // время за шаг
                   );
            }
            catch (Exception e)
            { 
                MessageBox.Show("Неправильный формат данных!");
                throw e;
            }

           
        }

        void Restart()
        {
            system = new ComputingSystem(parseSettings());
        }

       
        public Form1()
        {
            InitializeComponent();

            var settings = parseSettings();

            system = new ComputingSystem(settings);
                     
            label16.Text = system.ToStringGeneralSett();
            label16.Font = new Font("Arial", 12);

            label18.Text = system.ToStringAVM();
            label18.Font = new Font("Arial", 12);

            label19.Text = system.ToStringSys();
            label19.Font = new Font("Arial", 12);
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
            Restart();

            label16.Text = system.ToStringGeneralSett();
            label18.Text = system.ToStringAVM();
            label19.Text = system.ToStringSys();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                var settings = parseSettings();

                system.loadNewParameters(settings);

                timer1.Interval = 1000 / Convert.ToInt32(textBox12.Text);

            }
            catch
            {
                MessageBox.Show("Неправильный формат данных!");
            }
        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            system.Process();

            if (system.completedTaskCount == system.settings.maxTasks)
            {
                timer1.Stop();
                button1.Text = "Запустить";
            }

            label16.Text = system.ToStringGeneralSett();
            label18.Text = system.ToStringAVM();
            label19.Text = system.ToStringSys();

            if (Convert.ToInt32(system.AllGetTime1()) == 0)
                progressBar1.Value = 0;
            else
                progressBar1.Value = Convert.ToInt32(system.AllGetTime1());

            if (Convert.ToInt32(system.AllGetTime2()) == 0)
                progressBar2.Value = 0;
            else
                progressBar2.Value = Convert.ToInt32(system.AllGetTime2());

            if (Convert.ToInt32(system.AllGetTime3()) == 0)
                progressBar3.Value = 0;
            else
                progressBar3.Value = Convert.ToInt32(system.AllGetTime3());

        }

        private void button3_Click(object sender, EventArgs e)
        {

            Restart();

            while(system.completedTaskCount < system.settings.maxTasks)
            {
                system.Process(0.1);
            }

            system.InstantlyFinish();

            label16.Text = system.ToStringGeneralSett();
            label18.Text = system.ToStringAVM();
            label19.Text = system.ToStringSys();
        }
        private void Computer_WorkTimeChanged(object sender, double newWorkTime)
        {
            // Обновляем значение ProgressBar на форме
            progressBar4.Value = (int)newWorkTime;
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
