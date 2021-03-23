using BakeryWebApp.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.SaveReports
{
    public class SaveReportToExcel
    {
        string path = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\Reports";
        private void SaveDailyMaterialReport(List<MaterialReportViewModel> dailyMaterialReportViewModels, string newPath)
        {
            newPath += "\\Material.xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(new FileInfo(newPath));
            package.Workbook.Worksheets.Add("Material");
            if(package.Workbook.Worksheets[0].Dimension == null)
            {
                package.Workbook.Worksheets[0].Cells[1, 1].Value = "Name";
                package.Workbook.Worksheets[0].Cells[1, 2].Value = "Quantity";
            }
            int columnEnd = package.Workbook.Worksheets[0].Dimension.End.Row;
            foreach(MaterialReportViewModel d in dailyMaterialReportViewModels)
            {
                columnEnd++;
                package.Workbook.Worksheets[0].Cells[columnEnd, 1].Value = d.MaterialName;
                package.Workbook.Worksheets[0].Cells[columnEnd, 2].Value = d.MaterialQuantity;
            }
            File.WriteAllBytes(newPath, package.GetAsByteArray());
        }

        private void SaveDailyRawReport(List<RawReportViewModel> dailyRawReportViewModels, string newPath)
        {
            newPath += "\\Raw.xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(new FileInfo(newPath));
            package.Workbook.Worksheets.Add("Raw");
            if (package.Workbook.Worksheets[0].Dimension == null)
            {
                package.Workbook.Worksheets[0].Cells[1, 1].Value = "Name";
                package.Workbook.Worksheets[0].Cells[1, 2].Value = "Quantity";
            }
            int columnEnd = package.Workbook.Worksheets[0].Dimension.End.Row;
            foreach (RawReportViewModel d in dailyRawReportViewModels)
            {
                columnEnd++;
                package.Workbook.Worksheets[0].Cells[columnEnd, 1].Value = d.RawName;
                package.Workbook.Worksheets[0].Cells[columnEnd, 2].Value = d.RawQuantity;
            }
            File.WriteAllBytes(newPath, package.GetAsByteArray());
        }
        public void FormDailyReport(DateTime date, List<RawReportViewModel> dailyRawReportViewModels, List<MaterialReportViewModel> dailyMaterialReportViewModels)
        {
            string dirName = date.ToString("D");
            string newPath = path + "\\" + dirName;
            if (Directory.Exists(newPath))
            {
                File.Delete(newPath + "\\Material.xlsx");
                File.Delete(newPath + "\\Raw.xlsx");
            }
            else
                Directory.CreateDirectory(newPath);
            SaveDailyMaterialReport(dailyMaterialReportViewModels, newPath);
            SaveDailyRawReport(dailyRawReportViewModels, newPath);
        }

        public void FormPeriodReport(DateTime beginDate, DateTime endDate, List<RawReportViewModel> dailyRawReportViewModels, List<MaterialReportViewModel> dailyMaterialReportViewModels)
        {
            string dirName = beginDate.ToString("D") + " - " + endDate.ToString("D");
            string newPath = path + "\\" + dirName;
            if (Directory.Exists(newPath))
            {
                File.Delete(newPath + "\\Material.xlsx");
                File.Delete(newPath + "\\Raw.xlsx");
            }
            else
                Directory.CreateDirectory(newPath);
            SaveDailyMaterialReport(dailyMaterialReportViewModels, newPath);
            SaveDailyRawReport(dailyRawReportViewModels, newPath);
        }
        public void FormStatistic(List<RawReportViewModel> dailyRawReportViewModels, List<MaterialReportViewModel> dailyMaterialReportViewModels)
        {
            string dirName = "Statistic";
            string newPath = path + "\\" + dirName;
            if (Directory.Exists(newPath))
            {
                File.Delete(newPath + "\\Material.xlsx");
                File.Delete(newPath + "\\Raw.xlsx");
            }
            else
                Directory.CreateDirectory(newPath);
            SaveDailyMaterialReport(dailyMaterialReportViewModels, newPath);
            SaveDailyRawReport(dailyRawReportViewModels, newPath);
        }
    }
}
