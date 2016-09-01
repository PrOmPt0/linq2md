using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace linq2md {
    public static class Emiter {
        public static string Emit(this MarkDown markdown){
            var code = new StringBuilder();
            code.EmitImpl(markdown);
            return code.ToString();
        }

        private static StringBuilder EmitImpl(this StringBuilder code, MarkDown markdown) {
            code.Line("<!DOCTYPE html>")
                .Begin("head")
                    .Begin("meta","charset".v("utf-8"))
                    .Begin("meta","name".v("viewport"),"content".v("width=device-width, initial-scale=1.0"))
                    
                    .Begin("tile")
                        .Line(markdown.Title)
                    .End("title")

                    .Begin("link",
                        "rel".v("stylesheet"),
                        "href".v("./base.css"))
                    .End("link")
                    
                    .Begin("script", 
                        "type".v("text/javascript"), 
                        "src".v("./math.js"))
                    .End("script")
                    
                    .Begin("body")
                        .Begin("div","class".v("container"))
                            .EmitElements(markdown.RootElements)
                        .End("div")
                    .End("body")
                .End("head")
                .Line("");
            return code;
        }

        private static StringBuilder EmitElements(this StringBuilder code,IEnumerable<Element> l){
            foreach (var e in l){
                code.EmitElement(e);
            }
            return code;
        }

        private static StringBuilder EmitElement(this StringBuilder code,Element e){
            switch (e.Kind){
                case Kind.Section:
                    code.EmitSection(e as Section);
                    break;
                case Kind.BlockQuote:
                    code.EmitBlockQuote(e as Blockquote);
                    break;
                case Kind.BlockLine:
                    code.EmitBlockLine(e as Seq);
                    break;
                case Kind.Cell:
                    code.EmitCell(e as Cell);
                    break;
                case Kind.Row:
                    code.EmitRow(e as Row);
                    break;
                case Kind.Table:
                    code.EmitTable(e as Table);
                    break;
                case Kind.List:
                    code.EmitList(e as List);
                    break;
                case Kind.Line:
                    code.EmitLine(e as Line);
                    break;
                case Kind.Code:
                    code.EmitCode(e as Code);
                    break;
                case Kind.Picture:
                    code.EmitPicture(e as Picture);
                    break;
                case Kind.Formula:
                    code.EmitFormula(e as Formula);
                    break;
                case Kind.HyperLink:
                    code.EmitHyperLink(e as HyperLink);
                    break;
                case Kind.Paragraph:
                    code.EmitParagraphics(e as Paragraph);
                    break;
                case Kind.Text:
                    code.EmitText(e as Text);
                    break;
                case Kind.Stronger:
                    code.EmitStronger(e as Stronger);
                    break;
                case Kind.Italic:
                    code.EmitItalic(e as Italic);
                    break;
                case Kind.Delete:
                    code.EmitDelete(e as Delete);
                    break;
                case Kind.Seq:
                    code.EmitSeq(e as Seq);
                    break;
                case Kind.HorizontalRule:
                    code.EmitRule(e as Rule);
                    break;
                default:
                    break;
            }
            return code;
        }

        private static StringBuilder EmitRule(this StringBuilder code, Rule r) {
            code.Append("<hr/>");
            return code;
        }

        private static StringBuilder EmitSeq(this StringBuilder code, Seq seq) {
            foreach (var s in seq.Values) {
                code.EmitElement(s);
            }
            return code;
        }

        private static StringBuilder EmitSection(this StringBuilder code,Section title){
            var level = title.Level;
            var h = string.Format("h{0}", level);
            var id = title.ToString();
            code.Begin(h, "id".v(id))
                .EmitElement(title.Title)
                .End(h);

            foreach (var v in title.Values) {
                code.EmitElement(v);
            }

            foreach (var subSection in title.SubSectons) {
                code.EmitSection(subSection);
            }

            return code;
        }

        private static StringBuilder EmitCell(this StringBuilder code,Cell cell){
            code.Begin(cell.Tag, "align".v(cell.Align))
                .EmitElement(cell.Value)
                .End(cell.Tag);
            return code;
        }

        private static StringBuilder EmitRow(this StringBuilder code,Row row){

            code.Begin("tr");
            foreach (var cell in row.Cells) {
                code.EmitCell(cell);        
            }
            code.End("tr");

            return code;
        }

        private static StringBuilder EmitTable(this StringBuilder code, Table table){
            code.Begin("table");

            code.EmitRow(table.Head);

            foreach (var row in table.Rows) {
                code.EmitRow(row);        
            }

            code.End("table");
            return code;
        }

        private static StringBuilder EmitList(this StringBuilder code, List list) {
            var tag = list.ListKind == ListKind.Order ? "ol" : "ul";

            if(list.Value!=null){
                code.EmitElement(list.Value);
            }

            code.Begin(tag);
            foreach (var i in list.Items) {
                if(i.Kind == Kind.List){
                    code.EmitList(i as List);
                }else{
                    code.Begin("li");
                    code.EmitElement(i);
                    code.End("li");
                }
            }

            code.End(tag);
            
            return code;
        }

        private static StringBuilder EmitLine(this StringBuilder code, Line l) {
            // TODO(fanfeilong): 引入code parser，做高级高亮
            code.AppendFormat("{0}{1}\n",new string(' ',l.Indent),l.Value);
            return code;
        }

        private static StringBuilder EmitCode(this StringBuilder code, Code c) {
            
            if(c.CodeKind == CodeKind.Block){
                code.Begin("pre", "class".v("prettyprint"))
                    .Begin("code", "class".v("hljs cs"));

                foreach (var l in c.Lines){
                    code.EmitLine(l);
                }

                code.End("code")
                    .End("pre");
            }else{
                code.Begin("code");
                code.Append(c.Lines[0].Value);
                code.End("code");
            }

            return code;
        }

        private static StringBuilder EmitPicture(this StringBuilder code, Picture row) {
            return code;
        }

        private static StringBuilder EmitFormula(this StringBuilder code, Formula row) {
            return code;
        }

        private static StringBuilder EmitHyperLink(this StringBuilder code, HyperLink link ){
            
            code.Begin("a","href".v(link.Url.AbsoluteUri))
                .EmitElement(link.Text)
                .End("a");

            return code;
        }

        private static StringBuilder EmitParagraphics(this StringBuilder code, Paragraph p) {
            code.Begin("p");
            foreach (var v in p.Values) {
                code.EmitElement(v);
            }
            code.End("p");
            return code;
        }

        private static StringBuilder EmitText(this StringBuilder code, Text text) {
            code.Append(text.Value);
            return code;
        }

        private static StringBuilder EmitStronger(this StringBuilder code, Stronger text){
            code.Begin("strong");
            code.EmitElement(text.Value);    
            code.End("strong");
            return code;
        }

        private static StringBuilder EmitItalic(this StringBuilder code, Italic text) {
            code.Begin("em")
                 .Append(text.Value)
                 .End("em");
            return code;
        }

        private static StringBuilder EmitDelete(this StringBuilder code, Delete text) {
            code.Begin("del");
            code.EmitElement(text.Value);     
            code.End("del");
            return code;
        }

        private static StringBuilder EmitBlockQuote(this StringBuilder code, Blockquote quote) {
            code.Begin("blockquote");
            foreach (var q in quote.Values) {
                code.EmitElement(q);
            }    
            code.End("blockquote");
            return code;
        }

        private static StringBuilder EmitBlockLine(this StringBuilder code,Seq line){
            foreach (var element in line.Values){
                code.EmitElement(element);
            }
            return code;
        }

        private enum HilightKind{
            Addition,
            Aggregate,
            Annotation,
            Argument,
            Array,
            Attr_selector,
            Attributer, //TODO(change css class)
            Blockquote,
            Built_in,
            Bullet,
            Cbracket,
            Cdata,
            Change,
            Char,
            Chunk,
            Class,
            Command,
            Comment,
            Constant,
            Date,
            Decorator,
            Deletion,
            Doctype,
            Emphasis,
            Envvar,
            Filter,
            Flow,
            Formula,
            Function_or_atom,
            Header,
            Hexcolor,
            Id,
            Important,
            Javadoc,
            Javadoctag,
            Keyword,
            Label,
            Link_label,
            Link_url,
            Literal,
            Localvars,
            Number,
            Parent,
            Phony,
            Phpdoc,
            Pi,
            Pragma,
            Preprocessor,
            Prompt,
            Property,
            Pseudo,
            Regexp,
            Request,
            Reserved,
            Rules,
            Shebang,
            Special,
            Sqbracket,
            Status,
            Stream,
            String,
            Strong,
            Subst,
            Symbol,
            Tag,
            Template_comment,
            Template_tag,
            Title,
            Typename,
            Value,
            Variable,
            Winutils,
            Yardoctag,
        }
        private static StringBuilder EmitSpan(this StringBuilder code,Text t,HilightKind hl){
            code.Begin("span", "class".v(string.Format("hjls-{0}", hl.ToString().ToLower())))
                .EmitText(t)
                .End("span");
            return code;
        }
    }
}
