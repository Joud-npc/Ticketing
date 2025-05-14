using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Models;

public class Ticket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id_Ticket { get; set; }
    
    public string Titre { get; set; }
    public string Descriptions { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public string Categorie { get; set; }
    public DateTime DateCreation { get; set; }
    public string Statut { get; set; } = "Ouvert";
}
