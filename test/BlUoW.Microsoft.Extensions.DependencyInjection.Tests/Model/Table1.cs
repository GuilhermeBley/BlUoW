namespace BlUoW.Microsoft.Extensions.DependencyInjection.Tests.Model;

internal class Table1
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid Execution { get; set; }
    public string? Message { get; set; }
    public DateTime InsertAt { get; set; } = DateTime.Now;
}