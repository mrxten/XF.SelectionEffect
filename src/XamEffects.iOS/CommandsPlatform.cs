using System;
using System.ComponentModel;
using System.Windows.Input;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamEffects;
using XamEffects.iOS;
using XamEffects.iOS.GestureCollectors;
using XamEffects.iOS.GestureRecognizers;

[assembly: ExportEffect(typeof(CommandsPlatform), nameof(Commands))]

namespace XamEffects.iOS {
    public class CommandsPlatform : PlatformEffect {
        public UIView View => Control ?? Container;

        DateTime _tapTime;
        private System.Timers.Timer _holdTimer;

        ICommand _tapCommand;
        ICommand _longCommand;
        ICommand _holdCommand;
        object _tapParameter;
        object _longParameter;
        object _holdParameter;

        protected override void OnAttached() {
            View.UserInteractionEnabled = true;

            UpdateTap();
            UpdateTapParameter();
            UpdateLongTap();
            UpdateLongTapParameter();
            UpdateHold();
            UpdateHoldParameter();

            TouchGestureCollector.Add(View, OnTouch);
        }

        protected override void OnDetached() {
            TouchGestureCollector.Delete(View, OnTouch);
        }

        void OnTouch(TouchGestureRecognizer.TouchArgs e) {
            switch (e.State) {
                case TouchGestureRecognizer.TouchState.Started:
                    _tapTime = DateTime.Now;
                    StartHoldTimer();
                    break;

                case TouchGestureRecognizer.TouchState.Ended:
                    if (e.Inside) {
                        var range = (DateTime.Now - _tapTime).TotalMilliseconds;
                        if (range > 800)
                            LongClickHandler();
                        else
                            ClickHandler();
                    }
                    StopHoldTimer();
                    break;

                case TouchGestureRecognizer.TouchState.Cancelled:
                    StopHoldTimer();
                    break;
            }
        }

        void ClickHandler() {
            if (_tapCommand?.CanExecute(_tapParameter) ?? false)
                _tapCommand.Execute(_tapParameter);
        }

        void LongClickHandler() {
            if (_longCommand == null)
                ClickHandler();
            else if (_longCommand.CanExecute(_longParameter))
                _longCommand.Execute(_longParameter);
        }

        void StartHoldTimer()
        {
            StopHoldTimer();

            _holdTimer = new System.Timers.Timer
            {
                Interval = 800,
                AutoReset = false
            };
            _holdTimer.Elapsed += _holdTimer_Elapsed;
            _holdTimer.Start();
        }

        void StopHoldTimer()
        {
            if (_holdTimer != null)
            {
                _holdTimer.Stop();
                _holdTimer.Dispose();
                _holdTimer = null;
            }
        }

        void _holdTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // UI Dispatcher because the timer runs on a different thread
            Device.BeginInvokeOnMainThread(() =>
            {
                HoldHandler();
            });
        }

        void HoldHandler()
        {
            if (_holdCommand?.CanExecute(_holdParameter) ?? false)
                _holdCommand.Execute(_holdParameter);
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args) {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == Commands.TapProperty.PropertyName)
                UpdateTap();
            else if (args.PropertyName == Commands.TapParameterProperty.PropertyName)
                UpdateTapParameter();
            else if (args.PropertyName == Commands.LongTapProperty.PropertyName)
                UpdateLongTap();
            else if (args.PropertyName == Commands.LongTapParameterProperty.PropertyName)
                UpdateLongTapParameter();
            else if (args.PropertyName == Commands.HoldProperty.PropertyName)
                UpdateHold();
            else if (args.PropertyName == Commands.HoldParameterProperty.PropertyName)
                UpdateHoldParameter();
        }

        void UpdateTap() {
            _tapCommand = Commands.GetTap(Element);
        }

        void UpdateTapParameter() {
            _tapParameter = Commands.GetTapParameter(Element);
        }

        void UpdateLongTap() {
            _longCommand = Commands.GetLongTap(Element);
        }

        void UpdateLongTapParameter() {
            _longParameter = Commands.GetLongTapParameter(Element);
        }

        void UpdateHold()
        {
            _holdCommand = Commands.GetHold(Element);
        }

        void UpdateHoldParameter()
        {
            _holdParameter = Commands.GetHoldParameter(Element);
        }

        public static void Init() {
        }
    }
}