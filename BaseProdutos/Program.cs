using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FluentMigrator.Runner;
using BaseProdutos;
using BaseProdutos.Utils;

Console.WriteLine("**** BaseProdutos no SQL Server - Execucao de Migrations ****");
Console.WriteLine();

Console.WriteLine("String de conexao com o SQL Server:");
var connectionString = Console.ReadLine();
if (String.IsNullOrWhiteSpace(connectionString))
{
    ConsoleExtensions.ShowError(
        "Informe a string de conexao com o SQL Server!");
    return;
}

using var connectionDB = new SqlConnection();
try
{
    connectionDB.ConnectionString = connectionString;
    connectionDB.Open();
}
catch (Exception ex)
{
    ConsoleExtensions.ShowError(
        $"Erro ao estabelecer conexao com o SQL Server: {ex.Message}");
    return;
}
finally
{
    if (connectionDB.State == System.Data.ConnectionState.Open)
        connectionDB.Close();
}

Console.WriteLine();
Console.WriteLine("Numero de produtos a serem incluidos na base:");
var qtdRegistrosCarga = Console.ReadLine();
if (String.IsNullOrWhiteSpace(qtdRegistrosCarga) ||
    !int.TryParse(qtdRegistrosCarga, out var qtdRegistros) ||
    qtdRegistros <= 0)
{
    ConsoleExtensions.ShowError(
        "Informe um numero de produtos maior do que zero para a carga dos dados!");
    return;
}

var services = new ServiceCollection();

Console.WriteLine("Configurando recursos...");

services.AddSingleton(new LoadDataConfigurations() { NumberOfInsertions = qtdRegistros });
services.AddLogging(configure => configure.AddConsole());

services.AddFluentMigratorCore()
    .ConfigureRunner(cfg => cfg
        .AddSqlServer()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(ConsoleApp).Assembly).For.Migrations()
    )
    .AddLogging(cfg => cfg.AddFluentMigratorConsole());

services.AddTransient<ConsoleApp>();

services.BuildServiceProvider()
    .GetService<ConsoleApp>()!.Run();