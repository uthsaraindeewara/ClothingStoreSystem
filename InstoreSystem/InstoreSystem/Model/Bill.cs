using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace InstoreSystem.Model
{
    internal class Bill
    {
        private int billId;
        private TimeSpan time;
        private DateTime date;
        private decimal billAmount;
        private decimal discount;

        // Constructor to create a new bill
        public Bill(TimeSpan time, DateTime date, decimal billAmount, decimal discount)
        {
            this.time = time;
            this.date = date;
            this.billAmount = billAmount;
            this.discount = discount;
        }

        // Constructor to retrieve an existing bill by ID
        public Bill(int billId)
        {
            string query = "SELECT bill_id, time, date, billAmount, discount FROM bill WHERE bill_id = @billId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@billId", billId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.billId = Convert.ToInt32(dr["bill_id"]);
                        this.time = (TimeSpan)dr["time"];
                        this.date = Convert.ToDateTime(dr["date"]);
                        this.billAmount = Convert.ToDecimal(dr["billAmount"]);
                        this.discount = Convert.ToDecimal(dr["discount"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add a new bill to the database
        public void addBill()
        {
            string query = "INSERT INTO bill (time, date, billAmount, discount) " +
                           "VALUES (@time, @date, @billAmount, @discount)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@time", time);
                    com.Parameters.AddWithValue("@date", date);
                    com.Parameters.AddWithValue("@billAmount", billAmount);
                    com.Parameters.AddWithValue("@discount", discount);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Bill added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update an existing bill in the database
        public void updateBill()
        {
            string query = "UPDATE bill SET " +
                           "time = @time, " +
                           "date = @date, " +
                           "billAmount = @billAmount, " +
                           "discount = @discount " +
                           "WHERE bill_id = @billId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@billId", billId);
                    com.Parameters.AddWithValue("@time", time);
                    com.Parameters.AddWithValue("@date", date);
                    com.Parameters.AddWithValue("@billAmount", billAmount);
                    com.Parameters.AddWithValue("@discount", discount);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Bill updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating bill unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Method to get bill details as a dictionary
        public Dictionary<string, string> getBillDetails()
        {
            Dictionary<string, string> billDetails = new Dictionary<string, string>()
            {
                { "billId", this.billId.ToString() },
                { "time", this.time.ToString() },
                { "date", this.date.ToString("yyyy-MM-dd") },
                { "billAmount", this.billAmount.ToString("F2") },
                { "discount", this.discount.ToString("F2") }
            };

            return billDetails;
        }

        // Getters and Setters
        public int getBillId() => billId;
        public void setBillId(int billId) => this.billId = billId;

        public TimeSpan getTime() => time;
        public void setTime(TimeSpan time) => this.time = time;

        public DateTime getDate() => date;
        public void setDate(DateTime date) => this.date = date;

        public decimal getBillAmount() => billAmount;
        public void setBillAmount(decimal billAmount) => this.billAmount = billAmount;

        public decimal getDiscount() => discount;
        public void setDiscount(decimal discount) => this.discount = discount;
    }
}
