
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Accord.Audio;
using Accord.Audio.Formats;
using Accord.DirectSound;
using System.Threading.Tasks;
using System.Drawing;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace PTVision
{
    public class RecordingClass
    {

        //public static VideoFileWriter vf;
        AviWriter writer;
        IAviVideoStream videoStream;
        FourCC Codec;

        public static AudioCaptureDevice AudioSource;
       


        System.DateTime sartRecordingTime;
        System.DateTime lastRecordingVideoTime;
        System.DateTime lastRecordingSoundTime;




        private MemoryStream stream;


        private WaveEncoder encoder;

        Process process;

        private float[] current;

        private int frames;
        private int samples;
        private TimeSpan duration;

        string filenameOriginal;
        string filenameAudio;
        string filenameCombined;
        string filenameMp4;

        
        bool audioReady=false;

        private Thread screenThread;
        ManualResetEvent stopThread = new ManualResetEvent(false);
        AutoResetEvent videoFrameWritten = new AutoResetEvent(false);
        AutoResetEvent audioBlockWritten = new AutoResetEvent(false);

       
        public bool isRecording = false;

        Bitmap bmpScreenShot;

        int width, height;

        public RecordingClass(string id)
        {

            foreach (var process in Process.GetProcessesByName("ffmpeg"))
            {
                process.Kill();
            }

            
            filenameAudio = System.IO.Path.Combine(Globals.usersPathVideos, id + ".wav");
            
            filenameCombined = System.IO.Path.Combine(Globals.usersPathVideos, id + "c.mp4");
            
            filenameOriginal = System.IO.Path.Combine(Globals.usersPathVideos, id + ".avi");

            filenameMp4 = System.IO.Path.Combine(Globals.usersPathVideos, id + ".mp4");

            initalizeAudioStuff();

           // vf = new VideoFileWriter();
          
            
        }

        #region audio
        private void initalizeAudioStuff()
        {

            try
            {
                audioReady = false;
                AudioSource = new AudioCaptureDevice();
                AudioDeviceInfo info = null;
                var adc = new AudioDeviceCollection(AudioDeviceCategory.Capture);
                foreach (var ad in adc)
                {
                    string desc = ad.Description;
                    if (desc.IndexOf("Audio") > -1)
                    {
                        info = ad;
                    }
                }
                if (info == null)
                {
                    AudioSource = new AudioCaptureDevice();
                }
                else
                {
                    AudioSource = new AudioCaptureDevice(info);
                }

                //AudioCaptureDevice source = new AudioCaptureDevice();
                AudioSource.DesiredFrameSize = 4096;
                AudioSource.SampleRate = 44100;
                //int sampleRate = 44100;
                //int sampleRate = 22050;

                AudioSource.NewFrame += AudioSource_NewFrame;
                // AudioSource.Format = SampleFormat.Format64BitIeeeFloat;
                AudioSource.AudioSourceError += AudioSource_AudioSourceError;
                // AudioSource.Start();
                //int x = 1;
            }
            catch
            {

            }

            // Create buffer for wavechart control
            current = new float[AudioSource.DesiredFrameSize];

            // Create stream to store file
            stream = new MemoryStream();
            encoder = new WaveEncoder(stream);

            frames = 0;
            samples = 0;





        }

        private void AudioSource_AudioSourceError(object sender, AudioSourceErrorEventArgs e)
        {
            int x = 1;
            x++;
        }

        private void AudioSource_NewFrame(object sender, Accord.Audio.NewFrameEventArgs e)
        {



            if (isRecording)
            {
                System.TimeSpan diff1 = DateTime.Now.Subtract(sartRecordingTime);
                if (diff1.Seconds >= 0.0)
                {
                    //writer.WriteAudioFrame(e.Signal.RawData);
                    e.Signal.CopyTo(current);

                    encoder.Encode(e.Signal);


                    duration += e.Signal.Duration;

                    samples += e.Signal.Samples;
                    frames += e.Signal.Length;
                }


            }
        }

        private void doAudioStop()
        {
            // Stops both cases
            if (AudioSource != null)
            {
                // If we were recording
                AudioSource.SignalToStop();
                AudioSource.WaitForStop();
            }


            var fileStream = File.Create(filenameAudio);
            stream.WriteTo(fileStream);
            fileStream.Close();
            fileStream.Dispose();
            // Also zero out the buffers and screen
            Array.Clear(current, 0, current.Length);
            // AudioSource.Dispose();
            audioReady = true;

        }

        #endregion

        public void startRecording()
        {
            isRecording = true;

            // Start
            AudioSource.Start();

            startVideo();

        }

     

      

        #region videoStuff

        void startVideo()
        {
           

            writer = new AviWriter(filenameOriginal)
            {
                FramesPerSecond = 20,
                EmitIndex1 = true,
            };

            FourCC codec = CodecIds.MotionJpeg;

            width = Globals.SourceWidth * 2;
            height = Globals.SourceHeight * 2;

            videoStream = CreateVideoStream(codec, 40);

            sartRecordingTime = DateTime.Now;
            screenThread = new Thread(new ThreadStart(RecordScreen));
            screenThread.Start();
        }


        private IAviVideoStream CreateVideoStream(FourCC codec, int quality)
        {
            // Select encoder type based on FOURCC of codec
            if (codec == CodecIds.Uncompressed)
            {
                return writer.AddUncompressedVideoStream(Globals.SourceWidth , Globals.SourceHeight );
            }
            else if (codec == CodecIds.MotionJpeg)
            {
                // Use M-JPEG based on WPF (Windows only)
                return writer.AddMJpegWpfVideoStream(width, height, quality);
            }
            else
            {
                return writer.AddMpeg4VcmVideoStream(width, height, (double)writer.FramesPerSecond,
                    // It seems that all tested MPEG-4 VfW codecs ignore the quality affecting parameters passed through VfW API
                    // They only respect the settings from their own configuration dialogs, and Mpeg4VideoEncoder currently has no support for this
                    quality: quality,
                    codec: codec,
                    // Most of VfW codecs expect single-threaded use, so we wrap this encoder to special wrapper
                    // Thus all calls to the encoder (including its instantiation) will be invoked on a single thread although encoding (and writing) is performed asynchronously
                    forceSingleThreadedAccess: true);
            }
        }



        private void RecordScreen()
        {
            var stopwatch = new Stopwatch();
            var buffer = new byte[width * height * 4];
            Task videoWriteTask = null;
            var isFirstFrame = true;
            var shotsTaken = 0;
            var timeTillNextFrame = TimeSpan.Zero;
            stopwatch.Start();
            AudioSource.Start();
            while (!stopThread.WaitOne(timeTillNextFrame))
            {
                GetScreenshot(buffer);
                shotsTaken++;

                // Wait for the previous frame is written
                if (!isFirstFrame)
                {
                    videoWriteTask.Wait();
                    videoFrameWritten.Set();
                }


                // Start asynchronous (encoding and) writing of the new frame
                // Overloads with Memory parameters are available on .NET 5+
#if NET5_0_OR_GREATER
                videoWriteTask = videoStream.WriteFrameAsync(true, buffer.AsMemory(0, buffer.Length));
#else
                videoWriteTask = videoStream.WriteFrameAsync(true, buffer, 0, buffer.Length);
#endif

                timeTillNextFrame = TimeSpan.FromSeconds(shotsTaken / (double)writer.FramesPerSecond - stopwatch.Elapsed.TotalSeconds);
                if (timeTillNextFrame < TimeSpan.Zero)
                    timeTillNextFrame = TimeSpan.Zero;

                isFirstFrame = false;
            }

            stopwatch.Stop();

            // Wait for the last frame is written
            if (!isFirstFrame)
            {
                videoWriteTask.Wait();
            }
        }


        private void GetScreenshot(byte[] buffer)
        {
            using (var bitmap = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(Globals.SourceLeft*2, Globals.SourceTop*2, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
                var bits = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                bitmap.UnlockBits(bits);

                
            }
        }




        #endregion


        #region stop

        public void stopRecording()
        {
            Dispose();
        }

        public void Dispose()
        {
            stopThread.Set();
            screenThread.Join();


            // Close writer: the remaining data is written to a file and file is closed
            writer.Close();

            stopThread.Close();
            isRecording = false;
            doAudioStop();
            combineFiles();
        }


        public void combineFiles()
        {

            Task taskA = Task.Run(() => combineFilesThread());
        }


        void combineFilesThread()
        {
            try
            {

                string FFmpegFilename;
                
                FFmpegFilename = Directory.GetCurrentDirectory() + "\\HelpExes\\FFmpeg\\bin\\ffmpeg.exe";

                process = new Process();
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.FileName = FFmpegFilename;
                string arguments = "-i " + filenameOriginal + " -strict experimental " + filenameMp4;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;

                process.Start();

               
                process.WaitForExit();


                process = new Process();
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.FileName = FFmpegFilename;
                process.StartInfo.Arguments = "-i " + filenameMp4 + " -i " + filenameAudio + " -c:v copy -c:a aac -strict experimental " + filenameCombined + " -shortest";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();



            }
            catch
            {
                int x = 0;
                x++;
            }


        }

        private void Process_Video_Exited(object? sender, EventArgs e)
        {
            try
            {
                string FFmpegFilename;

                FFmpegFilename = Directory.GetCurrentDirectory() + "\\HelpExes\\FFmpeg\\bin\\ffmpeg.exe";

                process = new Process();
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.FileName = FFmpegFilename;
                process.StartInfo.Arguments = "-i " + filenameMp4 + " -i " + filenameAudio + " -c:v copy -c:a aac -strict experimental " + filenameCombined + " -shortest";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();
            }
            catch
            {

            }
            
        }

        #endregion

    }
}

