INSERT INTO "Marque" ("nomMarque") 
VALUES ('Nike'), ('Adidas'), ('Puma');


INSERT INTO "TypeProduit" ("nomTypeProduit") 
VALUES ('Chaussures'), ('Vêtements'), ('Accessoires');

INSERT INTO "Produit" ("nomProduit", "description", "nomPhoto", "uriPhoto", "idTypeProduit", "idMarque", "stockReel", "stockMin", "stockMax") 
VALUES 
('Air Max 90', 'Chaussures de course emblématiques', 'airmax90.jpg', '/images/airmax90.jpg', 1, 1, 100, 10, 200),
('T-shirt Performance', 'T-shirt léger pour l/entraînement', 'tshirt_performance.jpg', '/images/tshirt_performance.jpg', 2, 2, 50, 5, 100),
('Casquette Sport', 'Casquette unisexe ajustable', 'casquette_sport.jpg', '/images/casquette_sport.jpg', 3, 3, 25, 2, 50);
