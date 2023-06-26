namespace Unite.Essentials.Tsv.Tests.Models;

public class ParentModel
{
    public string StringValue { get; set; }
    public int? IntValue { get; set; }

    public ChildModel ChildModel { get; set; }
}
