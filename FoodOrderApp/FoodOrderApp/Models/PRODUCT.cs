//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoodOrderApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PRODUCT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODUCT()
        {
            this.CARTs = new HashSet<CART>();
            this.RECEIPT_DETAIL = new HashSet<RECEIPT_DETAIL>();
        }

        public static List<PRODUCT> products { get; internal set; }
        public string ID_ { get; set; }
        public string NAME_ { get; set; }
        public string IMAGE_ { get; set; }
        public int PRICE_ { get; set; }
        public decimal DISCOUNT_ { get; set; }
        public Nullable<decimal> RATING_ { get; set; }
        public Nullable<int> RATE_TIMES_ { get; set; }
        public string DESCRIPTION_ { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CART> CARTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RECEIPT_DETAIL> RECEIPT_DETAIL { get; set; }
    }
}
