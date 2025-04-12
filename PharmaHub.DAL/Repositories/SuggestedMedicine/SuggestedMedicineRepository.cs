using Microsoft.EntityFrameworkCore;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.DALRepositories;

public class SuggestedMedicineRepository : ISuggestedMedicineRepository
{
    private readonly ApplicationDbContext _context;

    public SuggestedMedicineRepository(ApplicationDbContext context)
    {
        _context=context;
    }

    public async Task AddSuggestedMedicineAsync(SuggestedMedicine newMed)
    {
        if (newMed == null)
            throw new ArgumentNullException(nameof(newMed));

        await _context.SuggestedMedicines.AddAsync(newMed);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<SuggestedMedicine>> GetAllByUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Invalid user ID");

        return await _context.SuggestedMedicines
                             .Where(m => m.Id == userId)
                             .ToListAsync();
    }
}

