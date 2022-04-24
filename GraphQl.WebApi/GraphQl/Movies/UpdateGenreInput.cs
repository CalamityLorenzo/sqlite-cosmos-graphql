namespace GraphQl.WebApi.GraphQl.Movies
{
    public class UpdateGenreInput
    {
        public string MovieId { get; set; }
        public string[] Genres { get; set; } = new string[0];
    }
}
