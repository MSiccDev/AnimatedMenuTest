using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace AnimatedMenuTest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public bool IsMenuOpen { get; set; } = false;

        public MainPage()
        {
            InitializeComponent();

            On<iOS>().SetUseSafeArea(true);


            var menuoptions = new List<string>();

            for (int i = 1; i < 13; i++)
                menuoptions.Add($"Option \n {i}");

            this.MenuOptionsView.ItemsSource = menuoptions;

            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += async (s, e) =>
            {
                await this.MenuButtoGrid.FadeTo(0.5, 150, Easing.CubicInOut);
                await this.MenuButtoGrid.FadeTo(1, 150, Easing.CubicInOut);

                if (!this.IsMenuOpen)
                    await ScrollMenuIn();
                else
                    await ScrollMenuOut();
            };

            this.MenuButtoGrid.GestureRecognizers.Add(tapRecognizer);

            this.ButtonTextLabel.PropertyChanging += async (s, e) =>
            {
                if (e.PropertyName == Label.TextProperty.PropertyName)
                {                   
                    await this.ButtonTextLabel.ScaleTo(0, 10, Easing.CubicInOut);
                    await this.ButtonTextLabel.ScaleTo(1, 300, Easing.CubicInOut);
                }
            };



        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ScrollMenuOut(0);
        }



        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsMenuOpen)
                await ScrollMenuOut();

            this.MenuOptionsView.SelectedItem = null;

        }


        private async Task ScrollMenuIn()
        {
            this.ButtonTextLabel.Text = "Hide Menu";

            var buttonRotationTask = this.ButtonImage.RotateTo(180, 300, Easing.Linear);

            var rect = new Rectangle(0, 0, this.ScrolledInView.Width, this.ScrolledInView.Height);
            var menuScrollTask = this.ScrolledInView.LayoutTo(rect, 500, Easing.CubicInOut);
            var menuScaleTask = this.ScrolledInView.ScaleTo(1, 500, Easing.BounceIn);

            await Task.WhenAll(buttonRotationTask, menuScrollTask, menuScaleTask);

            this.IsMenuOpen = true;
        }


        private async Task ScrollMenuOut(int duration = 500)
        {
            this.ButtonTextLabel.Text = "Show Menu";

            var buttonRotationTask = this.ButtonImage.RotateTo(0, 300, Easing.Linear);

            var rect = new Rectangle(0, - this.ScrolledInView.Height, this.ScrolledInView.Width, this.ScrolledInView.Height);
            var menuScrollTask = this.ScrolledInView.LayoutTo(rect, (uint)duration, Easing.CubicIn);
            var menuScaleTask = this.ScrolledInView.ScaleTo(0, 500, Easing.BounceOut);

            await Task.WhenAll(buttonRotationTask, menuScrollTask, menuScaleTask);
            this.IsMenuOpen = false;
        }
    }
}
