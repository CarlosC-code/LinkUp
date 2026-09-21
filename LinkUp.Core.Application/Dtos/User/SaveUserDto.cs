

namespace LinkUp.Core.Application.Dtos.User
{
    public class SaveUserDto
    {

        public  string? Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public  string? Email { get; set; }
        public required string UserName { get; set; }
        public  string? Password { get; set; }
        public string? Phone { get; set; }
        public string? ProfileImage { get; set; }
        

    }
}
