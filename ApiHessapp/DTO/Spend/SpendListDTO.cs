using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.Spend
{
    public class SpendListDTO
    {
        public int? GroupID { get; set; }
        public string From { get; set; }
        public string Description { get; set; }
        public double? TotalAmount { get; set; }
        public string Date { get; set; }

    }
}