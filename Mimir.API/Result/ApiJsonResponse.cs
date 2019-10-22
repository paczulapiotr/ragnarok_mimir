namespace Mimir.API.Result
{
    public class ApiJsonResponse<TData>
    {
        public ApiJsonResponse()
        {
        }

        public ApiJsonResponse(TData data, params ApiMessage[] messages)
        {
            Data = data;
            Messages = messages;
        }

        public ApiJsonResponse(params ApiMessage[] messages)
        {
            Messages = messages;
        }

        public TData Data { get; set; } = default;

        public ApiMessage [] Messages { get; set; }

    }

    public class ApiJsonResponse : ApiJsonResponse<object>
    {
        public ApiJsonResponse()
        {
        }

        public ApiJsonResponse(object data, params ApiMessage[] messages) : base(data, messages)
        {
        }

        public ApiJsonResponse(params ApiMessage[] messages) : base(messages)
        {
        }
    }

}
