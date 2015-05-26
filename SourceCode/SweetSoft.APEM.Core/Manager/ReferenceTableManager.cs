using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SubSonic;
using SweetSoft.APEM.DataAccess;
using SweetSoft.APEM.Core.Helper;
using System.Reflection;

namespace SweetSoft.APEM.Core.Manager
{
    public class ProductTypeExtension : TblReference
    {
        //Phần bổ sung
        public TblReference Parent { get; set; }
        //Chuyển Parents -> Children
        public ProductTypeExtension() { }
        public ProductTypeExtension(TblReference parent)
        {
            Parent = parent;
            try
            {
                foreach (PropertyInfo prop in this.GetType().GetProperties())
                {
                    if (Parent.GetType().GetProperty(prop.Name) != null)
                        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(parent, null), null);
                }
            }
            catch
            {
            }
        }
        public bool IsChecked { set; get; }
    }

    public class ReferenceTableManager
    {
        public static TblReference Insert(TblReference obj)
        {
            return new TblReferenceController().Insert(obj);
        }

        public static TblReference Update(TblReference obj)
        {
            return new TblReferenceController().Update(obj);
        }

        public static TblReference SelectByID(int id)
        {
            return new Select().From(TblReference.Schema)
              .Where(TblReference.Columns.ReferencesID).IsEqualTo(id)
              .OrderAsc(TblReference.Columns.Name).ExecuteSingle<TblReference>();
        }

        public static bool Delete(int id)
        {
            return new TblReferenceController().Delete(id);
        }

        public static bool Exist(int id)
        {
            return new Select().From(TblReference.Schema)
             .Where(TblReference.ReferencesIDColumn).IsEqualTo(id).GetRecordCount() > 0 ? true : false;
        }

        public static bool ExistName(int id, string name,byte type)
        {
            return new Select().From(TblReference.Schema)
              .Where(TblReference.ReferencesIDColumn).IsNotEqualTo(id)
              .And(TblReference.TypeColumn).IsEqualTo(type)
              .And(TblReference.NameColumn).IsEqualTo(name).GetRecordCount() > 0 ? true : false;
        }
        public static bool ExistCode(int id, string code,byte type)
        {
            return new Select().From(TblReference.Schema)
              .Where(TblReference.ReferencesIDColumn).IsNotEqualTo(id)
              .And(TblReference.TypeColumn).IsEqualTo(type)
              .And(TblReference.CodeColumn).IsEqualTo(code).GetRecordCount() > 0 ? true : false;
        }

        public static DataTable SelectAll(string KeyWord, short? Type , short? IsActive, int PageIndex, int PageSize, string SortColumn, string SortType, bool AddNew)
        {
            DataTable dt = new DataTable();
            dt.Load(SPs.TblReferenceSelectAll(KeyWord, Type, IsActive, PageIndex, PageSize, SortColumn, SortType).GetReader());

            if (AddNew)
            {
                Random rnd = new Random();
                int randID = 0;
                do
                {
                    randID = (int)rnd.Next(-10000, -1);
                } while (dt.AsEnumerable().Where(x => x.Field<int>("ReferencesID") == randID).Count() > 0);

                DataRow r = dt.NewRow();
                r["ReferencesID"] = randID;
                r["Code"] = "";
                r["Name"] = "";
                r["Type"] = 0;
                r["IsObsolete"] = 0;
                r["RowsCount"] = dt.Rows.Count == 0 ? 1 : Convert.ToInt32(dt.Rows[0]["RowsCount"]);
                dt.Rows.InsertAt(r, 0);
                dt.AcceptChanges();
            }
            return dt;
        }

        public static TblReferenceCollection SelectForDDL()
        {
            return new Select().From(TblReference.Schema)
                        .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                        .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>();
        }

        public static TblReferenceCollection SelectAll()
        {
            return new TblReferenceController().FetchAll();
        }

        public static TblReference SelectByReferenceID(int p)
        {
            return new Select(TblReference.NameColumn)
                .From(TblReference.Schema)
                .Where(TblReference.ReferencesIDColumn).IsEqualTo(p)
                .ExecuteSingle<TblReference>();
        }

        public static TblReferenceCollection SelectReferenceByType(byte type)
        {
            return new SubSonic.Select()
                .From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(type)
                .ExecuteAsCollection<TblReferenceCollection>();
        }

        public static bool IsBeingUsed(int ID)
        {
            bool isUsed1 = new SubSonic.Select().From(TblCustomer.Schema).Where(TblCustomer.CountryIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;
            bool isUsed2 = new SubSonic.Select().From(TblCustomer.Schema).Where(TblCustomer.GroupIDColumn).IsEqualTo(ID).GetRecordCount() > 0 ? true : false;

            if (isUsed1 || isUsed2)
                return true;
            else
                return false;
        }

        //Select product type for dropdownlist
        public static List<TblReference> SelectProductTypeForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.ProductType)
                .OrderAsc(TblReference.Columns.Code).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "-Product type-";
            obj.Name = "-Product type-";
            obj.Type = ReferenceTypeHelper.ProductType;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        //Select product type for checkboxList
        public static List<ProductTypeExtension> SelectProductTypeForCheckboxList(short DepartmentID)
        {
            //Select ProductType
            List<ProductTypeExtension> returnList = new List<ProductTypeExtension>();
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.ProductType)
                .OrderAsc(TblReference.Columns.Code).ExecuteAsCollection<TblReferenceCollection>().ToList();
            //Select department
            TblDepartment obj = DepartmentManager.SelectByID(DepartmentID);
            string ProductIDs = obj != null ? obj.ProductTypeID : string.Empty;
            foreach (TblReference item in list)
            {
                ProductTypeExtension pObj = new ProductTypeExtension();
                pObj.ReferencesID = item.ReferencesID;
                pObj.Code = item.Code;
                pObj.Name = pObj.Name;//(ProductTypeExtension)item;
                pObj.IsChecked = ProductIDs == null ? false : (ProductIDs.Contains(string.Format("--{0}--", item.ReferencesID)) ? true : false);
                returnList.Add(pObj);
            }
            return returnList;
        }

        //Select product type for checkboxList
        public static ProductTypeExtension SelectProductTypeByReferencesID(short DepartmentID, int RefID)
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.ProductType)
                .OrderAsc(TblReference.Columns.Code).ExecuteAsCollection<TblReferenceCollection>().ToList();
            //Select department
            TblDepartment obj = DepartmentManager.SelectByID(DepartmentID);
            string ProductIDs = obj != null ? obj.ProductTypeID : string.Empty;
            ProductTypeExtension pObj = new ProductTypeExtension();
            TblReference item = list.Find(i => i.ReferencesID == RefID);
            pObj.ReferencesID = item.ReferencesID;
            pObj.Code = item.Code;
            pObj.Name = pObj.Name;//(ProductTypeExtension)item;
            pObj.IsChecked = ProductIDs == null ? false : (ProductIDs.Contains(string.Format("--{0}--", item.ReferencesID)) ? true : false);

            return pObj;
        }

        //Select product type for dropdownlist
        public static List<TblReference> SelectProcessTypeForDDL(bool ShowCommonType)
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.ProcessType)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = ShowCommonType ? "Common" : "-Process type-";
            obj.Name = ShowCommonType ? "Common" : "-Process type--";
            obj.Type = ReferenceTypeHelper.ProcessType;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblReference> SelectHexagonalForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.HexaganolDot)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "----";
            obj.Name = "----";
            obj.Type = ReferenceTypeHelper.ProcessType;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblReference> SelectCellShapeForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.CellShape)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "----";
            obj.Name = "----";
            obj.Type = ReferenceTypeHelper.ProcessType;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblReference> SelectGradationForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.Gradation)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "----";
            obj.Name = "----";
            obj.Type = ReferenceTypeHelper.ProcessType;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblReference> SelectDeliveryForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.Delivery)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "----";
            obj.Name = "--Select delivery term--";
            obj.Type = ReferenceTypeHelper.ProcessType;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblReference> SelectPaymentForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.Payment)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "----";
            obj.Name = "--Select payment term--";
            obj.Type = ReferenceTypeHelper.ProcessType;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblReference> SelectPackingForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.Packing)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "----";
            obj.Name = "--Select packing--";
            obj.Type = ReferenceTypeHelper.Packing;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblReference> SelectPackingDimensionForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.PackingDimension)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "----";
            obj.Name = "--Select packing dimension--";
            obj.Type = ReferenceTypeHelper.PackingDimension;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblReference> SelectJobCategoryForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.JobCategory)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "----";
            obj.Name = "--Select Job Category--";
            obj.Type = ReferenceTypeHelper.PackingDimension;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }

        public static List<TblReference> SelectBrandOwnerForDDL()
        {
            List<TblReference> list = new List<TblReference>();
            list = new Select().From(TblReference.Schema)
                .Where(TblReference.IsObsoleteColumn).IsEqualTo(0)
                .And(TblReference.TypeColumn).IsEqualTo(ReferenceTypeHelper.BrandOwner)
                .OrderAsc(TblReference.Columns.Name).ExecuteAsCollection<TblReferenceCollection>().ToList();
            TblReference obj = new TblReference();
            obj.ReferencesID = 0;
            obj.Code = "----";
            obj.Name = "--Select Brand Owner--";
            obj.Type = ReferenceTypeHelper.BrandOwner;
            obj.IsObsolete = 0;
            list.Insert(0, obj);
            return list;
        }
    }
}
