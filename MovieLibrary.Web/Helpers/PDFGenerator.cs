using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MovieLibrary.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MovieLibrary.Web.Helpers
{
    public class PDFGenerator
    {     
        // The MigraDoc document    
        public Document document;
        public UserDisplayModel user { get; set; }
        public List<RentedMovieModel> rentedMovieModels { get; set; } 
        // The text frame of the MigraDoc document that contains the user address.  
        public TextFrame addressFrame;
        // The table of the MigraDoc document that contains the rented movies items.   
        public Table table;

        // Some pre-defined RGB colors
        readonly static Color TableBorder = new Color(81, 125, 192);
        readonly static Color TableBlue = new Color(235, 240, 249);
        readonly static Color TableGray = new Color(242, 242, 242);               

        public Document CreateDocument()
        {
            this.document = new Document();
            this.document.Info.Title = this.user.FirstName + " " + this.user.LastName + " Rented Movies Report";
            this.document.Info.Author = "Magi";
            this.document.Info.Subject = "time: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            DefineStyles();

            CreatePage();

            FillContent();

            return this.document;
        }

        public Document CreateEmptyDocument()
        {
            this.document = new Document();
            this.document.Info.Title = " Rented Movies Report";
            this.document.Info.Author = "Magi";
            this.document.Info.Subject = "time: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            DefineStyles();

            CreateEmptyPage();

            return this.document;
        }

        /// <summary>
        /// The Rented Movie document is created with the MigraDoc document object model and then rendered to PDF with PDFsharp.
        /// </summary>
        public void ExportDocument(string path, Document document)
        {
            // Create a renderer for PDF that uses Unicode font encoding
            var pdfRenderer = new PdfDocumentRenderer();
            // Set the MigraDoc document
            pdfRenderer.Document = document;
            // Create the PDF document
            pdfRenderer.RenderDocument();
            // Save the PDF document...
            string filename = path + this.user.FirstName + this.user.LastName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";           
            pdfRenderer.Save(filename);
            // ...and start a viewer in a google chrome
            Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",filename);
        }

        public void CreateEmptyPage()
        {
            // Each MigraDoc document needs at least one section.
            Section section = this.document.AddSection();

            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Movie Library House Inc · Sample Street 42 · 18000 Nis · Serbia");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Create the text frame for the address
            this.addressFrame = section.AddTextFrame();
            this.addressFrame.Height = "3.0cm";
            this.addressFrame.Width = "7.0cm";
            this.addressFrame.Left = ShapePosition.Left;
            this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.addressFrame.Top = "5.0cm";
            this.addressFrame.RelativeVertical = RelativeVertical.Page;

            // Put sender in address frame
            paragraph = this.addressFrame.AddParagraph("Movie Library House Inc · Sample Street 42 · 18000 Nis · Serbia");
            paragraph.Format.Font.Name = "Times New Roman";
            paragraph.Format.Font.Size = 7;
            paragraph.Format.SpaceAfter = 3;

            // Add the print date field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "8cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("NO USER FOUND!", TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddText("Nis, ");
            paragraph.AddDateField("dd.MM.yyyy");
        }

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        public void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this.document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = this.document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        /// <summary>
        /// Creates the static parts of the document.
        /// </summary>
        public void CreatePage()
        {
            // Each MigraDoc document needs at least one section.
            Section section = this.document.AddSection();

            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Movie Library House Inc · Sample Street 42 · 18000 Nis · Serbia");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Create the text frame for the address
            this.addressFrame = section.AddTextFrame();
            this.addressFrame.Height = "3.0cm";
            this.addressFrame.Width = "7.0cm";
            this.addressFrame.Left = ShapePosition.Left;
            this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.addressFrame.Top = "5.0cm";
            this.addressFrame.RelativeVertical = RelativeVertical.Page;

            // Put sender in address frame
            paragraph = this.addressFrame.AddParagraph("Movie Library House Inc · Sample Street 42 · 18000 Nis · Serbia");
            paragraph.Format.Font.Name = "Times New Roman";
            paragraph.Format.Font.Size = 7;
            paragraph.Format.SpaceAfter = 3;

            // Add the print date field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "8cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("RENTED MOVIES", TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddText("Nis, ");
            paragraph.AddDateField("dd.MM.yyyy");

            // Create the item table
            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Color = TableBorder;
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            //Item Number
            Column column = this.table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            //Movie Caption
            column = this.table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;
            //issued Date
            column = this.table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = TableBlue;
            row.Cells[0].AddParagraph("Item");
            row.Cells[0].Format.Font.Bold = false;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
            row.Cells[0].MergeDown = 1;
            paragraph = row.Cells[1].AddParagraph("Submitted By : ");
            paragraph.AddFormattedText(this.document.Info.Author, TextFormat.Italic);
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].MergeRight = 1;
            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = TableBlue;
            row.Cells[1].AddParagraph("Caption");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].AddParagraph("Issued Date");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;

            this.table.SetEdge(0, 0, 3, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
            AlternateRowShading(this.table);
        }

        //private void AddFooter(Section section)
        //{
        //    var footer = section.Footers.Primary.AddParagraph();
        //    footer.AddText("Movie Library House Inc · Sample Street 42 · 18000 Nis · Serbia");
        //    footer.Format.Font.Size = 9;
        //    footer.Format.Alignment = ParagraphAlignment.Center;          

        //    footer.AddTab();
        //    footer.AddText("Page ");
        //    footer.AddPageField();
        //    footer.AddText(" of ");
        //    footer.AddNumPagesField();
        //}

        private void AlternateRowShading(Table table)
        {
            // Start at i = 2 to skip column headers
            for (var i = 2; i < table.Rows.Count; i++)
            {
                if (i % 2 == 0)  // Even rows
                {
                    table.Rows[i].Shading.Color = Color.FromRgb(216, 216, 216);
                }
            }
        }

        public void FillContent()
        {
            // Fill address in address text frame         
            Paragraph paragraph = this.addressFrame.AddParagraph();
            paragraph.AddText(user.FirstName + " " + user.LastName);
            paragraph.AddLineBreak();
            paragraph.AddText(user.Address);
            paragraph.AddLineBreak();

            // Iterate the rentedmovies items
            int counter = 1;
            foreach (var item in rentedMovieModels)
            {
                // Each item fills one row
                Row row1 = this.table.AddRow();
                row1.TopPadding = 1.5;
                row1.Cells[0].Shading.Color = Colors.LightSlateGray;
                row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                row1.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[0].AddParagraph(counter.ToString());
                row1.Cells[1].AddParagraph(item.Movie.Caption);
                row1.Cells[2].AddParagraph($"{item.IssuedDate:g}");
                counter++;

                this.table.SetEdge(0, this.table.Rows.Count - 2, 3, 2, Edge.Box, BorderStyle.Single, 0.75);
            }

            // Add an invisible row as a space line to the table
            Row row = this.table.AddRow();
            row.Borders.Visible = false;

            // Add the notes paragraph
            paragraph = this.document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.Format.Borders.Width = 0.75;
            paragraph.Format.Borders.Distance = 3;
            paragraph.Format.Borders.Color = TableBorder;
            paragraph.Format.Shading.Color = TableGray;
            paragraph.AddText("Note text:");
        }



    }
}