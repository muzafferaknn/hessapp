using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.Spend
{
    public class Debts
    {
        public int ActivityId { get; set; }
        public string From { get; set; }
        public double? Amount { get; set; }
        public string Description { get; set; }

    }
}