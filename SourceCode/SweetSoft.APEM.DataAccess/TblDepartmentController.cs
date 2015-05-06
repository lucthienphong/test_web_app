using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
// <auto-generated />
namespace SweetSoft.APEM.DataAccess
{
    /// <summary>
    /// Controller class for tblDepartment
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblDepartmentController
    {
        // Preload our schema..
        TblDepartment thisSchemaLoad = new TblDepartment();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public TblDepartmentCollection FetchAll()
        {
            TblDepartmentCollection coll = new TblDepartmentCollection();
            Query qry = new Query(TblDepartment.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDepartmentCollection FetchByID(object DepartmentID)
        {
            TblDepartmentCollection coll = new TblDepartmentCollection().Where("DepartmentID", DepartmentID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblDepartmentCollection FetchByQuery(Query qry)
        {
            TblDepartmentCollection coll = new TblDepartmentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object DepartmentID)
        {
            return (TblDepartment.Delete(DepartmentID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object DepartmentID)
        {
            return (TblDepartment.Destroy(DepartmentID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public TblDepartment Insert(TblDepartment obj)
	    {
            obj.Save(UserName);
            return obj;
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public TblDepartment Update(TblDepartment obj)
	    {
		    TblDepartment item = new TblDepartment();
	        item.MarkOld();
	        item.IsLoaded = true;

            item.DepartmentID = obj.DepartmentID;

            item.DepartmentName = obj.DepartmentName;

            item.ShowInWorkFlow = obj.ShowInWorkFlow;

            item.ProcessTypeID = obj.ProcessTypeID;

            item.ProductTypeID = obj.ProductTypeID;

            item.TimelineOrder = obj.TimelineOrder;

            item.Description = obj.Description;

            item.IsObsolete = obj.IsObsolete;
				
	        item.Save(UserName);

            return item;
	    }
    }
}