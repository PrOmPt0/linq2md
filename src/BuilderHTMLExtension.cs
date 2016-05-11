using System;
using System.Collections.Generic;
using System.Linq;
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
    public static class BuilderHTMLExtension {
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
