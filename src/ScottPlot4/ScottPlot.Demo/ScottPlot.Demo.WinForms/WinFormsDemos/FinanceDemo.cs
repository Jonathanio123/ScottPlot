﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScottPlot.Demo.WinForms.WinFormsDemos
{
    public partial class FinanceDemo : Form
    {
        readonly ScottPlot.Plottable.FinancePlot CandlePlot;
        readonly ScottPlot.Plottable.BarPlot BarPlot;
        readonly Random Rand = new Random(0);

        public FinanceDemo()
        {
            InitializeComponent();

            CandlePlot = new Plottable.FinancePlot() { Candle = true };

            formsPlot1.Plot.Add(CandlePlot);
            formsPlot1.Plot.YLabel("Price");
            formsPlot1.Plot.XAxis.DateTimeFormat(true);
            formsPlot1.Plot.XAxis.Ticks(false);
            formsPlot1.Plot.XAxis.SetSizeLimit(max: 0);
            formsPlot1.AxesChanged += FormsPlot1_AxesChanged;

            double[] xs = { 1, 2, 3 };
            double[] ys = { 1, 2, 3 };
            BarPlot = new Plottable.BarPlot(xs, ys, null, null);

            formsPlot2.Plot.Add(BarPlot);
            formsPlot2.Plot.YLabel("Volume");
            formsPlot2.Plot.XAxis.DateTimeFormat(true);
            formsPlot2.Plot.XAxis2.SetSizeLimit(max: 0);
        }

        private void FinanceDemo_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
                AddNewDataPoint(false);

            AddNewDataPoint(true);
        }

        private void AddNewDataPoint(bool refresh = true)
        {
            double lastClose = CandlePlot.OHLCs.Any() ? CandlePlot.OHLCs.Last().Close : 100;
            double open = lastClose + (Rand.NextDouble() - .5) * 10;
            double close = open + (Rand.NextDouble() - .4) * 10;
            double low = Math.Min(open, close) - Rand.NextDouble() * 5;
            double high = Math.Max(open, close) + Rand.NextDouble() * 5;
            double volume = Rand.NextDouble() * 500 + 100;

            TimeSpan span = TimeSpan.FromSeconds(1);
            DateTime date = CandlePlot.OHLCs.Any() ? CandlePlot.OHLCs.Last().DateTime + span : DateTime.Today + TimeSpan.FromHours(9.5);

            OHLC ohlc = new OHLC(open, high, low, close, date, span, volume);
            CandlePlot.Add(ohlc);

            if (refresh)
            {
                formsPlot1.Plot.AxisAuto();

                BarPlot.Replace(
                    positions: CandlePlot.OHLCs.Select(x => x.DateTime.ToOADate()).ToArray(),
                    values: CandlePlot.OHLCs.Select(x => x.Volume).ToArray());

                BarPlot.BarWidth = .9 * span.TotalSeconds / TimeSpan.FromDays(1).TotalSeconds;

                formsPlot2.Plot.AxisAuto();
                formsPlot2.Plot.SetAxisLimits(yMin: 0);

                formsPlot1.Refresh();
                formsPlot2.Refresh();
            }
        }

        private void FormsPlot1_AxesChanged(object sender, EventArgs e)
        {
            formsPlot2.Plot.MatchAxis(formsPlot1.Plot, horizontal: true, vertical: false);
            formsPlot2.Plot.MatchLayout(formsPlot1.Plot, horizontal: true, vertical: false);
            formsPlot2.Refresh();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AddNewDataPoint();
        }
    }
}
