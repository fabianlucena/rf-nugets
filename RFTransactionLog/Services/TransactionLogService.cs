using RFService.Services;
using RFService.IRepo;
using RFTransactionLog.Entities;
using RFTransactionLog.IServices;
using RFService.Repo;
using System.Text.Json;
using RFService.Libs;
using Microsoft.AspNetCore.Http;

namespace RFTransactionLog.Services
{
    public class TransactionLogService(
        IRepo<TransactionLog> repo,
        ILogLevelService transactionLogLevelService,
        ILogActionService transactionLogActionService,
        IHttpContextAccessor httpContextAccessor
    )
        : ServiceIdUuid<IRepo<TransactionLog>, TransactionLog>(repo),
            ITransactionLogService
    {
        private HttpContext? HttpContext => httpContextAccessor?.HttpContext;

        public override Task<TransactionLog> ValidateForCreationAsync(TransactionLog data)
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

        public override Task<IEnumerable<TransactionLog>> GetListAsync(GetOptions? options)
        {
            options ??= new GetOptions();
            options.OrderBy.Add("LogTimestamp DESC");

            return base.GetListAsync(options);
        }

        public async Task<TransactionLog> AddAsync(Int64 levelId, Int64 actionId, string message, object? data = null, bool? dataRequest = null)
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

            return await CreateAsync(new TransactionLog
            {
                LevelId = levelId,
                ActionId = actionId,
                Message = message,
                JsonData = jsonData,
            });
        }

        public async Task<TransactionLog> AddAsync(TLLevel level, TLAction action, string message, object? data = null, bool? dataRequest = null)
        {
            var levelId = await transactionLogLevelService.GetSingleIdForNameOrCreateAsync(level.ToString(), new LogLevel { Name = level.ToString() });
            var actionId = await transactionLogActionService.GetSingleIdForNameOrCreateAsync(action.ToString(), new Entities.LogAction { Name = action.ToString() });

            return await AddAsync(levelId, actionId, message, data, dataRequest);
        }

        public Task<TransactionLog> AddInfoAsync(TLAction action, string message, object? data = null, bool? dataRequest = null)
            => AddAsync(TLLevel.INFO, action, message, data, dataRequest);
    }
}
