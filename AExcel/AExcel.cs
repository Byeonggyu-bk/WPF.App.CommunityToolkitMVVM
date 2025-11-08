using AExcel.Interface;
using AExcel.Local;
using DocumentFormat.OpenXml.Packaging;

namespace AExcel
{
    public static class AExcel
    {
        public static void Create(string path)
        {
            using (var doc = SpreadsheetDocument.Create(path, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
            {
                var workbook = new AWorkbook(doc);

                workbook.Save();
            }
        }

        public static IWorkbook Open(string path)
        {
            IWorkbook result = null;

            // Open
            var doc = SpreadsheetDocument.Open(path, true, new OpenSettings() { AutoSave = false });
            if(doc != null)
            {
                result = new AWorkbook(doc);
            }

            return result;
        }
    }
}
