using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenceManagement.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

    }
}
