-- =====================================================================
-- Pregunta 2: CTE recursivo - jerarquía de empleados (nivel 0 = CEO)
-- =====================================================================

-- Datos de ejemplo (opcional, para poder ejecutar y ver el resultado)
IF OBJECT_ID('dbo.Employees') IS NOT NULL DROP TABLE dbo.Employees;
CREATE TABLE dbo.Employees (
    EmployeeID   INT PRIMARY KEY,
    EmployeeName VARCHAR(100),
    ManagerID    INT NULL
);

INSERT INTO dbo.Employees (EmployeeID, EmployeeName, ManagerID) VALUES
(1, 'Odín (CEO)', NULL),
(2, 'Thor',       1),
(3, 'Freyja',     1),
(4, 'Ragnar',     2),
(5, 'Lagertha',   3),
(6, 'Bjorn',      4);

-- Respuesta
WITH Jerarquia AS
(
    -- 1) Ancla: el punto de partida. El CEO es quien no tiene jefe.
    SELECT  EmployeeID,
            EmployeeName,
            ManagerID,
            0 AS Nivel,
            CAST(EmployeeName AS VARCHAR(1000)) AS Ruta
    FROM    dbo.Employees
    WHERE   ManagerID IS NULL

    UNION ALL

    -- 2) Parte recursiva: busca a los subordinados de los que ya están en el CTE y les suma 1 al nivel.
    SELECT  e.EmployeeID,
            e.EmployeeName,
            e.ManagerID,
            j.Nivel + 1,
            CAST(j.Ruta + ' > ' + e.EmployeeName AS VARCHAR(1000))
    FROM    dbo.Employees e
    INNER JOIN Jerarquia j ON e.ManagerID = j.EmployeeID
)
SELECT  EmployeeID,
        EmployeeName,
        ManagerID,
        Nivel,
        Ruta
FROM    Jerarquia
ORDER BY Ruta
OPTION (MAXRECURSION 100); -- Protección contra ciclos (por defecto SQL Server ya corta en 100).
