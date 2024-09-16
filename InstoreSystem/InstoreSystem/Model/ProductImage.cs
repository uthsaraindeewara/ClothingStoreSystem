using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InstoreSystem.Model
{
    internal class ProductImage
    {
        private int imageID;
        private string name;
        private string path;
        private int productID;

        // Constructor called when creating a object of a new image
        public ProductImage(string name, string path, int productID)
        {
            this.name = name;
            this.path = path;
            this.productID = productID;
        }

        // Constructor called when creating a object of a already existing image
        public ProductImage(String Imagepath)
        {
            using (MySqlConnection connection = Connector.getConnection())
            {
                string sql = "SELECT imageID, name, path, productID FROM image WHERE path = @path";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@path", Imagepath);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // If the image exists
                            {
                                this.imageID = reader.GetInt32("imageID");
                                this.name = reader.GetString("name");
                                this.path = reader.GetString("path");
                                this.productID = reader.GetInt32("productID");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (log it, rethrow it, etc.)
                        Console.WriteLine($"Error loading image details: {ex.Message}");
                    }
                }
            }
        }

        public void addImage()
        {
            using (MySqlConnection connection = Connector.getConnection())
            {
                string sql = "INSERT INTO image (name, path, productID) VALUES (@Name, @Path, @ProductID)";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Path", path);
                    command.Parameters.AddWithValue("@ProductID", productID);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error");
                    }
                }
            }
        }

        public void deleteImage()
        {
            using (MySqlConnection connection = Connector.getConnection())
            {
                string sql = "DELETE FROM image WHERE imageID = @imageId";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@imageId", imageID);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Check if any rows were affected (i.e., image deleted)
                        if (result > 0)
                        {
                            Console.WriteLine("Image deleted successfully from the database.");
                        }
                        else
                        {
                            Console.WriteLine("Image not found in the database.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error or display it
                        Console.WriteLine($"Error deleting image from database: {ex.Message}");
                        throw; // Re-throw the exception if needed
                    }
                }
            }
        }
    }
}
