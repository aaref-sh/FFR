//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FFR.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class favorit
    {
        public int Id { get; set; }
        public int customer_id { get; set; }
        public int meal_id { get; set; }
    
        public virtual customer customer { get; set; }
        public virtual meal meal { get; set; }
    }
}
