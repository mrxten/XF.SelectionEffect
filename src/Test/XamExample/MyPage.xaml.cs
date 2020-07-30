using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XamEffects;

namespace XamExample {
    public partial class MyPage : ContentPage {

        private int _tapCount;
        private int _longTapCount;
        private int _holdCount;

        public MyPage() {
            InitializeComponent();

            Commands.SetTap(tap, new Command(TapHandler));
            Commands.SetLongTap(longTap, new Command(LongTapHandler));
            Commands.SetHold(hold, new Command(HoldHandler));


            Commands.SetTap(all, new Command(TapHandler));
            Commands.SetLongTap(all, new Command(LongTapHandler));
            Commands.SetHold(all, new Command(HoldHandler));
        }

        private void TapHandler(object obj)
        {
            _tapCount++;
            result.Text = "Tap " + _tapCount;
        }
        private void LongTapHandler(object obj)
        {
            _longTapCount++;
            result.Text = "Long Tap " + _longTapCount;
        }
        private void HoldHandler(object obj)
        {
            _holdCount++;
            result.Text = "Hold " + _holdCount;
        }
    }
}
