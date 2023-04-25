using System;
using System.Collections.Generic;

namespace AzHttpAPI.Models
{
    public partial class Employee : EntityBase
    {
        public int EmpNo { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public int Salary { get; set; }
        public int? DeptNo { get; set; }

        public virtual Department? DeptNoNavigation { get; set; }
    }
}
