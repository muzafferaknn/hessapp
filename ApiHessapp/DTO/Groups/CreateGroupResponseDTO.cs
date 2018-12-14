using ApiHessapp.Models.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiHessapp.DTO.Chats
{
    public class CreateGroupResponseDTO
    {
        public int groupID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string moderator { get; set; }
        public List<String> participants { get; set; }
        public List<Spends> spends { get; set; }
    }
}