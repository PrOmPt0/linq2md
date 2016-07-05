using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace linq2md {

    public enum Kind {
        Root,
        Section,
        List,
        Cell,
        Row,
        Table,
        Picture,
        Line,
        Code,
        HyperLink,
        Formula,
        Paragraph,
        Text,
        Stronger,
        Italic,
        Delete,
        BlockQuote,
        BlockLine,
    }
    
    public enum CellKind {
        Head,
        Cell
    }
    
    public enum RowKind {
        Head,
        Body
    }
    
    public enum ListKind {
        Order,
        UnOrder
    }

    public enum CodeKind{
        Block,
        Inline
    }

    // Support Main languages
    public enum Language {
        C,
        CPP,
        CSharp,
        ObjectC,
        Swift,
        Java,
        JavaScript,
        Lua,
        Scheme,
        Go,
        Python,
        PHP,
        Json,
        Xml,
        MarkDown,
        Tex,
        CSS
    }


    public class Element {
        public int Level {
            get;
            set;
        }

        public int Indent {
            get;
            set;
        }

        public Kind Kind {
            get;
            set;
        }
    }

    public class MarkDown : Element {
        public string Title{
            get;
            set;
        }

        public List<Element> RootElements{
            get;
            private set;
        } 
        public MarkDown(){
            Kind = Kind.Root;
            RootElements = new List<Element>();
        }
    }
    public class Section : Element {
        public Element Title{
            get;
            set;
        }
        public List<Element> Values {
            get;
            private set;
        }
        public List<Section> SubSectons {
            get;
            private set;
        }
        public Section Parent {
            get;
            set;
        }
        public Section(){
            Kind = Kind.Section;
            SubSectons = new List<Section>();
            Values = new List<Element>();
        }
    }
    
    public class List: Element {
        public ListKind ListKind {
            get;
            set;
        }
        public List<Element> Items {
            get;
            private set;
        }

        public Element Value{
            get;
            set;
        }
        public List(){
            Kind = Kind.List;
            Items = new List<Element>();
        } 
    }
    
    public class Cell : Element {
        public Element Value{
            get;
            set;
        }

        public string Align{
            get;
            set;
        }
        public string Tag{
            get{
                switch (CellKind){
                    case CellKind.Head:
                        return "th";
                    case CellKind.Cell:
                        return "td";
                    default:
                        Debug.Assert(false,"NOT Support CellKind");
                        return "td";
                }
            }
        }
        

        public CellKind CellKind{
            get;
            set;
        }
        public Cell(){
            Kind = Kind.Cell;
        }
    }

    public class Row : Element{
        public List<Cell> Cells {
            get;
            private set;
        }
        public RowKind RowKind {
            get;
            set;     
        }
        public Row() {
            Cells = new List<Cell>();
            Kind = Kind.Row;
        }
    }

    public class Table : Element{
        public Row Head {
            get;
            set;
        }
        public List<Row> Rows {
            get;
            private set;            
        }
        public Table() {
            Rows = new List<Row>();
            Kind = Kind.Table;
        }
    }

    public class Picture : Element{
        public Picture(){
            Kind = Kind.Picture;
        }
    }

    public class Line : Element {
        public string Value {
            get;
            set;
        }
        public Line(){
            Kind = Kind.Line;
        }
    }

    public class Code : Element{
        public List<Line> Lines {
            get;
            private set;
        }
        public Language Language {
            get;
            set;
        }

        public CodeKind CodeKind{
            get;
            set;
        }
        public Code() {
            Lines = new List<Line>();
            Kind = Kind.Code;
        }
    }

    public class HyperLink : Element{
        public Element Text {
            get;
            set;
        }
        public Uri Url {
            get;
            set;
        }
        public HyperLink(){
            Kind = Kind.HyperLink;
        }
    }

    public class Formula : Element {
        public Formula(){
            Kind = Kind.Formula;
        }
    }

    public class Paragraph : Element {
        public List<Element> Values {
            get;
            private set;
        }
        public Paragraph() {
            Values = new List<Element>();
            Kind = Kind.Paragraph;
        }
    }

    public class Seq : Element{
        public List<Element> Values{
            get;
            private set;
        }
        public Seq(){
            Values = new List<Element>();
            Kind = Kind.BlockLine;
        }
    }

    public class Text : Element{
        public string Value {
            get;
            set;
        }
        public Text(){
            Kind = Kind.Text;
        }
    }

    public class Stronger : Element{
        public Element Value {
            get;
            set;
        }
        public Stronger(){
            Kind = Kind.Stronger;
        }
    }

    public class Italic : Element{
        public string Value {
            get;
            set;
        }
        public Italic(){
            Kind = Kind.Italic;
        }
    }

    public class Delete : Element{
        public Element Value {
            get;
            set;
        }
        public Delete(){
            Kind = Kind.Delete;
        }
    }
    public class Blockquote : Element {
        public List<Element> Values {
            get;
            private set;
        }
        public Blockquote() {
            Values = new List<Element>();
            Kind = Kind.BlockQuote;
        }
    }
}
