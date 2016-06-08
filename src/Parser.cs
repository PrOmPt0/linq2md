using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace linq2md {
    public static class Parser {
        public static MarkDown Parse(this string file) {
            var md = new MarkDown();

            var lines = File.ReadAllLines(file).ToList();

            IEnumerable<Element> sections = lines.ParseSections();

            md.RootElements = sections.ToList();

            return md;
        }

        private static List<Element> ParseElement(this List<string> lines, int index) {
            return null;
        }

        private static IEnumerable<Section> ParseSections(this List<string> lines) {
            var sections = new List<Section>();
            var index = 0;

            Section section = null;
            var sectionValues = new List<string>();
            while (index < lines.Count) {
                var l = lines[index++];

                // begin sectoin
                if (l.StartsWith("#")) {
                    var newSection = new Section();
                    var title = l.ParseTitle();
                    newSection.Level = title.Item1;
                    newSection.Indent = 0;
                    newSection.Title = title.Item2;

                    if (section != null) {
                        section.Values = sectionValues.ParseValues();

                        if (section.Level < newSection.Level) {
                            section.SubSectons.Add(newSection);
                        } else {
                            var back = section.Level - newSection.Level+1;
                            Section parent = null;
                            while ((back--)>0) {
                                parent = section.Parent;
                                if (parent == null)
                                    break;
                            }
                            if (parent != null) {
                                parent.SubSectons.Add(newSection);
                            } else {
                                sections.Add(newSection);
                            }
                        }        
                    }

                    section = newSection;

                    continue;
                }

                if (section != null) {
                    sectionValues.Add(l);
                }
            }

            return sections;
        }

        private static Tuple<int, Element> ParseTitle(this string line) {
            int level = 0;
            while (line[level++] == '#') {
                //
            }

            return Tuple.Create(level, line.Substring(level).ParseLine());
        }

        private static List<Element> ParseValues(this List<string> values) {
            // TODO:
            return null;
        }

        private static Element ParseLine(this string line) {
            // TODO:
            return null;
        }
    }
}
