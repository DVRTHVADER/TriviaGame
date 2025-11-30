using TriviaGame.Domain.Entities;

namespace TriviaGame.Domain.Repositories;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(int id);
    Task<Question> GetRandomAsync(string mode);
    Task<IEnumerable<Question>> GetAllByModeAsync(string mode);
    Task AddAsync(Question q);
    Task SaveChangesAsync();
}