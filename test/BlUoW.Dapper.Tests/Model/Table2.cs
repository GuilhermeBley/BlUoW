namespace BlUoW.Dapper.Tests.Model;

internal class Table2
{
    public Guid Id { get; set; }
    public Guid Execution { get; set; }
    public string? Message { get; set; }
    public DateTime InsertAt { get; set; } = DateTime.Now;
}