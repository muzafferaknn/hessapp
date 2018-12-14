using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.TFA
{
    public class TFAuthRequestDTO
    {
        public string nickname { get; set; }
        public string pin { get; set; }
    }
}