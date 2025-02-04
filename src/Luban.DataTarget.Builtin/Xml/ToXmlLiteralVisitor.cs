using System.Text;
using Luban.Core.Datas;
using Luban.Core.DataVisitors;
using Luban.DataExporter.Builtin.Json;

namespace Luban.DataExporter.Builtin.Xml;

public class ToXmlLiteralVisitor : ToLiteralVisitorBase
{
    public static ToXmlLiteralVisitor Ins { get; } = new();
    
    public override string Accept(DBean type)
    {
        throw new NotSupportedException();
    }

    public override string Accept(DArray type)
    {
        throw new NotImplementedException();
    }

    public override string Accept(DList type)
    {
        throw new NotSupportedException();
    }

    public override string Accept(DSet type)
    {
        throw new NotSupportedException();
    }

    public override string Accept(DMap type)
    {
        throw new NotSupportedException();
    }
}