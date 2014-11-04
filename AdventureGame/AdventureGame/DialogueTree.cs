using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureGame
{
    class DialogueTree
    {
        public string StatementsFile { get; set; }
        public string AnswersFile { get; set; }

        public DialogueTree(string statements, string answers = null)
        {
            StatementsFile = statements;
            AnswersFile = answers;
        }

        /// <summary>
        /// DialogueTree takes over the gameplay; showing statements and giving answer options
        /// </summary>
        /// <returns>What the conversation led to</returns>
        public string startConversation(int startingStatement = 0) 
        {
            bool stillTalking = true;
            
            //Which statement is to be displayed at the moment
            int statementNumber = startingStatement;
            //Where the outcome of the conversation ends up
            int endState = 0;
            
            while(stillTalking)
            {
                statementNumber = displayStatement(statementNumber);
                statementNumber = displayAnswers(statementNumber, ref endState);
                if (statementNumber == 100) //Change 100 to appropriate number
                    stillTalking = false;
            }

            return resolveConversation(endState);

        }

        private int displayAnswers(int answerNumber, ref int endState)
        {
            if (this.AnswersFile == null)
            {
                return 0;
            }
            else
            {
                return 1; //change this parrt
            }
        }

        private int displayStatement(int statementNumber)
        { return 0; }

        private string resolveConversation(int state)
        { return "end state"; }

        /// <summary>
        /// Get specified lines from a txt file
        /// </summary>
        /// <param name="txtFile"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        private string[] getLines(string txtFile, int[] rows)
        {
            string[] linesOfText = null;
            return linesOfText;
        }
    }
}
