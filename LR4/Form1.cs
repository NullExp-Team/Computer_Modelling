using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection.Emit;
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
        long n = 1000000;
        int k = 5;
        double a = 0;
        double b = 1;
        Random rand = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds));
        private void Setup()
        {
            GraphPane plane = zedGraphControl1.GraphPane;
            plane.Title.Text = "NullExp Lab4";
            plane.XAxis.Title.Text = "Значение";
            plane.YAxis.Title.Text = "Вероятность";

            try
            {
                n = Convert.ToInt64(Math.Pow(10, Convert.ToInt64(textBox1.Text)));
                a = Convert.ToDouble(textBox2.Text);
                b = Convert.ToDouble(textBox3.Text);
                k = Convert.ToInt32(textBox4.Text);
            }
            catch
            {
                n = 1000000;
                a = 0;
                b = 1;
            }

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

        private PointPair4 DrawPolarFunctionAndGetMaxPoint(Func<double, PointPair> func, double minFi, double maxFi, double fiStep, string title, Color color, bool lastToFirst = false)
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

        private void DrawDiagram(RandFunction f)
        {
            List<double> parts = new List<double>();
            for (int i = 0; i < k; i++)
            {
                parts.Add(0);
            }

            double oldValue = Double.MaxValue;
            for (long i = 1; i <= n; i++)
            {
                double newValue = f(oldValue);
                double realNewValue = a + (b - a) * newValue;
                int partIndex = Convert.ToInt32(Math.Floor(newValue * k));
                parts[partIndex]++;

                oldValue = newValue;
            }
            for (int i = 0; i < k; i++)
            {
                parts[i] /= n;
            }

            PointPairList list = new PointPairList();
            for (int i = 0; i < k; i++)
            {
                list.Add(Convert.ToDouble(i) / k, parts[i]);
                list.Add(Convert.ToDouble(i + 1) / k, parts[i]);
            }

            AddCurve("Диаграмма", list, Color.Blue, SymbolType.Circle, false);
        }

        //функция возвращает случайное число от 0 и до 1
        // if previous == Double.MaxValue, то значит сейчас будет генериться 1-й элемент
        public delegate double RandFunction(double previous);

        private double SimpleRand(double previous)
        {
            return rand.NextDouble();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Setup();
            DrawDiagram(SimpleRand);
        }

        class Generator
        {
            protected int val = 0;

            public Generator(int initialValue)
            {
                val = initialValue;
            }

            public double GetNumber()
            {
                string stepValue = Convert.ToInt32(Math.Pow(val, 2)).ToString();
                int startIndex = stepValue.Length / 4;
                int len = val.ToString().Length;
                val = int.Parse(stepValue.Substring(startIndex, len));
                double newval = Convert.ToDouble(val) / 10000;

                return newval;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Setup();
            //3485
            var generator = new Generator(rand.Next(1000, 9999));

            List<double> parts = new List<double>();
            for (int i = 0; i < k; i++)
            {
                parts.Add(0);
            }

            double oldValue = Double.MaxValue;
            for (long i = 1; i <= n; i++)
            {
                double newValue = generator.GetNumber();
                int partIndex = Convert.ToInt32(Math.Floor(newValue * k));
                parts[partIndex]++;

                oldValue = newValue;
            }
            for (int i = 0; i < k; i++)
            {
                parts[i] /= n;
            }

            PointPairList list = new PointPairList();
            for (int i = 0; i < k; i++)
            {
                list.Add(Convert.ToDouble(i) / k, parts[i]);
                list.Add(Convert.ToDouble(i + 1) / k, parts[i]);
            }

            AddCurve("Диаграмма", list, Color.Blue, SymbolType.Circle, false);
        }

        static ulong RotateShift(ulong value, int shift)
        {
            int len = (int)Math.Log10(value) + 1;
            shift %= len;
            if (shift < 0) shift += len;
            ulong pow = (ulong)Math.Pow(10, shift);

            return (value % pow) * (ulong)Math.Pow(10, len - shift) + value / pow;
        }

        static int bin_to_dec(string a)
        {
            double b = 0;

            for (double i = a.Length - 1; i >= 0; i--)
                b += Convert.ToDouble(a.Substring(Convert.ToInt16(i), 1)) * Math.Pow(2, i);

            return Convert.ToInt32(b);
        }

        class GeneratorDV
        {
            protected int val = 0;

            public GeneratorDV(int initialValue)
            {
                val = initialValue;
            }

            public double getdv()
            {
                string r02 = Convert.ToString(val, 2);
                long r022 = Convert.ToInt64(r02);

                ulong r0z = RotateShift((ulong)r022, -2);
                ulong r0zz = RotateShift((ulong)r022, 2);

                string r0zs = Convert.ToString(r0z);
                string r0zzs = Convert.ToString(r0zz);

                int r0zdec = bin_to_dec(r0zs);
                int r0zzdec = bin_to_dec(r0zzs);

                val = r0zdec + r0zzdec;
                double r1doub = Convert.ToDouble(val) / 1000;

                return r1doub;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Setup();
            //rand.Next(100, 500)
            var generator = new GeneratorDV(rand.Next(100, 500));

            List<double> parts = new List<double>();
            for (int i = 0; i < k; i++)
            {
                parts.Add(0);
            }

            double oldValue = Double.MaxValue;
            for (long i = 1; i <= n; i++)
            {
                double newValue = generator.getdv();
                int partIndex = Convert.ToInt32(Math.Floor(newValue * k));
                if (newValue < 1) 
                    parts[partIndex]++;
                oldValue = newValue;
            }
            for (int i = 0; i < k; i++)
            {
                parts[i] /= n;
            }

            PointPairList list = new PointPairList();
            for (int i = 0; i < k; i++)
            {
                list.Add(Convert.ToDouble(i) / k, parts[i]);
                list.Add(Convert.ToDouble(i + 1) / k, parts[i]);
            }

            AddCurve("Диаграмма", list, Color.Blue, SymbolType.Circle, false);
        }
    }
}
