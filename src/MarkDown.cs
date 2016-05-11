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
        UnOrdedList,
        OrdedList,
        Cell,
        Row,
        Table,
        Picture,
        Code,
        HyperLink,
        Formula,
        Text,
        Stronger,
        Italic,
        Delete,
    }
    public enum CellKind {
        Head,
        Cell
    }

    public class Element{
        public int Level{
            get;
            private set;
        }

        public Kind Kind{
            get;
            private set;
        }
    }

    public class TreeableElement : Element{
        public List<Element> SubSectons{
            get;
            set;
        }
    }


    public class Section : TreeableElement{
        public Element Title{
            get;
            set;
        }
    }
    
    public class Listable: TreeableElement{
        
    }
    
    public class UnOrdedList : Listable{
        
    }
    
    public class OrdedList : Listable{
        
    }


    public class Cell : TreeableElement{
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
                }
            }
        }
        

        public CellKind CellKind{
            get;
            private set;
        }
    }

    public class Row : Element{
        
    }

    public class Table : Element{
        
    }

    public class Picture : Element{
        
    }

    public class Code : Element{
        
    }

    public class HyperLink : Element{
        
    }

    public class Formula : Element {
        
    }

    public class Text : Element{
        
    }

    public class Stronger : Element{
        
    }

    public class Italic : Element{
        
    }

    public class Delete : Element{
        
    }
}
