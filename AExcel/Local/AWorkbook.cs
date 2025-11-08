using AExcel.Interface;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AExcel.Local
{
    internal class AWorkbook : IWorkbook, IDisposable
    {
        public IWorksheet this[string sheetname] => worksheets.FirstOrDefault(x => x.Name == sheetname);

        public IEnumerable<IWorksheet> Worksheets => worksheets;

        public WorkbookPart? WorkbookPart => document.WorkbookPart;

        AWorksheet[]? worksheets;
        SpreadsheetDocument document;

        public AWorkbook(SpreadsheetDocument doc)
        {
            document = doc;

            SetWorkbookPart();
            SetWorkSheets();
        }

        private void SetWorkbookPart()
        {
            if (document.WorkbookPart != null) return;

            document.AddWorkbookPart();
            document.WorkbookPart.Workbook = new Workbook();

            var sharedStringTablePart = document.WorkbookPart.AddNewPart<SharedStringTablePart>();
            sharedStringTablePart.SharedStringTable = new SharedStringTable();

            WorksheetPart newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());


            Sheets sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>() ?? document.WorkbookPart.Workbook.AppendChild(new Sheets());
            string relationshipId = document.WorkbookPart.GetIdOfPart(newWorksheetPart);

            // Get a unique ID for the new worksheet.
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = (sheets.Elements<Sheet>().Select(s => s.SheetId?.Value).Max() + 1) ?? (uint)sheets.Elements<Sheet>().Count() + 1;
            }

            // Give the new worksheet a name.
            string sheetName = "Sheet" + sheetId;

            // Append the new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
        }

        private void SetWorkSheets()
        {
            int sheetCount = WorkbookPart.WorksheetParts.Count();
            worksheets = new AWorksheet[sheetCount];

            var sheetList = WorkbookPart.Workbook.Descendants<Sheet>();
            int k = 0;
            foreach (var sheet in sheetList)
            {
                var worksheetpart = WorkbookPart.GetPartById(sheet.Id) as WorksheetPart;

                var ws = new AWorksheet(worksheetpart, this);
                ws.Name = sheet.Name;

                worksheets[k++] = ws;
            }
        }

        public void Dispose()
        {
            document.Dispose();
        }

        public bool Save()
        {
            var isSuccuessed = false;
            try
            {
                worksheets.ToList().ForEach(x => x.Save());
                WorkbookPart.Workbook.Save();
                isSuccuessed = true;
            }
            catch (Exception ex)
            {

            }

            return isSuccuessed;
        }
    }
}
