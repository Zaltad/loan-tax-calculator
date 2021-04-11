using ClosedXML.Excel;
using LoanTaxCalculator.Dtos;
using LoanTaxCalculator.Dtos.Requests;
using System;
using System.Collections.Generic;

namespace LoanTaxCalculator.Services
{
    public class PeriodicTaxesSummaryExportService
    {
        public XLWorkbook ExportPeriodicTaxesSummary(List<TaxTypeDto> taxTypes, List<DateTime> periods,
            Dictionary<TaxTypeAndMonth, PeriodicTaxResponse> periodicTaxesByTaxTypeAndMonth)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Loan tax summary");
            populateMonths(periods, worksheet);
            var columnIndex = 2;

            foreach (var taxType in taxTypes)
            {
                var hasMeasurement = taxType.UnitOfMeasurement != null;
                populateHeaderForTaxType(taxType, hasMeasurement, worksheet, columnIndex);
                double? lastMeasurement = null;
                var rowIndex = 2;

                foreach (var month in periods)
                {
                    var taxTypeAndMonth = new TaxTypeAndMonth { TaxTypeId = taxType.Id, Month = month };

                    if (periodicTaxesByTaxTypeAndMonth.ContainsKey(taxTypeAndMonth))
                    {
                        var periodicTax = periodicTaxesByTaxTypeAndMonth[taxTypeAndMonth];

                        if (hasMeasurement)
                        {
                            var periodicTaxMeasurement = periodicTax.Measurement;
                            populateMeasurement(lastMeasurement, periodicTaxMeasurement, worksheet, rowIndex, columnIndex);
                            lastMeasurement = periodicTaxMeasurement.NowIs;
                        }
                        else
                        {
                            worksheet.Cell(rowIndex, columnIndex + 1).Value = periodicTax.Cost;
                        }
                    }

                    rowIndex++;
                }

                columnIndex += hasMeasurement ? 5 : 2;
            }

            return workbook;
        }

        private void populateHeaderForTaxType(TaxTypeDto taxType, bool hasMeasurement, IXLWorksheet worksheet,
            int columnIndex)
        {
            var taxTypeHeader = taxType.Type;

            if (hasMeasurement)
            {
                taxTypeHeader += $" ({taxType.UnitOfMeasurement})";
                worksheet.Cell(1, columnIndex + 1).Value = "Unit price";
                worksheet.Cell(1, columnIndex + 2).Value = "Then was";
                worksheet.Cell(1, columnIndex + 3).Value = "Now is";
                worksheet.Cell(1, columnIndex + 4).Value = "Cost";
            }
            else
            {
                worksheet.Cell(1, columnIndex + 1).Value = "Cost";
            }

            worksheet.Cell(1, columnIndex).Value = taxTypeHeader;
        }

        private void populateMeasurement(double? lastMeasurement, TaxMeasurementDto periodicTaxMeasurement,
            IXLWorksheet worksheet, int rowIndex, int columnIndex)
        {
            decimal? cost = null;

            if (lastMeasurement != null)
            {
                cost = (decimal)(periodicTaxMeasurement.NowIs - lastMeasurement) *
                    periodicTaxMeasurement.UnitPrice;
            }

            worksheet.Cell(rowIndex, columnIndex + 1).Value = periodicTaxMeasurement.UnitPrice;
            worksheet.Cell(rowIndex, columnIndex + 2).Value = lastMeasurement;
            worksheet.Cell(rowIndex, columnIndex + 3).Value = periodicTaxMeasurement.NowIs;
            worksheet.Cell(rowIndex, columnIndex + 4).Value = cost;
        }

        private void populateMonths(List<DateTime> periods, IXLWorksheet worksheet)
        {
            var rowIndex = 1;

            foreach (var month in periods)
            {
                worksheet.Cell(rowIndex, 1).Value = month.ToString("yyyy MMMM");
                rowIndex++;
            }
        }
    }
}
