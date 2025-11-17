using System;
using System.Collections.Generic;
using Fase04.Logging;

namespace Fase04.Tests.Doubles
{
    /// <summary>
    /// Dublê (fake) para IAccessLogger, acumula chamadas em memória.
    /// </summary>
    public sealed class FakeLogger : IAccessLogger
    {
        public List<(string Id, DateTime When, string Method, bool Success)> Calls { get; } = new();

        public void LogAccess(string identity, DateTime when, string method, bool success)
        {
            Calls.Add((identity, when, method, success));
        }
    }
}
