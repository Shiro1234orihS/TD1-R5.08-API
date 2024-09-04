using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD1_code.Models.EntityFramework
{
    [Table("Marque")]
    [PrimaryKey("IdMarque")]
    public class Marque
    {
        [Key]
        [Column("idMarque")]
        public int IdMarque { get; set; }

        [Column("nomMarque")]
        public string NomMarque {  get; set; }

        [InverseProperty(nameof(Produit.IdMarqueNavigation))]
        public virtual ICollection<Produit> Produits { get; set; }
    }
}
