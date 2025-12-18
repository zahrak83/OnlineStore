namespace OnlineStore.Domain.Core.Dtos
{
    public class UpdateProfileDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

}
