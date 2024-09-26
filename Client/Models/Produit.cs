﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Client.Models
{
    public class Produit
    {
        public int IdProduit { get; set; }
        public string NomProduit { get; set; }
        public string Description { get; set; }
        public string NomPhoto { get; set; }
        public string UriPhoto { get; set; }
        public int IdTypeProduit { get; set; }
        public int IdMarque { get; set; }
        public int StockReel { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Produit produit &&
                   IdProduit == produit.IdProduit &&
                   NomProduit == produit.NomProduit &&
                   Description == produit.Description &&
                   NomPhoto == produit.NomPhoto &&
                   UriPhoto == produit.UriPhoto &&
                   StockReel == produit.StockReel &&
                   StockMin == produit.StockMin &&
                   StockMax == produit.StockMax;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdProduit, NomProduit, Description, NomPhoto, UriPhoto, StockReel, StockMin , StockMax);
        }

        public Produit(Int32 idProduit, String nomProduit, String description, String nomPhoto, String uriPhoto, Int32 idTypeProduit, Int32 idMarque, Int32 stockReel, Int32 stockMin, Int32 stockMax)
        {
            this.IdProduit = idProduit;
            this.NomProduit = nomProduit;
            this.Description = description;
            this.NomPhoto = nomPhoto;
            this.UriPhoto = uriPhoto;
            this.IdTypeProduit = idTypeProduit;
            this.IdMarque = idMarque;
            this.StockReel = stockReel;
            this.StockMin = stockMin;
            this.StockMax = stockMax;
        }
        public Produit() : this(0, "", "", "","" , 0,0,0,0,0) { }
    }
}