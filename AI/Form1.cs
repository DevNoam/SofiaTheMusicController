using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace AI
{
    public partial class SofiaAI : Form
    {
        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 0;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);



        Choices list = new Choices();
        Grammar gm;


        public SofiaAI()
        {
            InitializeComponent();

            s.SelectVoiceByHints(VoiceGender.Neutral);
            list.Add(File.ReadAllLines(Application.StartupPath + "/commands.txt"));
            gm = new Grammar(new GrammarBuilder(list));

            try
            {
                sr.RequestRecognizerUpdate();
                sr.LoadGrammar(gm);
                sr.SpeechRecognized += Sr_SpeechRecognized;
                sr.SetInputToDefaultAudioDevice();
                sr.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }
        }

        SpeechSynthesizer s = new SpeechSynthesizer();
        SpeechRecognitionEngine sr = new SpeechRecognitionEngine();
        PromptBuilder pb = new PromptBuilder();


        private void button1_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Application.StartupPath + @"/beep.wav");
            player.Play();
            s.SelectVoiceByHints(VoiceGender.Neutral);
            isPressed = true;
            try
            {
                sr.RequestRecognizerUpdate();
                sr.LoadGrammar(gm);
                sr.SpeechRecognized += Sr_SpeechRecognized;
                sr.SetInputToDefaultAudioDevice();
                sr.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }
        }

        public void Say(string phrase)
        {
            s.SpeakAsync(phrase);
            wake = false;
            isPressed = false;
        }

        private bool wake = false;
        private bool isPressed = false;

        private void Sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speechSaid = e.Result.Text;
            if (isPressed == false)
            {
                if (speechSaid == "hey sofia")
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(Application.StartupPath + @"/beep.wav");
                    player.Play();
                    wake = true;
                }
            }

            if (speechSaid.Length > 1)
            {
                if (wake == true || isPressed == true)
                {
                    switch (speechSaid)
                    {
                        case ("hi"):
                            Say("hi");
                            break;

                        case ("hello"):
                            Say("hello");
                            break;

                        case ("hey"):
                            Say("hi");
                            break;

                        case ("are you a robot"):
                            Say("Yes I am");
                            break;

                        case ("how are you doing"):
                            Say("good, how about you");
                            break;

                        case ("close"):
                            Say("closing program");
                            Application.Exit();
                            break;

                        case ("exit"):
                            Say("Shutting down.");
                            Application.Exit();
                            break;
                        case ("open browser"):
                            Say("opening browser");
                            Process.Start("https://www.google.com");
                            break;
                        case ("next song"):
                            Say("Playing next song");
                            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("next track"):
                            Say("Playing next song");
                            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("next"):
                            Say("Playing next song");
                            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("prevoius"):
                            Say("Playing previous song");
                            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("prevoius track"):
                            Say("Playing previous song");
                            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("prevoius song"):
                            Say("Playing previous song");
                            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("pause"):
                            Say("paused");
                            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("play"):
                            Say("Playing");
                            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("play music"):
                            Say("Playing music");
                            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("pause music"):
                            Say("Music paused");
                            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("stop music"):
                            Say("Music paused");
                            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;
                        case ("stop"):
                            Say("Music paused");
                            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                            break;

                    }
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
    }
}
