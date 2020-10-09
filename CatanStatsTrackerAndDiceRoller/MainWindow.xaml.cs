using System.Windows;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Windows.Controls;



namespace CatanStatsTrackerAndDiceRoller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class player {
        public int playerNumberName { get; set; }

        //0 is equal to the "2" chance tile if this value is say 3 then the player has 3 points on the tile
        //
        public int[] ore = new int[11];
        public int[] wheat = new int[11];
        public int[] sheep = new int[11];
        public int[] wood = new int[11];
        public int[] brick = new int[11];

        //gross production count
        public int totalOre { get; set; }
        public int totalWheat { get; set; }
        public int totalSheep { get; set; }
        public int totalWood { get; set; }
        public int totalBrick { get; set; }

        public player()
        {
        
        }
       
    }

    public partial class MainWindow : Window
    {
        List<int> rollsDieOne = new List<int>();
        List<int> rollsDieTwo = new List<int>();
        int[] rollCount = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int totalRolls = 0;
        player[] players = new player[4];
        

        public MainWindow()
        {
            for (int i = 0; i < 7; i++) {
                rollsDieOne.Add(0);
                rollsDieTwo.Add(0);
            }
            players[0] = new player();
            players[0].playerNumberName = 1;
            players[1] = new player();
            players[1].playerNumberName = 2;
            players[2] = new player();
            players[2].playerNumberName = 3;
            players[3] = new player();
            players[3].playerNumberName = 4;
            
            InitializeComponent();
        }

        public void playerProdUpdate(int roll){
            for (int i = 0; i < 4; i++) {
                if (players[i].ore[roll - 2] > 0) {
                    TextBlock tempBlock = (TextBlock)this.FindName("OreProd" + (i + 1));
                    int tempProdNum = Int32.Parse(tempBlock.Text);
                    tempBlock.Text = (tempProdNum + players[i].ore[roll - 2]).ToString();
                }
                if (players[i].wheat[roll - 2] > 0)
                {
                    TextBlock tempBlock = (TextBlock)this.FindName("WheatProd" + (i + 1));
                    int tempProdNum = Int32.Parse(tempBlock.Text);
                    tempBlock.Text = (tempProdNum + players[i].wheat[roll - 2]).ToString();
                }
                if (players[i].sheep[roll - 2] > 0)
                {
                    TextBlock tempBlock = (TextBlock)this.FindName("SheepProd" + (i + 1));
                    int tempProdNum = Int32.Parse(tempBlock.Text);
                    tempBlock.Text = (tempProdNum + players[i].sheep[roll - 2]).ToString();
                }
                if (players[i].wood[roll - 2] > 0)
                {
                    TextBlock tempBlock = (TextBlock)this.FindName("WoodProd" + (i + 1));
                    int tempProdNum = Int32.Parse(tempBlock.Text);
                    tempBlock.Text = (tempProdNum + players[i].wood[roll - 2]).ToString();
                }
                if (players[i].brick[roll - 2] > 0)
                {
                    TextBlock tempBlock = (TextBlock)this.FindName("BrickProd" + (i + 1));
                    int tempProdNum = Int32.Parse(tempBlock.Text);
                    tempBlock.Text = (tempProdNum + players[i].brick[roll - 2]).ToString();
                }

            }
        }


        public void addPlayerChance(int playerName) {
           
            for (int i = 1; i < 6; i++) {
                TextBox tempTextBox = new TextBox();
                if (i == 1)
                {
                    tempTextBox = (TextBox)this.FindName("Ore" + playerName);
                }
                else if (i == 2)
                {
                    tempTextBox = (TextBox)this.FindName("Wheat" + playerName);
                }
                else if (i == 3)
                {
                    tempTextBox = (TextBox)this.FindName("Sheep" + playerName);
                }
                else if (i == 4)
                {
                    tempTextBox = (TextBox)this.FindName("Wood" + playerName);
                }
                else {
                    tempTextBox = (TextBox)this.FindName("Brick" + playerName);
                }

                string[] temp = tempTextBox.Text.Split(',');
                int inputChance = 0;

                foreach (var chanceNum in temp) {
                    if (Int32.TryParse(chanceNum, out inputChance)) {
                        if (inputChance < 13 && inputChance > 1) {
                            if (inputChance != 7)
                            {
                                switch (i)
                                {
                                    case 1:
                                        players[playerName - 1].ore[inputChance - 2]++;
                                        break;
                                    case 2:
                                        players[playerName - 1].wheat[inputChance - 2]++;
                                        break;
                                    case 3:
                                        players[playerName - 1].sheep[inputChance - 2]++;
                                        break;
                                    case 4:
                                        players[playerName - 1].wood[inputChance - 2]++;
                                        break;
                                    case 5:
                                        players[playerName - 1].brick[inputChance - 2]++;
                                        break;
                                }
                            }
                        }
                    }
                }
               

            }
        }

        public void rollMe() {
            totalRolls++;
            totalRollsText.Text = "Total Rolls: " + totalRolls.ToString();
            int dieOne = randomNumber();
            Thread.Sleep(randomNumber());
            int dieTwo = randomNumber();

            int dieTotal = dieOne + dieTwo;

            pastRolls(dieOne, dieTwo);

            rollDisplay.Text = dieTotal.ToString();

            rollCount[dieTotal - 2] = rollCount[dieTotal - 2] + 1;
             
            grapher(dieTotal);

            playerProdUpdate(dieTotal);
            

        }

        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            rollMe();
        }

        public void grapher(int rolled)
        {
            for (int i = 0; i < 11; i++) {
               double k = Math.Round((Convert.ToDouble(rollCount[i]) / Convert.ToDouble(totalRolls)) * 300, 0);
               singleBarChanger(i + 2, Convert.ToInt32(k));
            }
        }

        public void pastRolls(int dieOne, int dieTwo) {
            rollsDieOne.Insert(0, dieOne);
            rollsDieTwo.Insert(0, dieTwo);
            if (rollsDieOne[6] != 0)
            {
                rollsDieOne.RemoveAt(6);
                rollsDieTwo.RemoveAt(6);
            }

            dieOneText.Text = rollsDieOne[0] + " - " + rollsDieOne[1] + " - " + rollsDieOne[2] + " - " + rollsDieOne[3] + " - " + rollsDieOne[4] + " - " + rollsDieOne[5];
            dieTwoText.Text = rollsDieTwo[0] + " - " + rollsDieTwo[1] + " - " + rollsDieTwo[2] + " - " + rollsDieTwo[3] + " - " + rollsDieTwo[4] + " - " + rollsDieTwo[5];
        }

        public int randomNumber()
        {
            Thread.Sleep(4);
            Random randomNumber = new Random((int)DateTime.Now.Ticks);
            return randomNumber.Next(1, 7);
        }

        public void singleBarChanger(int barNumber, int value) {

            switch (barNumber) {
                case 2:
                    twoBar.Height = value;
                    break;
                case 3:
                    threeBar.Height = value;
                    break;
                case 4:
                    fourBar.Height = value;
                    break;
                case 5:
                    fiveBar.Height = value;
                    break;
                case 6:
                    sixBar.Height = value;
                    break;
                case 7:
                    sevenBar.Height = value;
                    break;
                case 8:
                    eightBar.Height = value;
                    break;
                case 9:
                    nineBar.Height = value;
                    break;
                case 10:
                    tenBar.Height = value;
                    break;
                case 11:
                    elevenBar.Height = value;
                    break;
                case 12:
                    twelveBar.Height = value;
                    break;
                default:
                    //nothing
                    break;
            }
        
        }

        private void enterPlayer(object sender, RoutedEventArgs e)
        {
            var buttonClicked = sender as Button;
            if (buttonClicked != null) {
                switch (buttonClicked.Name) {
                    case "Player1":
                        addPlayerChance(1);
                        break;
                    case "Player2":
                        addPlayerChance(2);
                        break;
                    case "Player3":
                        addPlayerChance(3);
                        break;
                    case "Player4":
                        addPlayerChance(4);
                        break;
                    default:
                        //
                        break;
                }
            }
        }

        
    }
}
