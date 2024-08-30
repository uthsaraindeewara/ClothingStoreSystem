using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstoreSystem.Employees
{
    internal class Employee
    {
        string cs = @"Server=localhost; Port=3307; Database=studentsdb; User Id=root;";

        protected int employeeId;
        protected string employeeName;
        protected string address;
        protected string contactNo;
        protected DateTime dOB;
        protected string gender;
        protected int storeId;
        protected int salary;

        public Employee(int employeeId, string employeeName, string address, string contactNo, DateTime dOB, string gender, int storeId, int salary)
        {
            this.employeeId = employeeId;
            this.employeeName = employeeName;
            this.address = address;
            this.contactNo = contactNo;
            this.dOB = dOB;
            this.gender = gender;
            this.storeId = storeId;
            this.salary = salary;
        }
        public void addEmployee()
        {
            string query = "INSERT INTO employees (employeeId, employeeName, address, contactNo, dOB, gender, storeId, salary) " +
                           "VALUES (@employeeId, @employeeName, @address, @contactNo, @dOB, @gender, @storeId, @salary)";

            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();
                SqlCommand com = new SqlCommand(query, connection);

                com.Parameters.AddWithValue("@employeeId", employeeId);
                com.Parameters.AddWithValue("@employeeName", employeeName);
                com.Parameters.AddWithValue("@address", address);
                com.Parameters.AddWithValue("@contactNo", contactNo);
                com.Parameters.AddWithValue("@dOB", dOB);
                com.Parameters.AddWithValue("@gender", gender);
                com.Parameters.AddWithValue("@storeId", storeId);
                com.Parameters.AddWithValue("@salary", salary);

                if (com.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Employee added Successfully", "Information");
                }
            }
        }

        public void updateEmployee()
        {
            string query = "UPDATE employees SET " +
                           "name = @employeeName, " +
                           "address = @Address, " +
                           "contactNo = @ContactNo, " +
                           "dOB = @DOB, " +
                           "gender = @Gender, " +
                           "storeId = @StoreId, " +
                           "salary = @Salary " +
                           "WHERE id = @employeeId";

            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();
                SqlCommand com = new SqlCommand(query, connection);

                com.Parameters.AddWithValue("@employeeId", employeeId);
                com.Parameters.AddWithValue("@employeeName", employeeName);
                com.Parameters.AddWithValue("@address", address);
                com.Parameters.AddWithValue("@contactNo", contactNo);
                com.Parameters.AddWithValue("@dOB", dOB);
                com.Parameters.AddWithValue("@gender", gender);
                com.Parameters.AddWithValue("@storeId", storeId);
                com.Parameters.AddWithValue("@salary", salary);

                if (com.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Employee updated successfully", "Information");
                }
                else
                {
                    MessageBox.Show("Employee update failed", "Error");
                }
            }
        }

        public void LoadEmployeeById(int employeeId)
        {
            string query = "SELECT id, name, address, contactNo, dOB, gender, storeId, salary FROM employees WHERE id = @employeeId";

            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();
                SqlCommand com = new SqlCommand(query, connection);
                com.Parameters.AddWithValue("@employeeId", employeeId);

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("employeeId"));
                        string name = reader.GetString(reader.GetOrdinal("employeeName"));
                        string address = reader.GetString(reader.GetOrdinal("address"));
                        string contactNo = reader.GetString(reader.GetOrdinal("contactNo"));
                        DateTime dOB = reader.GetDateTime(reader.GetOrdinal("dOB"));
                        string gender = reader.GetString(reader.GetOrdinal("gender"));
                        int storeId = reader.GetInt32(reader.GetOrdinal("storeId"));
                        int salary = reader.GetInt32(reader.GetOrdinal("salary"));
                    }
                }
            }
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
        public int getSalary()
        {
            return salary;
        }

        // Setter for Salary
        public void setSalary(int salary)
        {
            this.salary = salary;
        }
    }
}
