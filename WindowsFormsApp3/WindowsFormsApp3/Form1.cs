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
        List<Pair<int, double>> pointList = new List<Pair<int, double>>();

        List<double> errors = new List<double>();

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
            var logPoints = pointList.Select(p => new PointPair(p.First, p.Second)).ToList();

            int n = logPoints.Count;
            double sumX = logPoints.Sum(p => Math.Round(p.X, 4));
            double sumY = logPoints.Sum(p => Math.Round(p.Y, 4));
            double sumXX = logPoints.Sum(p => Math.Round(p.X * p.X, 4));
            double sumXY = logPoints.Sum(p => Math.Round(p.X * p.Y, 4));


            double delta = Math.Round(sumXX * n - sumX * sumX, 4);
            double a = Math.Round((sumXY * n - sumY * sumX) / delta, 2);
            double b = Math.Round((sumXX * sumY - sumXY * sumX) / delta, 2);

            label6.Text = "A= " + Math.Round(a, 2).ToString();
            label7.Text = "B= " + Math.Round(b, 2).ToString();

           
            int xi = 0;
            double error = 0;
            PointPairList result = new PointPairList();
            for (int i = pointList[0].First; i <= pointList.Last().First; i += 1)
            {
                double x = Math.Round(a * i, 2);
                result.Add(i, Math.Round(x + b, 2));

      
                if (xi < pointList.Count && i == pointList[xi].First)
                {
                    error += Math.Pow(Math.Round(pointList[xi].Second - (x + b), 2), 2);
                    xi++;
                }
            }
            errors.Add(error);
            label1.Text = "Ошибка линнейной функции: " + Math.Round(error, 2).ToString();


            return result;
        }



        //Степенная функция
        PointPairList PowerFunk()
        {
            var logPoints = pointList.Select(p => new PointPair(Math.Log(p.First), Math.Log(p.Second))).ToList();

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
            label9.Text = "A= " + Math.Round(a, 2).ToString();
            label8.Text = "B= " + Math.Round(Math.Exp(b) , 2).ToString();

            int xi = 0;
            double error = 0;
            double bb = Math.Round(Math.Exp(b), 2);

            PointPairList result = new PointPairList();
            for (int i = pointList[0].First; i <= pointList.Last().First; i += 1)
            {
                double xa = Math.Round(Math.Pow(i, a), 2);

                result.Add(i, bb * xa);


                if (xi < pointList.Count && i == pointList[xi].First)
                {
                    error += Math.Round(Math.Pow(pointList[xi].Second - Math.Round(bb * xa, 2), 2), 2);
                    xi++;
                }
            }
            errors.Add(error);
            label2.Text = "Ошибка степенной функции: " + Math.Round(error, 2).ToString();

            return result;
        }

        //Степенная функция
        PointPairList ExpFunk()
        {
            var logPoints = pointList.Select(p => new PointPair((p.First), (p.Second))).ToList();

            int n = logPoints.Count;
            double sumX = logPoints.Sum(p => Math.Round(p.X, 4));
            double sumY = logPoints.Sum(p => Math.Round(Math.Log(p.Y), 4));
            double sumXX = logPoints.Sum(p => Math.Round(p.X * p.X, 4));
            double sumXY = logPoints.Sum(p => Math.Round(p.X * Math.Log(p.Y), 4));

            double delta = Math.Round(sumXX * n - sumX * sumX, 2);
            double a = Math.Round((sumXY * n - sumY * sumX) / delta, 2);
            double b = Math.Round((sumXX * sumY - sumXY * sumX) / delta, 2);
            label12.Text = "A= " + Math.Round(a, 2).ToString();
            label11.Text = "B= " + Math.Round(Math.Exp(b), 2).ToString();

            int xi = 0;
            double error = 0;
            PointPairList result = new PointPairList();

            double bb = Math.Round(Math.Exp(b), 2);

            for (int i = pointList[0].First; i <= pointList.Last().First; i += 1)
            {
                
                result.Add(i, bb * Math.Exp(a * i));

                if (xi < pointList.Count && i == pointList[xi].First)
                {
                    error += Math.Round(Math.Pow(pointList[xi].Second - bb * Math.Round(Math.Exp(a * i), 4), 2), 4);
                    xi++;
                }
            }
            errors.Add(error);
            label3.Text = "Ошибка показательной функции: " + Math.Round(error, 2).ToString();

            return result;
        }
        PointPairList FourthFunk()
        {
            var logPoints = pointList.Select(p => new PointPair((p.First), (p.Second))).ToList();

            double sumX = logPoints.Sum(p => Math.Round(p.X, 4));
            double sumY = logPoints.Sum(p => Math.Round(p.Y, 4));
            double sumXX = logPoints.Sum(p => Math.Round(p.X * p.X, 4));
            double sumXY = logPoints.Sum(p => Math.Round(p.X * p.Y, 4));
            double sumXXY = logPoints.Sum(p => Math.Round(p.X * p.X * p.Y, 4));

            double sumXXX = logPoints.Sum(p => Math.Round(p.X * p.X * p.X, 4));
            double sumXXXX = logPoints.Sum(p => Math.Round(p.X * p.X * p.X * p.X, 4));


            double delta = pointList.Count * sumXXXX * sumXX + sumXXX * sumX * sumXX + sumXXX * sumX * sumXX - Math.Pow(sumXX, 3) - Math.Pow(sumX, 2) * sumXXXX - Math.Pow((sumXXX), 2) * pointList.Count;
            double a = (pointList.Count * sumXX * sumXXY + sumXY * sumX * sumXX + sumXXX * sumX * sumY - (Math.Pow(sumXX, 2)) * sumY - Math.Pow(sumX, 2) * sumXXY - pointList.Count * sumXY * sumXXX) / delta;
            double b = (pointList.Count * sumXY * sumXXXX + sumXXX * sumY * sumXX + sumXXY * sumX * sumXX - Math.Pow(sumXX, 2) * sumXY - sumY * sumX * sumXXXX - pointList.Count * sumXXX * sumXXY) / delta;
            double c = (sumXXXX * sumXX * sumY + sumXXX * sumXY * sumXX + sumXXX * sumX * sumXXY - Math.Pow(sumXX, 2) * sumXXY - Math.Pow(sumXXX, 2) * sumY - sumX * sumXY * sumXXXX) / delta;
            
            label15.Text = "A= " + Math.Round(a, 2).ToString();
            label14.Text = "B= " + Math.Round(b, 2).ToString();
            label17.Text = "C= " + Math.Round(c, 2).ToString();

            int xi = 0;
            double error = 0;
            PointPairList result = new PointPairList();
            for (int i = pointList[0].First; i <= pointList.Last().First; i += 1)
            {
                
                result.Add(i, a * i * i + b * i + c);

                if (xi < pointList.Count && i == pointList[xi].First)
                {
                    error += Math.Pow(pointList[xi].Second - (a * i * i + b * i + c), 2);
                    xi++;
                }
            }
            errors.Add(error);
            label4.Text = "Ошибка квадратичной функции: " + Math.Round(error, 2).ToString();

            return result;
        }

        public void build(ZedGraphControl Zed_GraphControl)
        {
            if (!isBuilding)
                return;

            PointPairList startLine = new PointPairList();
            for (int i = 0; i < pointList.Count; i++)
            {
                startLine.Add(pointList[i].First, pointList[i].Second);
            }

            PointPairList lin = linFunk();
            PointPairList step = PowerFunk();
            PointPairList exp = ExpFunk();
            PointPairList chlin = FourthFunk();

            String str = "";

            int ind = errors.IndexOf(errors.Min());

            switch(ind)
            {
                case 0:
                    str = "Линейная";
                    break;
                case 1:
                    str = "Степенная";
                    break;
                case 2:
                    str = "Показательная";
                    break;
                case 3:
                    str = "Квадратичная";
                    break;
                default: str = "";
                    break;
            }
            label18.Text = "Минимальная ошибка: " + str;
            GraphPane my_Pane = Zed_GraphControl.GraphPane;
            LineItem myCircle1 = my_Pane.AddCurve("Эксперимент", startLine, Color.Blue, SymbolType.Diamond);
            myCircle1.Line.IsVisible = false;
            myCircle1.Symbol.Size = 6;
            LineItem myCircle2 = my_Pane.AddCurve("Линейная", lin, Color.Black, SymbolType.None);
            LineItem myCircle3 = my_Pane.AddCurve("Степенная", step, Color.Red, SymbolType.None);
            LineItem myCircle4 = my_Pane.AddCurve("Показательная", exp, Color.Orange, SymbolType.None);
            LineItem myCircle5 = my_Pane.AddCurve("Квадратичная", chlin, Color.Purple, SymbolType.None);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Clear(zedGraphControl1);
            try
            {

                pointList.Clear();

                pointList.Add(new Pair<int, double>(1, 1.0));
                pointList.Add(new Pair<int, double>(2, 1.5));
                pointList.Add(new Pair<int, double>(3, 3.0));
                pointList.Add(new Pair<int, double>(4, 4.5));
                pointList.Add(new Pair<int, double>(5, 7.0));
                pointList.Add(new Pair<int, double>(6, 8.5));

                //pointList.Add(new Pair<int, double>(2, 2.8));
                //pointList.Add(new Pair<int, double>(3, 2.4));
                //pointList.Add(new Pair<int, double>(4, 2.0));
                //pointList.Add(new Pair<int, double>(5, 1.5));
                //pointList.Add(new Pair<int, double>(6, 1.3));
                //pointList.Add(new Pair<int, double>(7, 1.2));

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
