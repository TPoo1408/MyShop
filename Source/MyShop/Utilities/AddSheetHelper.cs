using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop.Utilities
{
    class AddSheetHelper
    {
        enum Category
        {
            name,
            description
        }
        enum Product
        {
            name,
            brand,
            description,
            price,
            stock,
            image,
            categoryID
        }
        public List<ProductDTO>? GetProductBySheet(string filePath)
        {
            List<ProductDTO> result = new List<ProductDTO>();
            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart!;
                    Sheet sheet = workbookPart.Workbook.Sheets!.GetFirstChild<Sheet>()!;

                    WorksheetPart worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheet.Id!));

                    SharedStringTablePart sharedStringTablePart;

                    if (workbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    {
                        sharedStringTablePart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    }
                    else
                    {
                        return null;
                    }

                    IEnumerable<Row> rows = worksheetPart.Worksheet.Descendants<Row>();

                    rows = rows.Skip(1);
                    foreach (Row row in rows)
                    {
                        int columnIndex = 0;
                        ProductDTO productDTO = new ProductDTO();
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            string cellValue = cell.InnerText;

                            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                            {
                                int sharedStringIndex = int.Parse(cellValue);
                                cellValue = sharedStringTablePart.SharedStringTable.ElementAt(sharedStringIndex).InnerText;
                            }
                            else if (cell.DataType != null && cell.DataType.Value == CellValues.String && cell.CellValue != null)
                            {
                                byte[] bytes = Encoding.UTF8.GetBytes(cell.CellValue.Text);
                                cellValue = Encoding.UTF8.GetString(bytes);
                            }

                            if (columnIndex == (int)Product.name)
                            {
                                productDTO.name = cellValue;
                            }
                            else if (columnIndex == (int)Product.brand)
                            {
                                productDTO.brand= cellValue;
                            }
                            else if (columnIndex == (int)Product.description)
                            {
                                productDTO.description = cellValue;
                            }
                            else if (columnIndex == (int)Product.price)
                            {
                                productDTO.price = decimal.Parse(cellValue);
                            }
                            else if (columnIndex == (int)Product.stock)
                            {
                                productDTO.stock = int.Parse(cellValue);
                            }
                            else if (columnIndex == (int)Product.categoryID)
                            {
                                productDTO.categoryID = int.Parse(cellValue);
                            } 
                            else if(columnIndex == (int)Product.image)
                            {
                                productDTO.image = cellValue;
                            }    
                            productDTO.promotionPrice = productDTO.price;
                            columnIndex++;
                        }
                        result.Add(productDTO);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi! Không thể đọc file khi file đang mở!!!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return result;
        }

        public List<CategoryDTO>? GetCategoryBySheet(string filePath)
        {
            List<CategoryDTO> result = new List<CategoryDTO>();
            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false))
                {
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart!;
                    Sheet sheet = workbookPart.Workbook.Sheets!.GetFirstChild<Sheet>()!;

                    WorksheetPart worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheet.Id!));

                    SharedStringTablePart sharedStringTablePart;

                    if (workbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    {
                        sharedStringTablePart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    }
                    else
                    {
                        return null;
                    }

                    IEnumerable<Row> rows = worksheetPart.Worksheet.Descendants<Row>();

                    rows = rows.Skip(1);
                    foreach (Row row in rows)
                    {
                        int columnIndex = 0;
                        CategoryDTO categoryDTO = new CategoryDTO();
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            string cellValue = cell.InnerText;

                            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                            {
                                int sharedStringIndex = int.Parse(cellValue);
                                cellValue = sharedStringTablePart.SharedStringTable.ElementAt(sharedStringIndex).InnerText;
                            }
                            else if (cell.DataType != null && cell.DataType.Value == CellValues.String && cell.CellValue != null)
                            {
                                byte[] bytes = Encoding.UTF8.GetBytes(cell.CellValue.Text);
                                cellValue = Encoding.UTF8.GetString(bytes);
                            }

                            if (columnIndex == (int)Category.name)
                            {
                                categoryDTO.name = cellValue;
                            }
                            else if (columnIndex == (int)Category.description)
                            {
                                categoryDTO.description = cellValue;
                            }
                            columnIndex++;
                        }
                        result.Add(categoryDTO);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi! Không thể đọc file khi file đang mở!!!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return result;
        }

    }
}
