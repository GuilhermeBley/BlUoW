namespace BlUoW.Dapper.Tests.Model;

internal class Table2
{
    public Guid IdGuid { get; } = Guid.NewGuid();
    public string Id => IdGuid.ToString("N");
    public Guid Execution { get; set; }
    public string ExecutionGuid => Execution.ToString("N");
    public string? Message { get; set; }
    public DateTime InsertAt { get; set; } = DateTime.Now;
}