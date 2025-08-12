using RFService.Services;
using RFService.IRepo;
using RFService.Repo;
using System.Text.Json;
using RFService.Libs;
using Microsoft.AspNetCore.Http;
using RFLoggerProvider.Entities;
using RFLoggerProvider.IServices;
using RFLogger.Types;

namespace RFLoggerProvider.Services
{
    public class LogService(
        IRepo<Log> repo,
        ILogLevelService logLevelService,
        ILogActionService logActionService,
        IHttpContextAccessor httpContextAccessor
    )
        : ServiceIdUuid<IRepo<Log>, Log>(repo),
            ILogService
    {
        private HttpContext? HttpContext => httpContextAccessor?.HttpContext;

        public override Task<Log> ValidateForCreationAsync(Log data)
        {
            if (data.LogTimestamp == default)
                data.LogTimestamp = DateTime.UtcNow;

            if (data.SessionId == null || data.SessionId == default)
            {
                var sessionId = HttpContext?.Items["SessionId"] as Int64?;
                if (sessionId != null && sessionId != default)
                    data.SessionId = sessionId;
            }

            return base.ValidateForCreationAsync(data);
        }

        public override Task<IEnumerable<Log>> GetListAsync(QueryOptions? options)
        {
            options ??= new QueryOptions();
            options.OrderBy.Add("LogTimestamp DESC");

            return base.GetListAsync(options);
        }

        public async Task<Log> AddAsync(Int64 levelId, Int64 actionId, string message, object? data = null, bool? dataRequest = null)
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
            var levelId = await logLevelService.GetSingleIdForNameOrCreateAsync(level.ToString(), new LogLevel { Name = level.ToString() });
            var actionId = await logActionService.GetSingleIdForNameOrCreateAsync(action.ToString(), new Entities.LogAction { Name = action.ToString() });

            return await AddAsync(levelId, actionId, message, data, dataRequest);
        }

        public Task<Log> AddInfoAsync(LAction action, string message, object? data = null, bool? dataRequest = null)
            => AddAsync(LLevel.INFO, action, message, data, dataRequest);
    }
}
