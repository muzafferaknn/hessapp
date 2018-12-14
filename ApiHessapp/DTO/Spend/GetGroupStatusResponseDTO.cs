using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.Spend
{
    public class GetGroupStatusResponseDTO
    {
        public double? totalDebt { get; set; }
        public double? totalCredit { get; set; }
        public List<Debts> listDebt { get; set; }
    }
}