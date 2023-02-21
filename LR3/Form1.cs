using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace LR3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Setup();
        }

        double error3;
        double error4;
        enum RungeKuttaOrder
        {
            thirdOrderAccuracy,
            fourthOrderAccuracy,
        }

        private void Setup()
        {
            GraphPane plane = zedGraphControl1.GraphPane;
            plane.Title.Text = "NullExp Lab3";
            plane.XAxis.Title.Text = "Ось X";
            plane.YAxis.Title.Text = "Ось Y";
        }

        private void Clear()
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();

            zedGraphControl1.GraphPane.XAxis.Type = AxisType.Linear;
            zedGraphControl1.GraphPane.XAxis.Scale.TextLabels = null;
            zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.YAxis.MinorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.XAxis.MinorGrid.IsVisible = false;


            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        /// Этот метод добавляет на график кривую с указанным заголовком, списком точек, цветом и типом символа.
        /// Если для параметра lastToFirst задано значение true и список точек содержит хотя бы одну точку, первая точка добавляется в конец списка.
        private void AddCurve(string title, PointPairList points, Color color, SymbolType symbolType = SymbolType.None, bool lastToFirst = false)
        {
            GraphPane plane = zedGraphControl1.GraphPane;

            PointPairList pointCloned = points.Clone();

            if (lastToFirst && pointCloned.Count() > 0) pointCloned.Add(pointCloned.First());

            plane.AddCurve(title, pointCloned, color, symbolType);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        /// Этот метод рисует функцию на графике, оценивая function func для диапазона значений x между xMin и xMax с шагом xStep.
        /// Если указаны yMin и yMax, на график будут добавлены только точки со значениями y в пределах этого диапазона.
        /// Название и цвет кривой задаются параметрами title и color соответственно.
        /// Если для параметра lastToFirst установлено значение true, первая и последняя точки будут соединены линией.
        private PointPair DrawFunction(Func<double, double> func, double xMin, double xMax, double? yMin, double? yMax, double xStep, string title, Color color, bool lastToFirst = false)
        {
            PointPairList list = new PointPairList();

            double maxX = Double.MinValue, maxY = Double.MinValue;

            for (double x = xMin; x <= xMax; x += xStep)
            {
                double y = func(x);
                if (yMin == null || y >= yMin) list.Add(x, y);
                if (yMax == null || y >= yMax) list.Add(x, y);
                maxX = list.Last().X > maxX ? list.Last().X : maxX;
                maxY = list.Last().Y > maxY ? list.Last().Y : maxY;
            }

            AddCurve(title, list, color, SymbolType.None, lastToFirst);

            return new PointPair(maxX, maxY);
        }

        /// Это перегрузка предыдущего метода DrawFunction, который не принимает параметры yMin и yMax и не фильтрует значения y.
        private PointPair DrawFunction(Func<double, double> func, double xMin, double xMax, double xStep, string title, Color color, bool lastToFirst = false)
        {
            return DrawFunction(func, xMin, xMax, null, null, xStep, title, color, lastToFirst);
        }

        private PointPair4 DrawPolarFunctionAndGetMaxPoint(Func<double, PointPair> func, double minFi, double maxFi, double fiStep,  string title, Color color, bool lastToFirst = false)
        {
            PointPairList list = new PointPairList();

            double maxX = Double.MinValue;
            double maxY = Double.MinValue;

            for (double fi = minFi; fi <= maxFi; fi += fiStep)
            {
                PointPair point = func(fi);
                list.Add(point.X, point.Y);
            }

            AddCurve(title, list, color, SymbolType.None, lastToFirst);

            return new PointPair4(maxX, maxY, 0, 0);

        }

        
        
        
        public delegate double Function(double x, double y);

        public delegate double DoubleFunction(double x, double y, double t);

        public delegate double RungeKuttaFunction(Function f, double x0, double y0, double x);

        public delegate PointPair DoubleRungeKuttaFunction(DoubleFunction f1, DoubleFunction f2, double x0, double y0, double t0, double t);

        //Рунге-Кутта 3 порядка
        public static double RungeKutta3(Function f, double x0, double y0, double x)
        {
            double xnew, ynew, k1, k2, k3, p1 = 1 / 3, p2 = 2 / 3, result = double.NaN;

            if (x == x0)
            {
                result = y0;
            }
            else if (x > x0)
            {
                do
                {
                    double h = x - x0;
                    k1 = h * f(x0, y0);
                    k2 = h * f(x0 + Math.Round(p1, 2) * h, y0 + Math.Round(p1, 2) * k1);
                    k3 = h * f(x0 + Math.Round(p2, 2) * h, y0 + Math.Round(p2, 2) * k2);
                    ynew = y0 + (k1 + 3 * k3) / 4;
                    xnew = x0 + h;
                    x0 = xnew;
                    y0 = ynew;
                } while (x0 < x);
                result = ynew;
            }
            return result;
        }

        //Рунге-Кутта 4 порядка
        public static double RungeKutta4(Function f, double x0, double y0, double x)
        {
            double xnew, ynew, k1, k2, k3, k4, result = double.NaN;

            if (x == x0)
                result = y0;

            else if (x > x0)
            {
                do
                {
                    double h = x - x0;
                    k1 = h * f(x0, y0);
                    k2 = h * f(x0 + 0.5 * h, y0 + 0.5 * k1);
                    k3 = h * f(x0 + 0.5 * h, y0 + 0.5 * k2);
                    k4 = h * f(x0 + h, y0 + k3);
                    ynew = y0 + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                    xnew = x0 + h;
                    x0 = xnew;
                    y0 = ynew;
                } while (x0 < x);
                result = ynew;
            }
            return result;
        }

        //Рунге-Кутта 3 порядка
        public static PointPair DoubleRungeKutta3(DoubleFunction f1, DoubleFunction f2, double x0, double y0, double t0, double t)
        {
            double xnew, ynew, l1, l2, l3, k1, k2, k3;

            if (t == t0)
            {
                return new PointPair(x0, y0);
            }
            else
            {
                double h = t - t0;
                k1 = h * f1(x0, y0, t0);
                l1 = h * f2(x0, y0, t0);
                k2 = h * f1(x0 + Math.Round(1.0 / 3.0, 2) * k1, y0 + Math.Round(1.0 / 3.0, 2) * l1, t0 + Math.Round(1.0 / 3.0, 2) * h);
                l2 = h * f2(x0 + Math.Round(1.0 / 3.0, 2) * k1, y0 + Math.Round(1.0 / 3.0, 2) * l1, t0 + Math.Round(1.0 / 3.0, 2) * h);
                k3 = h * f1(x0 + Math.Round(2.0 / 3.0, 2) * k2, y0 + Math.Round(2.0 / 3.0, 2) * l2, t0 + Math.Round(2.0 / 3.0, 2) * h);
                l3 = h * f2(x0 + Math.Round(2.0 / 3.0, 2) * k2, y0 + Math.Round(2.0 / 3.0, 2) * l2, t0 + Math.Round(2.0 / 3.0, 2) * h);
                xnew = x0 + (k1 + 3 * k3) / 4;
                ynew = y0 + (l1 + 3 * l3) / 4;
               
                return new PointPair(xnew, ynew);
            }
        }

        //Рунге-Кутта 4 порядка
        public static PointPair DoubleRungeKutta4(DoubleFunction f1, DoubleFunction f2, double x0, double y0, double t0, double t)
        {
            double xnew, ynew, l1, l2, l3, l4, k1, k2, k3, k4;

            if (t == t0)
            {
                return new PointPair(x0, y0);
            }
            else
            {
                double h = t - t0;
                k1 = h * f1(x0, y0, t0);
                l1 = h * f2(x0, y0, t0);
                k2 = h * f1(x0 + 0.5 * k1, y0 + 0.5 * l1, t0 + 0.5 * h);
                l2 = h * f2(x0 + 0.5 * k1, y0 + 0.5 * l1, t0 + 0.5 * h);
                k3 = h * f1(x0 + 0.5 * k2, y0 + 0.5 * l2, t0 + 0.5 * h);
                l3 = h * f2(x0 + 0.5 * k2, y0 + 0.5 * l2, t0 + 0.5 * h);
                k4 = h * f1(x0 + k3, y0 + l3, t0 + h);
                l4 = h * f2(x0 + k3, y0 + l3, t0 + h);
                xnew = x0 + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                ynew = y0 + (l1 + 2 * l2 + 2 * l3 + l4) / 6;
                
                return new PointPair(xnew, ynew);
            }
        }

        private void DrawDiffFunction(Function f, double x0, double y0, double h, double xMax, string title, Color color, RungeKuttaOrder orderAccuracy = RungeKuttaOrder.thirdOrderAccuracy)
        {
            PointPairList list = new PointPairList();

            RungeKuttaFunction choosenOrder = RungeKutta3;
            ref double error = ref error3;
            switch (orderAccuracy)
            {
                case RungeKuttaOrder.thirdOrderAccuracy:
                    choosenOrder = RungeKutta3;
                    error = ref error3;
                    break;
                case RungeKuttaOrder.fourthOrderAccuracy:
                    choosenOrder = RungeKutta4;
                    error = ref error4;
                    break;
            }

            double result = y0;
            error = 0;
            for (double x = x0; x < xMax; x+= h)
            {
                result = choosenOrder(f, x0, result, x);
                double realResult = f1(x);
                if (Math.Abs(realResult-result) > error)
                {
                    error = Math.Abs(realResult - result);
                }
                list.Add(new PointPair(x, result));
                x0 = x;
            }

            AddCurve(title, list, color);
        }

        private void DrawDoubleDiffFunction(DoubleFunction f1, DoubleFunction f2, double x0, double y0, double h, double xMax, string title, Color color, RungeKuttaOrder orderAccuracy, double t0)
        {
            PointPairList list = new PointPairList();

            DoubleRungeKuttaFunction choosenOrder = null;
            ref double error = ref error3;
            switch (orderAccuracy)
            {
                case RungeKuttaOrder.thirdOrderAccuracy:
                    choosenOrder = DoubleRungeKutta3;
                    error = ref error3;
                    break;
                case RungeKuttaOrder.fourthOrderAccuracy:
                    choosenOrder = DoubleRungeKutta4;
                    error = ref error4;
                    break;
            }

            error = 0;
            for (double t = t0; t < xMax; t += h)
            {
                list.Add(choosenOrder(f1, f2, x0, y0, t0, t));
                PointPair realResult = xy1(t);
                double nowError = Math.Sqrt(Math.Pow(realResult.X- list.Last().X, 2)+ Math.Pow(realResult.Y - list.Last().Y, 2));
                if (nowError > error)
                {
                    error = nowError;
                }
                x0 = list.Last().X;
                y0 = list.Last().Y;
                t0 = t;
            }//{( 88103,187603503, 44052,5938017515 )}
            //{( 485165196,409771, 970330390,819543 )}

            AddCurve(title, list, color);
        }

        //рандомный пример
        static double fSH1(double x, double y)
        {
            return y * Math.Cos(x); //1
        }
        static double f1(double x)
        {
            return -Math.Exp(Math.Sin(x) - Math.Sin(1)); //1
        }
   
        static PointPair xy1(double t)
        {
            return new PointPair(4*Math.Exp(- t) - Math.Exp(2*t), Math.Exp(-t) - Math.Exp(2 * t));
        }
        static double xSh1(double x, double y, double t)
        {
            return -2 * x + 4 * y;
            //return y;
        }
        static double ySh1(double x, double y, double t)
        {
            return -x + 3 * y;
            //return 2 * y;
        }

        bool flag = true;
        private void Mkmethod_Click(object sender, EventArgs e)
        {
            Clear();
            try
            {
                double step = Convert.ToDouble(textBox1.Text.Replace('.', ','));
                DrawFunction(f1, 0, 10, step, "Дефолтная функция", Color.Blue);
                DrawDiffFunction(fSH1, 1, -1, step, 10, "Рунге-Кутта 3 порядка", Color.Red);
                DrawDiffFunction(fSH1, 1, -1, step, 10, "Рунге-Кутта 4 порядка", Color.Purple, RungeKuttaOrder.fourthOrderAccuracy);
                textBox2.Text = error3.ToString();
                textBox3.Text = error4.ToString();
            } catch
            {
                textBox2.Text = "Неправильный формат";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clear();
            try
            {
                double step = Convert.ToDouble(textBox1.Text.Replace('.', ','));
                const double x0 = 3, y0 = 0, t0 = 0, maxT = 2;
                DrawPolarFunctionAndGetMaxPoint(xy1, t0, maxT, step, "Дефолтная функция", Color.Blue);
                DrawDoubleDiffFunction(xSh1, ySh1, x0, y0, step, maxT, "Рунге-Кутта 3 порядка", Color.Purple, RungeKuttaOrder.thirdOrderAccuracy, t0);
                DrawDoubleDiffFunction(xSh1, ySh1, x0, y0, step, maxT, "Рунге-Кутта 4 порядка", Color.Purple, RungeKuttaOrder.fourthOrderAccuracy, t0);
                textBox2.Text = error3.ToString();
                textBox3.Text = error4.ToString();
            }
            catch
            {
                textBox2.Text = "Неправильный формат";
            }
        }
    }
}
