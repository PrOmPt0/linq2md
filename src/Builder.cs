using System.Diagnostics;
using System.Text;

namespace linq2md {

     public class attr{
        public string key{
            get;
            private set;
        }

        public string value{
            get;
            private set;
        }
        public attr(string k,string v){
            key = k;
            value = v;
        }
    }

    public static class BuilderExtension {
        private static int indent=0;
        private static int lastIndent = 0;
        
        private static void ResetIndent(){
            if (lastIndent>0) {
                indent=lastIndent;
                lastIndent=0;
            }
        }
        
        public static StringBuilder Head(this StringBuilder code, string text) {
            lastIndent=indent;
            indent=0;
            var space=new string('\t', lastIndent);
            code.AppendFormat("{0}{1}", space, text);
            return code;
        }

        public static StringBuilder Push(this StringBuilder code) {
            if (lastIndent>0) {
                indent = ++lastIndent;
                lastIndent = 0;
            } else {
                indent++;
            }
            return code;
        }

        public static StringBuilder Pop(this StringBuilder code) {
            if (lastIndent>0){
                indent=--lastIndent;
                lastIndent=0;
                Debug.Assert(indent>=0);
            }else{
                indent--;
                Debug.Assert(indent>=0);
            }
            
            
            return code;
        }

        public static StringBuilder Line(this StringBuilder code) {
            code.AppendLine();
            ResetIndent();
            return code;
        }

        public static StringBuilder Line(this StringBuilder code, string text, bool ignoreIndent=false){
            if (ignoreIndent) {
                code.AppendLine(text);
            } else {
                var space=new string('\t', indent);
                code.AppendFormat("{0}{1}\n", space, text);
            }
            ResetIndent();
            return code;
        }
        
        public static StringBuilder FormatLine(this StringBuilder code, string format, params object[] args) {
            var space=new string('\t', indent);
            var text=string.Format(format, args);
            code.AppendFormat("{0}{1}\n", space, text);
            ResetIndent();
            return code;
        }
        
        public static StringBuilder PushLine(this StringBuilder code, string text) {
            return code.Line(text).Push();
        }
        
        public static StringBuilder PopLine(this StringBuilder code, string text) {
            return code.Pop().Line(text);
        }

        public static StringBuilder PopLine(this StringBuilder code) {
            return code.Pop().Line();
        }

        public static attr v(this string k,string v){
            return new attr(k,v);
        }

        public static StringBuilder Begin(this StringBuilder code,string div,params attr[] attributes){
            var sb = new StringBuilder();
            foreach (var attr in attributes){
                sb.AppendFormat(" {0}=\"{1}\"", attr.key, attr.value);
            }
            code.FormatLine("<{0}{1}>", div,sb.ToString());
            return code;
        }

        public static StringBuilder End(this StringBuilder code, string div) {
            code.FormatLine("</{0}>", div);
            return code;
        }
    }    
}