namespace CCenterApi.DTOs;
using System.Text.Json.Serialization;

    public class LoginDto
{
    public int Id { get; set; }
    public int User_id { get; set; }
    public int Extension { get; set; }
    public int TipoMov { get; set; }
    public DateTime Fecha { get; set; }

    [JsonIgnore]
    public string UserLogin { get; set; }
}
