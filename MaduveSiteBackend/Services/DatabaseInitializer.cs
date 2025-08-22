using Microsoft.EntityFrameworkCore;
using MaduveSiteBackend.Data;
using System.IO;

namespace MaduveSiteBackend.Services;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(ApplicationDbContext context, ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("Starting database initialization...");

            await _context.Database.EnsureCreatedAsync();

            var scriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scripts");
            
            if (!Directory.Exists(scriptsPath))
            {
                _logger.LogWarning("Scripts directory not found: {ScriptsPath}", scriptsPath);
                return;
            }

            var scriptFiles = Directory.GetFiles(scriptsPath, "*.sql")
                                      .OrderBy(f => Path.GetFileName(f))
                                      .ToList();

            foreach (var scriptFile in scriptFiles)
            {
                await ExecuteScriptAsync(scriptFile);
            }

            _logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database initialization");
            throw;
        }
    }

    private async Task ExecuteScriptAsync(string scriptPath)
    {
        try
        {
            var scriptName = Path.GetFileName(scriptPath);
            _logger.LogInformation("Executing script: {ScriptName}", scriptName);

            var scriptContent = await File.ReadAllTextAsync(scriptPath);
            
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                await _context.Database.ExecuteSqlRawAsync(scriptContent);
                await transaction.CommitAsync();
                
                _logger.LogInformation("Script executed successfully: {ScriptName}", scriptName);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing script: {ScriptPath}", scriptPath);
            throw;
        }
    }
}
