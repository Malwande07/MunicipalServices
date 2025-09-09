namespace MunicipalServices.Models
{
    public class UserPoints
    {
        public int Id { get; set; }  // primary key (useful if later stored in DB)
        public string Username { get; set; }  // later can link to Identity UserId
        public int Points { get; set; } = 0;  // default 0 points
    }
}
