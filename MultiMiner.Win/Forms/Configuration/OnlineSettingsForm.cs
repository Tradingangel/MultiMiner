﻿using MultiMiner.Utility.Forms;
using MultiMiner.Utility.Serialization;
using MultiMiner.Win.Data.Configuration;
using System;

namespace MultiMiner.Win.Forms.Configuration
{
    public partial class OnlineSettingsForm : MessageBoxFontForm
    {
        private readonly Application applicationConfiguration;
        private readonly Application workingApplicationConfiguration;

        public OnlineSettingsForm(Application applicationConfiguration)
        {
            InitializeComponent();

            this.applicationConfiguration = applicationConfiguration;
            workingApplicationConfiguration = ObjectCopier.CloneObject<Application, Application>(applicationConfiguration);
        }

        private void OnlineSettingsForm_Load(object sender, EventArgs e)
        {
            applicationConfigurationBindingSource.DataSource = workingApplicationConfiguration;

            remoteCommandsCheck.Enabled = workingApplicationConfiguration.MobileMinerMonitoring;
            pushNotificationsCheck.Enabled = workingApplicationConfiguration.MobileMinerMonitoring;
            httpsMobileMinerCheck.Enabled = workingApplicationConfiguration.MobileMinerMonitoring;

            LoadSettings();
        }

        private void LoadSettings()
        {
            intervalCombo.SelectedIndex = (int)applicationConfiguration.StrategyCheckInterval;

            if (applicationConfiguration.SuggestCoinsToMine)
            {
                if (applicationConfiguration.SuggestionsAlgorithm == Application.CoinSuggestionsAlgorithm.SHA256)
                    suggestionsCombo.SelectedIndex = 1;
                else if (applicationConfiguration.SuggestionsAlgorithm == Application.CoinSuggestionsAlgorithm.Scrypt)
                    suggestionsCombo.SelectedIndex = 2;
                else if (applicationConfiguration.SuggestionsAlgorithm == (Application.CoinSuggestionsAlgorithm.SHA256 | Application.CoinSuggestionsAlgorithm.Scrypt))
                    suggestionsCombo.SelectedIndex = 3;
                else
                    suggestionsCombo.SelectedIndex = 0;
            }
            else
            {
                suggestionsCombo.SelectedIndex = 0;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ObjectCopier.CopyObject(workingApplicationConfiguration, applicationConfiguration);
            
            applicationConfiguration.StrategyCheckInterval = (Application.TimerInterval)intervalCombo.SelectedIndex;

            switch (suggestionsCombo.SelectedIndex)
            {
                case 1:
                    applicationConfiguration.SuggestCoinsToMine = true;
                    applicationConfiguration.SuggestionsAlgorithm = Application.CoinSuggestionsAlgorithm.SHA256;
                    break;
                case 2:
                    applicationConfiguration.SuggestCoinsToMine = true;
                    applicationConfiguration.SuggestionsAlgorithm = Application.CoinSuggestionsAlgorithm.Scrypt;
                    break;
                case 3:
                    applicationConfiguration.SuggestCoinsToMine = true;
                    applicationConfiguration.SuggestionsAlgorithm = Application.CoinSuggestionsAlgorithm.SHA256 | Application.CoinSuggestionsAlgorithm.Scrypt;
                    break;
                default:
                    applicationConfiguration.SuggestCoinsToMine = false;
                    break;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
