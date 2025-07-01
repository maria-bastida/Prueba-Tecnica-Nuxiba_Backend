namespace CCenterApi.DTOs
{
    public class LoginCreateDto
    {
        public int User_id { get; set; }
        public int Extension { get; set; }
        public int TipoMov { get; set; } // 1 = login, 0 = logout
        public DateTime Fecha { get; set; }
    }
}
