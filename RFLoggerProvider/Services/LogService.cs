using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RFLogger.Types;
using RFLoggerProvider.Entities;
using RFLoggerProvider.Exceptions;
using RFLoggerProvider.IRepositories;
using RFLoggerProvider.IServices;
using RFServices.Services;
using System.Text.Json;

namespace RFLoggerProvider.Services
{
    public class LogService(
        ILogRepository logRepository,
        IServiceProvider serviceProvider
    )
        : CommonEntityService<Log>(logRepository),
        ILogService
    {
        private HttpContext HttpContext
        {
            get {
                return serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext
                    ?? throw new NoHTTPContyextException();
            }
        }

        public override Task<Log> ValidateForCreateAsync(Log data)
        {
            if (data.LogTimestamp == default)
                data.LogTimestamp = DateTime.UtcNow;

            if (data.SessionId == null || data.SessionId == default)
            {
                var sessionId = HttpContext.Items["SessionId"] as long?;
                if (sessionId != null && sessionId != default)
                    data.SessionId = sessionId;
            }

            return base.ValidateForCreateAsync(data);
        }

        public async Task<Log> AddAsync(long levelId, long actionId, string message, object? data = null, bool? dataRequest = null)
        {
            string? jsonData = (data != null) ?
                JsonSerializer.Serialize(data) :
                null;

            if (dataRequest == null && jsonData == null || dataRequest == true)
            {
                Dictionary<string, object?> newData = (jsonData != null) ?
                    JsonSerializer.Deserialize<Dictionary<string, object?>>(jsonData) ?? [] :
                    [];

                var request = HttpContext?.Request;
                if (request != null)
                {
                    var path = request.Path.ToString();
                    if (!string.IsNullOrEmpty(path))
                        newData["path"] = path;

                    var method = request.Method.ToString();
                    if (!string.IsNullOrEmpty(method))
                        newData["method"] = method;

                    request.EnableBuffering();
                    using var bodyReader = new StreamReader(request.Body, leaveOpen: true);
                    var body = await bodyReader.ReadToEndAsync();
                    request.Body.Position = 0;
                    if (!string.IsNullOrEmpty(body))
                        newData["body"] = body;

                    var query = request.Query.GetPascalized().ToDictionary();
                    if (query.Count > 0)
                        newData["query"] = query;
                }

                jsonData = JsonSerializer.Serialize(newData);
            }

            return await CreateAsync(new Log
            {
                LevelId = levelId,
                ActionId = actionId,
                Message = message,
                JsonData = jsonData,
            });
        }

        public async Task<Log> AddAsync(LLevel level, LAction action, string message, object? data = null, bool? dataRequest = null)
        {
            var logLevelService = serviceProvider.GetRequiredService<ILogLevelService>();
            var logActionService = serviceProvider.GetRequiredService<ILogActionService>();

            var levelId = await logLevelService.GetSingleIdByNameOrCreateAsync(level.ToString());
            var actionId = await logActionService.GetSingleIdByNameOrCreateAsync(action.ToString());

            return await AddAsync(levelId, actionId, message, data, dataRequest);
        }

        public Task<Log> AddInfoAsync(LAction action, string message, object? data = null, bool? dataRequest = null)
            => AddAsync(LLevel.INFO, action, message, data, dataRequest);
    }
}
