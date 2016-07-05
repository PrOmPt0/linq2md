// Copyright (c) 2016 Quinn Damerell
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Text;
using UniversalMarkdown.Helpers;

namespace UniversalMarkdown.Parse.Elements {
    /// <summary>
    /// Represents a block of text that is displayed in a fixed-width font.  Inline elements and
    /// escape sequences are ignored inside the code block.
    /// </summary>
    public class CodeBlock : MarkdownBlock {
        /// <summary>
        /// Initializes a new code block.
        /// </summary>
        public CodeBlock()
            : base(MarkdownBlockType.Code) {
            Lines = new List<string>();
        }

        /// <summary>
        /// The source code to display.
        /// </summary>
        public List<string> Lines {
            get;
            private set;
        }

        /// <summary>
        /// Parses a code block.
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The location of the first character in the block. </param>
        /// <param name="maxEnd"> The location to stop parsing. </param>
        /// <param name="quoteDepth"> The current nesting level for block quoting. </param>
        /// <param name="actualEnd"> Set to the end of the block when the return value is non-null. </param>
        /// <returns> A parsed code block, or <c>null</c> if this is not a code block. </returns>
        internal static CodeBlock Parse(string markdown, int start, int maxEnd, int quoteDepth, out int actualEnd){
            var lines = new List<string>();
            StringBuilder code=null;
            actualEnd=start;

            bool first=true;
            bool startWithTripleBackQuote = false;
            foreach (var lineInfo in Common.ParseLines(markdown, start, maxEnd, quoteDepth)) {

                // Add every line that starts with a tab character or at least 4 spaces.
                int pos=lineInfo.StartOfLine;

                // Start code with triple backquote
                if (first) {
                    var firstLine=markdown.Substring(lineInfo.StartOfLine, lineInfo.EndOfLine-lineInfo.StartOfLine);
                    if (firstLine.Trim()=="```"){
                        startWithTripleBackQuote = true;
                    }
                    first=false;
                    continue;
                }

                // End code with triple backquote
                if(startWithTripleBackQuote){
                    if (markdown.Substring(lineInfo.StartOfLine, lineInfo.EndOfLine - lineInfo.StartOfLine).Trim() == "```"){
                        actualEnd = lineInfo.StartOfNextLine;
                        break;
                    }
                }else{
                    // Start code with tab or 4 spaces
                    if (pos<maxEnd&&markdown[pos]=='\t') {
                        // tab
                        pos++;
                    } else {
                        // 4 spaces
                        int spaceCount=0;
                        while (pos<maxEnd&&spaceCount<4) {
                            if (markdown[pos]==' ') {
                                spaceCount++;
                            } else if (markdown[pos]=='\t') {
                                spaceCount+=4;
                            } else {
                                break;
                            }
                            pos++;
                        }

                        // NOT code scope
                        if (spaceCount<4) {
                            // We found a line that doesn't start with a tab or 4 spaces.
                            // But don't end the code block until we find a non-blank line.
                            if (lineInfo.IsLineBlank==false) {
                                break;
                            }
                        }
                    }                    
                }

                // Separate each line of the code text.
                if (code==null){
                    code = new StringBuilder();
                } else{
                    lines.Add(code.ToString());
                    code.Clear();
                }

                if (lineInfo.IsLineBlank==false) {
                    // Append the code text, excluding the first tab/4 spaces, and convert tab characters into spaces.
                    string lineText=markdown.Substring(pos, lineInfo.EndOfLine-pos);
                    int startOfLinePos=code.Length;
                    for (int i=0; i<lineText.Length; i++) {
                        char c=lineText[i];
                        if (c=='\t'){
                            code.Append(' ', 4 - ((code.Length - startOfLinePos)%4));
                        } else{
                            code.Append(c);
                        }
                    }
                }

                // Update the end position.
                actualEnd=lineInfo.StartOfNextLine;
            }

            if (code==null) {
                // Not a valid code block.
                actualEnd=start;
                return null;
            }else{
                if(code.Length>0){
                    lines.Add(code.ToString());
                    code.Clear();
                }
            }

            // Blank lines should be trimmed from the start and end.
            var codeBlock = new CodeBlock();
            codeBlock.Lines = lines;
            return codeBlock;
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString(){
            var sb = new StringBuilder();
            foreach (var line in Lines){
                sb.Append(line).AppendLine();
            }
            return sb.ToString();
        }
    }
}
