using System;
using System.IO;
using System.Windows.Forms;

namespace Pick_Client_lab1
{
    public partial class Form1 : Form
    {
        private string _currentFilePath;
        private const string _publicFolderPath = "C:\\Users\\Public\\ForHacker";

        public Form1()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    PathTB.Text = fbd.SelectedPath;
            }
        }

        private void ToPublic_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath) || !Directory.Exists(_publicFolderPath))
                return;

            var publicFilePath = Path.Combine(_publicFolderPath, FileName.Text + ".txt");

            string data = File.ReadAllText(_currentFilePath);
            File.WriteAllText(publicFilePath, data);

            MessageBox.Show("Data transfered to public folder.");
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileName.Text) || string.IsNullOrEmpty(Content.Text) || string.IsNullOrEmpty(PathTB.Text))
            {
                MessageBox.Show("Check input data.");
                return;
            }

            _currentFilePath = Path.Combine(PathTB.Text, FileName.Text + ".txt");

            File.WriteAllText(_currentFilePath, Content.Text);

            MessageBox.Show("Data saved.");
        }
    }
}
