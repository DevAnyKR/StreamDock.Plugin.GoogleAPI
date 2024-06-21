using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2.Data;

namespace StreamDock.Plugins.GoogleAPIs.AdSenseManagement
{
    internal class ChartReport
    {
        readonly PluginSettings pluginSettings;

        internal ChartReport(PluginSettings pluginSettings)
        {
            this.pluginSettings = pluginSettings;
        }

        internal ReportResult GetReportResult()
        {
            var key = ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions);

            if (Item.ReportResults.ContainsKey(key))
                return Item.ReportResults[ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions)];
            else return null;
        }
        internal DataTable ReportResultToDataTable(ReportResult reportResult)
        {
            DataTable dt = new DataTable();

            DataColumn[] dc =
            [
                new DataColumn(reportResult.Headers[0].Name, typeof(DateTime)),
                new DataColumn(reportResult.Headers[1].Name, typeof(double))
            ];

            dt.Columns.AddRange(dc);

            //  Rows 추출
            reportResult.Rows.ForEach(s =>
            {
                DataRow dr = dt.NewRow();

                DateTime.TryParse(s.Cells[0].Value, out DateTime date);
                dr[reportResult.Headers[0].Name] = date;
                dr[reportResult.Headers[1].Name] = s.Cells[1].Value.ToDouble();

                dt.Rows.Add(dr);
            });

            return dt;
        }
        /// <summary>
        /// 차트 이미지를 생성합니다.
        /// </summary>
        /// <returns></returns>
        internal Chart CreateChart()
        {
            var report = GetReportResult();
            if(report is null) return null;

            // chart
            Chart chart = new Chart();
            ChartArea chartArea1;
            Series series1;

            chart.Margin = new System.Windows.Forms.Padding(0);
            chart.Padding = new System.Windows.Forms.Padding(0);
            chart.Size = new Size(144, 144); //TODO to variables
            chart.BackColor = Color.Black;
            chart.ForeColor = Color.White;
            chart.BorderColor = Color.White;
            chart.BorderlineColor = Color.White;

            // chartArea
            chartArea1 = new ChartArea()
            {
                Name = "chartArea1",
            };

            Axis axisX = new Axis();
            axisX.MajorGrid.Enabled = false;
            axisX.LabelStyle.ForeColor = Color.White;
            axisX.LabelStyle.Format = "dd";
            axisX.LineColor = Color.Red;
            axisX.IsStartedFromZero = false;

            Axis axisY = new Axis();
            axisY.MajorGrid.LineColor = GraphicsTools.ColorFromHex("#E9ECEF");
            axisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            axisY.MajorTickMark.LineColor = Color.White;
            axisY.LabelStyle.ForeColor = Color.White;
            axisY.LineColor = Color.Red;
            axisY.IsStartedFromZero = false;

            chartArea1.AxisX = axisX;
            chartArea1.AxisY = axisY;

            // series
            series1 = new Series()
            {
                Name = "series1",
                ChartArea = chartArea1.Name,
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Date,
                YValueType = ChartValueType.Double,
                XValueMember = report.Headers[0].Name,
                YValueMembers = report.Headers[1].Name,
                BorderWidth = 2,
                MarkerStyle = MarkerStyle.Circle,
                MarkerBorderColor = GraphicsTools.ColorFromHex("#4782F2"),
                MarkerColor = Color.White
            };

            chart.ChartAreas.Add(chartArea1);
            chart.Series.Add(series1);

            // refresh
            DataTable dt = ReportResultToDataTable(report);
            chart.ChartAreas[0].RecalculateAxesScale();
            chart.DataSource = dt;
            chart.DataBind();

            return chart;
        }
    }
}
