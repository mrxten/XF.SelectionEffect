using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XamEffects;

namespace XamExample {
    public partial class MyPage : ContentPage {
        public MyPage() {
            InitializeComponent();

            var tapCount = 0;
            Commands.SetTap(touch, new Command(() => {
                tapCount++;
                text.Text = "tap " + tapCount;
            }));

            var longTapCount = 0;
            Commands.SetLongTap(touch, new Command(() => {
                longTapCount++;
                text.Text = "longTap " + longTapCount;
            }));

            var holdCount = 0;
            Commands.SetHold(touch, new Command(() => {
                holdCount++;
                text.Text = "holdCount " + holdCount;
            }));
        }
    }
}
