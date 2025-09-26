using FluentMigrator;

namespace PropostaService.Infrastructure.Migrations;

[Migration(001)]
public class CriarTabelaPropostas : Migration
{
    public override void Up()
    {
        Create.Table("propostas")
            .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("nomecliente").AsString(200).NotNullable()
            .WithColumn("cpfcliente").AsString(11).NotNullable()
            .WithColumn("emailcliente").AsString(250).NotNullable()
            .WithColumn("tiposeguro").AsInt32().NotNullable()
            .WithColumn("valorcobertura").AsDecimal(18, 2).NotNullable()
            .WithColumn("valorpremio").AsDecimal(18, 2).NotNullable()
            .WithColumn("status").AsInt32().NotNullable()
            .WithColumn("datacriacao").AsDateTime2().NotNullable()
            .WithColumn("dataatualizacao").AsDateTime2().Nullable()
            .WithColumn("observacoes").AsString(1000).Nullable();

        Create.Index("IX_Propostas_CpfCliente")
            .OnTable("propostas")
            .OnColumn("cpfcliente");

        Create.Index("IX_Propostas_Status")
            .OnTable("propostas")
            .OnColumn("status");

        Create.Index("IX_Propostas_DataCriacao")
            .OnTable("propostas")
            .OnColumn("datacriacao");
    }

    public override void Down()
    {
        Delete.Table("propostas");
    }
}