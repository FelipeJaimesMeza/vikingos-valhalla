-- =====================================================================
-- Pregunta 4: Subconsulta correlacionada - productos más caros que el promedio de su categoría
-- =====================================================================

IF OBJECT_ID('dbo.Sales') IS NOT NULL DROP TABLE dbo.Sales;
IF OBJECT_ID('dbo.Products') IS NOT NULL DROP TABLE dbo.Products;
CREATE TABLE dbo.Products (
    ProductID   INT PRIMARY KEY,
    ProductName VARCHAR(100),
    CategoryID  INT,
    Price       DECIMAL(18,2)
);
CREATE TABLE dbo.Sales (
    SaleID    INT PRIMARY KEY,
    ProductID INT REFERENCES dbo.Products(ProductID),
    SaleDate  DATETIME,
    Quantity  INT
);

INSERT INTO dbo.Products (ProductID, ProductName, CategoryID, Price) VALUES
(1, 'Hacha',    1, 100), (2, 'Espada', 1, 200), (3, 'Martillo', 1, 300),
(4, 'Escudo',   2,  50), (5, 'Casco',  2, 150);

-- Respuesta
SELECT  p.ProductID,
        p.ProductName,
        p.CategoryID,
        p.Price
FROM    dbo.Products p
WHERE   p.Price > (
            -- Correlacionada: se evalúa "para cada fila p", usando la categoría de ESE producto.
            SELECT AVG(p2.Price)
            FROM   dbo.Products p2
            WHERE  p2.CategoryID = p.CategoryID
        )
ORDER BY p.CategoryID, p.Price DESC;
-- Resultado esperado: Martillo (300 > 200) y Casco (150 > 100).

-- Alternativa equivalente con función de ventana (una sola lectura de la tabla):
-- SELECT ProductID, ProductName, CategoryID, Price
-- FROM (
--     SELECT *, AVG(Price) OVER (PARTITION BY CategoryID) AS PromedioCategoria
--     FROM dbo.Products
-- ) t
-- WHERE Price > PromedioCategoria;
