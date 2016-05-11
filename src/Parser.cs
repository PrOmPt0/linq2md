using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace linq2md {
    public static class Parser {

        private static IEnumerable<string> Format(this string protocolFileName) {
            var allText=File.ReadAllText(protocolFileName);
            var lines=new List<string>();
            var level=0;
            int index=0;
            int count=allText.Length;
            var line=new StringBuilder();
            while (index<count) {
                char c=allText[index++];
                if (c=='\r'||
                    c=='\n') {
                    //line.Append(' ');
                    continue;
                }

                if (c=='{') {
                    var space=new string('\t', level);
                    lines.Add(string.Format("{0}{1}", space, line.ToString().Trim()));
                    lines.Add(string.Format("{0}{{", space));
                    line.Clear();
                    level++;
                    continue;
                }
                if (c=='}') {
                    var preLine=line.ToString().Trim();
                    if (!string.IsNullOrEmpty(preLine)) {
                        var space1=new string('\t', level);
                        lines.Add(string.Format("{0}{1}", space1, preLine));
                    }

                    level--;
                    var space=new string('\t', level);

                    int j=index;
                    bool f=false;
                    while (index<count) {
                        c=allText[j++];
                        if (c==' ') {
                            continue;
                        }
                        if (c==';') {
                            f=true;
                            index=j;
                            break;
                        }
                        break;
                    }
                    lines.Add(string.Format("{0}}}{1}\r\n", space, f?";":""));
                    line.Clear();
                    continue;
                }
                if (c==';') {
                    line.Append(c);
                    var space=new string('\t', level);
                    lines.Add(string.Format("{0}{1}", space, line.ToString().Trim()));
                    line.Clear();
                    continue;
                }

                // Ignore single line comment
                if (c=='/') {
                    int j=index;
                    line.Append(c);
                    while (index<count) {
                        c=allText[j++];
                        if (c=='\r'||
                            c=='\n') {
                            break;
                        }
                    }
                    line.Clear();
                    index=j;
                    continue;
                }
                line.Append(c);
            }
            return lines.Where(l=>!string.IsNullOrWhiteSpace(l)).Select(l=>l.Trim());
        }
    }
}
