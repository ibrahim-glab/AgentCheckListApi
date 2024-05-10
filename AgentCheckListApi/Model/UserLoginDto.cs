using System.ComponentModel.DataAnnotations;

namespace AgentCheckListApi.Model{

    public class UserLoginDto {
        [Required]
        public required string UserName { get; set; }
        [Required] 
        public required string Password { get; set; }
    }
}