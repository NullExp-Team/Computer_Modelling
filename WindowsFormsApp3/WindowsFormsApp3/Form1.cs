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
            double sumOfX = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfX += inputList[i].First;
            }
            double sumOfY = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfY += inputList[i].Second;
            }
            double sumOfXpow2 = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfXpow2 += inputList[i].First * inputList[i].First;
            }
            double sumOfXmultY = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfXmultY += inputList[i].First * inputList[i].Second;
            }
            // метод Крамера
            double delta = sumOfXpow2 * inputList.Count - sumOfX * sumOfX;
            double a = (sumOfXmultY * inputList.Count - sumOfY * sumOfX) / delta;
            double b = (sumOfXpow2 * sumOfY - sumOfXmultY * sumOfX) / delta;
            //y = a*x + b

            // рисуем полученную функцию
            int XOfInput = 0;
            double error = 0;
            PointPairList rezult = new PointPairList();
            for (int i = inputList[0].First-10; i< inputList.Last().First+10; i+=1)
            {
                rezult.Add(i, a * i + b);
                
                //считаем ошибку
                if (XOfInput < inputList.Count && i == inputList[XOfInput].First)
                {
                    error += Math.Pow(inputList[XOfInput].Second - (a * i + b), 2);
                    XOfInput++;
                }
            }
            label1.Text = Math.Round(error, 2).ToString();


            return rezult;
        }
        //PointPairList SecondFunk()
        //{

        //}
        //PointPairList ThirdFunk()
        //{

        //}
        PointPairList FourthFunk()
        {
            double sumOfX = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfX += inputList[i].First;
            }

            double sumOfY = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfY += inputList[i].Second;
            }

            double sumOfXpow2 = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfXpow2 += inputList[i].First * inputList[i].First;
            }

            double sumOfXmultY = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfXmultY += inputList[i].First * inputList[i].Second;
            }

            double sumOfX2multY = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfX2multY += inputList[i].First * inputList[i].First * inputList[i].Second;
            }

            double sumOfXpow3 = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfXpow3 += inputList[i].First * inputList[i].First * inputList[i].First;
            }

            double sumOfXpow4 = 0;
            for (int i = 0; i < inputList.Count; i++)
            {
                sumOfXpow4 += inputList[i].First * inputList[i].First * inputList[i].First * inputList[i].First;
            }

            double delta = inputList.Count * sumOfXpow4 * sumOfXpow2 + sumOfXpow3 * sumOfX * sumOfXpow2 + sumOfXpow3 * sumOfX * sumOfXpow2 - Math.Pow(sumOfXpow2, 3) - Math.Pow(sumOfX, 2) * sumOfXpow4 - Math.Pow((sumOfXpow3), 2) * inputList.Count;          
            double a = (inputList.Count * sumOfXpow2 * sumOfX2multY + sumOfXmultY * sumOfX * sumOfXpow2 + sumOfXpow3 * sumOfX * sumOfY - (Math.Pow(sumOfXpow2, 2)) * sumOfY - Math.Pow(sumOfX,2) * sumOfX2multY - inputList.Count * sumOfXmultY * sumOfXpow3) / delta;
            double b = (inputList.Count * sumOfXmultY * sumOfXpow4 + sumOfXpow3 * sumOfY * sumOfXpow2 + sumOfX2multY * sumOfX * sumOfXpow2 - Math.Pow(sumOfXpow2, 2) * sumOfXmultY - sumOfY * sumOfX * sumOfXpow4 - inputList.Count * sumOfXpow3 * sumOfX2multY) / delta;
            double c = (sumOfXpow4 * sumOfXpow2 * sumOfY + sumOfXpow3 * sumOfXmultY * sumOfXpow2 + sumOfXpow3 * sumOfX * sumOfX2multY - Math.Pow(sumOfXpow2, 2) * sumOfX2multY - Math.Pow(sumOfXpow3, 2) * sumOfY - sumOfX * sumOfXmultY * sumOfXpow4) / delta;

            int XOfInput4 = 0;
            double error4 = 0;
            PointPairList rezult4 = new PointPairList();
            for (int i = inputList[0].First - 10; i < inputList.Last().First + 10; i += 1)
            {
                rezult4.Add(i, a * i*i + b * i + c);

                //считаем ошибку давай давай считай мы же миллионеры еще посчитаем
                if (XOfInput4 < inputList.Count && i == inputList[XOfInput4].First)
                {
                    error4 += Math.Pow(inputList[XOfInput4].Second - (a * i*i + b * i + c), 2);
                    XOfInput4++;
                }
            }
            label4.Text = Math.Round(error4, 2).ToString();

            return rezult4;
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
            //
            //
            PointPairList chlin = FourthFunk();

            GraphPane my_Pane = Zed_GraphControl.GraphPane;
            LineItem myCircle1 = my_Pane.AddCurve("это че вообще?", startLine, Color.Green, SymbolType.Circle);
            LineItem myCircle2 = my_Pane.AddCurve("Линейная функция", lin, Color.Blue, SymbolType.Circle);
            //
            //
            LineItem myCircle5 = my_Pane.AddCurve("Квадратичная функция", chlin, Color.Red, SymbolType.Circle);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Clear(zedGraphControl1);
            try
            {
                inputList.Clear();
                inputList.Add(new Pair<int, double>(1, 1.0));
                inputList.Add(new Pair<int, double>(2, 1.5));
                inputList.Add(new Pair<int, double>(3, 3.0));
                inputList.Add(new Pair<int, double>(4, 4.5));
                inputList.Add(new Pair<int, double>(5, 7.0));
                inputList.Add(new Pair<int, double>(6, 8.5));
                  
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

    }
}
