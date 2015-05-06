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
    /// Controller class for tblSystemSetting
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TblSystemSettingController
    {
        // Preload our schema..
        TblSystemSetting thisSchemaLoad = new TblSystemSetting();
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
        public TblSystemSettingCollection FetchAll()
        {
            TblSystemSettingCollection coll = new TblSystemSettingCollection();
            Query qry = new Query(TblSystemSetting.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblSystemSettingCollection FetchByID(object SettingID)
        {
            TblSystemSettingCollection coll = new TblSystemSettingCollection().Where("SettingID", SettingID).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblSystemSettingCollection FetchByQuery(Query qry)
        {
            TblSystemSettingCollection coll = new TblSystemSettingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object SettingID)
        {
            return (TblSystemSetting.Delete(SettingID) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object SettingID)
        {
            return (TblSystemSetting.Destroy(SettingID) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string SettingName,string SettingType,string SettingValue)
	    {
		    TblSystemSetting item = new TblSystemSetting();
		    
            item.SettingName = SettingName;
            
            item.SettingType = SettingType;
            
            item.SettingValue = SettingValue;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int SettingID,string SettingName,string SettingType,string SettingValue)
	    {
		    TblSystemSetting item = new TblSystemSetting();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.SettingID = SettingID;
				
			item.SettingName = SettingName;
				
			item.SettingType = SettingType;
				
			item.SettingValue = SettingValue;
				
	        item.Save(UserName);
	    }
    }
}