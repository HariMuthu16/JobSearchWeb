//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class SkillTable
    {
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public Nullable<int> EmployeeID { get; set; }
    
        public virtual EmployeeTable EmployeeTable { get; set; }
    }
}