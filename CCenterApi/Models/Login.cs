using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCenterApi.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int User_id { get; set; }

        public int Extension { get; set; }

        [Required]
        [Range(0, 1)]
        public int TipoMov { get; set; } // 1 = login, 0 = logout

        public DateTime fecha { get; set; }

        [ForeignKey("User_id")]
        public User User { get; set; }
    }
}
