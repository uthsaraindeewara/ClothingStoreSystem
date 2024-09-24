using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;
using WindowsFormsApp10;

namespace InstoreSystem.Employees
{
    internal class Employee
    {
        protected int employeeId;
        protected string employeeName;
        protected string address;
        protected string contactNo;
        protected DateTime dOB;
        protected string gender;
        protected int storeId;
        protected double salary;

        // Constructor called when a object of a new employee is created
        public Employee(string employeeName, string address, string contactNo, DateTime dOB, string gender, double salary, int storeId)
        {
            this.employeeName = employeeName;
            this.address = address;
            this.contactNo = contactNo;
            this.dOB = dOB;
            this.gender = gender;
            this.storeId = storeId;
            this.salary = salary;
        }

        // Constructor called when a object of a already excisting employee is created
        public Employee(int employeeId)
        {
            string query = "SELECT employeeID, employeeName,  address, contactNo, dOB, gender, salary, storeID FROM employee WHERE employeeID = @employeeId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);
                    com.Parameters.AddWithValue("@employeeId", employeeId);
                    MySqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        this.employeeId = Convert.ToInt32(dr["employeeID"]);
                        this.employeeName = dr["employeeName"].ToString();
                        this.contactNo = dr["contactNo"].ToString();
                        this.address = dr["address"].ToString();
                        this.dOB = Convert.ToDateTime(dr["dOB"]);
                        this.gender = dr["gender"].ToString();
                        this.salary = Convert.ToDouble(dr["salary"]);
                        this.storeId = Convert.ToInt32(dr["storeID"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Add new employee
        public void addEmployee(int admin_id)
        {
            string query = "INSERT INTO employee (employeeName, contactNo, address, dOB, gender, salary, storeID, adminID) " +
                           "VALUES (@employeeName, @contactNo, @address, @dOB, @gender, @salary, @storeId, @adminId)";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@employeeName", employeeName);
                    com.Parameters.AddWithValue("@contactNo", contactNo);
                    com.Parameters.AddWithValue("@address", address);
                    com.Parameters.AddWithValue("@dOB", dOB);
                    com.Parameters.AddWithValue("@gender", gender);
                    com.Parameters.AddWithValue("@salary", salary);
                    com.Parameters.AddWithValue("@storeId", storeId);
                    com.Parameters.AddWithValue("@adminId", admin_id);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        SuccessMessage message = new SuccessMessage("Employee Added Successfully");
                        message.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Update the employee
        public void updateEmployee()
        {
            string query = "UPDATE employee SET " +
                           "name = @employeeName, " +
                           "contactNo = @ContactNo, " +
                           "address = @Address, " +
                           "dOB = @DOB, " +
                           "gender = @Gender, " +
                           "salary = @Salary " +
                           "storeID = @StoreId, " +
                           "WHERE id = @employeeId";

            using (MySqlConnection connection = Connector.getConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand com = new MySqlCommand(query, connection);

                    com.Parameters.AddWithValue("@employeeId", employeeId);
                    com.Parameters.AddWithValue("@employeeName", employeeName);
                    com.Parameters.AddWithValue("@contactNo", contactNo);
                    com.Parameters.AddWithValue("@address", address);
                    com.Parameters.AddWithValue("@dOB", dOB);
                    com.Parameters.AddWithValue("@gender", gender);
                    com.Parameters.AddWithValue("@salary", salary);
                    com.Parameters.AddWithValue("@storeId", storeId);

                    if (com.ExecuteNonQuery() > 0)
                    {
                        SuccessMessage message = new SuccessMessage("Employee Updaed Successfully");
                        message.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Updating employee unsuccessfull", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        // Return details as a dictionary of strings
        public Dictionary<String, String> getEmployeeDetails()
        {
            Dictionary<String, String> employeeDetails = new Dictionary<String, String>()
            {
                { "employeeId", this.employeeId.ToString() },
                { "employeeName", this.employeeName },
                { "contactNo", this.contactNo },
                { "address", this.address },
                { "dOB", this.dOB.ToString() },
                { "gender", this.gender },
                { "salary", this.salary.ToString() },
                { "storeId", this.storeId.ToString() }
            };

            return employeeDetails;
        }

        // Getter for Id
        public int getId()
        {
            return employeeId;
        }

        // Setter for Id
        public void setId(int employeeId)
        {
            this.employeeId = employeeId;
        }

        // Getter for Name
        public string getName()
        {
            return employeeName;
        }

        // Setter for Name (converts name to uppercase)
        public void setName(string employeeName)
        {
            this.employeeName = employeeName;
        }

        // Getter for Address
        public string getAddress()
        {
            return address;
        }

        // Setter for Address (capitalizes each word in the address)
        public void setAddress(string address)
        {
            this.address = address;
        }

        // Getter for ContactNumber
        public string getContactNo()
        {
            return contactNo;
        }

        // Setter for ContactNumber
        public void setContactNo(string contactNo)
        {
            this.contactNo = contactNo;
        }

        // Getter for DateOfBirth
        public DateTime getdOB()
        {
            return dOB;
        }

        // Setter for DateOfBirth
        public void setdOB(DateTime dateOfBirth)
        {
            this.dOB = dateOfBirth;
        }

        // Getter for Gender
        public string getGender()
        {
            return gender;
        }

        // Setter for Gender
        public void setGender(string gender)
        {
            this.gender = gender;
        }

        // Getter for Store
        public int getStoreId()
        {
            return storeId;
        }

        // Setter for Store
        public void setStoreId(int storeId)
        {
            this.storeId = storeId;
        }

        // Getter for Salary
        public double getSalary()
        {
            return salary;
        }

        // Setter for Salary
        public void setSalary(double salary)
        {
            this.salary = salary;
        }
    }
}
