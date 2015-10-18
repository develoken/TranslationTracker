using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TranslationTracker
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxDirectory.Text) ||
                string.IsNullOrWhiteSpace(tbxSearchString.Text))
            {
                MessageBox.Show("Please specify directory and search string",
                                "Warning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
            else
            {
                var task = await PopulateGridView();
            }
        }

        private async Task<bool> PopulateGridView()
        {
            StringBuilder resultString = new StringBuilder();
            gridView.Rows.Clear();

            try
            {
                foreach (List<string> items in ExcelReader.Instance.Load(tbxDirectory.Text, tbxSearchString.Text))
                {
                    resultString.Clear();

                    DataGridViewRow row = (DataGridViewRow)gridView.Rows[0].Clone();
                    foreach (string item in items)
                    {
                        resultString.Append(item + " ");
                    }
                    row.Cells[0].Value = resultString.ToString().Trim();
                    gridView.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return true;
        }
    }
}
