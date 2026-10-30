-- =====================================================================
-- Pregunta 3: ROW_NUMBER() + PARTITION BY - top 3 pedidos más recientes por cliente
-- =====================================================================

IF OBJECT_ID('dbo.Orders') IS NOT NULL DROP TABLE dbo.Orders;
CREATE TABLE dbo.Orders (
    OrderID    INT PRIMARY KEY,
    CustomerID INT,
    OrderDate  DATETIME,
    OrderTotal DECIMAL(18,2)
);

INSERT INTO dbo.Orders (OrderID, CustomerID, OrderDate, OrderTotal) VALUES
(1, 100, '2026-01-10', 50),  (2, 100, '2026-02-15', 80),
(3, 100, '2026-03-20', 20),  (4, 100, '2026-04-01', 95),
(5, 200, '2026-01-05', 300), (6, 200, '2026-05-09', 150);

-- Respuesta
WITH PedidosNumerados AS
(
    SELECT  OrderID,
            CustomerID,
            OrderDate,
            OrderTotal,
            -- Reinicia el contador en 1 por cada cliente (PARTITION BY)
            -- y numera del pedido más reciente al más antiguo (ORDER BY ... DESC).
            ROW_NUMBER() OVER (PARTITION BY CustomerID
                               ORDER BY OrderDate DESC, OrderID DESC) AS Fila
    FROM    dbo.Orders
)
SELECT  OrderID,
        CustomerID,
        OrderDate,
        OrderTotal
FROM    PedidosNumerados
WHERE   Fila <= 3
ORDER BY CustomerID, OrderDate DESC;

-- Índice recomendado para que la consulta no tenga que ordenar toda la tabla:
-- CREATE INDEX IX_Orders_Customer_Date ON dbo.Orders (CustomerID, OrderDate DESC) INCLUDE (OrderTotal);
