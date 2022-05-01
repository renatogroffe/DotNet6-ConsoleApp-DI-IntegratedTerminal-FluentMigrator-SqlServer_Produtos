using FluentMigrator;
using BaseProdutos.Utils;

namespace BaseProdutos.Migrations;

[Migration(1)]
public class DadosProdutosMigration_v1 : Migration
{
    private readonly LoadDataConfigurations _configurations;

    public DadosProdutosMigration_v1(LoadDataConfigurations configurations)
    {
        _configurations = configurations;
    }

    public override void Up()
    {
        Create.Table("Produtos")
            .WithColumn("Id").AsInt32().Identity().NotNullable().PrimaryKey()
            .WithColumn("Nome").AsAnsiString(50).NotNullable()
            .WithColumn("Preco").AsDecimal(18, 2).NotNullable()
            .WithColumn("Codigo").AsAnsiString(50).NotNullable();

        InsertProdutos();
    }

    public override void Down()
    {
        Delete.Table("Produtos");
    }

    private void InsertProdutos()
    {
        // Geracao de massa de dados para testes
        for (int i = 1; i <= _configurations.NumberOfInsertions; i++)
        {
            Insert.IntoTable("Produtos").Row(new
            {
                Nome = $"Produto {i}",
                Preco = i / 10.0m,                
                Codigo = Guid.NewGuid().ToString()
            });
        }
    }
}