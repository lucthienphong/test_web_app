using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.DataAccess
{
    public class TblCylinderCollectionModel
    {
        public TblCylinder objCylinder { get; set; }
        public string PricingName { get; set; }
        public string Status { get; set; }
        public string Base { get; set; }
        public string UnitPriceExtension { get; set; }
        public decimal Total { get; set; }
        public string CylinderType { get; set; }
        public int Quantity { get; set; }
        public string ProcessType { set; get; }
    }
}
