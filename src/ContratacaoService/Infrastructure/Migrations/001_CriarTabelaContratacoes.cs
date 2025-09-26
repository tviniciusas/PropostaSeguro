using FluentMigrator;

namespace ContratacaoService.Infrastructure.Migrations;

[Migration(001)]
public class CriarTabelaContratacoes : Migration
{
    public override void Up()
    {
        Create.Table("contratacoes")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("propostaid").AsGuid().NotNullable()
            .WithColumn("datacontratacao").AsDateTime2().NotNullable()
            .WithColumn("numeroapolice").AsString(50).NotNullable()
            .WithColumn("datavigenciainicio").AsDateTime2().NotNullable()
            .WithColumn("datavigenciafim").AsDateTime2().NotNullable();

        Create.Index("IX_Contratacoes_PropostaId")
            .OnTable("contratacoes")
            .OnColumn("propostaid")
            .Unique();

        Create.Index("IX_Contratacoes_NumeroApolice")
            .OnTable("contratacoes")
            .OnColumn("numeroapolice")
            .Unique();

        Create.Index("IX_Contratacoes_DataContratacao")
            .OnTable("contratacoes")
            .OnColumn("datacontratacao");
    }

    public override void Down()
    {
        Delete.Table("contratacoes");
    }
}