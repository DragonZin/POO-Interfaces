namespace Fase04.Logging
{
    /// <summary>
    /// Cliente que depende apenas de IAccessLogger.
    /// </summary>
    public class AuditService
    {
        private readonly IAccessLogger _logger;

        public AuditService(IAccessLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void RegisterAccess(string identity, string method, bool success)
        {
            _logger.LogAccess(identity, DateTime.UtcNow, method, success);
        }
    }
}
