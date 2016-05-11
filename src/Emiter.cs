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
                case Kind.Cell:
                    code.EmitCell(e as Cell);
                    break;
                case Kind.Row:
                    code.EmitRow(e as Row);
                    break;
                case Kind.Table:
                    code.EmitTable(e as Table);
                    break;
                case Kind.OrdedList:
                    code.EmitOrdedList(e as OrdedList);
                    break;
                case Kind.UnOrdedList:
                    code.EmitUnOrdedList(e as UnOrdedList);
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
                default:
                    break;
            }
            return code;
        }
        private static StringBuilder EmitSection(this StringBuilder code,Section title){
            var level = title.Level;
            var h = string.Format("h{0}", level);
            var id = title.ToString();
            code.Begin(h, "id".v(id))
                .EmitElement(title)
                .End(h);
            return code;
        }
        private static StringBuilder EmitCell(this StringBuilder code,Cell cell){
            code.Begin(cell.Tag, "align".v(cell.Align))
                .EmitElement(cell.Value)
                .End(cell.Tag);
            return code;
        }
        private static StringBuilder EmitRow(this StringBuilder code,Row row){
            return code;
        }
        private static StringBuilder EmitTable(this StringBuilder code, Table row) {
            return code;
        }
        private static StringBuilder EmitOrdedList(this StringBuilder code, OrdedList row) {
            return code;
        }
        private static StringBuilder EmitUnOrdedList(this StringBuilder code, UnOrdedList row) {
            return code;
        }
        private static StringBuilder EmitCode(this StringBuilder code, Code row) {
            return code;
        }
        private static StringBuilder EmitPicture(this StringBuilder code, Picture row) {
            return code;
        }
        private static StringBuilder EmitFormula(this StringBuilder code, Formula row) {
            return code;
        }
        private static StringBuilder EmitHyperLink(this StringBuilder code, HyperLink row) {
            return code;
        }
        private static StringBuilder EmitText(this StringBuilder code, Text row) {
            return code;
        }
        private static StringBuilder EmitStronger(this StringBuilder code, Stronger row) {
            return code;
        }
        private static StringBuilder EmitItalic(this StringBuilder code, Italic row) {
            return code;
        }
        private static StringBuilder EmitDelete(this StringBuilder code, Delete row) {
            return code;
        }
    }
}
