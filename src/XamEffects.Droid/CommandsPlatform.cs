﻿using System;
using Android.Graphics;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamEffects;
using XamEffects.Droid;
using View = Android.Views.View;
using System.Threading;
using XamEffects.Droid.GestureCollectors;

[assembly: ExportEffect(typeof(CommandsPlatform), nameof(Commands))]

namespace XamEffects.Droid {
    public class CommandsPlatform : PlatformEffect {
        public View View => Control ?? Container;
        public bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;

        DateTime _tapTime;
        private System.Timers.Timer _holdTimer;
        readonly Rect _rect = new Rect();
        readonly int[] _location = new int[2];

        public static void Init() {
        }

        protected override void OnAttached() {
            View.Clickable = true;
            View.LongClickable = true;
            View.SoundEffectsEnabled = true;
            TouchCollector.Add(View, OnTouch);
        }

        void OnTouch(View.TouchEventArgs args) {
            switch (args.Event.Action) {
                case MotionEventActions.Down:
                    _tapTime = DateTime.Now;
                    StartHoldTimer();
                    break;

                case MotionEventActions.Up:
                    if (IsViewInBounds((int)args.Event.RawX, (int)args.Event.RawY)) {
                        var range = (DateTime.Now - _tapTime).TotalMilliseconds;
                        if (range > 800)
                            LongClickHandler();
                        else
                            ClickHandler();
                    }
                    StopHoldTimer();
                    break;
            }
        }

        bool IsViewInBounds(int x, int y) {
            View.GetDrawingRect(_rect);
            View.GetLocationOnScreen(_location);
            _rect.Offset(_location[0], _location[1]);
            return _rect.Contains(x, y);
        }

        void ClickHandler() {
            var cmd = Commands.GetTap(Element);
            var param = Commands.GetTapParameter(Element);
            if (cmd?.CanExecute(param) ?? false)
                cmd.Execute(param);
        }

        void LongClickHandler() {
            var cmd = Commands.GetLongTap(Element);

            if (cmd == null) {
                ClickHandler();
                return;
            }

            var param = Commands.GetLongTapParameter(Element);
            if (cmd.CanExecute(param))
                cmd.Execute(param);
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
            var cmd = Commands.GetHold(Element);
            var param = Commands.GetHoldParameter(Element);
            if (cmd?.CanExecute(param) ?? false)
                cmd.Execute(param);
        }

        protected override void OnDetached() {
            if (IsDisposed) return;
            StopHoldTimer();
            TouchCollector.Delete(View, OnTouch);
        }
    }
}