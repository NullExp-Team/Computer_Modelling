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
            plane.YAxis.Title.Text = "Количество";

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

            PointPairList list = new PointPairList();
            list.Add(0, 0);
            for (int i = 0; i < k; i++)
            {
                list.Add(Convert.ToDouble(i) / k, parts[i]);
                list.Add(Convert.ToDouble(i + 1) / k, parts[i]);
            }
            list.Add(1, 0);
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
                String strVal = stepValue.Substring(startIndex, len);
                if (strVal.Length < 4)
                {
                    strVal = "1" + new String('1', 3 - strVal.Length) + strVal;
                }
                val = int.Parse(strVal);
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

            PointPairList list = new PointPairList();
            for (int i = 0; i < k; i++)
            {
                list.Add(Convert.ToDouble(i) / k, parts[i]);
                list.Add(Convert.ToDouble(i + 1) / k, parts[i]);
            }

            AddCurve("Диаграмма", list, Color.Blue, SymbolType.Circle, false);
        }

        static String RotateShift(String value, int shift)
        {
            if (shift < 0)
            {
                return (value.Substring(10 + shift, -shift) + value.Substring(0, 10 + shift));
            } else
            {
                return (value.Substring(shift) + value.Substring(0, shift));
            }
        }

        static int bin_to_dec(string a)
        {
            double b = 0;

            for (double i = a.Length - 1; i >= 0; i--)
                b += Convert.ToDouble(a.Substring(Convert.ToInt16(a.Length - 1 - i), 1)) * Math.Pow(2, i);

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
                int r = r02.Length;
                for (int i = 1; i <= 10-r; i++)
                {
                    r02 = "0" + r02;
                }
                if (r > 10)
                {
                    r02 = r02.Substring(0, 10);
                }

                string r0z = RotateShift(r02, -2);
                string r0zz = RotateShift(r02, 2);

                int r0zdec = bin_to_dec(r0z);
                int r0zzdec = bin_to_dec(r0zz);

                val = r0zdec + r0zzdec;
                
                double r1doub = Convert.ToDouble(val % 1000) / Math.Pow(10, (val % 1000).ToString().Length);

                return r1doub;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Setup();
            //rand.Next(100, 500)
            var generator = new GeneratorDV(rand.Next(1, 1023));

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

            PointPairList list = new PointPairList();
            for (int i = 0; i < k; i++)
            {
                list.Add(Convert.ToDouble(i) / k, parts[i]);
                list.Add(Convert.ToDouble(i + 1) / k, parts[i]);
            }

            AddCurve("Диаграмма", list, Color.Blue, SymbolType.Circle, false);
        }

    

        private void button5_Click(object sender, EventArgs e)
        {
            Setup();

            //int MidSquare(int seed) // метод серединных произведений
            //{
            //    int square = seed * seed; // возводим число в квадрат
            //    string squareStr = square.ToString("D8"); // преобразуем квадрат в строку с ведущими нулями
            //    string middleStr = squareStr.Substring(2, 4); // берем серединные четыре цифры из строки
            //    int middle = int.Parse(middleStr); // преобразуем серединные цифры обратно в число
            //    return middle; // возвращаем результат
            //}

            int init = rand.Next(1000, 9999);

            // Задаем начальные значения R0 и R1
            int r0 = init;
            int r1 = 5678;
         

            double MiddleSquare(double seed)
            {
                // Вычисляем произведение R0 и R1
                int r2 = r0 * r1;

                // Преобразуем произведение в строку
                string s2 = r2.ToString();

                // Добавляем нули слева и справа, если длина строки меньше 8 символов
                while (s2.Length < 8)
                {
                    s2 = "0" + s2 + "0";
                }

                // Извлекаем середину строки (4 символа)
                string s2star = s2.Substring(2, 4);

                // Преобразуем середину в число
                int r2star = int.Parse(s2star);

                // Делим середину на 10000, чтобы получить число от 0 до 1
                double randomNumber = (double)r2star / 10000;


                // Обновляем значение R0 равным середине
                r0 = r2star;

                return randomNumber;
            }
            
            

            //RandFunction f = (double previous) => {
            //    int prev = previous == double.MaxValue ? init : (int)(previous * 10000);
            //    int random = MidSquare(prev); 
            //    double result = (double)random / 10000; // делим результат на степень десятки, равную количеству цифр в числе
            //    return result; // возвращаем результат
            //};


            RandFunction f2 = (double previous) => {
                return MiddleSquare(previous);
            };

                DrawDiagram(f2);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Setup();
            // Коэффициенты для линейного конгруэнтного метода
            int a = 1664525;
            int b = 1013904223;
            int m = (int) Math.Pow(2, 32) - 1;

            double LCG(double previous)
            {
                // Вычисляем следующее случайное значение по формуле
                double next = (a * (previous*m) + b) % m;

                // Добавляем m и берем остаток от деления снова для положительного результата
                next = (next + m) % m;

                // Нормализуем значение в диапазон от 0 до 1
                return next / m;
            }
            int init = rand.Next(1000, 9999);

            RandFunction f = (double previous) =>
            {
                double prev = previous == double.MaxValue ? init : previous;
                double result = LCG(prev);
                return result; 
            };

            DrawDiagram(f);
        }
    }
}
