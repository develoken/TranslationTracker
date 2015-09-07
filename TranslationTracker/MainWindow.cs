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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string rootPath = tbxDirectory.Text;
            string searchString = tbxSearchString.Text;

            gridView.Rows.Clear();
            foreach(string path in Directory.GetFiles(rootPath, "*.xls", SearchOption.AllDirectories))
            {
                try
                {
                    foreach (Tuple<string, string> item in ExcelReader.Instance.Load(path.Trim(), searchString))
                    {
                        DataGridViewRow row = (DataGridViewRow)gridView.Rows[0].Clone();
                        row.Cells[0].Value = item.Item1;
                        row.Cells[1].Value = item.Item2;

                        gridView.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    string temp = ex.Message;
                }
            }
        }
    }
}
