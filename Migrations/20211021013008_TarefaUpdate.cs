using Microsoft.EntityFrameworkCore.Migrations;

namespace MinhasTarefasApi.Migrations
{
    public partial class TarefaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdtarefaApi",
                table: "Tarefas",
                newName: "IdTarefaApi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdTarefaApi",
                table: "Tarefas",
                newName: "IdtarefaApi");
        }
    }
}
