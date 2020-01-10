using IniParser;
using IniParser.Model;
using Serializer.Enteties;
using Serializer.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;
using TicTacToeMatch.Factory;
using TicTacToeMatch.Interfaces;

namespace Serializer.Internal
{
    internal class IniParseData : IIniParseData
    {
        private IMatrixAlgorithm MatrixAlgorithm { get; set; }

        public void IniWriter(String fileName, Data data)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData iniData = new IniData();

            iniData["Settings"].AddKey("Round", data.Round.ToString());
            iniData["Settings"].AddKey("Difficulty", data.Difficulty.ToString());

            iniData["CurrentGame"].AddKey("CurrentTurnCount", data.CurrentGame.CurrentTurnCount.ToString());
            iniData["CurrentGame"].AddKey("BoardSize", data.CurrentGame.BoardSize.ToString());
            iniData["CurrentGame"].AddKey("CurrentTurn", data.CurrentGame.CurrentTurn.ToString());
            iniData["CurrentGame"].AddKey($"Board", data.CurrentGame.BoardData);

            for (Int32 i = 0; i < data.HistoryList.Count; i++)
            {
                iniData[$"HistoryList-{i}"].AddKey("Winner", data.HistoryList[i].Winner);
                iniData[$"HistoryList-{i}"].AddKey("RoundCount", data.HistoryList[i].RoundCount.ToString());
            }

            parser.WriteFile(fileName, iniData);
        }

        public Data ReadIni(String fileName)
        {
            this.MatrixAlgorithm = (IMatrixAlgorithm)GlobalFactory.Create(typeof(IMatrixAlgorithm));
            FileIniDataParser parser = new FileIniDataParser();
            IniData iniData = parser.ReadFile(fileName);
            SavedGameState savedGameState = new SavedGameState
            {
                BoardSize = this.MatrixAlgorithm.BoardSize,
                CurrentTurn = this.MatrixAlgorithm.CurrentTurn,
                CurrentTurnCount = this.MatrixAlgorithm.CurrentTurnCount,
            };
            Data data = new Data
            {
                CurrentGame = savedGameState,
                HistoryList = new List<History>()
            };
            data.CurrentGame.CurrentTurnCount = Convert.ToInt32(iniData["CurrentGame"]["CurrentTurnCount"]);
            data.CurrentGame.BoardSize = Convert.ToInt32(iniData["CurrentGame"]["BoardSize"]);
            data.Round = Convert.ToInt32(iniData["Settings"]["Difficutly"]);
            data.Difficulty = (Difficulty)Enum.Parse(typeof(Difficulty), iniData["Settings"]["Difficulty"]);
            Int32 sectionIndex = 0;
            using (StreamReader sr = File.OpenText(fileName))
            {
                foreach (SectionData section in iniData.Sections)
                {
                    sectionIndex++;
                }
            }

            for (Int32 i = 0; i < sectionIndex - 2; i++)
            {
                History history = new History
                {
                    Winner = iniData[$"HistoryList-{i}"]["Winner"],
                    RoundCount = Convert.ToInt32(iniData[$"HistoryList-{i}"]["RoundCount"])
                };
                data.HistoryList.Add(history);
            }

            String key = $"Board";
            String value = iniData["CurrentGame"][key];

            data.CurrentGame.BoardData = value;

            return data;
        }
    }
}