using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App_para_dividir_videos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            string inputFilePath = @"C:\Users\lacar\Downloads\Carpeta\video.mp4";
            string outputFilePath = @"C:\Users\lacar\Downloads\Carpeta\VideoCortado\";
            int startTimeInSeconds = 0; // Tiempo de inicio en segundos
            int durationInSeconds = 0; // Duración del segmento en segundos

            Console.WriteLine("cada cuantos segundos quieres que el video se corte");
            durationInSeconds = Convert.ToInt32(Console.ReadLine());
            SplitVideo(inputFilePath, outputFilePath, startTimeInSeconds, durationInSeconds);

            Console.WriteLine("Video dividido con éxito.");
            Console.ReadKey();
        }

        static void SplitVideo(string inputFilePath, string outputFilePath, int startTime, int duration)
        {
            int i = 0;

            while (true)
            {
                using (var process = new Process())
                {
                    process.StartInfo.FileName = @"C:\ffmpeg\bin\ffmpeg.exe";
                    process.StartInfo.Arguments = $"-ss {startTime} -i {inputFilePath}  -t {duration} -c:v libx264 -c:a aac {outputFilePath}resultado{i}.mp4";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;


                    process.Start();
                    process.WaitForExit();

                    //Verificar si el proceso termino
                    if (process.HasExited)
                    {
                        startTime += duration;
                        i++;


                        int resta = (Convert.ToInt32(GetVideoDurationInSeconds(inputFilePath)) - 1000);
                        int aproximacion = resta / 1000000;
                        //verificar si ha llegado al final
                        if (startTime >= aproximacion)
                        {
                            break;
                        }
                    }

                }

            }




        }
        static double GetVideoDurationInSeconds(string inputFilePath)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = @"C:\ffmpeg\bin\ffprobe.exe";
                process.StartInfo.Arguments = $"-v error -select_streams v:0 -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{inputFilePath}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();

                string durationStr = process.StandardOutput.ReadToEnd();
                //double durationC = double.Parse(durationStr);
                //int numeroAMover = 6;
                //Mover coma hacia a la izquierda
                //int numeroConComa = durationStr * (int)Math.Pow(10, numeroAMover);

                //string durationStrConComa = durationStr.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
                if (double.TryParse(durationStr, out double durationvideo))
                {
                    return Convert.ToInt32(durationvideo);
                }
                else
                {
                    return 0;
                }
            }
        }
        

    }
}


