using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;
using NuGet.Protocol;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        [HttpGet("SearchByTitle/{title}")]
        public async Task<ActionResult<IEnumerable<Movie>>> SearchByTitle(string title)
        {
            return await _context.Movies.Where(m => m.Title.Contains(title)).ToListAsync();
        }

        [HttpGet("SearchByGenre/{genre}")]
        public async Task<ActionResult<IEnumerable<Movie>>> SearchByMovie(string genre)
        {
            return await _context.Movies.Where(m => m.Genre.Contains(genre)).ToListAsync();
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        [HttpGet("GetTitles")]
        public async Task<ActionResult<IEnumerable<String>>> GetTitlesAlphabetical()
        {
            return GetTitles();
        }

        //this method handles the functionality 
        private List<string> GetTitles()
        {
            List<string> titles = new List<string>();
            List<Movie> movies = _context.Movies.ToList();

            foreach (Movie m in movies)
            {
                titles.Add(m.Title);
            }
            titles.Sort();
            return titles;
        }

        [HttpGet("GetGenres")]
        public async Task<ActionResult<IEnumerable<String>>> GetGenresAlphabetical()
        {
            return GetGenres();
        }

        //this method handles the functionality 
        private List<string> GetGenres()
        {
            List<string> genres = new List<string>();
            List<Movie> movies = _context.Movies.ToList();

            foreach (Movie m in movies)
            {
                genres.Add(m.Genre);
            }
            genres.Sort();
            return genres;
        }
        [HttpGet("GetRandomMovie")]
        public Movie GetRandomMovie()
        {
            List<Movie> movies = _context.Movies.ToList();
            var rnd = new Random();
            int index = rnd.Next(movies.Count);

            return movies[index];
        }
        [HttpGet("GetRandomGenreMovie")]
        public async Task<ActionResult<IEnumerable<Movie>>> SearchByRandomGenreMovie(string genre)
        {
            List<Movie> movies = _context.Movies.ToList();
            List<string> genres = new List<string>();
            foreach(Movie m in movies)
            {
                if (m.Genre == genre)
                {
                    genres.Add(m.Title);
                }
            }
            var rnd = new Random();
            int index = rnd.Next(genres.Count);

            string gen = genres[index];

            return await _context.Movies.Where(m => m.Title.Contains(gen)).ToListAsync();
        }



    }
}
