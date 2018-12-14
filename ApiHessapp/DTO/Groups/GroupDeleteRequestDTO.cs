using System.ComponentModel.DataAnnotations;

namespace ApiHessapp.ModelClassies.Chats
{
    public class GroupDeleteRequestDTO
    {
        [Required]
        public int groupId { get; set; }
        [Required]
        public string nickname { get; set; }
    }
}