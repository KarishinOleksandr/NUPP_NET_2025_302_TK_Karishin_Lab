using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Common
{
    public class MovieService
    {
        private List<Movie> _movies = new List<Movie>();

        public void Create(Movie movie) => _movies.Add(movie);
        public List<Movie> ReadAll() => _movies;
        public void Update(int index, Movie updatedMovie)
        {
            if (index >= 0 && index < _movies.Count)
                _movies[index] = updatedMovie;
        }
        public void Delete(int index)
        {
            if (index >= 0 && index < _movies.Count)
                _movies.RemoveAt(index);
        }
    }
}
