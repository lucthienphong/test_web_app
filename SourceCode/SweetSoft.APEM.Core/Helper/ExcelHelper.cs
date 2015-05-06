using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Threading;
namespace SweetSoft.APEM.Core
{
    public class Export
    {
        public enum ExportFormat : int { CSV = 1, Excel = 2 }; // Export format enumeration			
        System.Web.HttpResponse response;
        private string appType;

        public Export()
        {
            appType = "Web";
            response = System.Web.HttpContext.Current.Response;
        }

        public Export(string ApplicationType)
        {
            appType = ApplicationType;
            if (appType != "Web" && appType != "Win") throw new Exception("Provide valid application format (Web/Win)");
            if (appType == "Web") response = System.Web.HttpContext.Current.Response;
        }

        #region ExportDetails OverLoad : Type#1

        // Function  : ExportDetails 
        // Arguments : DetailsTable, FormatType, FileName
        // Purpose	 : To get all the column headers in the datatable and 
        //			   exorts in CSV / Excel format with all columns

        public void ExportDetails(DataTable DetailsTable, ExportFormat FormatType, string FileName)
        {
            try
            {
                if (DetailsTable.Rows.Count == 0)
                    throw new Exception("There are no details to export.");

                // Create Dataset
                DataSet dsExport = new DataSet("Export");
                DataTable dtExport = DetailsTable.Copy();
                dtExport.TableName = "Values";
                dsExport.Tables.Add(dtExport);

                // Getting Field Names
                string[] sHeaders = new string[dtExport.Columns.Count];
                string[] sFileds = new string[dtExport.Columns.Count];

                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    //sHeaders[i] = ReplaceSpclChars(dtExport.Columns[i].ColumnName);
                    sHeaders[i] = dtExport.Columns[i].ColumnName;
                    sFileds[i] = ReplaceSpclChars(dtExport.Columns[i].ColumnName);
                }

                if (appType == "Web")
                    Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
                else if (appType == "Win")
                    Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // ExportDetails OverLoad : Type#1

        #region ExportDetails OverLoad : Type#2

        // Function  : ExportDetails 
        // Arguments : DetailsTable, ColumnList, FormatType, FileName		
        // Purpose	 : To get the specified column headers in the datatable and
        //			   exorts in CSV / Excel format with specified columns

        public void ExportDetails(DataTable DetailsTable, int[] ColumnList, ExportFormat FormatType, string FileName)
        {
            try
            {
                if (DetailsTable.Rows.Count == 0)
                    throw new Exception("There are no details to export");

                // Create Dataset
                DataSet dsExport = new DataSet("Export");
                DataTable dtExport = DetailsTable.Copy();
                dtExport.TableName = "Values";
                dsExport.Tables.Add(dtExport);

                if (ColumnList.Length > dtExport.Columns.Count)
                    throw new Exception("ExportColumn List should not exceed Total Columns");

                // Getting Field Names
                string[] sHeaders = new string[ColumnList.Length];
                string[] sFileds = new string[ColumnList.Length];

                for (int i = 0; i < ColumnList.Length; i++)
                {
                    if ((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
                        throw new Exception("ExportColumn Number should not exceed Total Columns Range");

                    sHeaders[i] = dtExport.Columns[ColumnList[i]].ColumnName;
                    sFileds[i] = ReplaceSpclChars(dtExport.Columns[ColumnList[i]].ColumnName);
                }

                if (appType == "Web")
                    Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
                else if (appType == "Win")
                    Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // ExportDetails OverLoad : Type#2

        #region ExportDetails OverLoad : Type#3

        // Function  : ExportDetails 
        // Arguments : DetailsTable, ColumnList, Headers, FormatType, FileName	
        // Purpose	 : To get the specified column headers in the datatable and	
        //			   exorts in CSV / Excel format with specified columns and 
        //			   with specified headers

        public void ExportDetails(DataTable DetailsTable, int[] ColumnList, string[] Headers, ExportFormat FormatType,
            string FileName)
        {
            try
            {
                if (DetailsTable.Rows.Count == 0)
                    throw new Exception("There are no details to export");

                // Create Dataset
                DataSet dsExport = new DataSet("Export");
                DataTable dtExport = DetailsTable.Copy();
                dtExport.TableName = "Values";
                dsExport.Tables.Add(dtExport);

                if (ColumnList.Length != Headers.Length)
                    throw new Exception("ExportColumn List and Headers List should be of same length");
                else if (ColumnList.Length > dtExport.Columns.Count || Headers.Length > dtExport.Columns.Count)
                    throw new Exception("ExportColumn List should not exceed Total Columns");

                // Getting Field Names
                string[] sFileds = new string[ColumnList.Length];

                for (int i = 0; i < ColumnList.Length; i++)
                {
                    if ((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
                        throw new Exception("ExportColumn Number should not exceed Total Columns Range");

                    sFileds[i] = ReplaceSpclChars(dtExport.Columns[ColumnList[i]].ColumnName);
                }

                if (appType == "Web")
                    Export_with_XSLT_Web(dsExport, Headers, sFileds, FormatType, FileName);
                else if (appType == "Win")
                    Export_with_XSLT_Windows(dsExport, Headers, sFileds, FormatType, FileName);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // ExportDetails OverLoad : Type#3

        #region Export_with_XSLT_Web

        // Function  : Export_with_XSLT_Web 
        // Arguments : dsExport, sHeaders, sFileds, FormatType, FileName
        // Purpose   : Exports dataset into CSV / Excel format

        private void Export_with_XSLT_Web(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName)
        {
            try
            {
                // Appending Headers                
                response.Clear();
                response.Buffer = true;

                if (FormatType == ExportFormat.CSV)
                {
                    response.ContentType = "text/csv";

                    response.Charset = "Unicode";
                    response.ContentEncoding = System.Text.Encoding.GetEncoding("Unicode");
                    response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

                    response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
                    response.AppendHeader("ACCEPT-CHARSET", "UTF-8");
                    response.AppendHeader("ACCEPT-ENCODING", "UTF-8");
                    response.HeaderEncoding = System.Text.Encoding.UTF8;
                }
                else
                {
                    response.ContentType = "application/ms-excel";
                    //response.ContentType = "application/vnd.ms-excel";
                    response.Charset = "Unicode";
                    response.ContentEncoding = System.Text.Encoding.GetEncoding("Unicode");
                    response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
                    //response.Charset = "UTF-8";
                    response.AppendHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
                    response.AppendHeader("ACCEPT-CHARSET", "UTF-8");
                    response.AppendHeader("ACCEPT-ENCODING", "UTF-8");
                    response.HeaderEncoding = System.Text.Encoding.UTF8;
                }

                //Response.Clear();
                //Response.AddHeader("content-disposition", "attachment;filename=Test.xls");
                //Response.ContentType = "application/ms-excel";
                //Response.ContentEncoding = System.Text.Encoding.Unicode;
                //Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

                //System.IO.StringWriter sw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);

                //FormView1.RenderControl(hw);

                //Response.Write(sw.ToString());
                //Response.End();




                // XSLT to use for transforming this dataset.						
                MemoryStream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);

                CreateStylesheet(writer, sHeaders, sFileds, FormatType);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                XmlDataDocument xmlDoc = new XmlDataDocument(dsExport);

                //dsExport.WriteXml("Data.xml");
                XslTransform xslTran = new XslTransform();
                //XslCompiledTransform xslTran = new XslCompiledTransform();
                xslTran.Load(new XmlTextReader(stream), null, null);

                System.IO.StringWriter sw = new System.IO.StringWriter();
                //StreamWriter sw = new StreamWriter(FileName, false, Encoding.UTF8);
                xslTran.Transform(xmlDoc, null, sw);
                //xslTran.Transform(System.Web.HttpContext.Current.Server.MapPath("Data.xml"), null, sw, null);

                //Writeout the Content			
                response.Write(sw.ToString());
                //response.WriteFile("D:/test.csv");                
                sw.Close();
                writer.Close();
                stream.Close();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                response.End();
            }
            catch (ThreadAbortException Ex)
            {
                string ErrMsg = Ex.Message;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // Export_with_XSLT

        #region Export_with_XSLT_Windows

        // Function  : Export_with_XSLT_Windows 
        // Arguments : dsExport, sHeaders, sFileds, FormatType, FileName
        // Purpose   : Exports dataset into CSV / Excel format

        private void Export_with_XSLT_Windows(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName)
        {
            try
            {
                // XSLT to use for transforming this dataset.						
                MemoryStream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);

                CreateStylesheet(writer, sHeaders, sFileds, FormatType);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                XmlDataDocument xmlDoc = new XmlDataDocument(dsExport);
                XslTransform xslTran = new XslTransform();
                xslTran.Load(new XmlTextReader(stream), null, null);

                System.IO.StringWriter sw = new System.IO.StringWriter();
                xslTran.Transform(xmlDoc, null, sw, null);

                //Writeout the Content									
                StreamWriter strwriter = new StreamWriter(FileName, false, Encoding.UTF8);
                strwriter.WriteLine(sw.ToString());
                strwriter.Close();

                sw.Close();
                writer.Close();
                stream.Close();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // Export_with_XSLT

        #region CreateStylesheet

        // Function  : WriteStylesheet 
        // Arguments : writer, sHeaders, sFileds, FormatType
        // Purpose   : Creates XSLT file to apply on dataset's XML file 

        private void CreateStylesheet(XmlTextWriter writer, string[] sHeaders, string[] sFileds, ExportFormat FormatType)
        {
            try
            {
                // xsl:stylesheet
                string ns = "http://www.w3.org/1999/XSL/Transform";
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("xsl", "stylesheet", ns);
                writer.WriteAttributeString("version", "1.0");
                writer.WriteStartElement("xsl:output");
                writer.WriteAttributeString("method", "text");
                writer.WriteAttributeString("version", "4.0");
                writer.WriteEndElement();

                // xsl-template
                writer.WriteStartElement("xsl:template");
                writer.WriteAttributeString("match", "/");

                // xsl:value-of for headers
                for (int i = 0; i < sHeaders.Length; i++)
                {
                    writer.WriteString("\"");
                    writer.WriteStartElement("xsl:value-of");
                    writer.WriteAttributeString("select", "'" + sHeaders[i] + "'");
                    writer.WriteEndElement(); // xsl:value-of
                    writer.WriteString("\"");
                    if (i != sFileds.Length - 1) writer.WriteString((FormatType == ExportFormat.CSV) ? "," : "	");
                }

                // xsl:for-each
                writer.WriteStartElement("xsl:for-each");
                writer.WriteAttributeString("select", "Export/Values");
                writer.WriteString("\r\n");

                // xsl:value-of for data fields
                for (int i = 0; i < sFileds.Length; i++)
                {
                    writer.WriteString("\"");
                    writer.WriteStartElement("xsl:value-of");
                    writer.WriteAttributeString("select", sFileds[i]);
                    writer.WriteEndElement(); // xsl:value-of
                    writer.WriteString("\"");
                    if (i != sFileds.Length - 1) writer.WriteString((FormatType == ExportFormat.CSV) ? "," : "	");
                }

                writer.WriteEndElement(); // xsl:for-each
                writer.WriteEndElement(); // xsl-template
                writer.WriteEndElement(); // xsl:stylesheet
                writer.WriteEndDocument();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // WriteStylesheet

        #region ReplaceSpclChars

        // Function  : ReplaceSpclChars 
        // Arguments : fieldName
        // Purpose   : Replaces special characters with XML codes 

        private string ReplaceSpclChars(string fieldName)
        {
            //			space 	-> 	_x0020_
            //			%		-> 	_x0025_
            //			#		->	_x0023_
            //			&		->	_x0026_
            //			/		->	_x002F_

            fieldName = fieldName.Replace(" ", "_x0020_");
            fieldName = fieldName.Replace("%", "_x0025_");
            fieldName = fieldName.Replace("#", "_x0023_");
            fieldName = fieldName.Replace("&", "_x0026_");
            fieldName = fieldName.Replace("/", "_x002F_");
            return fieldName;
        }

        #endregion // ReplaceSpclChars

    }
    //   public class CExcelReader : IDisposable
    //   {
    //       #region Variables
    //       private int[] _PKCol;
    //       private string _strExcelFilename;
    //       private bool _blnMixedData = true;
    //       private bool _blnHeaders = false;
    //       private string _strSheetName;
    //       private string _strSheetRange;
    //       private bool _blnKeepConnectionOpen = false;
    //       private OleDbConnection _oleConn;
    //       private OleDbCommand _oleCmdSelect;
    //       private OleDbCommand _oleCmdUpdate;
    //       #endregion

    //       #region properties

    //       public int[] PKCols
    //       {
    //           get { return _PKCol; }
    //           set { _PKCol = value; }
    //       }

    //       public string ColName(int intCol)
    //       {
    //           string sColName = "";
    //           if (intCol < 26)
    //               sColName = Convert.ToString(Convert.ToChar((Convert.ToByte((char)'A') + intCol)));
    //           else
    //           {
    //               int intFirst = ((int)intCol / 26);
    //               int intSecond = ((int)intCol % 26);
    //               sColName = Convert.ToString(Convert.ToByte((char)'A') + intFirst);
    //               sColName += Convert.ToString(Convert.ToByte((char)'A') + intSecond);
    //           }
    //           return sColName;
    //       }

    //       public int ColNumber(string strCol)
    //       {
    //           strCol = strCol.ToUpper();
    //           int intColNumber = 0;
    //           if (strCol.Length > 1)
    //           {
    //               intColNumber = Convert.ToInt16(Convert.ToByte(strCol[1]) - 65);
    //               intColNumber += Convert.ToInt16(Convert.ToByte(strCol[1]) - 64) * 26;
    //           }
    //           else
    //               intColNumber = Convert.ToInt16(Convert.ToByte(strCol[0]) - 65);
    //           return intColNumber;
    //       }

    //       public String[] GetExcelSheetNames()
    //       {

    //           System.Data.DataTable dt = null;

    //           try
    //           {
    //               if (_oleConn == null) Open();

    //               // Get the data table containing the schema
    //               dt = _oleConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

    //               if (dt == null)
    //               {
    //                   return null;
    //               }

    //               String[] excelSheets = new String[dt.Rows.Count];
    //               int i = 0;

    //               // Add the sheet name to the string array.
    //               foreach (DataRow row in dt.Rows)
    //               {
    //                   string strSheetTableName = row["TABLE_NAME"].ToString().Trim();
    //                   excelSheets[i] = strSheetTableName.Substring(0, strSheetTableName.Length - 1).Trim('\'', '$');
    //                   i++;
    //               }


    //               return excelSheets;
    //           }
    //           catch (Exception)
    //           {
    //               return null;
    //           }
    //           finally
    //           {
    //               // Clean up.
    //               if (this.KeepConnectionOpen == false)
    //               {
    //                   this.Close();
    //               }
    //               if (dt != null)
    //               {
    //                   dt.Dispose();
    //                   dt = null;
    //               }
    //           }
    //       }

    //       public string ExcelFilename
    //       {
    //           get { return _strExcelFilename; }
    //           set { _strExcelFilename = value; }
    //       }

    //       public string SheetName
    //       {
    //           get { return _strSheetName; }
    //           set { _strSheetName = value; }
    //       }

    //       public string SheetRange
    //       {
    //           get { return _strSheetRange; }
    //           set
    //           {
    //               if (value.IndexOf(":") == -1) throw new Exception("Invalid range length");
    //               _strSheetRange = value;
    //           }
    //       }

    //       public bool KeepConnectionOpen
    //       {
    //           get { return _blnKeepConnectionOpen; }
    //           set { _blnKeepConnectionOpen = value; }
    //       }

    //       public bool Headers
    //       {
    //           get { return _blnHeaders; }
    //           set { _blnHeaders = value; }
    //       }

    //       public bool MixedData
    //       {
    //           get { return _blnMixedData; }
    //           set { _blnMixedData = value; }
    //       }
    //       #endregion

    //       #region Methods

    //       #region Excel Connection
    //       private string ExcelConnectionOptions()
    //       {
    //           string strOpts = "";
    //           if (this.MixedData == true)
    //               strOpts += "Imex=2;";
    //           if (this.Headers == true)
    //               strOpts += "HDR=Yes;";
    //           else
    //               strOpts += "HDR=No;";
    //           return strOpts;
    //       }



    //       private string ExcelConnection()
    //       {
    //           return
    //               @"Provider=Microsoft.Jet.OLEDB.4.0;" +
    //               @"Data Source=" + _strExcelFilename + ";" +
    //               @"Extended Properties=" + Convert.ToChar(34).ToString() +
    //               @"Excel 8.0;" + ExcelConnectionOptions() + Convert.ToChar(34).ToString();
    //       }
    //       #endregion


    //       #region Open / Close
    //       public void Open()
    //       {
    //           try
    //           {
    //               if (_oleConn != null)
    //               {
    //                   if (_oleConn.State == ConnectionState.Open)
    //                   {
    //                       _oleConn.Close();
    //                   }
    //                   _oleConn = null;
    //               }

    //               if (System.IO.File.Exists(_strExcelFilename) == false)
    //               {
    //                   throw new Exception("Không tìm thấy file " + _strExcelFilename);
    //               }
    //               _oleConn = new OleDbConnection(ExcelConnection());
    //               _oleConn.Open();
    //           }
    //           catch (Exception ex)
    //           {
    //               throw ex;
    //           }
    //       }

    //       public void Close()
    //       {
    //           if (_oleConn != null)
    //           {
    //               if (_oleConn.State != ConnectionState.Closed)
    //                   _oleConn.Close();
    //               _oleConn.Dispose();
    //               _oleConn = null;
    //           }
    //       }
    //       #endregion

    //       #region Command Select
    //       private bool SetSheetQuerySelect()
    //       {
    //           try
    //           {
    //               if (_oleConn == null)
    //               {
    //                   throw new Exception("Connection is unassigned or closed.");
    //               }

    //               if (_strSheetName.Length == 0)
    //                   throw new Exception("Sheetname was not assigned.");

    //               _oleCmdSelect = new OleDbCommand(
    //                   @"SELECT * FROM ["
    //                   + _strSheetName
    //                   + "$" + _strSheetRange
    //                   + "]", _oleConn);

    //               return true;
    //           }
    //           catch (Exception ex)
    //           {
    //               throw ex;
    //           }


    //       }
    //       #endregion

    //       #region simple utilities
    //       private string AddWithComma(string strSource, string strAdd)
    //       {
    //           if (strSource != "") strSource = strSource += ", ";
    //           return strSource + strAdd;
    //       }

    //       private string AddWithAnd(string strSource, string strAdd)
    //       {
    //           if (strSource != "") strSource = strSource += " and ";
    //           return strSource + strAdd;
    //       }
    //       #endregion

    //       private OleDbDataAdapter SetSheetQueryAdapter(DataTable dt)
    //       {
    //           // Deleting in Excel workbook is not possible
    //           //So this command is not defined
    //           try
    //           {

    //               if (_oleConn == null)
    //               {
    //                   throw new Exception("Connection is unassigned or closed.");
    //               }

    //               if (_strSheetName.Length == 0)
    //                   throw new Exception("Sheetname was not assigned.");

    //               if (PKCols == null)
    //                   throw new Exception("Cannot update excel sheet with no primarykey set.");
    //               if (PKCols.Length < 1)
    //                   throw new Exception("Cannot update excel sheet with no primarykey set.");

    //               OleDbDataAdapter oleda = new OleDbDataAdapter(_oleCmdSelect);
    //               string strUpdate = "";
    //               string strInsertPar = "";
    //               string strInsert = "";
    //               string strWhere = "";


    //               for (int iPK = 0; iPK < PKCols.Length; iPK++)
    //               {
    //                   strWhere = AddWithAnd(strWhere, dt.Columns[iPK].ColumnName + "=?");
    //               }
    //               strWhere = " Where " + strWhere;

    //               for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
    //               {
    //                   strInsert = AddWithComma(strInsert, dt.Columns[iCol].ColumnName);
    //                   strInsertPar = AddWithComma(strInsertPar, "?");
    //                   strUpdate = AddWithComma(strUpdate, dt.Columns[iCol].ColumnName) + "=?";
    //               }

    //               string strTable = "[" + this.SheetName + "$" + this.SheetRange + "]";
    //               strInsert = "INSERT INTO " + strTable + "(" + strInsert + ") Values (" + strInsertPar + ")";
    //               strUpdate = "Update " + strTable + " Set " + strUpdate + strWhere;


    //               oleda.InsertCommand = new OleDbCommand(strInsert, _oleConn);
    //               oleda.UpdateCommand = new OleDbCommand(strUpdate, _oleConn);
    //               OleDbParameter oleParIns = null;
    //               OleDbParameter oleParUpd = null;
    //               for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
    //               {
    //                   oleParIns = new OleDbParameter("?", dt.Columns[iCol].DataType.ToString());
    //                   oleParUpd = new OleDbParameter("?", dt.Columns[iCol].DataType.ToString());
    //                   oleParIns.SourceColumn = dt.Columns[iCol].ColumnName;
    //                   oleParUpd.SourceColumn = dt.Columns[iCol].ColumnName;
    //                   oleda.InsertCommand.Parameters.Add(oleParIns);
    //                   oleda.UpdateCommand.Parameters.Add(oleParUpd);
    //                   oleParIns = null;
    //                   oleParUpd = null;
    //               }

    //               for (int iPK = 0; iPK < PKCols.Length; iPK++)
    //               {
    //                   oleParUpd = new OleDbParameter("?", dt.Columns[iPK].DataType.ToString());
    //                   oleParUpd.SourceColumn = dt.Columns[iPK].ColumnName;
    //                   oleParUpd.SourceVersion = DataRowVersion.Original;
    //                   oleda.UpdateCommand.Parameters.Add(oleParUpd);
    //               }
    //               return oleda;
    //           }
    //           catch (Exception ex)
    //           {
    //               throw ex;
    //           }

    //       }

    //       #region command Singe Value Update
    //       private bool SetSheetQuerySingelValUpdate(string strVal)
    //       {
    //           try
    //           {
    //               if (_oleConn == null)
    //               {
    //                   throw new Exception("Connection is unassigned or closed.");
    //               }

    //               if (_strSheetName.Length == 0)
    //                   throw new Exception("Sheetname was not assigned.");

    //               _oleCmdUpdate = new OleDbCommand(
    //                   @" Update ["
    //                   + _strSheetName
    //                   + "$" + _strSheetRange
    //                   + "] set F1=" + strVal, _oleConn);
    //               return true;
    //           }
    //           catch (Exception ex)
    //           {
    //               throw ex;
    //           }


    //       }
    //       #endregion



    //       public void SetPrimaryKey(int intCol)
    //       {
    //           _PKCol = new int[1] { intCol };
    //       }

    //       public DataTable GetTable()
    //       {
    //           return GetTable("ExcelTable");
    //       }

    //       private void SetPrimaryKey(DataTable dt)
    //       {
    //           try
    //           {
    //               if (PKCols != null)
    //               {
    //                   //set the primary key
    //                   if (PKCols.Length > 0)
    //                   {
    //                       DataColumn[] dc;
    //                       dc = new DataColumn[PKCols.Length];
    //                       for (int i = 0; i < PKCols.Length; i++)
    //                       {
    //                           dc[i] = dt.Columns[PKCols[i]];
    //                       }


    //                       dt.PrimaryKey = dc;

    //                   }
    //               }
    //           }
    //           catch (Exception ex)
    //           {
    //               throw ex;
    //           }
    //       }

    //       public DataTable GetTable(string strTableName)
    //       {
    //           try
    //           {
    //               //Open and query
    //               if (_oleConn == null) Open();
    //               if (_oleConn.State != ConnectionState.Open)
    //                   throw new Exception("Connection cannot open error.");
    //               if (SetSheetQuerySelect() == false) return null;

    //               //Fill table
    //               OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
    //               oleAdapter.SelectCommand = _oleCmdSelect;
    //               DataTable dt = new DataTable(strTableName);
    //               oleAdapter.FillSchema(dt, SchemaType.Source);
    //               oleAdapter.Fill(dt);
    //               //				if (this.Headers ==false)
    //               //				{
    //               //					if (_strSheetRange.IndexOf(":")>0)
    //               //					{
    //               //						string FirstCol = _strSheetRange.Substring(0,_strSheetRange.IndexOf(":")-1); 
    //               //						int intCol = this.ColNumber(FirstCol);
    //               //						for (int intI=0;intI<dt.Columns.Count;intI++)
    //               //						{
    //               //							dt.Columns[intI].Caption =ColName(intCol+intI);
    //               //						}
    //               //					}
    //               //				}
    //               SetPrimaryKey(dt);
    //               //Cannot delete rows in Excel workbook
    //               dt.DefaultView.AllowDelete = false;

    //               //Clean up
    //               _oleCmdSelect.Dispose();
    //               _oleCmdSelect = null;
    //               oleAdapter.Dispose();
    //               oleAdapter = null;
    //               if (KeepConnectionOpen == false) Close();
    //               return dt;

    //           }
    //           catch (Exception ex)
    //           {
    //               throw ex;
    //           }
    //       }


    //       private void CheckPKExists(DataTable dt)
    //       {
    //           if (dt.PrimaryKey.Length == 0)
    //               if (this.PKCols != null)
    //               {
    //                   SetPrimaryKey(dt);
    //               }
    //               else
    //                   throw new Exception("Provide an primary key to the datatable");
    //       }

    //       public DataTable SetTable(DataTable dt)
    //       {
    //           try
    //           {
    //               DataTable dtChanges = dt.GetChanges();
    //               if (dtChanges == null) throw new Exception("There are no changes to be saved!");
    //               CheckPKExists(dt);
    //               //Open and query
    //               if (_oleConn == null) Open();
    //               if (_oleConn.State != ConnectionState.Open)
    //                   throw new Exception("Connection cannot open error.");
    //               if (SetSheetQuerySelect() == false) return null;

    //               //Fill table
    //               OleDbDataAdapter oleAdapter = SetSheetQueryAdapter(dtChanges);

    //               oleAdapter.Update(dtChanges);
    //               //Clean up
    //               _oleCmdSelect.Dispose();
    //               _oleCmdSelect = null;
    //               oleAdapter.Dispose();
    //               oleAdapter = null;
    //               if (KeepConnectionOpen == false) Close();
    //               return dt;
    //           }
    //           catch (Exception ex)
    //           {
    //               throw ex;
    //           }
    //       }

    //       #region Get/Set Single Value

    //       public void SetSingleCellRange(string strCell)
    //       {
    //           _strSheetRange = strCell + ":" + strCell;
    //       }

    //       public object GetValue(string strCell)
    //       {
    //           SetSingleCellRange(strCell);
    //           object objValue = null;
    //           //Open and query
    //           if (_oleConn == null) Open();
    //           if (_oleConn.State != ConnectionState.Open)
    //               throw new Exception("Connection is not open error.");

    //           if (SetSheetQuerySelect() == false) return null;
    //           objValue = _oleCmdSelect.ExecuteScalar();

    //           _oleCmdSelect.Dispose();
    //           _oleCmdSelect = null;
    //           if (KeepConnectionOpen == false) Close();
    //           return objValue;
    //       }

    //       public void SetValue(string strCell, object objValue)
    //       {

    //           try
    //           {

    //               SetSingleCellRange(strCell);
    //               //Open and query
    //               if (_oleConn == null) Open();
    //               if (_oleConn.State != ConnectionState.Open)
    //                   throw new Exception("Connection is not open error.");

    //               if (SetSheetQuerySingelValUpdate(objValue.ToString()) == false) return;
    //               objValue = _oleCmdUpdate.ExecuteNonQuery();

    //               _oleCmdUpdate.Dispose();
    //               _oleCmdUpdate = null;
    //               if (KeepConnectionOpen == false) Close();
    //           }
    //           catch (Exception ex)
    //           {
    //               throw ex;
    //           }
    //           finally
    //           {
    //               if (_oleCmdUpdate != null)
    //               {
    //                   _oleCmdUpdate.Dispose();
    //                   _oleCmdUpdate = null;
    //               }
    //           }

    //       }
    //       #endregion


    //       #endregion

    //       public

    //       #region Dispose / Destructor
    //void Dispose()
    //       {
    //           if (_oleConn != null)
    //           {
    //               _oleConn.Dispose();
    //               _oleConn = null;
    //           }
    //           if (_oleCmdSelect != null)
    //           {
    //               _oleCmdSelect.Dispose();
    //               _oleCmdSelect = null;
    //           }
    //           // Dispose of remaining objects.
    //       }
    //       #endregion

    //       #region CTOR
    //       public CExcelReader()
    //       {
    //           //
    //           // TODO: Add constructor logic here
    //           //
    //       }
    //       #endregion
    //   }
    public class ExcelHelper
    {
        #region Attributes

        private string filename;
        private string connectionString;



        #endregion

        #region properties

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        #endregion

        #region Methods
        public ExcelHelper()
        {

            filename = "";
            connectionString = "";
        }
        public string ExcelConnectString2003()// chuoi ket noi cho excel 2003
        {
            return @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                   @"Data Source={0};" +
                   @"Extended Properties=" + Convert.ToChar(34).ToString() +
                   @"Excel 8.0;HDR=Yes;IMEX=1" + Convert.ToChar(34).ToString();
        }

        public string ExcelConnectString2007()// chuoi ket noi cho excel 2007
        {
            return @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                   @"Data Source={0};" +
                   @"Extended Properties=" + Convert.ToChar(34).ToString() +
                   @"Excel 12.0 Xml;HDR=YES" + Convert.ToChar(34).ToString();
        }
        public void GetConnectionString()// lua chon mot trong hai chuoi ket noi den excel 2003 hay 2007
        {
            string filename_extension = filename;
            if (filename_extension == "")
            {
                return;
            }
            else
            {
                string[] s = filename_extension.Split(Convert.ToChar(46));      //46 ma thap phan cua dau .
                if (s[s.Length - 1] == "xls")
                    connectionString = string.Format(ExcelConnectString2003(), filename);
                else
                    if (s[s.Length - 1] == "xlsx")
                        connectionString = string.Format(ExcelConnectString2007(), filename);
                    else
                        return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"> cau lenh sql</param>
        /// <returns>datatable</returns>
        public DataTable ReadExcel(string cmdText)
        {
            try
            {
                DataTable table = new DataTable();
                GetConnectionString();//lay ve chuoi ket noi excel 
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand(cmdText, conn);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    adapter.Fill(table);
                    adapter.Dispose();
                    conn.Close();
                }
                return table;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public DataTable ReadExcelBySheet(string SheetName)
        {
            try
            {
                SheetName = SheetName.Replace("'", "");
                string strSQL = "Select * " +
                                "From [{0}$]";
                strSQL = string.Format(strSQL, SheetName);
                return ReadExcel(strSQL);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #endregion
        /// <summary>
        /// Lay tat ca cac sheet trong excel
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public DataTable GetAllSheet()
        {
            GetConnectionString();
            using (OleDbConnection connection = new
                      OleDbConnection(connectionString))
            {
                connection.Open();
                DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                connection.Close();
                return schemaTable;
            }
            return null;
        }

        /// <summary>
        /// Hàm đoc file excel theo tên và sheetName
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ReadDataFromExcelSheet(string filename, string sheetName)
        {
            System.Data.DataTable tbl = new System.Data.DataTable();
            try
            {
                if (sheetName == null)
                    return null;

                ExcelHelper excel = new ExcelHelper();
                excel.Filename = filename;
                tbl = excel.ReadExcelBySheet(sheetName);
            }
            catch (Exception ex)
            {

            }
            return tbl;
        }
    }
}
