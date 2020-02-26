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

        private bool _allSelectChecked;
        private bool _registryChecked;
        private bool _drawChatChecked;
        private bool _screenshotChecked;
        private bool _petAiChecked;
        private bool _keyAlermChecked;
        private bool _interactionChecked;
        private bool _movieChecked;

        #endregion

        #region Properties

        public bool RegistryChecked
        {
            get => _registryChecked;
            set
            {
                SetProperty(ref _registryChecked, value);
                _service.Share.ContainsRegistry = value;
            }
        }

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

        public bool InteractionChecked
        {
            get => _interactionChecked;
            set
            {
                SetProperty(ref _interactionChecked, value);
                _service.Share.ContainsInteraction = value;
            }
        }

        public bool MovieChecked
        {
            get => _movieChecked;
            set
            {
                SetProperty(ref _movieChecked, value);
                _service.Share.ContainsMovie = value;
            }
        }

        #endregion

        #region Event Methods

        public ICommand AllSelectCheckedCommand { get; set; }
        public ICommand AllCheckedCommand { get; set; }

        #endregion

        #region Methods

        public override void RefreshValues()
        {
            base.RefreshValues();

            var singleCollector = BoolCollectorValueChange(typeof(BoolSingleCollector), this);
            BindableValue.CanGoNext = singleCollector.Value;
        }

        public void AllSelect_Checked()
        {
            RegistryChecked = AllSelectChecked;
            DrawChatChecked = AllSelectChecked;
            ScreenshotChecked = AllSelectChecked;
            PetAiChecked = AllSelectChecked;
            KeyAlermChecked = AllSelectChecked;
            InteractionChecked = AllSelectChecked;
            MovieChecked = AllSelectChecked;

            BindableValue.CanGoNext = AllSelectChecked;
        }

        public void AllChecked()
        {
            var collector = BoolCollectorValueChange(typeof(BoolCollector), this);
            AllSelectChecked = collector.Value;

            var singleCollector = BoolCollectorValueChange(typeof(BoolSingleCollector), this);
            // どれか一つチェックあれば次のボタン有効化
            BindableValue.CanGoNext = singleCollector.Value;
        }

        private static BoolCollector BoolCollectorValueChange(Type type, BackupSelectionPageViewModel viewModel)
        {
            var obj = Activator.CreateInstance(type);
            if (!(obj is BoolCollector collector)) return null;

            collector.ChangeBool(nameof(RegistryChecked), viewModel.RegistryChecked);
            collector.ChangeBool(nameof(DrawChatChecked), viewModel.DrawChatChecked);
            collector.ChangeBool(nameof(ScreenshotChecked), viewModel.ScreenshotChecked);
            collector.ChangeBool(nameof(PetAiChecked), viewModel.PetAiChecked);
            collector.ChangeBool(nameof(KeyAlermChecked), viewModel.KeyAlermChecked);
            collector.ChangeBool(nameof(InteractionChecked), viewModel.InteractionChecked);
            collector.ChangeBool(nameof(MovieChecked), viewModel.MovieChecked);

            return collector;

        }

        #endregion
    }
}
