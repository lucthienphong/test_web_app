using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SweetSoft.APEM.DataAccess;

namespace SweetSoft.CMS.DataAccess
{
    public class JobInvoice
    {
        public int JobID { get; set; }
        public string CustomerPO1 { get; set; }
        public string CustomerPO2 { get; set; }
        public string OrderConfirmNo { get; set; }
        public string ContactPerson { get; set; }
        public string JobNumber { get; set; }
        public int JobRev { get; set; }
        public string JobName { get; set; }
        public string JobDesign { get; set; }
        public double Discount { get; set; }
        public double TaxRate { get; set; }
        public decimal CylinderTotalPrice { get; set; }
        public DataTable CylinderDataSource { get; set; }

        public decimal OtherChargesTotalPrice { get; set; }
        public TblOtherChargeCollection OtherChargesDataSource { get; set; }

        public decimal ServiceJobTotalPrice { get; set; }
        public TblServiceJobDetailCollection ServiceJobDataSource { get; set; }

        public decimal NetTotalCylinderPrice { get; set; }
        public decimal NetTotalOtherChargesPrice { get; set; }
        public decimal NetTotalServicePrice { get; set; }

    }
}
