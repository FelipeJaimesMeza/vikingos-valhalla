# ⚔️ Registro al Valhalla — Prueba técnica Ingeniero de Desarrollo Senior

**Candidato:** Felipe Jaimes Meza · felipe.jaimes@cenet.co · 30/10/2026

Aplicación web en **Blazor WebAssembly** que consume una **ASP.NET Core Web API** (REST) para gestionar
vikingos (CRUD), calcular sus **Valhalla Points**, clasificarlos como **Guerreros de Thor, Freyja u Odín**
y aprobar su entrada con el mensaje **"¡Listo para el banquete en Valhalla!"**.

## Estructura

```
VikingosValhalla.sln
├── src/
│   ├── Vikingos.Shared/   Contratos (DTOs y enums) compartidos por la API y Blazor
│   ├── Vikingos.Api/      ASP.NET Core Web API (backend, datos en memoria)
│   └── Vikingos.Web/      Blazor WebAssembly (frontend)
├── tests/
│   └── Vikingos.Tests/    Pruebas unitarias xUnit (lógica de negocio + pregunta de lógica C#)
└── sql/                   Respuestas de las preguntas de SQL Server (scripts ejecutables)
```

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download) (`dotnet --version` ≥ 10.0)

## Cómo ejecutar

Abrir **dos terminales** en la carpeta raíz:

```bash
# Terminal 1 — API en http://localhost:5080
dotnet run --project src/Vikingos.Api

# Terminal 2 — Blazor en http://localhost:5090
dotnet run --project src/Vikingos.Web
```

Abrir **http://localhost:5090** en el navegador.

- Los puertos se configuran en `Properties/launchSettings.json` de cada proyecto.
- La URL de la API que usa Blazor está en `src/Vikingos.Web/wwwroot/appsettings.json`.
- El origen permitido por CORS está en `src/Vikingos.Api/appsettings.json`.
- Documento OpenAPI de la API: http://localhost:5080/openapi/v1.json
- Peticiones de ejemplo: `src/Vikingos.Api/Vikingos.Api.http` (Visual Studio / VS Code REST Client).

En Visual Studio también se puede: clic derecho en la solución → *Configurar proyectos de inicio* →
*Varios proyectos de inicio* → `Vikingos.Api` y `Vikingos.Web` en *Iniciar*.

## Pruebas

```bash
dotnet test
```

20 pruebas: fórmula de puntos, clasificación por dioses, umbral de aprobación, CRUD del servicio
(incluido nombre duplicado) y la tabla de la pregunta de lógica C#.

## Endpoints

| Método | Ruta                  | Respuesta                                      |
|--------|-----------------------|------------------------------------------------|
| GET    | `/api/vikingos`       | 200 lista                                      |
| GET    | `/api/vikingos/{id}`  | 200 vikingo · 404 no existe                    |
| POST   | `/api/vikingos`       | 201 creado · 400 inválido · 409 nombre repetido |
| PUT    | `/api/vikingos/{id}`  | 200 actualizado · 400 · 404 · 409              |
| DELETE | `/api/vikingos/{id}`  | 204 eliminado · 404                            |

Los errores se devuelven en formato **ProblemDetails** (JSON estándar RFC 7807).

## Reglas de negocio

| Concepto            | Regla                                                                                   |
|---------------------|-----------------------------------------------------------------------------------------|
| Valhalla Points     | `batallas × 10` + honor (Bajo 0 · Medio 50 · Alto 100) + muerte (Aldea 60 · Campo abierto 50 · Duelo 40) |
| Aprobación          | `≥ 300` puntos → "¡Listo para el banquete en Valhalla!"                                 |
| Guerrero de Thor    | Fuerza: arma Martillo o Hacha                                                           |
| Guerrero de Freyja  | Táctica y amor por la batalla cuerpo a cuerpo: Espada                                   |
| Guerrero de Odín    | Sabios y estrategas, combate a distancia: Lanza o Arco                                  |

## Respuestas SQL y Lógica C#

| Pregunta | Respuesta |
|----------|-----------|
| Optimización de consultas | **A, B, C, E** |
| CTE recursivo | [`sql/01_cte_jerarquia_empleados.sql`](sql/01_cte_jerarquia_empleados.sql) |
| ROW_NUMBER top 3 | [`sql/02_top3_pedidos_por_cliente.sql`](sql/02_top3_pedidos_por_cliente.sql) |
| Subconsulta correlacionada | [`sql/03_productos_sobre_promedio_categoria.sql`](sql/03_productos_sobre_promedio_categoria.sql) |

| Guerrero | Total puntos | Clasificación |
|----------|-------------:|---------------|
| Ragnar   | 465 | Campeón del Valhalla |
| Floki    | 120 | Guerrero Valiente |
| Bjorn    | 400 | Campeón del Valhalla |
| Lagertha | 250 | Veterano Valeroso |
