using System;

/// <summary>
/// Representa um evento de acesso no SecureGate.
/// Ex: alguém tentou entrar em uma área com um crachá.
/// </summary>
public sealed record AccessEvent(
    int Id,
    string BadgeId,
    string AreaName,
    DateTimeOffset Timestamp,
    bool AccessGranted
);
