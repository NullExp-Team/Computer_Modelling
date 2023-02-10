using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public class Pair<T, U>
        {
            public Pair()
            {
            }

            public Pair(T first, U second)
            {
                this.First = first;
                this.Second = second;
            }
            public T First { get; set; }
            public U Second { get; set; }
        };

        bool isBuilding = false;
        List<Pair<int, double>> inputList = new List<Pair<int, double>>();
        
        private void Clear(ZedGraphControl Zed_GraphControl)
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();

            zedGraphControl1.GraphPane.XAxis.Type = AxisType.Linear;
            zedGraphControl1.GraphPane.XAxis.Scale.TextLabels = null;
            zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.YAxis.MinorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.XAxis.MinorGrid.IsVisible = false;
            zedGraphControl1.RestoreScale(zedGraphControl1.GraphPane);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        PointPairList linFunk()
        {
            var logPoints = inputList.Select(p => new PointPair(p.First, p.Second)).ToList();

            int n = logPoints.Count;
            double sumX = logPoints.Sum(p => Math.Round(p.X, 4));
            double sumY = logPoints.Sum(p => Math.Round(p.Y, 4));
            double sumXX = logPoints.Sum(p => Math.Round(p.X * p.X, 4));
            double sumXY = logPoints.Sum(p => Math.Round(p.X * p.Y, 4));

            //double b = (n * sumXY - sumX * sumY) / (n * sumXX - sumX * sumX);
            //double a = (sumY - b * sumX) / n;

            double delta = Math.Round(sumXX * n - sumX * sumX, 4);
            double a = Math.Round((sumXY * n - sumY * sumX) / delta, 2);
            double b = Math.Round((sumXX * sumY - sumXY * sumX) / delta, 2);

            label6.Text = "A= " + Math.Round(a,2).ToString();
            label7.Text = "B= " + Math.Round(b,2).ToString();
            //y = a*x + b

            // рисуем полученную функцию
            int XOfInput = 0;
            double error = 0;
            PointPairList result = new PointPairList();
            for (int i = inputList[0].First; i <= inputList.Last().First; i += 1) 
            {
                //if (i >= 1 && a * i + b >= 1) 
                double x = Math.Round(a * i, 2);
                result.Add(i, Math.Round(x + b,2));

                //считаем ошибку
                if (XOfInput < inputList.Count && i == inputList[XOfInput].First) 
                {
                    error += Math.Pow(Math.Round(inputList[XOfInput].Second - (x + b),2), 2);
                    XOfInput++;
                }
            }
            label1.Text = "Ошибка для линнейной функции= "+Math.Round(error, 2).ToString();


            return result;
        }



        //Степенная функция
        PointPairList SecondFunk()
        {
            var logPoints = inputList.Select(p => new PointPair(Math.Log(p.First), Math.Log(p.Second))).ToList();

            int n = logPoints.Count;
            double sumX = logPoints.Sum(p => Math.Round(p.X, 4));
            double sumY = logPoints.Sum(p => Math.Round(p.Y,4));
            double sumXX = logPoints.Sum(p => Math.Round(p.X * p.X, 4));
            double sumXY = logPoints.Sum(p => Math.Round(p.X * p.Y,4));

            //double b = (n * sumXY - sumX * sumY) / (n * sumXX - sumX * sumX);
            //double a = (sumY - b * sumX) / n;

            double delta = Math.Round(sumXX * n - sumX * sumX,4);
            double a = Math.Round((sumXY * n - sumY * sumX) / delta,2);
            double b = Math.Round((sumXX * sumY - sumXY * sumX) / delta,2);
            label9.Text = "A= " + Math.Round(a, 2).ToString();
            label8.Text = "B= " + Math.Round(b, 2).ToString();

            int XOfInput = 0;
            double error = 0;
            double bb = Math.Round (Math.Exp(b), 2);
           
            PointPairList result = new PointPairList();
            for (int i = inputList[0].First; i <= inputList.Last().First; i += 1)
            {
                double xa = Math.Round(Math.Pow(i, a),2);
                //if (i >= 1 && bb * xa >= 1)
                    result.Add(i, bb * xa);

             
                if (XOfInput < inputList.Count && i == inputList[XOfInput].First)
                {
                    error += Math.Round(Math.Pow(inputList[XOfInput].Second - Math.Round(bb * xa, 2), 2), 2);
                    XOfInput++;
                }
            }
            label2.Text = "Ошибка для степенной функции= " + Math.Round(error, 2).ToString();

            return result;
        }

        //Степенная функция
        PointPairList ThirdFunk()
        {
            var logPoints = inputList.Select(p => new PointPair((p.First), (p.Second))).ToList();

            int n = logPoints.Count;
            double sumX = logPoints.Sum(p => Math.Round( p.X,4));
            double sumY = logPoints.Sum(p => Math.Round(Math.Log(p.Y),4));
            double sumXX = logPoints.Sum(p => Math.Round(p.X * p.X, 4));
            double sumXY = logPoints.Sum(p => Math.Round(p.X * Math.Log(p.Y),4));

            double delta = Math.Round(sumXX * n - sumX * sumX,2);
            double a = Math.Round((sumXY * n - sumY * sumX) / delta,2);
            double b = Math.Round((sumXX * sumY - sumXY * sumX) / delta,2);
            label12.Text = "A= " + Math.Round(a, 2).ToString();
            label11.Text = "B= " + Math.Round(b, 2).ToString();

            int XOfInput = 0;
            double error = 0;
            PointPairList result = new PointPairList();

            double bb = Math.Round(Math.Exp(b), 2);

            for (int i = inputList[0].First; i <= inputList.Last().First; i += 1)
            {
               // if (i >= 1 && b * Math.Exp(a * i) >= 1)
                    result.Add(i, bb * Math.Exp(a * i));

                if (XOfInput < inputList.Count && i == inputList[XOfInput].First)
                {
                    error += Math.Round(Math.Pow(inputList[XOfInput].Second - bb *  Math.Round(Math.Exp(a * i),4), 2),4);
                    XOfInput++;
                }
            }
            label3.Text = "Ошибка для показательной функции= " + Math.Round(error, 2).ToString();

            return result;
        }
        PointPairList FourthFunk()
        {
            var logPoints = inputList.Select(p => new PointPair((p.First), (p.Second))).ToList();

            double sumX = logPoints.Sum(p => Math.Round(p.X, 4));
            double sumY = logPoints.Sum(p => Math.Round(p.Y, 4));
            double sumXX = logPoints.Sum(p => Math.Round(p.X * p.X, 4));
            double sumXY = logPoints.Sum(p => Math.Round(p.X * p.Y, 4));
            double sumXXY = logPoints.Sum(p => Math.Round(p.X * p.X * p.Y, 4));

            double sumXXX = logPoints.Sum(p => Math.Round(p.X * p.X * p.X, 4));
            double sumXXXX = logPoints.Sum(p => Math.Round(p.X * p.X * p.X * p.X, 4));

           

             

          

             

           

          

            double delta = inputList.Count * sumXXXX * sumXX + sumXXX * sumX * sumXX + sumXXX * sumX * sumXX - Math.Pow(sumXX, 3) - Math.Pow(sumX, 2) * sumXXXX - Math.Pow((sumXXX), 2) * inputList.Count;          
            double a = (inputList.Count * sumXX * sumXXY + sumXY * sumX * sumXX + sumXXX * sumX * sumY - (Math.Pow(sumXX, 2)) * sumY - Math.Pow(sumX,2) * sumXXY - inputList.Count * sumXY * sumXXX) / delta;
            double b = (inputList.Count * sumXY * sumXXXX + sumXXX * sumY * sumXX + sumXXY * sumX * sumXX - Math.Pow(sumXX, 2) * sumXY - sumY * sumX * sumXXXX - inputList.Count * sumXXX * sumXXY) / delta;
            double c = (sumXXXX * sumXX * sumY + sumXXX * sumXY * sumXX + sumXXX * sumX * sumXXY - Math.Pow(sumXX, 2) * sumXXY - Math.Pow(sumXXX, 2) * sumY - sumX * sumXY * sumXXXX) / delta;
            label15.Text = "A= " + Math.Round(a, 2).ToString();
            label14.Text = "B= " + Math.Round(b, 2).ToString();
            label17.Text = "C= " + Math.Round(c, 2).ToString();

            int XOfInput = 0;
            double error = 0;
            PointPairList result = new PointPairList();
            for (int i = inputList[0].First; i <= inputList.Last().First; i += 1)
            {
                //if (i >= 1 && a * i * i + b * i + c >= 1)
                    result.Add(i, a * i*i + b * i + c);

                if (XOfInput < inputList.Count && i == inputList[XOfInput].First)
                {
                    error += Math.Pow(inputList[XOfInput].Second - (a * i*i + b * i + c), 2);
                    XOfInput++;
                }
            }
            label4.Text = "Ошибка для квадратичной функции= " + Math.Round(error, 2).ToString();

            return result;
        }

        public void build(ZedGraphControl Zed_GraphControl)
        {
           if (!isBuilding)
                return;

            PointPairList startLine = new PointPairList();
            for (int i = 0; i < inputList.Count; i++)
            { 
               startLine.Add(inputList[i].First, inputList[i].Second);
            }

            PointPairList lin = linFunk();
            PointPairList step = SecondFunk();
            PointPairList exp = ThirdFunk();
            PointPairList chlin = FourthFunk();

            GraphPane my_Pane = Zed_GraphControl.GraphPane;
            LineItem myCircle1 = my_Pane.AddCurve("6 начальных точек", startLine, Color.Green, SymbolType.Diamond);
            myCircle1.Line.IsVisible = false;
            myCircle1.Symbol.Size = 6;
            LineItem myCircle2 = my_Pane.AddCurve("Линейная функция", lin, Color.Blue, SymbolType.None);
            LineItem myCircle3 = my_Pane.AddCurve("Степенная функция", step, Color.Orange, SymbolType.None);
            LineItem myCircle4 = my_Pane.AddCurve("Показательная функция", exp, Color.DarkViolet, SymbolType.None);
            LineItem myCircle5 = my_Pane.AddCurve("Квадратичная функция", chlin, Color.Red, SymbolType.None);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Clear(zedGraphControl1);
            try
            {
               
                inputList.Clear();

                //inputList.Add(new Pair<int, double>(1, 1.0));
                //inputList.Add(new Pair<int, double>(2, 1.5));
                //inputList.Add(new Pair<int, double>(3, 3.0));
                //inputList.Add(new Pair<int, double>(4, 4.5));
                //inputList.Add(new Pair<int, double>(5, 7.0));
                //inputList.Add(new Pair<int, double>(6, 8.5));

                inputList.Add(new Pair<int, double>(2, 2.8));
                inputList.Add(new Pair<int, double>(3, 2.4));
                inputList.Add(new Pair<int, double>(4, 2.0));
                inputList.Add(new Pair<int, double>(5, 1.5));
                inputList.Add(new Pair<int, double>(6, 1.3));
                inputList.Add(new Pair<int, double>(7, 1.2));

                isBuilding = true;
                build(zedGraphControl1);
                return;
            }
            catch { return; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
