﻿using DataBaseManager.Entities;
using DataBaseManager.Internal;
using Serializer.Enteties;
using Serializer.Factory;
using Serializer.Interface;
using Serializer.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;
using TicTacToeMatch.Events;
using TicTacToeMatch.Factory;
using TicTacToeMatch.Interfaces;
using TicTacToeUi.Interfaces;
using TicTacToeUi.Internal;

namespace TicTacToeUi
{
    public partial class MainWindow : Form
    {
        private IMatrixAlgorithm matrixAlgorithm;
        private readonly ISettings settings;
        private readonly IGamePanel gamePanel;
        private IAiMove aiMove;
        private ISerializeData serialize;
        private IDeSerializeData deserializeData;
        private IIniParseData iniParseData;
        private SavedGameState savedGameState;
        private Mapper mapper = new Mapper();
        private Int32 CountX;
        private Int32 CountO;
        private Int32 CountDraw;

        public MainWindow(IMatrixAlgorithm matrixAlgorithm, IAiMove aiMove, ISerializeData serialize, IDeSerializeData deserializeData, IIniParseData iniParseData)
        {
            this.matrixAlgorithm = matrixAlgorithm ?? throw new ArgumentNullException(nameof(matrixAlgorithm));
            this.aiMove = aiMove ?? throw new ArgumentNullException(nameof(aiMove));
            this.serialize = serialize ?? throw new ArgumentNullException(nameof(serialize));
            this.deserializeData = deserializeData ?? throw new ArgumentNullException(nameof(deserializeData));
            this.iniParseData = iniParseData ?? throw new ArgumentNullException(nameof(iniParseData));
            this.settings = new Settings
            {
                HistoryList = new List<History>()
            };
            this.gamePanel = new GamePanel();
            this.InitializeComponent();
            this.txtBoxTrackBar.Text = this.boardSizeTrackBar.Value.ToString();
        }

        private void BtnNew_Click(Object sender, EventArgs e)
        {
            this.gamePanel.ButtonList.Clear();
            this.settings.HistoryList.Clear();
            this.dataGrid.DataSource = null;

            this.RestartGame();
        }

        #region Game Setup / Restart

        private void RestartGame()
        {
            if (this.matrixAlgorithm != null)
            {
                this.matrixAlgorithm.EndGame -= this.TicTacToeMatrix_EndGame;
                this.matrixAlgorithm = null;
            }

            this.aiMove = (IAiMove)GlobalFactory.Create(typeof(IAiMove));
            this.Setup(this.boardSizeTrackBar.Value);
        }

        public void Setup(Int32 dimension)
        {
            this.matrixAlgorithm = (IMatrixAlgorithm)GlobalFactory.Create(typeof(IMatrixAlgorithm));
            this.matrixAlgorithm.EndGame += this.TicTacToeMatrix_EndGame;
            this.gamePanel.Difficulty = this.ChooseDifficulty();

            if (dimension < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(dimension));
            }

            this.mainTableLayloutPanel.Controls.Clear();

            this.mainTableLayloutPanel.RowCount = dimension;
            this.mainTableLayloutPanel.ColumnCount = dimension;

            this.gamePanel.ButtonList = new List<Button>();

            for (Int32 row = 0; row < dimension; row++)
            {
                for (Int32 col = 0; col < dimension; col++)
                {
                    Button current = new Button
                    {
                        Height = 62,
                        Width = 62,
                        Tag = new PointIndex(col, row)
                    };
                    this.gamePanel.ButtonList.Add(current);

                    current.Click += this.PlayerButtonOnClick;

                    this.mainTableLayloutPanel.Controls.Add(current, col, row);
                }
            }

            this.matrixAlgorithm.Initialize(dimension);

            if (this.gamePanel.Difficulty == Difficulty.Middle)
            {
                this.ClickAiMiddle();
            }

            this.ShowStats();
        }

        public void ShowStats()
        {
            this.CountDraw = 0;
            this.CountO = 0;
            this.CountX = 0;
            foreach (History item in this.settings.HistoryList)
            {
                if (item.Winner == PlayerType.X.ToString())
                {
                    this.CountX++;
                }
                else if (item.Winner == PlayerType.O.ToString())
                {
                    this.CountO++;
                }
                else
                {
                    this.CountDraw++;
                }
            }
            this.txtBoxX.Text = this.CountX.ToString();
            this.txtBoxO.Text = this.CountO.ToString();
            this.txtBoxDraw.Text = this.CountDraw.ToString();
        }


        public Difficulty ChooseDifficulty()
        {
            if (this.radioBtnEasy.Checked)
            {
                return Difficulty.Easy;
            }

            if (this.radioBtnMedium.Checked)
            {
                return Difficulty.Middle;
            }

            if (this.radioBtnHard.Checked)
            {
                return Difficulty.Hard;
            }

            return Difficulty.FaceToFace;
        }

        #endregion Game Setup / Restart

        #region GameBoard

        private void PlayerButtonOnClick(Object sender, EventArgs e)
        {
            if (!(sender is Button btn))
            {
                return;
            }

            if (!String.IsNullOrWhiteSpace(btn.Text))
            {
                return;
            }

            PointIndex point = (PointIndex)btn.Tag;

            this.matrixAlgorithm.Board[point.X, point.Y] = this.matrixAlgorithm.CurrentTurn;
            btn.Text = this.matrixAlgorithm.CurrentTurn.ToString();
            this.gamePanel.ButtonList.Remove(btn);
            this.matrixAlgorithm.CheckWinner(point, this.matrixAlgorithm.CurrentTurn);

            if (this.matrixAlgorithm.WinnerState)
            {
                this.RestartGame();
                return;
            }

            switch (this.gamePanel.Difficulty)
            {
                case Difficulty.FaceToFace:
                    return;

                case Difficulty.Easy:
                    this.AiEasyMode();
                    break;

                case Difficulty.Hard:
                    this.AiHardMode();
                    break;

                case Difficulty.Middle:
                    this.AiMiddleMode();
                    break;
            }
        }

        #endregion GameBoard

        #region AiDifficulty

        private void ClickAiMiddle()
        {
            Random random = new Random();

            Int32 row = random.Next(0, this.matrixAlgorithm.BoardSize);
            Int32 col = random.Next(0, this.matrixAlgorithm.BoardSize);

            PointIndex point = new PointIndex(col, row);

            foreach (Button btn in this.gamePanel.ButtonList)
            {
                if (btn.Tag.Equals(point))
                {
                    btn.PerformClick();
                    return;
                }
            }
        }

        private void AiMiddleMode()
        {
            if (this.matrixAlgorithm.CurrentTurn != PlayerType.X)
            {
                return;
            }

            PointIndex aiHardPoint = this.aiMove.GetAiHardPointIndex(this.matrixAlgorithm);

            foreach (Button button in this.gamePanel.ButtonList)
            {
                if (aiHardPoint.Equals(button.Tag))
                {
                    this.matrixAlgorithm.Board[aiHardPoint.X, aiHardPoint.Y] = this.matrixAlgorithm.CurrentTurn;
                    button.Text = this.matrixAlgorithm.CurrentTurn.ToString();
                    this.matrixAlgorithm.CheckWinner(aiHardPoint, this.matrixAlgorithm.CurrentTurn);

                    if (this.matrixAlgorithm.WinnerState)
                    {
                        this.RestartGame();
                    }
                    break;
                }
            }
        }

        private void AiHardMode()
        {
            PointIndex aiMediumPoint = this.aiMove.GetAiMediumPointIndex(this.matrixAlgorithm);
            foreach (Button button in this.gamePanel.ButtonList)
            {
                if (!button.Tag.Equals(aiMediumPoint))
                {
                    continue;
                }

                button.Text = this.matrixAlgorithm.CurrentTurn.ToString();
                this.matrixAlgorithm.Board[aiMediumPoint.X, aiMediumPoint.Y] = this.matrixAlgorithm.CurrentTurn;
                this.matrixAlgorithm.CheckWinner(aiMediumPoint, this.matrixAlgorithm.CurrentTurn);

                if (this.matrixAlgorithm.WinnerState)
                {
                    this.RestartGame();
                }

                break;
            }
        }

        public void AiEasyMode()
        {
            if (this.gamePanel.ButtonList.Count > 0)
            {
                Int32 index = this.aiMove.GetAiEasyPointIndex(this.gamePanel.ButtonList);
                PointIndex point = (PointIndex)this.gamePanel.ButtonList[index].Tag;
                this.matrixAlgorithm.Board[point.X, point.Y] = this.matrixAlgorithm.CurrentTurn;
                this.gamePanel.ButtonList[index].Text = this.matrixAlgorithm.CurrentTurn.ToString();
                this.gamePanel.ButtonList.RemoveAt(index);
                this.matrixAlgorithm.CheckWinner(point, this.matrixAlgorithm.CurrentTurn);
            }
            if (this.matrixAlgorithm.WinnerState)
            {
                this.RestartGame();
            }
        }

        #endregion AiDifficulty

        #region Events

        private void TicTacToeMatrix_EndGame(Object sender, WinnerMessageEventArgs e)
        {
            MessageBox.Show(e.Message);

            this.settings.HistoryList.Add(new History()
            {
                Winner = e.Winner,
                RoundCount = this.settings.HistoryList.Count + 1,
            });
            this.dataGrid.DataSource = this.settings.HistoryList.ToList();
        }

        #endregion Events

        #region Buttons

        private void BtnSave_Click(Object sender, EventArgs e)
        {
            if (this.gamePanel.ButtonList == null || this.matrixAlgorithm == null)
            {
                MessageBox.Show("Game could not be saved !");
                return;
            }

            DataBaseWriter dataBaseWriter = new DataBaseWriter();
            this.savedGameState = new SavedGameState
            {
                BoardSize = this.matrixAlgorithm.BoardSize,
                CurrentTurn = this.matrixAlgorithm.CurrentTurn,
                CurrentTurnCount = this.matrixAlgorithm.CurrentTurnCount,
                BoardData = this.mapper.CurrentBoardToString(this.matrixAlgorithm),
            };
            Data data = new Data
            {
                CurrentGame = savedGameState,
                Round = this.settings.RoundCount,
                HistoryList = this.settings.HistoryList,
                Difficulty = this.ChooseDifficulty()
            };
            HistoryData historyData = new HistoryData
            {
                HistoryList = this.settings.HistoryList,
            };

            if (this.settings.HistoryList.Count >= 1)
            {
                dataBaseWriter.WriteToDataBase(historyData);
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = Application.StartupPath + "/settings/";
                saveFileDialog.Filter = "JSON File(*.json)| *.json; | XML Document(*.xml) | *.xml; | Configuration settings(*.ini) | *.ini";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                if (saveFileDialog.FileName.Contains(".json"))
                {
                    this.serialize.JsonSerialize(saveFileDialog.FileName, data);
                    MessageBox.Show($"Your progress was successfully saved to {saveFileDialog.FileName}");
                }
                else if (saveFileDialog.FileName.Contains(".xml"))
                {
                    this.serialize.XmlSerialize(saveFileDialog.FileName, data);
                    MessageBox.Show($"Your progress was successfully saved to {saveFileDialog.FileName}");
                }
                else if (saveFileDialog.FileName.Contains(".ini"))
                {
                    this.iniParseData.WriteIni(saveFileDialog.FileName, data);
                }
                else
                {
                    MessageBox.Show("Unable to save your progress!");
                }
            }
        }

        private void BtnLoad_Click(Object sender, EventArgs e)
        {
            using (OpenFileDialog loadFileDialog = new OpenFileDialog())
            {
                loadFileDialog.Filter = "Configuration Files(*.json; *.xml; *.ini;)|*.json; *.xml; *.ini;";
                loadFileDialog.ShowDialog();

                this.iniParseData = (IIniParseData)SerDesFactory.Create(typeof(IIniParseData));
                this.deserializeData = (IDeSerializeData)SerDesFactory.Create(typeof(IDeSerializeData));

                if (loadFileDialog.FileName.Contains(".json"))
                {
                    Data data = this.deserializeData.JsonDeserialize(loadFileDialog.FileName);
                    this.RecoverData(data);
                    MessageBox.Show(Path.GetFileName(loadFileDialog.FileName) + " sucessfully loaded!");
                }
                else if (loadFileDialog.FileName.Contains(".xml"))
                {
                    Data data = this.deserializeData.XmlDeserialize(loadFileDialog.FileName);
                    this.settings.XmlIsUsed = true;
                    this.RecoverData(data);
                    this.settings.XmlIsUsed = false;
                    MessageBox.Show(Path.GetFileName(loadFileDialog.FileName) + " sucessfully loaded!");
                }
                else if (loadFileDialog.FileName.EndsWith(".ini"))
                {
                    Data data = this.iniParseData.ReadIni(loadFileDialog.FileName);
                    this.RecoverData(data);
                    MessageBox.Show(Path.GetFileName(loadFileDialog.FileName) + " sucessfully loaded!");
                }
                else
                {
                    MessageBox.Show("Unable to load your progress!");
                }
            }
        }

        private void BtnStart_Click(Object sender, EventArgs e)
        {
            this.Setup(this.boardSizeTrackBar.Value);
        }

        #endregion Buttons

        #region Scrollbar

        private void BoardSizeTrackBar_Scroll(Object sender, EventArgs e)
        {
            this.UpdateScrollBar();
        }

        private void UpdateScrollBar()
        {
            this.txtBoxTrackBar.Text = this.boardSizeTrackBar.Value.ToString();
            if (this.boardSizeTrackBar.Value > 3)
            {
                this.radioBtnHard.Enabled = false;
                this.radioBtnMedium.Enabled = false;

                if (this.radioBtnEasy.Checked)
                {
                    this.radioBtnEasy.Checked = true;
                }
                else if (this.radioBtn1vs1.Checked)
                {
                    this.radioBtn1vs1.Checked = true;
                }
                else
                {
                    this.radioBtnEasy.Checked = true;
                }
            }
            else
            {
                this.radioBtnHard.Enabled = true;
                this.radioBtnMedium.Enabled = true;
            }
        }

        #endregion Scrollbar

        #region LoadSaveGame Methods

        private void RecoverData(Data data)
        {
            this.boardSizeTrackBar.Value = data.CurrentGame.BoardSize;
            this.settings.HistoryList = data.HistoryList;
            this.dataGrid.DataSource = this.settings.HistoryList;
            this.RecoverDifficulty(data);
            this.UpdateScrollBar();
            this.RestartGame();
            this.savedGameState = data.CurrentGame;
            this.settings.RoundCount = data.Round;
            this.RecoverBoard(data.Difficulty);
        }

        public void RecoverBoard(Difficulty difficulty)
        {
            this.UpdateMatrixAlgorithm();
            this.matrixAlgorithm.EndGame += this.TicTacToeMatrix_EndGame;
            this.gamePanel.Difficulty = difficulty;

            this.mainTableLayloutPanel.Controls.Clear();

            this.mainTableLayloutPanel.RowCount = this.matrixAlgorithm.BoardSize;
            this.mainTableLayloutPanel.ColumnCount = this.matrixAlgorithm.BoardSize;

            this.gamePanel.ButtonList = new List<Button>();

            for (Int32 row = 0; row < this.matrixAlgorithm.BoardSize; row++)
            {
                for (Int32 col = 0; col < this.matrixAlgorithm.BoardSize; col++)
                {
                    Button current = new Button
                    {
                        Height = 62,
                        Width = 62,
                        Tag = new PointIndex(col, row)
                    };

                    this.gamePanel.ButtonList.Add(current);
                    if (this.matrixAlgorithm.Board[col, row] != PlayerType.Unassigned)
                    {
                        current.Text = this.matrixAlgorithm.Board[col, row].ToString();
                        this.gamePanel.ButtonList.Remove(current);
                    }

                    current.Click += this.PlayerButtonOnClick;

                    this.mainTableLayloutPanel.Controls.Add(current, col, row);
                }
            }

            if (this.gamePanel.Difficulty == Difficulty.Middle && this.gamePanel.ButtonList.Count == 9)
            {
                this.ClickAiMiddle();
            }
        }

        private void UpdateMatrixAlgorithm()
        {
            this.matrixAlgorithm.EndGame -= this.TicTacToeMatrix_EndGame;
            this.matrixAlgorithm.Board = this.mapper.CurrentStringToBoard(this.savedGameState.BoardData, this.matrixAlgorithm);
            this.matrixAlgorithm.BoardSize = this.savedGameState.BoardSize;
            this.matrixAlgorithm.CurrentTurn = this.savedGameState.CurrentTurn;
            this.matrixAlgorithm.CurrentTurnCount = this.savedGameState.CurrentTurnCount;
        }

        private void RecoverDifficulty(Data data)
        {
            this.radioBtnEasy.Checked = false;
            this.radioBtnMedium.Checked = false;
            this.radioBtnHard.Checked = false;
            this.radioBtn1vs1.Checked = false;
            this.boardSizeTrackBar.Value = data.CurrentGame.BoardSize;
            this.txtBoxTrackBar.Text = data.CurrentGame.BoardSize.ToString();
            if (data.Difficulty == Difficulty.Easy)
            {
                this.radioBtnEasy.Checked = true;
                return;
            }
            if (data.Difficulty == Difficulty.Middle)
            {
                this.radioBtnMedium.Checked = true;
                return;
            }
            if (data.Difficulty == Difficulty.Hard)
            {
                this.radioBtnHard.Checked = true;
                return;
            }
            this.radioBtn1vs1.Checked = true;
        }

        #endregion LoadSaveGame Methods

        #region MainWindow Events

        private void MainWindow_FormClosing(Object sender, FormClosingEventArgs e)
        {
            DataBaseWriter dataBaseWrite = new DataBaseWriter();
            this.mapper = new Mapper();
            this.savedGameState = new SavedGameState
            {
                BoardSize = this.matrixAlgorithm.BoardSize,
                CurrentTurn = this.matrixAlgorithm.CurrentTurn,
                CurrentTurnCount = this.matrixAlgorithm.CurrentTurnCount,
                BoardData = this.mapper.CurrentBoardToString(this.matrixAlgorithm),
            };
            Data data = new Data
            {
                CurrentGame = this.savedGameState,
                Round = this.settings.RoundCount,
                HistoryList = this.settings.HistoryList,
                Difficulty = this.ChooseDifficulty()
            };
            HistoryData historyData = new HistoryData
            {
                HistoryList = this.settings.HistoryList,
            };
            dataBaseWrite.WriteToDataBase(historyData);
            this.serialize = (ISerializeData)SerDesFactory.Create(typeof(ISerializeData));
            this.serialize.JsonSerialize(Application.StartupPath + "/settings/autosave.json", data);
            this.serialize.XmlSerialize(Application.StartupPath + "/settings/autosave.xml", data);
        }

        private void MainWindow_Load(Object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "/settings/"))
            {
                Directory.CreateDirectory(Application.StartupPath + "/settings/");
            }

            if (File.Exists(Application.StartupPath + "/settings/autosave.json"))
            {
                this.deserializeData = (IDeSerializeData)SerDesFactory.Create(typeof(IDeSerializeData));
                Data data = this.deserializeData.JsonDeserialize(Application.StartupPath + "/settings/autosave.json");
                this.RecoverData(data);
            }
        }

        #endregion MainWindow Events
    }
}