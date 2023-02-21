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

        private PointPair4 DrawPolarFunctionAndGetMaxPoint(Func<double, PointPair> func, double fiStep, string title, Color color, bool lastToFirst = false)
        {
            PointPairList list = new PointPairList();

            double minX = Double.MaxValue;
            double minY = Double.MaxValue;

            double maxX = Double.MinValue;
            double maxY = Double.MinValue;

            for (double fi = 0; fi <= 2 * Math.PI; fi += fiStep)
            {
                PointPair point = func(fi);
                list.Add(point.X, point.Y);
                if (point.X < minX)
                {
                    minX = point.X;
                }
                if (point.Y < minY)
                {
                    minY = point.Y;
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].X -= minX;
                list[i].Y -= minY;

                maxX = list[i].X > maxX ? list[i].X : maxX;
                maxY = list[i].Y > maxY ? list[i].Y : maxY;
            }

            AddCurve(title, list, color, SymbolType.None, lastToFirst);

            return new PointPair4(maxX, maxY, minX, minY);

        }

        public delegate double Function(double x, double y);
        
        //Рунге-Кутта 3 порядка
        public static double RungeKutta3(Function f, double x0, double y0, double h, double x)
        {
            double xnew, ynew, k1, k2, k3, p1 = 1 / 3, p2 = 2 / 3, result = double.NaN;

            if (x == x0)
                result = y0;
           
            else if (x > x0)
            {
                do
                {
                    if (h > x - x0) h = x - x0;
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
        public static double RungeKutta4(Function f, double x0, double y0, double h, double x)
        {
            double xnew, ynew, k1, k2, k3, k4, result = double.NaN;

            if (x == x0)
                result = y0;

            else if (x > x0)
            {
                do
                {
                    if (h > x - x0) h = x - x0;
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

        //рандомный пример
        static double f(double x, double y)
        {
            return y * Math.Cos(x);
        }

        private void Mkmethod_Click(object sender, EventArgs e)
        {
            //не работает!!!!!!!!!

            //гиренко разберись я запутался в х и у ааа
            Clear();

            Func<double, double> f1 = (x) =>
            {
                double h=0.001, x0 = 0.0, y0 = 1.0;
                double result = y0;

                for (int i = 0; i < 20; i++)
                {
                    x = 0.1 * i;
                    result = RungeKutta3(f, x0, result, h, x);
                    x0 = x;
                }

                return result;
            };

            DrawFunction(f1, 0.0, 20.0, 0.01, "Task 1", Color.Red);
        }
    }
}
