﻿using Luban.Core.DataLoader;
using Luban.Core.Datas;
using Luban.Core.Defs;
using Luban.Core.Types;
using Luban.Core.Utils;
using Luban.DataLoader.Builtin.DataVisitors;
using YamlDotNet.RepresentationModel;

namespace Luban.DataLoader.Builtin.Yaml;

[DataLoader("yml")]
public class YamlDataLoader : DataLoaderBase
{
    private YamlNode _root;
    public override void Load(DefTable table, string rawUrl, string sheetOrFieldName, Stream stream)
    {
        var ys = new YamlStream();
        ys.Load(new StreamReader(stream));
        var rootNode = ys.Documents[0].RootNode;

        this._root = rootNode;

        if (!string.IsNullOrEmpty(sheetOrFieldName))
        {
            if (sheetOrFieldName.StartsWith("*"))
            {
                sheetOrFieldName = sheetOrFieldName.Substring(1);
            }
            if (!string.IsNullOrEmpty(sheetOrFieldName))
            {
                foreach (var subField in sheetOrFieldName.Split('.'))
                {
                    this._root = _root[new YamlScalarNode(subField)];
                }
            }
        }
    }

    public override List<Record> ReadMulti(DefTable table, TBean type)
    {
        var records = new List<Record>();
        foreach (var ele in (YamlSequenceNode)_root)
        {
            var rec = ReadRecord(ele, type);
            if (rec != null)
            {
                records.Add(rec);
            }
        }
        return records;
    }

    private static readonly YamlScalarNode s_tagNameNode = new(FieldNames.TAG_KEY);

    public override Record ReadOne(DefTable table, TBean type)
    {
        return ReadRecord(_root, type);
    }

    private Record ReadRecord(YamlNode yamlNode, TBean type)
    {
        string tagName;
        if (((YamlMappingNode)yamlNode).Children.TryGetValue(s_tagNameNode, out var tagNode))
        {
            tagName = (string)tagNode;
        }
        else
        {
            tagName = null;
        }
        if (DataUtil.IsIgnoreTag(tagName))
        {
            return null;
        }
        var data = (DBean)type.Apply(YamlDataCreator.Ins, yamlNode, type.DefBean.Assembly);
        var tags = DataUtil.ParseTags(tagName);
        return new Record(data, RawUrl, tags);
    }
}