using ApiHessapp.Models.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.Activitys
{
    public class CreateSpendResponseDTO
    {
        public int groupId { get; set; }
        public string from { get; set; }
        public string description { get; set; }
        public double totalAmount { get; set; }
        public string date { get; set; }



    }
}