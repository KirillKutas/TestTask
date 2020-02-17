using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Test
{
    class DrawingGraph
    {
        private MainWindow main;
        List<int> axisXData = new List<int>();
        public DrawingGraph(MainWindow main)
        {
            for (int a = 1; a <= 30; a++)
            {
                axisXData.Add(a);
            }
            this.main = main;
            main.chart.ChartAreas.Clear();
            main.chart.Series.Clear();
            main.chart.ChartAreas.Add(new ChartArea("Default"));
        }
        public void Drawing(PersonModel Person)
        {
            main.chart.Series.Add(new Series("Series1"));
            main.chart.Series["Series1"].ChartArea = "Default";
            main.chart.Series["Series1"].ChartType = SeriesChartType.Line;
            main.chart.Series["Series1"].Points.DataBindXY(axisXData, Person.AllSteps);            
        }
        public void DrawingMinAdnMax(PersonModel Person)
        {
            main.chart.Series.Add(new Series("Series2"));
            main.chart.Series["Series2"].ChartArea = "Default";
            main.chart.Series["Series2"].ChartType = SeriesChartType.Point;

            List<int?> axisYData = new List<int?>();
            for(int currentStep = 0; currentStep < 30; currentStep++)
            {
                if (currentStep < Person.AllSteps.Count)
                {
                    if (Person.AllSteps[currentStep] == Person.MinSteps)
                    {
                        axisYData.Add(Person.MinSteps);
                    }
                    else if (Person.AllSteps[currentStep] == Person.MaxSteps)
                    {
                        axisYData.Add(Person.MaxSteps);
                    }
                    else
                    {
                        axisYData.Add(null);
                    }
                }
                else
                {
                    axisYData.Add(null);
                }
                
            }
            
            main.chart.Series["Series2"].Palette = ChartColorPalette.Fire;
            main.chart.Series["Series2"].Points.DataBindXY(axisXData, axisYData);
        }
    }
}
