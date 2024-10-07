
INSERT INTO "Marque" ("nomMarque") 
VALUES 
('Apple'), 
('Samsung'), 
('Sony');

INSERT INTO "TypeProduit" ("nomTypeProduit") 
VALUES 
('Smartphone'), 
('Ordinateur Portable'), 
('Télévision');


INSERT INTO "Produit" ("nomProduit", "description", "nomPhoto", "uriPhoto", "idTypeProduit", "idMarque", "stockReel", "stockMin", "stockMax")
VALUES 
('iPhone 14', 'Smartphone Apple avec écran OLED', 'iphone14.jpg', '/images/iphone14.jpg', 1, 1, 100, 10, 150),
('Galaxy S23', 'Smartphone Samsung avec écran AMOLED', 'galaxys23.jpg', '/images/galaxys23.jpg', 1, 2, 200, 20, 250),
('Bravia X90J', 'Télévision 4K de Sony avec écran LED', 'braviax90j.jpg', '/images/braviax90j.jpg', 3, 3, 50, 5, 100);
