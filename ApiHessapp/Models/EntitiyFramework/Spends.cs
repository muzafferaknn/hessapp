//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiHessapp.Models.EntitiyFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Spends
    {
        public int SpendID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public string From { get; set; }
        public string Description { get; set; }
        public double? TotalAmount { get; set; }
        public DateTime? Date { get; set; }
    
        public virtual Group Group { get; set; }

    }
}