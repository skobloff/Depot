using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Parse;
using Eto.Parse.Parsers;

namespace Syntax
{
    /// <summary>
    /// Describes the RuleForse test grammar.
    /// </summary>
    public class Syntax : Grammar
    {
        /// <summary>
        /// List of runBase objects for save execute queri of script.
        /// </summary>
        public List<RunBase> runners;

        public Syntax()
            : base("RuleForse-script")
        {
            runners = new List<RunBase>();
            
            #region special symbols
            var WS = -Terminals.WhiteSpace;
            var Terminaler = -Terminals.Set(';');
            #endregion

            #region reserved words
            var rwDeclare = "declare";
            var rwDictionary = "dictionary";
            var rwDocument = "document";
            var rwField = "field";
            var rwOf = "of";
            var rwSelect = "select";
            var rwKnowledge = "knowledge";
            var rwData = "data";
            var rwFrom = "from";
            #endregion

            var Name        = Terminals.Repeat(new RepeatCharItem(Char.IsLetter, 1, 1), new RepeatCharItem(Char.IsLetterOrDigit, 0)).WithName("name");
            var Name1 = +Terminals.LetterOrDigit.WithName("name1");
            var ObjectName  = Terminals.Repeat(new RepeatCharItem(Char.IsLetter, 1, 1), new RepeatCharItem(Char.IsLetterOrDigit, 0)).WithName("objectName");
            var Command     = new RepeatParser { Separator = Terminaler };

            #region declare
            var DeclareDictionary = (WS & rwDeclare & WS & rwDictionary & WS & Name & WS).WithName("declare dictionary");
            DeclareDictionary.Matched += (match) => 
            {
                runners.Add(new RunDeclareDictionary(match));
            }; 

            var DeclareDocument = (WS & rwDeclare & WS & rwDocument & WS & Name & WS).WithName("declare document");
            DeclareDocument.Matched += (match) =>
            {
                runners.Add(new RunDeclareDocument(match));
            };

            var DeclareField = (WS & rwDeclare & WS & rwField & WS & Name & WS & rwOf & WS & ObjectName).WithName("declare field");
            DeclareField.Matched += (match) =>
            {
                runners.Add(new RunDeclareField(match));
            };
            #endregion

            #region select
            var SelectKnowledge = (WS & rwSelect & WS & rwKnowledge).WithName("select knowledge");
            var SelectData = (WS & rwSelect & WS & rwData & WS & rwFrom & ObjectName).WithName("select data");
            #endregion

            Command.Inner = DeclareDictionary | DeclareDocument | DeclareField | SelectKnowledge | SelectData;
            Inner = Command & Terminaler;
        }

       
    }

}
