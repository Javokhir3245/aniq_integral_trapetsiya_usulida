using NCalc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace KursIshi
{
    public partial class Form1 : Form
    {
        bool trapezia = false;
        double a;
        double b;
        int n;
        string func;
        public Form1()
        {
            InitializeComponent();
            num1.TextChanged += TextBox_TextChanged;
            num2.TextChanged += TextBox_TextChanged;
            num3.TextChanged += TextBox_TextChanged;
        }

        private bool T_changed()
        {
            string inputText = function.Text;
            string pattern = @"\bx\b";
            if (!Regex.IsMatch(inputText, pattern))
            {
                
                return false;
            }
            return true;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string inputText = textBox.Text;
            string pattern = @"^\d+$";
            if (!Regex.IsMatch(inputText, pattern))
            {
                MessageBox.Show("Faqat raqamlar!");
            }
        }
        private void yechish(object sender, EventArgs e)
        {
            try
            {
                func = function.Text.ToString();
                func = replace(func);
                if (T_changed())
                {
                    a = Convert.ToDouble(num1.Text.ToString());
                    b = Convert.ToDouble(num2.Text.ToString());
                    if (a > b)
                        throw new Exception("a b dan katta bo'lishi mumkin emas!");
                    n = Convert.ToInt32(num3.Text.ToString());
                    double h = (b - a) / n;
                    List<double> x = new List<double>();
                    List<double> y = new List<double>();
                    Expression expr = new Expression(func);
                    double result = 0;
                    if (!trapezia)
                    {
                        do
                        {
                            x.Add(a);
                            y.Add(result * h);
                            expr.Parameters["x"] = a;
                            a += h;
                            result += (double)expr.Evaluate();

                        } while (a < b);
                        result *= h;
                        CreateScatterPlot(x, y);
                        label4.Text = "Yechim: " + (result.ToString());
                    }
                    else
                    {


                        double y0 = 0;
                        x.Add(a);
                        y.Add(result * h);
                        expr.Parameters["x"] = a;
                        y0 = (double)expr.Evaluate();
                        result += y0;


                        double yn = 0;
                        x.Add(a);
                        y.Add(result * h);
                        expr.Parameters["x"] = b;
                        yn = (double)expr.Evaluate();
                        result += yn;




                        result = (y0 + yn) / 2;
                        for (int i = 1; i <= n - 1; i++)
                        {
                            x.Add(a);
                            y.Add(result * h);
                            expr.Parameters["x"] = a;
                            a += h;
                            result += (double)expr.Evaluate();
                        }
                        result *= h;
                        CreateScatterPlot(x, y);
                        label4.Text = "Yechim: " + (result.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("x qatnashmadi!!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void CreateScatterPlot(List<double> x,List<double> y)
        {
            panel1.Controls.Clear();
            Chart chart = new Chart();
            chart.Size = new Size(panel1.Width, panel1.Height);
            chart.ChartAreas.Add(new ChartArea());
            chart.Dock = DockStyle.Fill;
            Series series = new Series("ScatterPlot");
            series.ChartType = SeriesChartType.Point;
            for (int i = 0; i < x.Count; i++)
            {
                series.Points.AddXY(x[i], y[i]);
            }
            chart.Series.Add(series);
            panel1.Controls.Add(chart);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            trapezia = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            trapezia= false;
        }
        private string replace(string a)
        {
            a=a.ToLower();
            a = a.Replace("sin", "Sin");
            a = a.Replace("cos", "Cos");
            a = a.Replace("log", "Log");
            a = a.Replace("pow", "Pow");
            a = a.Replace("sqrt", "Sqrt");
            a = a.Replace("tan", "Tan");
            return a;
        }
    }
}
