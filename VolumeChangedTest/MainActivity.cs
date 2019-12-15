using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using LibVLCSharp.Shared;
using VideoView = LibVLCSharp.Platforms.Android.VideoView;

namespace VolumeChangedTest
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        VideoView _videoView;
        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC)
            {
                EnableHardwareDecoding = true
            };
            _mediaPlayer.VolumeChanged += MediaPlayer_VolumeChanged;

            ((Button)FindViewById(Resource.Id.stopButton)).Click += StopButton_Click;

            _videoView = new VideoView(this) { MediaPlayer = _mediaPlayer };
            _videoView.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, 1);
            ((ViewGroup)FindViewById(Resource.Id.videoViewLayout)).AddView(_videoView, 0);

            var media = new Media(_libVLC, "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", FromType.FromLocation);
            _videoView.MediaPlayer.Play(media);
        }

        private void StopButton_Click(object sender, System.EventArgs e)
        {
            _mediaPlayer.Stop();
        }

        private void MediaPlayer_VolumeChanged(object sender, MediaPlayerVolumeChangedEventArgs e)
        {
        }

        protected override void OnPause()
        {
            base.OnPause();

            _videoView.MediaPlayer.Stop();
            _videoView.Dispose();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
