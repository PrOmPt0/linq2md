﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UniversalMarkdown.Parse;
using UniversalMarkdown.Parse.Elements;

namespace linq2md {
    public static class Parser{

        public static MarkDown Parse(this string file){
            var doc = new MarkdownDocument();
            doc.Parse(File.ReadAllText(file));
            return doc.Block2Element() as MarkDown;
        }

        public static Element Block2Element(this MarkdownBlock block){
            switch (block.Type){
                case MarkdownBlockType.Root:
                    var doc = block as MarkdownDocument;
                    var md = new MarkDown();
                    md.Title = "";
                    md.RootElements.AddRange(doc.Blocks.Select(b=>b.Block2Element()));
                    return md;
                case MarkdownBlockType.Header:
                    var headerBlock = block as HeaderBlock;
                    var section = new Section();
                    var titleLine = new Seq();
                    titleLine.Values.AddRange(headerBlock.Inlines.Select(l=>l.Inline2Element()));
                    section.Title = titleLine;
                    section.Level = headerBlock.HeaderLevel;
                    return section;
                case MarkdownBlockType.Code:
                    var code = block as CodeBlock;
                    
                    var lines = code.Lines.Select(l => new Line(){
                        Value = l
                    });
                    var c = new Code();
                    c.Lines.AddRange(lines);
                    c.CodeKind = CodeKind.Block;
                    return c;
                case MarkdownBlockType.Quote:
                    var quote = block as QuoteBlock;
                    var bq = new Blockquote();
                    bq.Values.AddRange(quote.Blocks.Select(b=>b.Block2Element()));
                    return bq;
                case MarkdownBlockType.Paragraph:
                    var paraBlock = block as ParagraphBlock;
                    var paragraph = new Paragraph();
                    paragraph.Values.AddRange(paraBlock.Inlines.Select(l=>l.Inline2Element()));
                    return paragraph;
                case MarkdownBlockType.Table:
                    var tableBlock = block as TableBlock;
                    var table = new Table();

                    int ri = 0;
                    foreach (var rowBlock in tableBlock.Rows){
                        var row = new Row();
                        var cd = 0;
                        if(ri==0){
                            row.RowKind = tableBlock.HasHeaderRow ? RowKind.Head : RowKind.Body;
                            ri++;
                        }else{
                            row.RowKind = RowKind.Body;
                        }

                        foreach (var cellBlock in rowBlock.Cells) {
                            var column=tableBlock.ColumnDefinitions[cd++];
                            var cell = new Cell();
                            var blockLine = new Seq();
                            blockLine.Values.AddRange(cellBlock.Inlines.Select(i => i.Inline2Element()));
                            cell.Value = blockLine;
                            cell.Align = column.Alignment.ColumnAlignToString();

                            cell.CellKind = row.RowKind == RowKind.Head ? CellKind.Head : CellKind.Cell;
                            row.Cells.Add(cell);
                        }

                        table.Rows.Add(row);
                    }
                    return table;
                case MarkdownBlockType.LinkReference:
                    var link = block as LinkReferenceBlock;
                    var hyperlink = new HyperLink();
                    hyperlink.Text = new Text{Value = link.ToString()};
                    hyperlink.Url =  new Uri(link.Url);
                    return hyperlink;
                case MarkdownBlockType.HorizontalRule:
                    var hr = block as HorizontalRuleBlock;
                    return new Rule { Value = hr.ToString()};
                case MarkdownBlockType.List:
                    var listBlock = block as ListBlock;
                    var list = new List();
                    list.ListKind = listBlock.Style == ListStyle.Numbered ? ListKind.Order : ListKind.UnOrder;

                    foreach (var listItemBlock in listBlock.Items){
                        foreach (var b in listItemBlock.Blocks) {
                            var e = b.Block2Element();
                            list.Items.Add(e);
                        }
                    }
                    return list;
                default:
                    return null;
            }
        }

        private static Element Paragraph2Seq(this Element e){
            if(e.Kind == Kind.Paragraph){
                var p = e as Paragraph;
                var b = new Seq();
                b.Values.AddRange(p.Values);
                return b;
            }
            return e;
        }

        private static string ColumnAlignToString(this ColumnAlignment ca){
            switch (ca){
                case ColumnAlignment.Left:
                    return "left";
                case ColumnAlignment.Right:
                    return "right";
                case ColumnAlignment.Center:
                    return "center";
                default:
                    return "left";
            }
        }

        public static Element Inline2Element(this MarkdownInline inline){
            switch (inline.Type){
                case MarkdownInlineType.Italic:
                    return new Italic{Value = inline.ToString()};
                case MarkdownInlineType.MarkdownLink:
                    var ml = inline as MarkdownLinkInline;
                    var text = new Seq();
                    text.Values.AddRange(ml.Inlines.Select(i=>i.Inline2Element()));
                    return new HyperLink(){Text = text, Url = new Uri(ml.Url)};
                case MarkdownInlineType.TextRun:
                    var t = inline as TextRunInline;
                    return new Text{Value = t.Text};
                case MarkdownInlineType.Bold:
                    var b = inline as BoldTextInline;
                    var bv = new Seq();
                    bv.Values.AddRange(b.Inlines.Select(i => i.Inline2Element()));
                    return new Stronger{Value = bv};
                case MarkdownInlineType.RawHyperlink:
                    var h = inline as HyperlinkInline;
                    var ht = new Text();
                    ht.Value = h.Text;
                    return new HyperLink{Text = ht,Url = new Uri(h.Url)};
                case MarkdownInlineType.Strikethrough:
                    var s = inline as StrikethroughTextInline;
                    var st = new Seq();
                    st.Values.AddRange(s.Inlines.Select(i => i.Inline2Element()));
                    return new Delete{Value = st};
                case MarkdownInlineType.Superscript:
                    // TODO: 暂时不支持
                    return null;
                case MarkdownInlineType.RawSubreddit:
                    // TODO: 暂时不支持
                    return null;
                case MarkdownInlineType.Code:
                    var c = inline as CodeInline;
                    var cc = new Code();
                    cc.CodeKind = CodeKind.Inline;
                    var ll = new Line{Value = c.Text};
                    cc.Lines.Add(ll);
                    return cc;
                default:
                    return null;
            }
        }
    }
}
