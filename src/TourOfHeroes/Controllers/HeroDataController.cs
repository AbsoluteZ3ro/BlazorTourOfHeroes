
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TourOfHeroes.Data;

namespace TourOfHeroes.Controllers
{

    /// <summary>
    /// Hero data controller is the API entry point.
    /// <br/>
    /// relative path:  /api/herodata
    /// </summary>
    /// <remarks>This class is not thread safe.  It will happily let other's trump your edits.</remarks>
    [Route("api/{controller}")]
    [ApiController]
    public class HeroDataController : ControllerBase
    {

        /// <summary>
        /// The next ID to use for adding a new hero.
        /// </summary>
        /// <value></value>
        private static int NextId {get;set;} = 21;

        /// <summary>
        /// Provides a common object to lock on when updating a hero.
        /// </summary>
        /// <returns></returns>
        public static object AddLockObject {get;} = new object();

        /// <summary>
        /// List of all heros.   
        /// </summary>
        /// <remarks>A real application would use some sort of persisted data such as a database.</remarks>
        private static IList<Hero> Heroes = new List<Hero>( new [] {
            new Hero { Id = 11, Name = "Dr. Nice" },
            new Hero { Id = 12, Name = "Narco" },
            new Hero { Id = 13, Name = "Bombasto" },
            new Hero { Id = 14, Name = "Celeritas" },
            new Hero { Id = 15, Name = "Magneta" },
            new Hero { Id = 16, Name = "RubberMan" },
            new Hero { Id = 17, Name = "Dynama" },
            new Hero { Id = 18, Name = "Dr IQ" },
            new Hero { Id = 19, Name = "Magma" },
            new Hero { Id = 20, Name = "Tornado" }
        });

        /// <summary>
        /// Default action get's all heroes in our &quot;database&quot;
        /// </summary>
        /// <returns>
        /// List of <see cref="Heroes"/>
        /// </returns>
        public IActionResult GetHeroes()
        {
            return Ok(Heroes.ToArray());
        }
            
        /// <summary>
        /// Gets a specific hero associated with an ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult FindHero(int id)
        {
            var hero = Heroes.FirstOrDefault(h => h.Id == id);

            if(hero == null)
                return NotFound();

            return Ok(hero);
         }

        /// <summary>
        /// Creates a new hero
        /// </summary>
        /// <param name="heroName">Name of the hero</param>
        /// <remarks>Hero name does not need to be unique.</remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddHero([FromBody]string heroName)
        {
            Hero hero = null;

            lock(AddLockObject)
            {
                hero = new Hero {Id = NextId++, Name = heroName };
            }

            Heroes.Add(hero);

            return Ok(hero);
        }

        /// <summary>
        /// Updates the name of an existing hero
        /// </summary>
        /// <param name="hero"></param>
        /// <returns>The updated <see cref="Hero"/></returns>
        /// <remarks>Returns bad request if the update is for a hero object that does not exit.</remarks>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SaveHero([FromBody] Hero hero)
        {
            var existingHero = Heroes.FirstOrDefault(h => h.Id == hero.Id);

            if(existingHero == null)
                return BadRequest();

            existingHero.Name = hero.Name;
            return Ok(existingHero);

        }

        /// <summary>
        /// Deletes a hero from the &quot;databavse&quot;
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The deleted <see cref="Hero"/></returns>
        [HttpDelete("{id:int}")]
        public IActionResult DeleteHero(int id)
        {
            var hero = Heroes.FirstOrDefault(h => h.Id == id);
            if(hero == null)
                return BadRequest();

            Heroes.Remove(hero);
            return Ok(hero);
        }
            
    }
}