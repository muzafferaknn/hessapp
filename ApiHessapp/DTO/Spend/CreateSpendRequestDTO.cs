using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.Activity
{
    public class CreateSpendRequestDTO
    {
        public int groupId { get; set; }
        public string from { get; set; }
        public string description { get; set; }
        public double totalAmount { get; set; }

    }
}