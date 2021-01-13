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

        bool triggerByVoice = true;

        public SofiaAI()
        {
            InitializeComponent();

            speech.SelectVoiceByHints(VoiceGender.Neutral);
            list.Add(File.ReadAllLines(Application.StartupPath + "/commands.txt"));
            gm = new Grammar(new GrammarBuilder(list));

            try
            {
                speechRecognition.RequestRecognizerUpdate();
                speechRecognition.LoadGrammar(gm);
                speechRecognition.SpeechRecognized += Sr_SpeechRecognized;
                speechRecognition.SetInputToDefaultAudioDevice();
                speechRecognition.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }
        }

        SpeechSynthesizer speech = new SpeechSynthesizer();
        SpeechRecognitionEngine speechRecognition = new SpeechRecognitionEngine();


        private void button1_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Application.StartupPath + @"/beep.wav");
            player.Play();
            speech.SelectVoiceByHints(VoiceGender.Neutral);
            isPressed = true;
            try
            {
                speechRecognition.RequestRecognizerUpdate();
                speechRecognition.LoadGrammar(gm);
                speechRecognition.SpeechRecognized += Sr_SpeechRecognized;
                speechRecognition.SetInputToDefaultAudioDevice();
                speechRecognition.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }
        }

        public void Say(string phrase)
        {
            speech.SpeakAsync(phrase);
            wake = false;
            isPressed = false;
        }

        private bool wake = false;
        private bool isPressed = false;

        private void Sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string userSpeech = e.Result.Text;
            if (isPressed == false)
            {
                if (triggerByVoice == true)
                {
                    if (userSpeech == "hey sofia")
                    {
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(Application.StartupPath + @"/beep.wav");
                        player.Play();
                        wake = true;
                    }
                }         
            }

            if (userSpeech.Length > 1)
            {
                if (wake == true || isPressed == true)
                {
                    switch (userSpeech)
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
                        case ("cancel"):
                            Say("");
                            break;
                        case ("stop listening to me"):
                            checkBox1.Checked = false;
                            triggerByVoice = false;
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

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/sapirnoam");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                triggerByVoice = true;
            }
            if(!checkBox1.Checked)
            {
                triggerByVoice = false;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
