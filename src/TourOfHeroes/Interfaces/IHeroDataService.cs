using System.Threading.Tasks;
using TourOfHeroes.Data;

namespace TourOfHeroes.Interfaces
{

    /// <summary>
    /// Abstraction interface for classes that work with storing and retrieveing
    /// <see cref="Hero"/> objects
    /// </summary>
    public interface IHeroDataService
    {
        /// <summary>
        /// Asynchronously gets all heroes
        /// </summary>
        /// <returns>An array of <see cref="Hero"/> objects</returns>
        Task<Hero[]> GetHeroesAsync();

        /// <summary>
        /// Asynchronously finds a hero by their ID
        /// </summary>
        /// <param name="id">Primary key of our hero</param>
        /// <returns>If found, returns the <see cref="Hero"/>.  Otherwise null</returns>
        Task<Hero> FindHeroAsync(int id);

        /// <summary>
        /// Asynchronously adds a hero.
        /// </summary>
        /// <param name="heroName">Name of the new hero.  (Does not need to be unique)</param>
        /// <returns>The new <see cref="Hero"/></returns>
        Task<Hero> AddHeroAsync(string heroName);

        /// <summary>
        /// Asynchronously deletes a hero.
        /// </summary>
        /// <param name="id">Primary key of our washed up hero.</param>
        /// <returns>The deleted <see cref="Hero"/></returns>
        Task<Hero> DeleteHeroAsync(int id);

        /// <summary>
        /// Asynchronously Persists changes to a hero.
        /// </summary>
        /// <param name="hero"><see cref="Hero"/> object with our changes.</param>
        /// <returns>The modified <see cref="Hero"/></returns>
        Task<Hero> SaveHeroAsync(Hero hero);
    }

    
}