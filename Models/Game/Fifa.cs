using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentFunctions.Models.Game
{
    [Table("games")] // Ensure this matches your SQL table name
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; } // Primary Key with auto-increment

        [Required]
        public int Year { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty; // Constraint: 'Men' or 'Women'

        [Required]
        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Continent { get; set; } = string.Empty; // Constraint: predefined values

        [Required]
        [StringLength(50)]
        public string Winner { get; set; } = string.Empty;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; } // Default value: GETDATE() in SQL
    }
}
