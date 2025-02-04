namespace Luban.Core.RawDefs;

public class RawTarget
{
    public string Name { get; set; }

    public string Manager { get; set; }
    
    public string TopModule { get; set; }

    public List<string> Groups { get; set; } = new List<string>();

    public List<string> Refs { get; set; } = new List<string>();
}