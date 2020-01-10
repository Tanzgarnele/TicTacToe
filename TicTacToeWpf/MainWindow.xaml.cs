using DataBaseManager.Entities;
using DataBaseManager.Internal;
using Serializer.Enteties;
using Serializer.Factory;
using Serializer.Interface;
using Serializer.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;
using TicTacToeMatch.Events;
using TicTacToeMatch.Factory;
using TicTacToeMatch.Interfaces;
using TicTacToeWPF.Interfaces;
using TicTacToeWPF.Internal;
using Button = System.Windows.Controls.Button;

namespace TicTacToeWPF
{
    public partial class MainWindow : Window
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
        private Int32 countX;
        private Int32 countO;
        private Int32 countDraw;
        private Grid grid;

        public MainWindow()
        {
            this.InitializeComponent();
            this.settings = new Settings
            {
                HistoryList = new List<History>()
            };
            this.gamePanel = new GamePanel();
            this.txtBoxTrackBar.Text = this.boardSizeTrackBar.Value.ToString();
            this.aiMove = (IAiMove)GlobalFactory.Create(typeof(IAiMove));
            this.grid = new Grid() { Name = "Grid" };
        }

        private void RestartGame()
        {
            if (this.matrixAlgorithm != null)
            {
                this.matrixAlgorithm.EndGame -= this.TicTacToeMatrix_EndGame;
                this.matrixAlgorithm = null;
            }

            this.aiMove = (IAiMove)GlobalFactory.Create(typeof(IAiMove));
            this.Setup(Convert.ToInt32(this.boardSizeTrackBar.Value));
        }

        public void Setup(Int32 dimension)
        {
            this.grid = new Grid() { Name = "Grid" };
            this.matrixAlgorithm = (IMatrixAlgorithm)GlobalFactory.Create(typeof(IMatrixAlgorithm));
            this.matrixAlgorithm.EndGame += this.TicTacToeMatrix_EndGame;
            this.gamePanel.Difficulty = this.ChooseDifficulty();

            if (dimension < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(dimension));
            }

            this.StackPanel.Children.Clear();

            this.gamePanel.ButtonList = new List<Button>();

            for (Int32 row = 0; row < dimension; row++)
            {
                this.grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(65) });
                this.grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(65) });
                for (Int32 col = 0; col < dimension; col++)
                {
                    Button current = new Button
                    {
                        Height = 62,
                        Width = 62,
                        Tag = new PointIndex(col, row)
                    };
                    Grid.SetRow(current, row);
                    Grid.SetColumn(current, col);
                    this.grid.Children.Add(current);
                    this.gamePanel.ButtonList.Add(current);

                    current.Click += this.PlayerButtonOnClick;
                }
            }
            this.StackPanel.Children.Add(this.grid);
            this.matrixAlgorithm.Initialize(dimension);

            if (this.gamePanel.Difficulty == Difficulty.Middle)
            {
                this.ClickAiMiddle();
            }

            this.ShowStats();
        }

        public void ShowStats()
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

        public Difficulty ChooseDifficulty()
        {
            if (this.radioBtnEasy.IsChecked == true)
            {
                return Difficulty.Easy;
            }

            if (this.radioBtnMedium.IsChecked == true)
            {
                return Difficulty.Middle;
            }

            if (this.radioBtnHard.IsChecked == true)
            {
                return Difficulty.Hard;
            }

            return Difficulty.FaceToFace;
        }

        private void PlayerButtonOnClick(Object sender, EventArgs e)
        {
            if (!(sender is Button btn))
            {
                return;
            }

            PointIndex point = (PointIndex)btn.Tag;

            this.matrixAlgorithm.Board[point.X, point.Y] = this.matrixAlgorithm.CurrentTurn;
            btn.Content = this.matrixAlgorithm.CurrentTurn.ToString();
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
                    btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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

            PointIndex aiHardPoint = this.aiMove.AiHardMode(this.matrixAlgorithm);

            foreach (Button button in this.gamePanel.ButtonList)
            {
                if (aiHardPoint.Equals(button.Tag))
                {
                    this.matrixAlgorithm.Board[aiHardPoint.X, aiHardPoint.Y] = this.matrixAlgorithm.CurrentTurn;
                    button.Content = this.matrixAlgorithm.CurrentTurn.ToString();
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
            PointIndex aiMediumPoint = this.aiMove.AiMediumMode(this.matrixAlgorithm);
            foreach (Button button in this.gamePanel.ButtonList)
            {
                if (!button.Tag.Equals(aiMediumPoint))
                {
                    continue;
                }

                button.Content = this.matrixAlgorithm.CurrentTurn.ToString();
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
                Random random = new Random();
                Int32 index = random.Next(this.gamePanel.ButtonList.Count);
                PointIndex point = (PointIndex)this.gamePanel.ButtonList[index].Tag;
                this.matrixAlgorithm.Board[point.X, point.Y] = this.matrixAlgorithm.CurrentTurn;
                this.gamePanel.ButtonList[index].Content = this.matrixAlgorithm.CurrentTurn.ToString();
                this.gamePanel.ButtonList.RemoveAt(index);
                this.matrixAlgorithm.CheckWinner(point, this.matrixAlgorithm.CurrentTurn);
            }
            if (this.matrixAlgorithm.WinnerState)
            {
                this.RestartGame();
            }
        }

        #endregion AiDifficulty

        private void TicTacToeMatrix_EndGame(Object sender, WinnerMessageEventArgs e)
        {
            MessageBox.Show(e.Message);

            this.settings.HistoryList.Add(new History()
            {
                Winner = e.Winner,
                RoundCount = this.settings.HistoryList.Count + 1,
            });
            this.dataGrid.ItemsSource = this.settings.HistoryList.ToList();
        }

        private void StartButton_Click(Object sender, RoutedEventArgs e)
        {
            this.Setup(Convert.ToInt32(this.boardSizeTrackBar.Value));
        }

        private void BtnSave_Click(Object sender, RoutedEventArgs e)
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

            Microsoft.Win32.OpenFileDialog saveFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = this.StartupPath + "/settings/",
                Filter = "JSON File(*.json)| *.json; | XML Document(*.xml) | *.xml; | Configuration settings(*.ini) | *.ini"
            };
            Boolean? result = saveFileDialog.ShowDialog();
            if (result == true)
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

        private void BtnLoad_Click(Object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog loadFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Configuration Files(*.json; *.xml; *.ini;)|*.json; *.xml; *.ini;"
            };
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

        private void BoardSizeTrackBar_ValueChanged(Object sender, RoutedPropertyChangedEventArgs<Double> e)
        {
            this.UpdateScrollBar();
        }

        private void UpdateScrollBar()
        {
            if (this.boardSizeTrackBar.Value > 3)
            {
                this.radioBtnHard.IsEnabled = false;
                this.radioBtnMedium.IsEnabled = false;

                if (this.radioBtnEasy.IsChecked == true)
                {
                    this.radioBtnEasy.IsChecked = true;
                }
                else if (this.radioBtn1vs1.IsChecked == true)
                {
                    this.radioBtn1vs1.IsChecked = true;
                }
                else
                {
                    this.radioBtnEasy.IsChecked = true;
                }
            }
            else
            {
                this.radioBtnHard.IsEnabled = true;
                this.radioBtnMedium.IsEnabled = true;
            }
        }

        private void RecoverData(Data data)
        {
            this.boardSizeTrackBar.Value = data.CurrentGame.BoardSize;
            this.settings.HistoryList = data.HistoryList;
            this.dataGrid.ItemsSource = this.settings.HistoryList;
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


            this.StackPanel.Children.Clear();
            this.StackPanel.Children.Remove(this.grid);
            this.StackPanel = new StackPanel();
            this.grid = new Grid() { Name = "Grid" };

            this.gamePanel.ButtonList = new List<Button>();

            for (Int32 row = 0; row < this.matrixAlgorithm.BoardSize; row++)
            {
                this.grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(65) });
                this.grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(65) });
                for (Int32 col = 0; col < this.matrixAlgorithm.BoardSize; col++)
                {
                    Button current = new Button
                    {
                        Height = 62,
                        Width = 62,
                        Tag = new PointIndex(col, row)
                    };
                    Grid.SetRow(current, row);
                    Grid.SetColumn(current, col);
                    this.grid.Children.Add(current);

                    this.StackPanel.Children.Add(this.grid);
                    this.gamePanel.ButtonList.Add(current);
                    if (this.matrixAlgorithm.Board[col, row] != PlayerType.Unassigned)
                    {
                        current.Content = this.matrixAlgorithm.Board[col, row].ToString();
                        this.gamePanel.ButtonList.Remove(current);
                    }

                    current.Click += this.PlayerButtonOnClick;
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
            this.radioBtnEasy.IsChecked = false;
            this.radioBtnMedium.IsChecked = false;
            this.radioBtnHard.IsChecked = false;
            this.radioBtn1vs1.IsChecked = false;
            this.boardSizeTrackBar.Value = data.CurrentGame.BoardSize;
            this.txtBoxTrackBar.Text = data.CurrentGame.BoardSize.ToString();
            if (data.Difficulty == Difficulty.Easy)
            {
                this.radioBtnEasy.IsChecked = true;
                return;
            }
            if (data.Difficulty == Difficulty.Middle)
            {
                this.radioBtnMedium.IsChecked = true;
                return;
            }
            if (data.Difficulty == Difficulty.Hard)
            {
                this.radioBtnHard.IsChecked = true;
                return;
            }
            this.radioBtn1vs1.IsChecked = true;
        }

        private void Window_Closing(Object sender, System.ComponentModel.CancelEventArgs e)
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
            this.serialize.JsonSerialize(this.StartupPath + "/settings/autosave.json", data);
            this.serialize.XmlSerialize(this.StartupPath + "/settings/autosave.xml", data);
        }

        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(this.StartupPath + "/settings/"))
            {
                Directory.CreateDirectory(this.StartupPath + "/settings/");
            }

            if (File.Exists(this.StartupPath + "/settings/autosave.json"))
            {
                this.deserializeData = (IDeSerializeData)SerDesFactory.Create(typeof(IDeSerializeData));
                Data data = this.deserializeData.JsonDeserialize(this.StartupPath + "/settings/autosave.json");
                this.RecoverData(data);
            }
        }

        public String StartupPath
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}