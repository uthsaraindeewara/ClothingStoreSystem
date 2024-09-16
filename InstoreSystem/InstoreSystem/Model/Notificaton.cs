using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstoreSystem.Model;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Model
{
    internal class Notification
    {
        protected int notificationId;
        protected string type;
        protected TimeSpan time;
        protected string description;
        protected DateTime date;

        // Constructor called when a new notification is created
        public Notification(int notificationId, string type, TimeSpan time, string description, DateTime date)
        {
            this.notificationId = notificationId;
            this.type = type;
            this.time = time;
            this.description = description;
            this.date = date;
        }

        // Constructor called when an existing notification is retrieved
        public Notification(int notificationId)
        {
            string query = "SELECT notificationID, type, time, description, date FROM notification WHERE notificationID = @notificationId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@notificationId", notificationId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.notificationId = Convert.ToInt32(dr["notificationID"]);
                        this.type = dr["type"].ToString();
                        this.time = TimeSpan.Parse(dr["time"].ToString());
                        this.description = dr["description"].ToString();
                        this.date = Convert.ToDateTime(dr["date"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new notification
        public void addNotification()
        {
            string query = "INSERT INTO notification (notificationID, type, time, description, date) " +
                           "VALUES (@notificationId, @type, @time, @description, @date)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@notificationId", notificationId);
                    com.Parameters.AddWithValue("@type", type);
                    com.Parameters.AddWithValue("@time", time);
                    com.Parameters.AddWithValue("@description", description);
                    com.Parameters.AddWithValue("@date", date);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Notification added successfully", "Information");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update the notification
        public void updateNotification()
        {
            string query = "UPDATE notification SET " +
                           "type = @type, " +
                           "time = @time, " +
                           "description = @description, " +
                           "date = @date " +
                           "WHERE notificationID = @notificationId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@notificationId", notificationId);
                    com.Parameters.AddWithValue("@type", type);
                    com.Parameters.AddWithValue("@time", time);
                    com.Parameters.AddWithValue("@description", description);
                    com.Parameters.AddWithValue("@date", date);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Notification updated successfully", "Information");
                    }
                    else
                    {
                        MessageBox.Show("Updating notification unsuccessful", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getNotificationDetails()
        {
            Dictionary<String, String> notificationDetails = new Dictionary<String, String>()
            {
                { "notificationId", this.notificationId.ToString() },
                { "type", this.type },
                { "time", this.time.ToString(@"hh\:mm\:ss") },
                { "description", this.description },
                { "date", this.date.ToString("yyyy-MM-dd") }
            };

            return notificationDetails;
        }

        // Getter for Notification ID
        public int getNotificationId()
        {
            return notificationId;
        }

        // Setter for Notification ID
        public void setNotificationId(int notificationId)
        {
            this.notificationId = notificationId;
        }

        // Getter for Type
        public string getType()
        {
            return type;
        }

        // Setter for Type
        public void setType(string type)
        {
            this.type = type;
        }

        // Getter for Time
        public TimeSpan getTime()
        {
            return time;
        }

        // Setter for Time
        public void setTime(TimeSpan time)
        {
            this.time = time;
        }

        // Getter for Description
        public string getDescription()
        {
            return description;
        }

        // Setter for Description
        public void setDescription(string description)
        {
            this.description = description;
        }

        // Getter for Date
        public DateTime getDate()
        {
            return date;
        }

        // Setter for Date
        public void setDate(DateTime date)
        {
            this.date = date;
        }
    }
}

