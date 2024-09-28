using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TD1_code.Models.EntityFramework
{
    [Table("Produit")]
    public class Produit
    {
        [Key]
        [Column("IdProduit")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public int IdProduit { get; set; }

        [Column("nomProduit")]
        public string NomProduit { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("nomPhoto")]
        public string NomPhoto { get; set; }

        [Column("uriPhoto")]
        public string UriPhoto { get; set; }

        [Column("idTypeProduit")]
        public int IdTypeProduit { get; set; }

        [Column("idMarque")]
        public int IdMarque { get; set; }

        [Column("stockReel")]
        public int StockReel { get; set; }

        [Column("stockMin")]
        public int StockMin { get; set; }

        [Column("stockMax")]
        public int StockMax { get; set; }


        [ForeignKey("IdMarque")]
        [InverseProperty(nameof(Marque.Produits))]
        public virtual Marque? IdMarqueNavigation { get; set; } 

        [ForeignKey("IdTypeProduit")]
        [InverseProperty(nameof(TypeProduit.Produits))]
        public virtual TypeProduit? IdTypeProduitNavigation { get; set; } 
    }
}
