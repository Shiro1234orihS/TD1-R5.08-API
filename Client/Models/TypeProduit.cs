namespace Client.Models
{
    public class TypeProduit
    {
        public int IdTypeProduit { get; set; }
        public string NomTypeProduit { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TypeProduit typeProduit &&
                   IdTypeProduit == typeProduit.IdTypeProduit &&
                   NomTypeProduit == typeProduit.NomTypeProduit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdTypeProduit, NomTypeProduit);
        }

        public TypeProduit(Int32 idTypeProduit, String nomTypeProduit)
        {
            this.IdTypeProduit = idTypeProduit;
            this.NomTypeProduit = nomTypeProduit;
        }
        public TypeProduit() : this(0, "") { }
    }
}
