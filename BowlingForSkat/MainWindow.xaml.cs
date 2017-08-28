using System;
using System.Collections.Generic;
using System.Windows;

namespace BowlingForSkat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BowlingCalculator : Window
    {
        public BowlingCalculator()
        {
            InitializeComponent();
        }

        private async void OnClick(object sender, RoutedEventArgs e)
        {
            BowlingServerGETData data = await SkatCommunicator.GetBowlingResults();
            calculateScores(data);
        }

        private void calculateScores(BowlingServerGETData data)
        {
            NewBowlingMatch match = new NewBowlingMatch(data);
            verifyAlgorithm(match, data);
        }

        private async void verifyAlgorithm(NewBowlingMatch match, BowlingServerGETData data)
        {
            string verification = await SkatCommunicator.VerifyBowlingResults(match);

            string frameResults = "Frame results: ";

            foreach (NewBowlingFrame frame in match.Frames)
            {
                frameResults += string.Format("[{0},{1}]", frame.GetPinsDownAtThrow(0), frame.GetPinsDownAtThrow(1));
            }

            MessageBox.Show(frameResults + Environment.NewLine + Environment.NewLine + verification);
        }
    }
}
