using ApiHessapp.ModelClassies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO
{
    public class CreateGroupRequestDTO
    {
        [Required]
        public string groupName { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string moderator { get; set; }
        [Required]
        public List<String> participants { get; set; }
    }
}