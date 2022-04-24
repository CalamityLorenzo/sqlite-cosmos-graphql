using GraphQl.WebApi.GraphQl.Common;

namespace GraphQl.WebApi.GraphQl.Movies
{
    public class UpdateKeywordPayload : BasePayload
    {
        public UpdateKeywordPayload(IEnumerable<UserError> errors) : base(errors)
        {
        }

        public UpdateKeywordPayload(string[] Keywords) : base(Enumerable.Empty<UserError>())
        {
            this.Keywords = Keywords;
        }

        public string[] Keywords { get; } = new string[0];
    }
}