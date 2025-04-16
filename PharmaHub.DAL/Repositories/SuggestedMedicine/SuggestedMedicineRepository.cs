using Microsoft.EntityFrameworkCore;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.DALRepositories;

public class SuggestedMedicineRepository : GenericRepository<SuggestedMedicine>, ISuggestedMedicineRepository
{
    private readonly ApplicationDbContext _context;
    public SuggestedMedicineRepository(ApplicationDbContext context) : base(context) { }

    public async Task AddSuggestedMedicineAsync(SuggestedMedicine newMed)
    {
        if (newMed == null)
            throw new ArgumentNullException(nameof(newMed));

        await _context.SuggestedMedicines.AddAsync(newMed);
    }

    public async Task<IReadOnlyList<SuggestedMedicine>> GetSuggestedMedicineByName(string name)
    {
        return await _context.SuggestedMedicines
            .AsNoTracking()
            .Where(s =>EF.Functions.Like(s.Name,$"%{name}%"))
            .ToListAsync();
    }


}

