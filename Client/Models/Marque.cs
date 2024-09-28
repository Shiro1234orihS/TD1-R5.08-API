namespace Client.Models
{
    public class Marque
    {
        public int IdMarque { get; set; }
        public string NomMarque { get; set; }
       
        public override bool Equals(object? obj)
        {
            return obj is Marque produit &&
                   IdMarque == produit.IdMarque &&
                   NomMarque == produit.NomMarque;         
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdMarque, NomMarque);
        }

        public Marque(Int32 idMarque, String nomMarque)
        {
            this.IdMarque = idMarque;
            this.NomMarque = nomMarque;
        }
        public Marque() : this(0, "") { }
    }
}
