using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace linq2md {
    public class MarkDown {
        public string Title{
            get;
            set;
        }

        public List<Element> RootElements{
            get;
            set;
        } 
    }

    public enum Kind{
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

    public class Element{
        public int Level{
            get;
            private set;
        }

        public int Indent {
            get;
            private set;
        }

        public Kind Kind{
            get;
            private set;
        }
    }

    public class Section : Element {
        public Element Title{
            get;
            set;
        }
        public List<Element> Values {
            get;
            set;
        }
        public List<Section> SubSectons {
            get;
            set;
        }
    }
    
    public class List: Element {
        public ListKind ListKind {
            get;
            set;
        }
        public List<Element> Items {
            get;
            set;
        }
    }
    
    public class Cell : Element {
        public Element Value{
            get;
            set;
        }
        public string Align{
            get{
                return "left";
            }
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
        }
    }

    public class Picture : Element{
        
    }

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
        APL,
        Forth,
        Haskell
    }

    public class Line : Element {
        public string Value {
            get;
            set;
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
        public Code() {
            Lines = new List<Line>();
        }
    }

    public class HyperLink : Element{
        public string Text {
            get;
            set;
        }
        public Uri Url {
            get;
            set;
        }
    }

    public class Formula : Element {
        
    }

    public class Paragraph : Element {
        public List<Element> Values {
            get;
            private set;
        }
        public Paragraph() {
            Values = new List<Element>();
        }
    }

    public class Text : Element{
        public string Value {
            get;
            set;
        }
    }

    public class Stronger : Element{
        public string Value {
            get;
            set;
        }
    }

    public class Italic : Element{
        public string Value {
            get;
            set;
        }
    }

    public class Delete : Element{
        public string Value {
            get;
            set;
        }
    }
    public class Blockquote : Element {
        public List<Element> Values {
            get;
            private set;
        }
        public Blockquote() {
            Values = new List<Element>();
        }
    }
}
