using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD1_code.Models.EntityFramework
{
    [Table("TypeProduit")]
    [PrimaryKey("IdTypeProduit")]
    public class TypeProduit
    {
        [Key]
        [Column("idTypeProduit")]
        public int IdTypeProduit { get; set; }

        [Column("nomTypeProduit")]
        public string NomTypeProduit { get; set; }

        [InverseProperty(nameof(Produit.IdTypeProduitNavigation))]
        public virtual ICollection<Produit> Produits { get; set; }
    }
}
