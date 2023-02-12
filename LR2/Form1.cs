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
        private void DrawFunction(Func<double, double> func, double xMin, double xMax, double xStep, string title, Color color, bool lastToFirst = false)
        {
        

            PointPairList list = new PointPairList();
            for (double x = xMin; x <= xMax; x += xStep)
            {
                double y = func(x);
                list.Add(x, y);
            }
            if(lastToFirst && list.Count() > 0)
            {
                list.Add(list.First());
            }
           

            GraphPane plane = zedGraphControl1.GraphPane;

            plane.AddCurve(title, list, color, SymbolType.None);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();



        }

        private void Clear(ZedGraphControl control)
        {
            control.GraphPane.CurveList.Clear();
            control.GraphPane.GraphObjList.Clear();

            control.GraphPane.XAxis.Type = AxisType.Linear;
            control.GraphPane.XAxis.Scale.TextLabels = null;
            control.GraphPane.XAxis.MajorGrid.IsVisible = false;
            control.GraphPane.YAxis.MajorGrid.IsVisible = false;
            control.GraphPane.YAxis.MinorGrid.IsVisible = false;
            control.GraphPane.XAxis.MinorGrid.IsVisible = false;
            control.RestoreScale(control.GraphPane);

            control.AxisChange();
            control.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
