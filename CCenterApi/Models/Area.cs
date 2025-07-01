using System.ComponentModel.DataAnnotations;

namespace CCenterApi.Models
{
    public class Area
    {
        [Key]
        public int IDArea { get; set; }

        public string AreaName { get; set; }
        public int StatusArea { get; set; }
        public DateTime CreateDate { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
