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



namespace LR2
{
    public partial class Form1 : Form
    {

        Random random = new Random();

        public static double n = 2;

        bool isThird = false;

        int currentPointCount { 
            get {
                try
                {

                    return int.Parse(pointTestCount.Text);

                }
                catch 
                {
                    MessageBox.Show(
                                   "Укажите количество точек",
                                   "Сообщение",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information,
                                   MessageBoxDefaultButton.Button1,
                                   MessageBoxOptions.DefaultDesktopOnly);


                    return 100;
                }
            } 
        }

        public Form1()
        {
            InitializeComponent();

            Setup();
        }

        private void Setup()
        {
            GraphPane plane = zedGraphControl1.GraphPane;

            plane.Title.Text = "NullExp Lab2";

            plane.XAxis.Title.Text = "Ось X";

            plane.YAxis.Title.Text = "Ось Y";

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
                



                
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
        /// Если для параметра lastToFirst задано значение true и список точек содержит хотя бы одну точку, 
        //первая точка добавляется в конец списка.
        private void addCurve(string title, PointPairList points, Color color, SymbolType symbolType = SymbolType.None, bool lastToFirst = false)
        {
            GraphPane plane = zedGraphControl1.GraphPane;

            PointPairList pointCloned = points.Clone();

            if(lastToFirst && pointCloned.Count() > 0) pointCloned.Add(pointCloned.First());

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

            addCurve(title, list, color, SymbolType.None, lastToFirst);

            return new PointPair(maxX, maxY);
        }

        /// Это перегрузка предыдущего метода DrawFunction, который не принимает параметры yMin и yMax и не фильтрует значения y.
        private PointPair DrawFunction(Func<double, double> func, double xMin, double xMax, double xStep, string title, Color color, bool lastToFirst = false)
        {

            return DrawFunction(func, xMin, xMax, 
                                                null, null, 
                                                xStep, title, color, lastToFirst);
            
        }

        /// этот метод генерирует набор случайных точек в указанном диапазоне и добавляет на график два набора точек,
        /// которые соответствуют или не соответствуют заданному условию, определяемое функцией func.
        /// Метод возвращает отношение количества точек, удовлетворяющих условию, к общему количеству точек. 
        private double PointTest(int countOfPoints, PointPair minPoint, PointPair maxPoint, Func<PointPair, bool> func)
        {
            GraphPane plane = zedGraphControl1.GraphPane;
            PointPairList inPoints = new PointPairList();
            PointPairList outPoints = new PointPairList();

            for (int i = 0; i < countOfPoints; i++)
            {
                PointPair t = new PointPair(
                    random.NextDouble() * (maxPoint.X - minPoint.X) + minPoint.X,
                    random.NextDouble() * (maxPoint.Y - minPoint.Y) + minPoint.Y
                );
                
                if (func(t)) inPoints.Add(t);
                else outPoints.Add(t);
            }
          
            
            var inLine = plane.AddCurve("", inPoints, Color.Green, SymbolType.Circle);
            inLine.Line.IsVisible = false;
            inLine.Symbol.Size = 4;

            var outLine = plane.AddCurve("", outPoints, Color.Black, SymbolType.Circle);
            outLine.Line.IsVisible = false;
            outLine.Symbol.Size = 4;

            inPointsCountLabel.Text = inPoints.Count.ToString();

            double k = inPoints.Count() * 1.0 / countOfPoints;

            double area = k * (maxPoint.X - minPoint.X) * (maxPoint.Y - minPoint.Y);

            if (!isThird)
            {
                label3.Text = " Площадь (приближённо):";
                areaLabel.Text = area.ToString();
            }
            else
            {
                label3.Text = " Пи (приближённо):";
                areaLabel.Text = ((area/n)/n).ToString();
            }


            return k;
        }

        private void firstTask_Click(object sender, EventArgs e)
        {
            Clear();
            Func<double, double> f1 = (x) =>
            {
                if (x < n)
                {
                    return (10 * x) / n;
                }
                else
                {
                    return (10 * ((x - 20) / (n - 20)));
                }
            };
            Func<double, double> f2 = (x) =>
            {
                //Эту фигню самостоятельно подбирать (это точка пересечения функций), т.к. нужно найти фигуру, которая ограничина функциями,
                //а эта сложна, поэтому строим на всё одну функцию и не выпендриваемся
                // ок!!
                //20.9 - для 10 варианта
                //23.99 - для 18 варианта
                if (x < 20.9)
                {
                    return (10 * x) / n;
                }
                else
                {
                    return (10 * ((x - 20) / (n - 20))) + 20;
                }
            };

            if (n<11)
            {
                DrawFunction(f1, 0, 20, 0.01, "Task1", Color.Red, true);
                PointTest(currentPointCount, new PointPair(0, 0), new PointPair(20, 10),
                (point) => point.Y < f1(point.X));
            }
            else
            {
                DrawFunction(f2, 0, 50, 0, 100, 0.01, "Task1", Color.Purple, true);
                PointTest(currentPointCount, new PointPair(0, 0), new PointPair(25, 16),
                (point) => point.Y < f2(point.X));
            }
            absoluteErrorLabel.Text = Math.Round(3 * (1 / Math.Sqrt(currentPointCount)), 4).ToString();
        }

        private void secondTask_Click(object sender, EventArgs e)
        {
            Clear();

            double sin(double x,double n)
            {
                double sum = 0;

                for (int i = 1; i <= n; i++)
                {
                    sum += Math.Sqrt(11 - n * Math.Pow(Math.Sin(x), 2));
                }


                return (5.0 / n) * sum;
            }

            double cos(double x, double n)
            {
                double sum = 0;

                for (int i = 1; i <= n; i++)
                {
                    sum += Math.Sqrt(29 - n * Math.Pow(Math.Cos(x), 2));
                }


                return (7.0 / n) * sum;
            }

            Func<double, double> f1 = (x) =>
            {
                return sin(x, n);
            };

            Func<double, double> f2 = (x) =>
            {
                return cos(x, n);
            };
            
            if (n<11)
            {
                PointPair maxp = DrawFunction(f1, 0, 20, 0.01, "Task 2", Color.Chocolate);
                PointTest(currentPointCount, new PointPair(0, 0), maxp,
                (point) => point.Y < f1(point.X));
            }
            else
            {
                PointPair maxp = DrawFunction(f2, 10, 30, 0.01, "Task 2", Color.Chocolate);
                PointTest(currentPointCount, new PointPair(10, 0), maxp,
                (point) => point.Y < f2(point.X));
            }
            // умножая на 3 мы получаем правило 3ς, Монте-Карло не про точность так что нам пофиг 
            absoluteErrorLabel.Text = Math.Round(3 * (1 / Math.Sqrt(currentPointCount)), 4).ToString();
        }

        private PointPair4 DrawPolarFunctionAndGetMaxPoint(Func<double, PointPair> func, double fiStep, string title, Color color, bool lastToFirst)
        {
            PointPairList list = new PointPairList();

            double minX = Double.MaxValue;
            double minY = Double.MaxValue;

            double maxX = Double.MinValue;
            double maxY = Double.MinValue;

            for (double fi = 0; fi <= 2*Math.PI; fi += fiStep)
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

            addCurve(title, list, color, SymbolType.None, lastToFirst);



            return new PointPair4(maxX, maxY, minX, minY);


        }
        private void thirdTask_Click(object sender, EventArgs e)
        {
            isThird = true;
            Clear();
            double stepInDraw = 0.01;
            double rad = n;

            Func<double, PointPair> circle = (fi) =>
            {
                return new PointPair(rad+rad*Math.Cos(fi), rad + rad * Math.Sin(fi));
            };
            Func<PointPair, bool> circleTest = (p) =>
            {
                return (Math.Pow((p.X - rad), 2) + Math.Pow((p.Y - rad), 2)) < rad * rad;
            };
            PointPair maxPoint = DrawPolarFunctionAndGetMaxPoint(circle, stepInDraw, "Task1", Color.Red, false);

            PointTest(currentPointCount, new PointPair(0, 0), maxPoint, circleTest);
            isThird = false;
            absoluteErrorLabel.Text = Math.Round(3 * (1 / Math.Sqrt(currentPointCount)), 4).ToString();
        }

        private void fourthTask_Click(object sender, EventArgs e)
        {
            Clear();
            double stepInDraw = 0.01;
            double a, b;
            if (n <= 10)
            {
                a = 11 + n; b = 11 - n;
            } 
            else
            {
                a = n + 10; b = n - 10;
            }
            


            Func<double, double> p = (fi) =>
            {
                return Math.Sqrt(a*Math.Cos(fi) * Math.Cos(fi)+b*Math.Sin(fi) * Math.Sin(fi));
            };

            Func<double, PointPair> func = (fi) =>
            {
                return new PointPair(p(fi) * Math.Cos(fi), p(fi) * Math.Sin(fi));
            };

            PointPair4 maxPoint = DrawPolarFunctionAndGetMaxPoint(func, stepInDraw, "Task1", Color.Red, false);

            Func<PointPair, bool> test = (point) =>
            {
                point.X += maxPoint.Z;
                point.Y += maxPoint.T;
                double r = Math.Sqrt(point.X * point.X + point.Y * point.Y);
                double fi = 0;

                if (point.X > 0)
                {
                    fi = Math.Atan(point.Y / point.X);
                } 
                else if (point.X < 0)
                {
                    fi = Math.PI + Math.Atan(point.Y / point.X);
                }
                else if (point.X == 0)
                {
                    if (point.Y > 0)
                    {
                        fi = Math.PI / 2;
                    }
                    else if (point.Y < 0)
                    {
                        fi = -Math.PI / 2;
                    }
                    else if (point.Y == 0)
                    {
                        fi = 0;
                    }
                }
                point.X -= maxPoint.Z;
                point.Y -= maxPoint.T;

                double ada = p(fi);

                return r < ada;

                absoluteErrorLabel.Text = Math.Round(3 * (1 / Math.Sqrt(currentPointCount)), 4).ToString();
            };

            PointTest(currentPointCount,
                                        new PointPair(0, 0), 
                                        new PointPair(maxPoint.X, maxPoint.Y), 
                                        test);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


        bool flagScale = true;
 
        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;

            double koef = Convert.ToDouble(zedGraphControl1.Size.Height) / Convert.ToDouble(zedGraphControl1.Size.Width);
            if (flagScale)
            {
                myPane.XAxis.Scale.Max = myPane.YAxis.Scale.Max / koef + n;
                myPane.XAxis.Scale.Min = myPane.YAxis.Scale.Min / koef;
                myPane.XAxis.Scale.MajorStep = myPane.YAxis.Scale.MajorStep / koef + n;
            }
            else
            {
                myPane.XAxis.Scale.Max = (myPane.YAxis.Scale.Max - n) * koef;
                myPane.XAxis.Scale.Min = (myPane.YAxis.Scale.Min) * koef;
                myPane.XAxis.Scale.MajorStep = (myPane.YAxis.Scale.MajorStep - n) * koef;
            }
            flagScale = !flagScale;

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private void absoluteErrorLabel_Click(object sender, EventArgs e)
        {






        }
    }
    
}
