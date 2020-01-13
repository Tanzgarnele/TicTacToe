using DataBaseManager.Entities;
using DataBaseManager.Interface;
using Serializer.Enteties;
using Serializer.Factories;
using Serializer.Interfaces;
using Serializer.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;
using TicTacToeMatch.Events;
using TicTacToeMatch.Factories;
using TicTacToeMatch.Interfaces;
using TicTacToeUi.Interfaces;
using TicTacToeUi.Internal;
using TicTacToeUi.Internals;

namespace TicTacToeUi
{
    public partial class MainWindow : Form
    {
        private IMatrixAlgorithm matrixAlgorithm;
        private readonly ISettings settings;
        private readonly IGamePanel gamePanel;
        private readonly IDataBaseWriter dataBaseWriter;
        private IAiMove aiMove;
        private ISerializeData serialize;
        private IDeSerializeData deserializeData;
        private IIniParseData iniParseData;
        private SavedGameState savedGameState;
        private BoardDataMapper mapper = new BoardDataMapper();
        private Int32 countX;
        private Int32 countO;
        private Int32 countDraw;

        public MainWindow(IMatrixAlgorithm matrixAlgorithm, IAiMove aiMove, ISerializeData serialize,
            IDeSerializeData deserializeData, IIniParseData iniParseData, IDataBaseWriter dataBaseWriter)
        {
            this.matrixAlgorithm = matrixAlgorithm ?? throw new ArgumentNullException(nameof(matrixAlgorithm));
            this.aiMove = aiMove ?? throw new ArgumentNullException(nameof(aiMove));
            this.serialize = serialize ?? throw new ArgumentNullException(nameof(serialize));
            this.deserializeData = deserializeData ?? throw new ArgumentNullException(nameof(deserializeData));
            this.iniParseData = iniParseData ?? throw new ArgumentNullException(nameof(iniParseData));
            this.dataBaseWriter = dataBaseWriter ?? throw new ArgumentNullException(nameof(dataBaseWriter));
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
                this.matrixAlgorithm.GameEnd -= this.TicTacToeGameEnd;
                this.matrixAlgorithm = null;
            }

            this.aiMove = (IAiMove)GlobalFactory.Create(typeof(IAiMove));
            this.SetupBoard(this.boardSizeTrackBar.Value);
        }

        private void SetupBoard(Int32 dimension)
        {
            if (dimension < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(dimension));
            }
            
            this.matrixAlgorithm = (IMatrixAlgorithm)GlobalFactory.Create(typeof(IMatrixAlgorithm));
            this.matrixAlgorithm.GameEnd += this.TicTacToeGameEnd;
            this.gamePanel.Difficulty = this.ChooseDifficulty();


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

            this.matrixAlgorithm.InitializeBoard(dimension);

            if (this.gamePanel.Difficulty == Difficulty.Middle)
            {
                this.ClickAiMiddle();
            }

            this.ShowStats();
        }

        private void ShowStats()
        {
            this.countDraw = 0;
            this.countO = 0;
            this.countX = 0;
            foreach (History item in this.settings.HistoryList)
            {
                if (item.Winner == PlayerType.X.ToString())
                {
                    this.countX++;
                }
                else if (item.Winner == PlayerType.O.ToString())
                {
                    this.countO++;
                }
                else
                {
                    this.countDraw++;
                }
            }
            this.txtBoxX.Text = this.countX.ToString();
            this.txtBoxO.Text = this.countO.ToString();
            this.txtBoxDraw.Text = this.countDraw.ToString();
        }

        private Difficulty ChooseDifficulty()
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
                    this.PlayAiEasyMode();
                    break;

                case Difficulty.Hard:
                    this.PlayAiHardMode();
                    break;

                case Difficulty.Middle:
                    this.PlayAiMiddleMode();
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

        private void PlayAiMiddleMode()
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

        private void PlayAiHardMode()
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

        private void PlayAiEasyMode()
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

        private void TicTacToeGameEnd(Object sender, WinnerMessageEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

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

            //DataBaseWriter dataBaseWriter = new DataBaseWriter();
            this.savedGameState = new SavedGameState
            {
                BoardSize = this.matrixAlgorithm.BoardSize,
                CurrentTurn = this.matrixAlgorithm.CurrentTurn,
                CurrentTurnCount = this.matrixAlgorithm.CurrentTurnCount,
                BoardData = this.mapper.WriteCurrentBoardToString(this.matrixAlgorithm),
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
                this.dataBaseWriter.WriteDatabaseFile(historyData);
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
                    this.serialize.SerializeJson(saveFileDialog.FileName, data);
                    MessageBox.Show($"Your progress was successfully saved to {saveFileDialog.FileName}");
                }
                else if (saveFileDialog.FileName.Contains(".xml"))
                {
                    this.serialize.SerializeXml(saveFileDialog.FileName, data);
                    MessageBox.Show($"Your progress was successfully saved to {saveFileDialog.FileName}");
                }
                else if (saveFileDialog.FileName.Contains(".ini"))
                {
                    this.iniParseData.WriteToIni(saveFileDialog.FileName, data);
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

                //try
                //{
                //    Data data = this.deserializeData.DeserializeJson(loadFileDialog.FileName);

                //    if (data is null)
                //    {
                //        return;
                //    }


                //    //data.CurrentGame => nix

                //}
                //catch (ArgumentOutOfRangeException arg)
                //{
                //    MessageBox.Show("Ein Fehler " + arg.Message);
                //    return;
                //}




                if (loadFileDialog.FileName.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
                {
                    Data data = this.deserializeData.DeserializeJson(loadFileDialog.FileName);
                    this.RecoverData(data);
                    MessageBox.Show(Path.GetFileName(loadFileDialog.FileName) + " sucessfully loaded!");
                }
                else if (loadFileDialog.FileName.Contains(".xml"))
                {
                    Data data = this.deserializeData.DeserializeXml(loadFileDialog.FileName);
                    this.settings.XmlIsUsed = true;
                    this.RecoverData(data);
                    this.settings.XmlIsUsed = false;
                    MessageBox.Show(Path.GetFileName(loadFileDialog.FileName) + " sucessfully loaded!");
                }
                else if (loadFileDialog.FileName.EndsWith(".ini", StringComparison.InvariantCultureIgnoreCase))
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
            this.SetupBoard(this.boardSizeTrackBar.Value);
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
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

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

        private void RecoverBoard(Difficulty difficulty)
        {
            this.UpdateMatrixAlgorithm();
            this.matrixAlgorithm.GameEnd += this.TicTacToeGameEnd;
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
            this.matrixAlgorithm.GameEnd -= this.TicTacToeGameEnd;
            this.matrixAlgorithm.Board = this.mapper.WriteCurrentStringToBoard(this.savedGameState.BoardData, this.matrixAlgorithm);
            this.matrixAlgorithm.BoardSize = this.savedGameState.BoardSize;
            this.matrixAlgorithm.CurrentTurn = this.savedGameState.CurrentTurn;
            this.matrixAlgorithm.CurrentTurnCount = this.savedGameState.CurrentTurnCount;
        }

        private void RecoverDifficulty(Data data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

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
            //DataBaseWriter dataBaseWrite = new DataBaseWriter();
            this.mapper = new BoardDataMapper();
            this.savedGameState = new SavedGameState
            {
                BoardSize = this.matrixAlgorithm.BoardSize,
                CurrentTurn = this.matrixAlgorithm.CurrentTurn,
                CurrentTurnCount = this.matrixAlgorithm.CurrentTurnCount,
                BoardData = this.mapper.WriteCurrentBoardToString(this.matrixAlgorithm),
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
            dataBaseWriter.WriteDatabaseFile(historyData);
            this.serialize = (ISerializeData)SerDesFactory.Create(typeof(ISerializeData));
            this.serialize.SerializeJson(Application.StartupPath + "/settings/autosave.json", data);
            this.serialize.SerializeXml(Application.StartupPath + "/settings/autosave.xml", data);
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
                Data data = this.deserializeData.DeserializeJson(Application.StartupPath + "/settings/autosave.json");
                this.RecoverData(data);
            }
        }

        #endregion MainWindow Events
    }
}