using OnlineStore.Domain.Core.enums;

namespace OnlineStore.Domain.Core.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public decimal Balance { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }
    }

}
