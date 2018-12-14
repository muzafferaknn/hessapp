using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.Spend
{
    public class GetGroupStatusRequestDTO
    {
        public int groupId { get; set; }
        public string nickname { get; set; }

    }
}