using Microsoft.Extensions.Logging;
using FluentMigrator.Runner;

namespace BaseProdutos;

public class ConsoleApp
{
    private readonly ILogger<ConsoleApp> _logger;
    private readonly IMigrationRunner _migrationRunner;

    public ConsoleApp(ILogger<ConsoleApp> logger,
        IMigrationRunner migrationRunner)
    {
        _logger = logger;
        _migrationRunner = migrationRunner;
    }

    public void Run()
    {
       _logger.LogInformation("Iniciando a execucao das Migrations...");

       try
       {
           _migrationRunner.MigrateUp();
       }
       catch (Exception ex)
       {
           _logger.LogError(
               $"Erro durante a execucao das Migrations: {ex.Message} | {ex.GetType().Name}");
       }

       _logger.LogInformation("Verificacao das Migrations concluida.");
    }
}