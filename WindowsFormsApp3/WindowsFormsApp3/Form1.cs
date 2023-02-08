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
        //PointPairList Funk()
        //{

        //}

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

            GraphPane my_Pane = Zed_GraphControl.GraphPane;
            LineItem myCircle1 = my_Pane.AddCurve("Func", startLine, Color.Green, SymbolType.Circle);
            LineItem myCircle2 = my_Pane.AddCurve("Func", lin, Color.Blue, SymbolType.Circle);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Clear(zedGraphControl1);
            try
            {
                inputList.Clear();
                inputList.Add(new Pair<int, double>(10, 1.06));
                inputList.Add(new Pair<int, double>(20, 1.33));
                inputList.Add(new Pair<int, double>(30, 1.52));
                inputList.Add(new Pair<int, double>(40, 1.68));
                inputList.Add(new Pair<int, double>(50, 1.81));
                inputList.Add(new Pair<int, double>(60, 1.91));
                  


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
