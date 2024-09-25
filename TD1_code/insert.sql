INSERT INTO "Marque" ("idMarque", "nomMarque") 
VALUES (1, 'Nike'), (2, 'Adidas'), (3, 'Puma');


INSERT INTO "TypeProduit" ("idTypeProduit", "nomTypeProduit") 
VALUES (1, 'Chaussures'), (2, 'Vêtements'), (3, 'Accessoires');

INSERT INTO "Produit" ("idProduit", "nomProduit", "description", "nomPhoto", "uriPhoto", "idTypeProduit", "idMarque", "stockReel", "stockMin", "stockMax") 
VALUES 
(1, 'Air Max 90', 'Chaussures de course emblématiques', 'airmax90.jpg', '/images/airmax90.jpg', 1, 1, 100, 10, 200),
(2, 'T-shirt Performance', 'T-shirt léger pour l/entraînement', 'tshirt_performance.jpg', '/images/tshirt_performance.jpg', 2, 2, 50, 5, 100),
(3, 'Casquette Sport', 'Casquette unisexe ajustable', 'casquette_sport.jpg', '/images/casquette_sport.jpg', 3, 3, 25, 2, 50);
