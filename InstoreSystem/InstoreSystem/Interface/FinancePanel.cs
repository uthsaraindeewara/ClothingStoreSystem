using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using CrystalDecisions.CrystalReports.Engine;
using FinalTest.Interface;

namespace InstoreSystem.Interface
{
    public partial class FinancePanel : UserControl
    {
        public FinancePanel()
        {
            InitializeComponent();
        }

        private void FinancePanel_Load(object sender, EventArgs e)
        {
            loadSalesChart();
            loadSpendingChart();
            loadMonthlySpendingChart();
            loadMonthlyProfitChart();
        }

        private void loadSalesChart()
        {
            string query = @"SELECT 
                                 b.date AS SaleDate, 
                                 IFNULL(SUM(b.billAmount - b.discount), 0) AS InStoreSales, 
                                 IFNULL(SUM(o.amount - o.discount), 0) AS OnlineSales,
                                 (IFNULL(SUM(b.billAmount - b.discount), 0) + IFNULL(SUM(o.amount - o.discount), 0)) AS TotalSales
                             FROM 
                                 bill b
                             LEFT JOIN 
                                 order_tbl o ON b.date = o.date
                             GROUP BY 
                                 b.date";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime saleDate = reader.GetDateTime("SaleDate");
                        decimal inStoreSales = reader.GetDecimal("InStoreSales");
                        decimal onlineSales = reader.GetDecimal("OnlineSales");
                        decimal totalSales = reader.GetDecimal("TotalSales");

                        // Add data to the line chart
                        chart1.Series["InStoreSales"].Points.AddXY(saleDate, inStoreSales);
                        chart1.Series["OnlineSales"].Points.AddXY(saleDate, onlineSales);
                        chart1.Series["TotalSales"].Points.AddXY(saleDate, totalSales);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void loadSpendingChart()
        {
            // Define the SQL query for cumulative spending
            string query = @"SELECT 
                                 `date`,
                                 (SELECT IFNULL(SUM(`amount`), 0) 
                                  FROM `spending` AS s2
                                  WHERE s2.`date` <= s1.`date`) AS CumulativeTotal
                             FROM 
                                 `spending` AS s1
                             WHERE 
                                 `date` BETWEEN DATE_FORMAT(CURDATE(), '%Y-%m-01') AND LAST_DAY(CURDATE())
                             GROUP BY 
                                 `date`
                             ORDER BY 
                                 `date`";

            // Clear existing points in the series
            chart2.Series["TotalSpending"].Points.Clear();

            try
            {
                using (MySqlConnection connection = Connector.getConnection())
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime date = reader.GetDateTime("date");
                        decimal cumulativeTotal = reader.GetDecimal("CumulativeTotal");

                        // Add data to the existing line chart series
                        chart2.Series["TotalSpending"].Points.AddXY(date.Day, cumulativeTotal);
                    }
                }
            }
            catch (Exception ex)
            {
                // Display the error message for debugging
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadMonthlySpendingChart()
        {
            // Define the SQL query
            string query = @"SELECT 
                                 `type`, 
                                 IFNULL(SUM(`amount`), 0) AS TotalAmount
                             FROM 
                                 `spending`
                             WHERE 
                                 `date` BETWEEN DATE_FORMAT(CURDATE(), '%Y-%m-01') AND LAST_DAY(CURDATE())
                             GROUP BY 
                                 `type`";

            // Clear existing points in the series
            chart3.Series["Spending"].Points.Clear();

            try
            {
                using (MySqlConnection connection = Connector.getConnection())
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string type = reader.GetString("type");
                        decimal totalAmount = reader.GetDecimal("TotalAmount");

                        // Add data to the existing pie chart series
                        chart3.Series["Spending"].Points.AddXY(type, totalAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                // Display the error message for debugging
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadMonthlyProfitChart()
        {
            // Define the SQL query for monthly profit
            string query = @"SELECT
                                 DATE_FORMAT(S.date, '%Y-%m') AS Month,
                                 IFNULL(SUM(S.billSales), 0) AS TotalSales,
                                 IFNULL(SUM(Sp.spendingAmount), 0) AS TotalSpending,
                                 IFNULL(SUM(S.billSales), 0) - IFNULL(SUM(Sp.spendingAmount), 0) AS Profit
                             FROM
                                 (
                                     SELECT
                                         COALESCE(b.date, o.date) AS date,
                                         (SUM(b.billAmount - b.discount) + COALESCE(SUM(o.amount - o.discount), 0)) AS billSales
                                     FROM
                                         (SELECT date, billAmount, discount FROM bill) AS b
                                     LEFT JOIN
                                         (SELECT date, amount, discount FROM order_tbl) AS o
                                     ON b.date = o.date
                                     GROUP BY COALESCE(b.date, o.date)
                                 ) AS S
                             LEFT JOIN
                                 (
                                     SELECT
                                         date,
                                         SUM(amount) AS spendingAmount
                                     FROM
                                         spending
                                     GROUP BY date
                                 ) AS Sp
                             ON S.date = Sp.date
                             GROUP BY Month
                             ORDER BY Month";

            // Clear existing points in the series
            chart4.Series["MonthlyProfit"].Points.Clear();

            try
            {
                using (MySqlConnection connection = Connector.getConnection())
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string month = reader.GetString("Month");
                        decimal profit = reader.GetDecimal("Profit");

                        // Add data to the existing column chart series
                        chart4.Series["MonthlyProfit"].Points.AddXY(DateTime.Parse(month).Month, profit);
                    }
                }
            }
            catch (Exception ex)
            {
                // Display the error message for debugging
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            // Perform hit testing to check if a data point was clicked
            var result = chart1.HitTest(e.X, e.Y);

            // Check if the hit test result is on a data point
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                // Get the clicked data point
                DataPoint point = chart1.Series[result.Series.Name].Points[result.PointIndex];

                // Convert XValue to DateTime, assuming XValue is a DateTime (represented as OLE Automation date)
                DateTime dateValue = DateTime.FromOADate(point.XValue);

                // Get the YValue (in this case, the first Y value)
                double yValue = point.YValues[0];

                // Display the date (XValue) and the Y value
                MessageBox.Show($"Date: {dateValue.ToShortDateString()}, Value: {yValue}");
            }
        }

        private void chart2_MouseClick(object sender, MouseEventArgs e)
        {
            // Perform hit testing to check if a data point was clicked
            var result = chart2.HitTest(e.X, e.Y);

            // Check if the hit test result is on a data point
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                // Get the clicked data point
                DataPoint point = chart2.Series[result.Series.Name].Points[result.PointIndex];

                // Convert XValue to DateTime, assuming XValue is a DateTime (represented as OLE Automation date)
                DateTime dateValue = DateTime.FromOADate(point.XValue);

                // Get the YValue (in this case, the first Y value)
                double yValue = point.YValues[0];

                // Display the date (XValue) and the Y value
                MessageBox.Show($"Date: {dateValue.Day}, Value: {yValue}");
            }
        }

        private void chart3_MouseClick(object sender, MouseEventArgs e)
        {
            // Perform hit testing to check if a data point was clicked
            var result = chart3.HitTest(e.X, e.Y);

            // Check if the hit test result is on a data point
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                // Get the clicked data point
                DataPoint point = chart3.Series[result.Series.Name].Points[result.PointIndex];

                double yValue = point.YValues[0];

                // Display the date (XValue) and the Y value
                MessageBox.Show($"Value: {yValue}");
            }
        }

        private void chart4_MouseClick(object sender, MouseEventArgs e)
        {
            // Perform hit testing to check if a data point was clicked
            var result = chart4.HitTest(e.X, e.Y);

            // Check if the hit test result is on a data point
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                // Get the clicked data point
                DataPoint point = chart4.Series[result.Series.Name].Points[result.PointIndex];

                // Get the YValue (in this case, the first Y value)
                double xValue = point.XValue;
                double yValue = point.YValues[0];

                // Display the date (XValue) and the Y value
                MessageBox.Show($"Month: {xValue}, Value: {yValue}");
            }
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            SalesReportInput salesReport = new SalesReportInput();
            salesReport.ShowDialog();
        }

        private void btnExpenseReport_Click(object sender, EventArgs e)
        {
            ExpenseReportInput expenseReport = new ExpenseReportInput();
            expenseReport.ShowDialog();
        }

        private void btnProfit_Click(object sender, EventArgs e)
        {
            ReportDocument report = new ReportDocument();
            report.Load(@"D:\ClothingStoreSystem\InstoreSystem\InstoreSystem\Reports\YearlySales.rpt");

            report.SetParameterValue("storeId", 6);
            report.SetParameterValue("year", "2024");

            Reports rpt = new Reports(report);
            rpt.ShowDialog();
        }

        private void btnNewExpenses_Click(object sender, EventArgs e)
        {
            AddSpending spending = new AddSpending();
            spending.ShowDialog();
        }
    }
}
