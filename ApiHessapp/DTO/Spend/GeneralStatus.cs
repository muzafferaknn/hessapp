using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.Spend
{
    public class GeneralStatus
    {
        public int? groupId { get; set; }
        public string groupName { get; set; }
        public double? totalCredit { get; set; }
        public double? totalDebt { get; set; }

    }
}