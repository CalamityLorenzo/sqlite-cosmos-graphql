namespace GraphQl.WebApi.GraphQl.Common
{
    public abstract class BasePayload
    {
        public IEnumerable<UserError> Errors { get; }
        public BasePayload(IEnumerable<UserError> errors) => this.Errors = errors;

    }
}
