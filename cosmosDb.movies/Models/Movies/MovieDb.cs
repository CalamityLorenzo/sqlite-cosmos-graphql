using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public record MovieDb(

        Guid id,
        string Title,
        long Budget,
        string Homepage,
        string Overview,
        double Popularity,
        DateTime ReleaseDate,
        int yearReleased,
        long Revenue,
        long Runtime,
        string MovieStatus,
        string Tagline,
        double VoteAverage,
        long VoteCount,
        long MovieId
    )
    {
        public string entityType { get; } = "Movie";
    }
}
