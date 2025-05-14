using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Models
{
    public class Utilisateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Utilisateur { get; set; }
        public string Email { get; set; }
        public string MDP { get; set; }
        public string Rol { get; set; }
    }
}