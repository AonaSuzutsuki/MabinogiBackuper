using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonCoreLib.Bool;
using MabinogiBackuper.Models.Backup;
using Prism.Commands;
using Prism.Mvvm;

namespace MabinogiBackuper.ViewModels.Backup
{
    public class BackupSelectionPageViewModel : NavigationPageViewModelBase
    {
        public BackupSelectionPageViewModel(NavigationWindowService<BackupShare> service) : base(service?.NavigationValue)
        {
            _service = service;

            AllSelectCheckedCommand = new DelegateCommand(AllSelect_Checked);
            AllCheckedCommand = new DelegateCommand(AllChecked);
        }

        #region Fieds

        private readonly NavigationWindowService<BackupShare> _service;
        private readonly BoolCollector _collector = new BoolCollector();

        private bool _allSelectChecked;
        private bool _drawChatChecked;
        private bool _screenshotChecked;
        private bool _petAiChecked;
        private bool _keyAlermChecked;

        #endregion

        #region Properties

        public bool AllSelectChecked
        {
            get => _allSelectChecked;
            set => SetProperty(ref _allSelectChecked, value);
        }

        public bool DrawChatChecked
        {
            get => _drawChatChecked;
            set
            {
                SetProperty(ref _drawChatChecked, value);
                _service.Share.ContainsDrawChat = value;
            }
        }

        public bool ScreenshotChecked
        {
            get => _screenshotChecked;
            set
            {
                SetProperty(ref _screenshotChecked, value);
                _service.Share.ContainsScreenshot = value;
            }
        }

        public bool PetAiChecked
        {
            get => _petAiChecked;
            set
            {
                SetProperty(ref _petAiChecked, value);
                _service.Share.ContainsPetAi = value;
            }
        }

        public bool KeyAlermChecked
        {
            get => _keyAlermChecked;
            set
            {
                SetProperty(ref _keyAlermChecked, value);
                _service.Share.ContainsKeyAlerm = value;
            }
        }

        #endregion

        #region Event Methods

        public ICommand AllSelectCheckedCommand { get; set; }
        public ICommand AllCheckedCommand { get; set; }

        #endregion

        #region Methods

        public void AllSelect_Checked()
        {
            DrawChatChecked = AllSelectChecked;
            ScreenshotChecked = AllSelectChecked;
            PetAiChecked = AllSelectChecked;
            KeyAlermChecked = AllSelectChecked;
        }

        public void AllChecked()
        {
            _collector.ChangeBool(nameof(DrawChatChecked), DrawChatChecked);
            _collector.ChangeBool(nameof(ScreenshotChecked), ScreenshotChecked);
            _collector.ChangeBool(nameof(PetAiChecked), PetAiChecked);
            _collector.ChangeBool(nameof(KeyAlermChecked), KeyAlermChecked);
            AllSelectChecked = _collector.Value;
        }

        #endregion

        public override void RefreshValues()
        {
            BindableValue.NextBtContent = "開始する";
        }
    }
}
