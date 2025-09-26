namespace APZMS.Application.DTOs
{
    public class LoginResponseDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string AgeGroup { get; set; }
        public string AccessToken { get; set; }
        public string Role { get; set; }
    }
}
