using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstoreSystem.Model;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace InstoreSystem.Interface
{
    public partial class ImageManager : Form
    {
        int productId;

        public ImageManager(int productId)
        {
            InitializeComponent();
            this.productId = productId;
        }

        private void ImageManager_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            using (MySqlConnection connection = Connector.getConnection())
            {
                string sql = "SELECT imageID, name, path, productID FROM image WHERE productID = @productId";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string imagePath = reader.GetString("path");

                                // Create a panel to hold the PictureBox and the Button
                                Panel panel = new Panel
                                {
                                    Width = 220,
                                    Height = 390
                                };

                                // Load the image using a MemoryStream to avoid locking the file
                                System.Drawing.Image image;
                                using (var stream = new MemoryStream(File.ReadAllBytes(imagePath)))
                                {
                                    image = System.Drawing.Image.FromStream(stream);
                                }

                                // Create and configure the PictureBox
                                PictureBox pictureBox = new PictureBox
                                {
                                    Image = image,
                                    SizeMode = PictureBoxSizeMode.StretchImage,
                                    Width = 200,
                                    Height = 240,
                                    Padding = new Padding(5)
                                };

                                // Create and configure the Delete button
                                Button deleteButton = new Button
                                {
                                    Text = "Delete",
                                    Width = 200,
                                    Height = 40
                                };

                                // Handle PictureBox Click event to enlarge the image
                                pictureBox.Click += (s, eventArgs) => {
                                    Form enlargedForm = new Form();
                                    enlargedForm.Text = Path.GetFileName(imagePath);
                                    enlargedForm.Width = 800;
                                    enlargedForm.Height = 600;
                                    PictureBox enlargedPictureBox = new PictureBox();
                                    enlargedPictureBox.Image = System.Drawing.Image.FromFile(imagePath);
                                    enlargedPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                    enlargedPictureBox.Dock = DockStyle.Fill;
                                    enlargedForm.Controls.Add(enlargedPictureBox);
                                    enlargedForm.Show();
                                };

                                // Handle delete button click
                                deleteButton.Click += (s, eventArgs) => {
                                    try
                                    {
                                        // Dispose the image in the PictureBox
                                        pictureBox.Image?.Dispose();
                                        pictureBox.Image = null;

                                        // Create a ProductImage object using the image path (this will load the image details from the database)
                                        ProductImage imageToDelete = new ProductImage(imagePath);

                                        // Delete the image from the database
                                        imageToDelete.deleteImage();

                                        // Delete the image from the file system
                                        if (File.Exists(imagePath))
                                        {
                                            File.Delete(imagePath);

                                            // Remove the panel from the FlowLayoutPanel
                                            flowLayoutPanel1.Controls.Remove(panel);
                                            panel.Dispose(); // Dispose of the panel
                                            MessageBox.Show("Image successfully deleted from both the database and file system!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Image file not found, but it was deleted from the database.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Error deleting image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                };

                                // Add the PictureBox and Button to the panel
                                panel.Controls.Add(pictureBox);
                                panel.Controls.Add(deleteButton);

                                // Position the button below the PictureBox
                                deleteButton.Top = pictureBox.Bottom + 5;

                                // Add the panel to the FlowLayoutPanel
                                flowLayoutPanel1.Controls.Add(panel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error fetching image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Get the file path of the selected image
                string selectedImagePath = openFileDialog1.FileName;

                // Save the image to the application's ProductImages folder
                string imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\ProductImages");
                string fileName = Path.GetFileName(selectedImagePath);
                string destinationPath = Path.Combine(imageDirectory, fileName);

                if (File.Exists(destinationPath))
                {
                    MessageBox.Show("An image with this name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ProductImage image = new ProductImage(fileName, destinationPath, productId);
                image.addImage();

                try
                {
                    // Copy the selected image to the destination directory
                    File.Copy(selectedImagePath, destinationPath, true);
                    MessageBox.Show("Image successfully added and saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                ImageManager_Load(this, null);
            }
        }
    }
}
