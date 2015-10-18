using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace TranslationTracker
{
    public class ExcelReader : IDisposable
    {
        private Application application;
        private Workbook workbook;

        private static volatile ExcelReader instance;
        private static object syncRoot = new Object();

        private ExcelReader() 
        {
            application = new Application();
        }

        public void Dispose()
        {
            if (workbook != null)
            {
                workbook.Close();
            }
            if (application != null)
            {
                application.Quit();
            }
            Marshal.FinalReleaseComObject(workbook);
            Marshal.FinalReleaseComObject(application);
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
        
        public IEnumerable<List<string>> Load(string rootPath, string searchString)
        {
            foreach (string filePath in DirectoryHelper.Instance.FindXlsFiles(rootPath))
            {
                foreach (List<string> list in LoadFromFile(filePath, searchString))
                {
                    yield return list;
                }
            }
        }

        private IEnumerable<List<string>> LoadFromFile(string filePath, string searchString)
        {
            if (File.Exists(filePath))
            {
                bool found = false;
                List<string> list = new List<string>();
                try
                {
                    workbook = application.Workbooks.Open(filePath);

                    foreach (Worksheet worksheet in workbook.Worksheets)
                    {
                        for (int i = 1; i <= worksheet.UsedRange.Rows.Count; i++)
                        {
                            found = false;
                            list.Clear();

                            for (int j = 1; j <= worksheet.UsedRange.Columns.Count; j++)
                            {
                                var value = worksheet.Cells[i, j].Value;

                                if (value != null)
                                {
                                    string stringValue = value.ToString();
                                    list.Add(stringValue);

                                    if (stringValue.Contains(searchString))
                                    {
                                        found = true;
                                    }
                                }
                            }

                            if (found)
                            {
                                yield return list;
                            }
                        }
                        Marshal.FinalReleaseComObject(worksheet);
                    }
                }
                finally
                {
                    Marshal.FinalReleaseComObject(workbook.Worksheets);
                    if (workbook != null)
                    {
                        workbook.Close();
                    }
                    Marshal.FinalReleaseComObject(workbook);
                    GC.Collect();
                }
            }
        }
    }
}
