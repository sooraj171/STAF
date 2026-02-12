using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;

namespace STAF.CF.Excel
{
    public class ExcelDriver
    {
        public ExcelCompareStatus CompareFiles(string File1, string File2, int SheetIndexFile1 = 1, int SheetIndexFile2 = 1)
        {
            var compareStatus = new ExcelCompareStatus();
            if (!File.Exists(File1))
            {
                compareStatus.IsMatching = false;
                compareStatus.Messages.Add($"File 1 is not valid: {File1}");
                return compareStatus;
            }
            if (!File.Exists(File2))
            {
                compareStatus.IsMatching = false;
                compareStatus.Messages.Add($"File 2 is not valid: {File2}");
                return compareStatus;
            }
            XLWorkbook workbook1 = GetExcelWorkbook(File1);
            XLWorkbook workbook2 = GetExcelWorkbook(File2);
            return CompareExcelWorkbooks(workbook1, workbook2, SheetIndexFile1, SheetIndexFile2);

        }

        
        public XLWorkbook GetExcelWorkbook(string file, FileAccess fileAccessType= FileAccess.Read)
        {
            
            using (var stream = File.Open(file, FileMode.Open, fileAccessType))
            {
                return new XLWorkbook(stream);
            }
        }



        private ExcelCompareStatus CompareExcelWorkbooks(XLWorkbook workbook1, XLWorkbook workbook2, int SheetIndexFIle1 = 1, int SheetIndexFIle2 = 1)
        {
            var sheet1 = workbook1.Worksheet(SheetIndexFIle1);
            var sheet2 = workbook2.Worksheet(SheetIndexFIle2);

            int Sheet1Cnt = GetExcelRowCount(sheet1);
            int Sheet2Cnt = GetExcelRowCount(sheet2);

            int Sheet1ColCnt = GetExcelColumnCount(sheet1);
            int Sheet2ColCnt = GetExcelColumnCount(sheet2);
            List<string> ListOfDiff = new();

            if (Sheet1Cnt == Sheet2Cnt && Sheet1ColCnt == Sheet2ColCnt)
            {
                // Compare the cells of the two sheets
                ListOfDiff = CompareExcelSheets(sheet1, sheet2);
            }
            else
            {
                if (Sheet1Cnt != Sheet2Cnt)
                { 
                ListOfDiff.Add("Row counts are not matching between two sheets.");
                }
                if (Sheet1ColCnt != Sheet2ColCnt)
                {
                    ListOfDiff.Add("Column counts are not matching between two sheets.");
                }
            }

            ExcelCompareStatus compareStatus = new();
            if (ListOfDiff.Count > 0)
            {
                compareStatus.IsMatching = false;
                compareStatus.Messages = ListOfDiff;
                //return ListOfDiff;
            }
            else
            {
                ListOfDiff.Add("Both the files are matching");
                compareStatus.IsMatching=true;
                compareStatus.Messages = ListOfDiff;
                
            }
            
            return compareStatus;
        }

        public string GetExcelCellData(XLWorkbook workbook, int sheetNumber, int row, int col)
        {
            var sheet = workbook.Worksheet(sheetNumber);
            var cell = sheet.Cell(row, col);
            return cell.Value.ToString();
        }

        public void SetExcelCellData(XLWorkbook workbook, int sheetNumber, int row, int col, string StrValue)
        {
            var sheet = workbook.Worksheet(sheetNumber);
            var cell = sheet.Cell(row, col);
            cell.Value = StrValue;
        }

        public int GetExcelRowCount(XLWorkbook Workbook, int SheetIndex = 1)
        {

            return Workbook.Worksheet(SheetIndex).LastRowUsed().RowNumber();
        }

        public int GetExcelRowCount(IXLWorksheet Worksheet)
        {

            return Worksheet.LastRowUsed().RowNumber();
        }

        public int GetExcelColumnCount(IXLWorksheet Worksheet)
        {

            return Worksheet.LastColumnUsed().ColumnNumber();
        }

        private List<string> CompareExcelSheets(IXLWorksheet Sheet1, IXLWorksheet Sheet2)
        {

            List<string> differences = new List<string>();

            // Compare the cells of the two sheets
            for (int row = 1; row <= GetExcelRowCount(Sheet1); row++)
            {
                for (int col = 1; col <= GetExcelColumnCount(Sheet1); col++)
                {
                    var cell1 = Sheet1.Cell(row, col);
                    var cell2 = Sheet2.Cell(row, col);

                    if (!cell1.Value.Equals(cell2.Value))
                    {
                        differences.Add($"Difference found at row {row}, column {col}");
                    }
                }
            }
            return differences;
        }
    }

    public class ExcelCompareStatus
    {
        public bool IsMatching { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
