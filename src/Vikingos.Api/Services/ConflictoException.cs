namespace Vikingos.Api.Services;

/// <summary>Regla de negocio violada (ej. nombre repetido). Se traduce a HTTP 409 Conflict.</summary>
public class ConflictoException(string message) : Exception(message);
