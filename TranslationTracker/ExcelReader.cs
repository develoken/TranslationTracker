using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace TranslationTracker
{
    public class ExcelReader
    {
        private Application application;

        private static volatile ExcelReader instance;
        private static object syncRoot = new Object();

        private ExcelReader() 
        {
            application = new Application();
        }
        
        public static ExcelReader Instance
        {
            get 
            {
                if (instance == null) 
                {
                    lock (syncRoot) 
                    {
                        if (instance == null)
                            instance = new ExcelReader();
                    }
                }
                return instance;
            }
        }

        public IEnumerable<Tuple<string, string>> Load(string filePath, string searchString)
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            StringBuilder first = new StringBuilder();
            StringBuilder second = new StringBuilder();

            Workbook workbook = application.Workbooks.Open(filePath);
            try
            {
                foreach (Worksheet worksheet in workbook.Worksheets)
                {
                    int col = worksheet.UsedRange.Columns.Count;
                    int row = worksheet.UsedRange.Rows.Count;

                    for (int i = 1; i <= row; i++)
                    {
                        first.Clear();
                        second.Clear();

                        for (int j = 1; j <= col; j++)
                        {
                            var value = worksheet.Cells[i, j].Value;

                            if (value != null)
                            {
                                string stringValue = value.ToString();
                                if (stringValue.Contains(searchString))
                                {
                                    if (first.Length == 0)
                                    {
                                        first.Insert(0, stringValue);
                                    }
                                }
                                else if (first.Length != 0 && second.Length == 0)
                                {
                                    second.Insert(0, stringValue);
                                    yield return new Tuple<string, string>(first.ToString(), second.ToString());
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    Marshal.ReleaseComObject(worksheet);
                }
            }
            finally
            {
                workbook.Close();
            }
        }

    }
}
