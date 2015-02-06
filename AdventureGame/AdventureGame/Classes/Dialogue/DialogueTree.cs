﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureGame
{
    public class DialogueTree
    {
        public string StartingFilePath { get; set; }
        public string CurrentFilePath { get; set; }
        public string StatementsFile { get; set; }
        public string AnswersFile { get; set; }
        public int LastStatement { get; set; }

        public DialogueTree(string filePath)
        {
            if (filePath != "")
            {
                StartingFilePath = filePath;
                CurrentFilePath = SaveHandler.CurrentSavePath + System.IO.Path.GetFileName(StartingFilePath);
                if (System.IO.File.Exists(CurrentFilePath))
                {
                    ParseTextFile(CurrentFilePath);
                }
                else
                {
                    ParseTextFile(StartingFilePath);
                }
            }
        }

        private void ParseTextFile(string filePath)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(':');
                    switch (words[0])
                    {
                        case "Answers":
                            AnswersFile = words[1];
                            break;
                        case "Statements":
                            StatementsFile = words[1];
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// DialogueTree takes over the gameplay; showing statements and giving answer options
        /// </summary>
        /// <returns>What the conversation led to</returns>
        public string StartConversation(int startingStatement = 0) 
        {
            bool stillTalking = true;
            
            //Which statement is to be displayed at the moment
            int statementNumber = startingStatement;
            //Where the outcome of the conversation ends up
            int endState = 0;
            
            while(stillTalking)
            {
                statementNumber = DisplayStatement(statementNumber);
                LastStatement = statementNumber;
                statementNumber = DisplayAnswers(statementNumber, ref endState);
                if (statementNumber == 100) //Change 100 to appropriate number
                    stillTalking = false;
            }

            return ResolveConversation(endState);

        }

        private int DisplayAnswers(int answerNumber, ref int endState)
        {
            if (this.AnswersFile == null)
            {
                return 0;
            }
            else
            {
                return 1; //change this part
            }
        }

        private int DisplayStatement(int statementNumber)
        { return 0; }

        private string ResolveConversation(int state)
        { return "end state"; }

        /// <summary>
        /// Get specified lines from a txt file
        /// </summary>
        /// <param name="txtFile"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        private string[] GetLines(string txtFile, int[] rows)
        {
            string[] linesOfText = null;
            return linesOfText;
        }

        public void Save() { }
    }
}
