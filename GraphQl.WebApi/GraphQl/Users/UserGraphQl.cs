namespace GraphQl.WebApi.GraphQl.Users
{
    public record UserGraphQl(
        Guid UserId,
        string Username,
         string Firstname,
         string Surname,
         string EmailAddress,
         DateTimeOffset Birthdate,
         string[] FavouriteGenres,
         string[] AvoidGenres
        )
    {
        public Guid GraphQlId { get; set; } = UserId;
    }
}
