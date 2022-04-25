namespace GraphQl.WebApi.GraphQl.Users
{
    public record AddUserInput(
          string Username,
          string EmailAddress,
          string Firstname,
          string Surname,
          DateTimeOffset Birthday
        );

}
