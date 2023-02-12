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


        int currentPointCount { 
            get {
                try
                {
                    return int.Parse(pointTestCount.Text);
                }
                catch {
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
        private void DrawFunction(Func<double, double> func, double xMin, double xMax, double? yMin, double? yMax, double xStep, string title, Color color, bool lastToFirst = false)
        {
            PointPairList list = new PointPairList();

            for (double x = xMin; x <= xMax; x += xStep)
            {
                double y = func(x);
                if (yMin == null || y >= yMin) list.Add(x, y);
                if (yMax == null || y >= yMax) list.Add(x, y);
            }

            addCurve(title, list, color, SymbolType.None, lastToFirst);
        }

        /// Это перегрузка предыдущего метода DrawFunction, который не принимает параметры yMin и yMax и не фильтрует значения y.
        private void DrawFunction(Func<double, double> func, double xMin, double xMax, double xStep, string title, Color color, bool lastToFirst = false)
        {
            DrawFunction(func, xMin, xMax, null, null, xStep, title, color, lastToFirst);
        }

        /// этот метод генерирует набор случайных точек в указанном диапазоне и добавляет на график два набора точек,
        /// которые соответствуют или не соответствуют заданному условию, определяемое функцией func.
        /// Метод возвращает отношение количества точек, удовлетворяющих условию, к общему количеству точек. 
        private double PointTest(int n, PointPair minPoint, PointPair maxPoint, Func<PointPair, bool> func)
        {
            GraphPane plane = zedGraphControl1.GraphPane;
            PointPairList inPoints = new PointPairList();
            PointPairList outPoints = new PointPairList();

            for (int i = 0; i < n; i++)
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

            double k = inPoints.Count()*1.0 / n;

            double area = k * (maxPoint.X - minPoint.X) * (maxPoint.Y - minPoint.Y);

            areaLabel.Text = area.ToString();

            return k;
        }

        private void firstTask_Click(object sender, EventArgs e)
        {
            Clear();

            Func<double, double> f1 = (x) =>
            {
                int n = 10;
                if (x < n)
                {
                    return (10 * x) / n;
                }
                else
                {
                    return (10 * ((x - 20) / (n - 20)));
                }
            };

            DrawFunction(f1, 0, 20, 0.01, "Task1.1", Color.Red, true);

            PointTest(currentPointCount, new PointPair(0, 0), new PointPair(20, 10),
                (point) => point.Y < f1(point.X)
            );

            Func<double, double> f2 = (x) =>
            {
                int n = 11;
                //Эту хуйню самостоятельно подбирать (это точка пересечения функций), т.к. нужно найти фигуру, которая ограничина функциями,
                //а эта сложна, поэтому строим на всё одну функцию и не выёбываемся
                if (x < 20.9)
                {
                    return (10 * x) / n;
                }
                else
                {
                    return (10 * ((x - 20) / (n - 20))) + 20;
                }
            };

            DrawFunction(f2, 0, 40, 0, 100, 0.01, "Task1.2", Color.Purple, true);
        }

        private void secondTask_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void thirdTask_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void fourthTask_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
