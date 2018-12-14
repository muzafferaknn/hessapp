using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.TFA
{
    public class TFAQRRequestDTO
    {
        [Required]
        public string nickname { get; set; }
    }
}