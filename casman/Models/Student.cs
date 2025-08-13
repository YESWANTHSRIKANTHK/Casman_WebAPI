using System.ComponentModel.DataAnnotations;

namespace casman.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Phone { get; set; }

    }
}
